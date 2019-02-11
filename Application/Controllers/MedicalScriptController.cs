using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
//using MedicalScript.Models;
using ArVision.Pharma.Shared.DataModels;

namespace Pharma.Controllers
{
    public class MedicalScriptController : BaseController
    {
        //MedicalScriptEntities db = new MedicalScriptEntities();

        public ActionResult Index()
        {
            return View();
        }
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/MedicalScript/UploadScript")]
        //public HttpResponseMessage UploadScript()
        public ActionResult UploadScript()
        {
            try
            {
                string filename = "";
                string filePath = "";
                //HttpFileCollection files = HttpContext.Current.Request.Files;
                HttpFileCollectionBase files = Request.Files;
                //HttpPostedFile scriptfile = files["scriptfile"];
                HttpPostedFileBase scriptfile = files["scriptfile"];
                if (scriptfile != null)
                {
                    filename = string.Format("{0:yyyy-MM-dd-HH-mm-ss}", DateTime.Now)  + Path.GetExtension(scriptfile.FileName);
                    filePath = "~/Uploads/" + filename;
                    //scriptfile.SaveAs(HttpContext.Current.Server.MapPath(filePath));
                    scriptfile.SaveAs(Server.MapPath(filePath));
                }
                //---------
                //string path = HttpContext.Current.Server.MapPath(filePath);
                string path = Server.MapPath(filePath);
                //-------- Auto -----

                var Ocr = new IronOcr.AutoOcr();
                var Result = Ocr.Read(path);
                //---------Advanced -----
                //var Ocr = new IronOcr.AdvancedOcr()
                //{
                //    CleanBackgroundNoise = true,
                //    EnhanceContrast = true,
                //    EnhanceResolution = true,
                //    Language = IronOcr.Languages.English.OcrLanguagePack,
                //    Strategy = IronOcr.AdvancedOcr.OcrStrategy.Advanced,
                //    ColorSpace = AdvancedOcr.OcrColorSpace.Color,
                //    DetectWhiteTextOnDarkBackgrounds = true,
                //    InputImageType = AdvancedOcr.InputTypes.AutoDetect,
                //    RotateAndStraighten = true,
                //    ReadBarCodes = true,
                //    ColorDepth = 4
                //};
                //var testDocument = path;
                //var Result = Ocr.Read(testDocument);
                //-----------
                var medicalScriptObj = new MedicalScript();
                string scriptText = Result.Text.Trim();
                string[] LinesOfText = scriptText.Replace("\r", "").Replace("\n\n", "\n").Split('\n');
                // determine which script format
                if (scriptText.Contains("Total Dose For") || scriptText.Contains("OHIP") || scriptText.Contains("Tel:"))
                {
                    // form 2
                    medicalScriptObj = ExtractFieldsFromForm2(LinesOfText);
                }
                else if (scriptText.Contains("Dose for period") || scriptText.Contains("Health Card"))
                {
                    // form 1
                    medicalScriptObj = ExtractFieldsFromForm1(LinesOfText);
                }
                //return Request.CreateResponse(HttpStatusCode.OK, new { filePath = filePath.Substring(1), medicalScriptObj = medicalScriptObj });
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(new { filePath = filePath.Substring(1), medicalScriptObj = medicalScriptObj }, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //return Request.CreateErrorResponse(HttpStatusCode.InternalServerError,ex.Message );
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(ex, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            }

        }

        private MedicalScript ExtractFieldsFromForm2(string[] LinesOfText)
        {
            var medicalScriptObj = new MedicalScript();
            try
            {
                for (int l = 0; l < LinesOfText.Length; l++)
                {
                    // skip Empty lines
                    if (string.IsNullOrEmpty(LinesOfText[l].Trim()))
                    {
                        continue;
                    }
                    string crntTextLine = LinesOfText[l].Trim();
                    if (crntTextLine.Contains("NOVX"))
                    {
                        bool isClinicAddress = true;
                        string ClinicAddress = "";
                        while (isClinicAddress)
                        {
                            l++;
                            crntTextLine = LinesOfText[l].Trim();
                            if (crntTextLine.Contains("Tel:"))
                            {
                                medicalScriptObj.ClinicTel = crntTextLine.Replace("Tel:", "").Trim();
                                medicalScriptObj.ClinicAddress = ClinicAddress;
                                isClinicAddress = false;
                            }
                            else
                            {
                                if (ClinicAddress != "")
                                    ClinicAddress += "\r\n";
                                ClinicAddress += crntTextLine;
                            }
                        }// while

                        //continue;
                    }
                    else if( string.IsNullOrEmpty(medicalScriptObj.ClinicAddress)&&l==0)
                    {
                        bool isClinicAddress = true;
                        string ClinicAddress = "";
                        while (isClinicAddress)
                        {
                            
                            crntTextLine = LinesOfText[l].Trim();
                            if (crntTextLine.Contains("Tel:"))
                            {
                                medicalScriptObj.ClinicTel = crntTextLine.Replace("Tel:", "").Trim();
                                medicalScriptObj.ClinicAddress = ClinicAddress;
                                isClinicAddress = false;
                            }
                            else
                            {
                                if (ClinicAddress != "")
                                    ClinicAddress += "\r\n";
                                ClinicAddress += crntTextLine;
                                l++;
                            }
                            
                        }// while
                    }
                    //else if (crntTextLine.Contains("Tel:"))
                    //{
                    //    medicalScriptObj.ClinicTel = crntTextLine.Replace("Tel:", "").Trim();
                    //}
                    else if (crntTextLine.Contains("Fax") && !crntTextLine.Contains("Phone"))
                    {
                        medicalScriptObj.ClinicFax = crntTextLine.Replace("Fax:", "").Replace("Fax.", "").Replace("Fax", "").Trim();
                    }
                    else if (crntTextLine.Contains("Dr.")|| crntTextLine.Contains("Dr:"))
                    {
                        medicalScriptObj.DoctorName = crntTextLine.Replace("Dr.", "").Replace("Dr:", "").Trim();
                    }
                    else if (crntTextLine.Contains("Created On"))
                    {
                        medicalScriptObj.ScriptCreationDate = crntTextLine.Replace("Created On.", "").Replace("Created On:", "").Replace("Created On", "").Trim();
                    }
                    else if (crntTextLine.Contains("For:")|| crntTextLine.Contains("For."))
                    {
                        medicalScriptObj.PatientName = crntTextLine.Replace("For:", "|").Replace("For.", "|").Replace("For", "|").Trim().Split('|')[1];
                    }
                    else if (crntTextLine.Contains("DOB:") || crntTextLine.Contains("DOB.") || crntTextLine.Contains("DOB") )
                    {
                        medicalScriptObj.PatientDOB = crntTextLine.Replace("DOB:", "").Replace("DOB.", "").Replace("DOB", "").Trim();
                    }
                    else if((medicalScriptObj.PatientDOB==null && medicalScriptObj.PatientOHIP == null && medicalScriptObj.PatientName != null))
                    {
                        var DOB_Arr = crntTextLine.Trim().Split(' ');
                        var DOB = DOB_Arr[DOB_Arr.Length - 1].Trim();
                        if (DOB.Length>=8 && DOB.Contains("/"))
                        {
                            medicalScriptObj.PatientDOB = DOB;
                        }
                    }
                    else if (crntTextLine.Contains("OHIP:")|| crntTextLine.Contains("OHIP.")|| crntTextLine.Contains("OHIP"))
                    {
                        medicalScriptObj.PatientOHIP = crntTextLine.Replace("OHIP:", "").Replace("OHIP.", "").Replace("OHIP", "").Trim();
                    }
                    else if (crntTextLine.ToLower().Contains("phone") && !crntTextLine.ToLower().Contains("fax"))
                    {
                        medicalScriptObj.PatientPhone = crntTextLine.ToLower().Replace("phone:", "").Replace("phone.", "").Replace("phone", "").Trim();
                    }
                    else if (crntTextLine.ToLower().Contains("script intended for"))
                    {
                        medicalScriptObj.PharmacyName = crntTextLine.ToLower().Replace("script intended for:", "").Replace("script intended for.", "").Replace("script intended for", "").Trim();
                    }
                    else if (crntTextLine.Contains("Address"))
                    {
                        medicalScriptObj.PharmacyAddress = crntTextLine.Replace("Address:", "").Replace("Address.", "").Replace("Address", "").Trim();
                    }
                    else if (crntTextLine.ToLower().Contains("phone") && crntTextLine.ToLower().Contains("fax"))
                    {
                        string PhoneFax = crntTextLine.ToLower().Replace("phone:", "").Replace("phone.", "").Replace("phone", "").Replace("fax:", "|").Replace("fax.", "|").Replace("fax", "|").Trim();
                        medicalScriptObj.PharmacyPhone = PhoneFax.Split('|')[0];
                        medicalScriptObj.PharmacyFax = PhoneFax.Split('|')[1];
                        bool isDose = true;
                        string DoseName = "";
                        while (isDose)
                        {
                            l++;
                            crntTextLine = LinesOfText[l].Trim();
                            if (crntTextLine.ToLower().Contains("from")|| crntTextLine.ToLower().Contains("fr0m"))
                            {
                                string FromTo = crntTextLine.ToLower().Replace("from:", "").Replace("from.", "").Replace("from", "").Replace("to:", "|").Replace("to.", "|").Replace("to", "|").Replace("fr0m:", "").Replace("fr0m.", "").Replace("fr0m", "").Replace("t0:", "|").Replace("t0.", "|").Replace("t0", "|").Trim();
                                medicalScriptObj.DoseFromDate = FromTo.Split('|')[0].Trim();
                                medicalScriptObj.DoseToDate = FromTo.Split('|')[1].Trim();

                                medicalScriptObj.DoseName = DoseName;
                                isDose = false;
                            }
                            else
                            {
                                if (DoseName != "")
                                    DoseName += "\r\n";
                                DoseName += crntTextLine;
                            }

                        }// while
                    }
                    else if (crntTextLine.Contains("Total Dose for"))
                    {
                        string DaysAmount = crntTextLine.Replace("Total Dose for", "").Replace("‘", ":").Trim();
                        if (!DaysAmount.Contains(":"))
                            DaysAmount= DaysAmount.Replace("(s)", "(s):");
                        medicalScriptObj.DoseTotalDays = DaysAmount.Split(':')[0].Trim();
                        medicalScriptObj.DoseTotalAmount = DaysAmount.Split(':')[1].Trim();

                        bool isNote = true;
                        string Note = "";
                        while (isNote)
                        {
                            l++;
                            crntTextLine = LinesOfText[l].Trim();
                            if (crntTextLine.Contains("MM M D") || crntTextLine.Contains("3 E M D")||crntTextLine.Contains("MM MD")|| crntTextLine.Contains("MD")||crntTextLine.Contains("M [)"))
                            {
                                medicalScriptObj.MD = crntTextLine.Replace("MM M D", "").Replace("3 E M D", "").Replace("3 E M D", "").Replace("MD", "").Replace("M [)", "").Replace(")","").Replace("(", "");

                                medicalScriptObj.PharmacistNurseNotes = Note;
                                isNote = false;
                            }
                            else
                            {
                                if (Note != "")
                                    Note += "\r\n";
                                Note += crntTextLine;
                            }

                        }// while

                    }
                    else if (crntTextLine.Contains("Verification #") || crntTextLine.Contains("Venfucatlon #")|| crntTextLine.Contains("Veriﬁcation #")||crntTextLine.Contains("Venfncatlon t"))
                    {
                        medicalScriptObj.Verification = crntTextLine.Replace("Verification #:", "").Replace("Verification #", "").Replace("Venfucatlon #", "").Replace("Veriﬁcation #","").Replace("Venfncatlon t", "").Replace(":","").Trim().Split(' ')[0];
                    }


                }

                //db.MedicalScript.Add(medicalScriptObj);
                //db.SaveChanges();

            }
            catch
            { }
            return medicalScriptObj;
        }

        private MedicalScript ExtractFieldsFromForm1(string[] LinesOfText)
        {
            var medicalScriptObj = new MedicalScript();
            try
            {
                for (int l = 0; l < LinesOfText.Length; l++)
                {
                    // skip Empty lines
                    if (string.IsNullOrEmpty(LinesOfText[l].Trim()))
                    {
                        continue;
                    }
                    string crntTextLine = LinesOfText[l].Trim();

                    if (medicalScriptObj.PharmacyName == null && (crntTextLine.ToLower().Contains("phone") && crntTextLine.ToLower().Contains("fax"))|| (crntTextLine.ToLower().Contains("phone") && crntTextLine.ToLower().Contains("far")))
                    {
                        string PhoneFax = crntTextLine.ToLower().Replace("phone:", "").Replace("phone.", "").Replace("phone", "").Replace("fax:", "|").Replace("fax.", "|").Replace("fax", "|").Replace("far:", "|").Replace("far.", "|").Replace("far", "|").Trim();
                        medicalScriptObj.ClinicTel = PhoneFax.Split('|')[0];
                        medicalScriptObj.ClinicFax = PhoneFax.Split('|')[1];

                        medicalScriptObj.ClinicAddress = LinesOfText[l - 2] + "\r\n" + LinesOfText[l - 1];
                    }
                    else if (crntTextLine.Contains("Dr.") || crntTextLine.Contains("Dr:"))
                    {
                        medicalScriptObj.DoctorName = crntTextLine.Replace("Dr.", "").Replace("Dr:", "").Trim();
                    }
                    else if (crntTextLine.Contains("Created On")||crntTextLine.Contains("realm On"))
                    {
                        medicalScriptObj.ScriptCreationDate = crntTextLine.Replace("Created On.", "").Replace("Created On:", "").Replace("Created On", "").Replace("realm On", "").Replace(":", "").Trim();
                    }
                    else if (crntTextLine.Contains("For:") || crntTextLine.Contains("For."))
                    {
                        medicalScriptObj.PatientName = crntTextLine.Replace("For:", "|").Replace("For.", "|").Replace("For", "|").Trim().Split('|')[1];
                    }
                    else if (crntTextLine.Contains("DOB:") || crntTextLine.Contains("DOB.") || crntTextLine.Contains("DOB"))
                    {
                        medicalScriptObj.PatientDOB = crntTextLine.Replace("DOB:", "").Replace("DOB.", "").Replace("DOB", "").Trim();
                    }
                    else if (crntTextLine.Contains("Health"))
                    {
                        medicalScriptObj.HealthCard = crntTextLine.Replace("Health Card:", "|").Replace("Health Card.", "|").Replace("Health Card", "|").Replace("Health", "|").Replace("Card", "|").Trim().Split('|')[1];
                    }
                    else if ((medicalScriptObj.PatientDOB == null && medicalScriptObj.PatientOHIP == null && medicalScriptObj.PatientName != null))
                    {
                        var DOB_Arr = crntTextLine.Trim().Split(' ');
                        var DOB = DOB_Arr[DOB_Arr.Length - 1].Trim();
                        if (DOB.Length >= 8 && DOB.Contains("/"))
                        {
                            medicalScriptObj.PatientDOB = DOB;
                        }
                    }

                    //-------                                        
                    if (crntTextLine.ToLower().Contains("script intended for"))
                    {
                        medicalScriptObj.PharmacyName = crntTextLine.ToLower().Replace("script intended for:", "").Replace("script intended for.", "").Replace("script intended for", "").Trim();
                    }

                    if (medicalScriptObj.PharmacyName !=null && (crntTextLine.ToLower().Contains("phone") && crntTextLine.ToLower().Contains("fax")) || (crntTextLine.ToLower().Contains("phone") && crntTextLine.ToLower().Contains("far")))
                    {
                        string PhoneFax = crntTextLine.ToLower().Replace("phone:", "").Replace("phone.", "").Replace("phone", "").Replace("fax:", "|").Replace("fax.", "|").Replace("fax", "|").Replace("far", "|").Trim();
                        medicalScriptObj.PharmacyPhone = PhoneFax.Split('|')[0];
                        medicalScriptObj.PharmacyFax = PhoneFax.Split('|')[1];
                        bool isDose = true;
                        string DoseName = "";
                        while (isDose)
                        {
                            l++;
                            crntTextLine = LinesOfText[l].Trim();
                            if (crntTextLine.ToLower().Contains("from") || crntTextLine.ToLower().Contains("fr0m"))
                            {
                                string FromTo = crntTextLine.ToLower().Replace("from:", "").Replace("from.", "").Replace("from", "").Replace("to:", "|").Replace("to.", "|").Replace("to", "|").Replace("fr0m:", "").Replace("fr0m.", "").Replace("fr0m", "").Replace("t0:", "|").Replace("t0.", "|").Replace("t0", "|").Trim();
                                medicalScriptObj.DoseFromDate = FromTo.Split('|')[0].Trim();
                                medicalScriptObj.DoseToDate = FromTo.Split('|')[1].Trim();

                                medicalScriptObj.DoseName = DoseName;
                                isDose = false;
                            }
                            else
                            {
                                if (DoseName != "")
                                    DoseName += "\r\n";
                                DoseName += crntTextLine;
                            }

                        }// while
                    }
                    
                    if (crntTextLine.Contains("Dose for period"))
                    {
                        string DaysAmount = crntTextLine.Replace("Dose for period", "").Replace("Dose for", "").Replace("‘", ":").Trim();
                        if (!DaysAmount.Contains(":"))
                            DaysAmount = DaysAmount.Replace("(s)", "(s):");
                        medicalScriptObj.DoseTotalDays = DaysAmount.Split(':')[0].Trim();
                        medicalScriptObj.DoseTotalAmount = DaysAmount.Split(':')[1].Trim();

                        bool isNote = true;
                        string Note = "";
                        while (isNote)
                        {
                            l++;
                            crntTextLine = LinesOfText[l].Trim();
                            if (crntTextLine.Contains("MM M D") || crntTextLine.Contains("3 E M D") || crntTextLine.Contains("MM MD") || crntTextLine.Contains("MD") || crntTextLine.Contains("M [)"))
                            {
                                medicalScriptObj.MD = crntTextLine.Replace("MM M D", "").Replace("3 E M D", "").Replace("3 E M D", "").Replace("MD", "").Replace("M [)", "").Replace(")", "").Replace("(", "");

                                medicalScriptObj.PharmacistNurseNotes = Note;
                                isNote = false;
                            }
                            else
                            {
                                if (Note != "")
                                    Note += "\r\n";
                                Note += crntTextLine;
                            }

                        }// while

                    }
                    if (crntTextLine.Contains("Verification #") || crntTextLine.Contains("Venfucatlon #") || crntTextLine.Contains("Veriﬁcation #") || crntTextLine.Contains("Venfncatlon t"))
                    {
                        medicalScriptObj.Verification = crntTextLine.Replace("Verification #:", "").Replace("Verification #", "").Replace("Venfucatlon #", "").Replace("Veriﬁcation #", "").Replace("Venfncatlon t", "").Replace(":", "").Trim().Split(' ')[0];
                    }


                }

                //db.MedicalScript.Add(medicalScriptObj);
                //db.SaveChanges();

            }
            catch
            { }
            return medicalScriptObj;
        }


    }
}