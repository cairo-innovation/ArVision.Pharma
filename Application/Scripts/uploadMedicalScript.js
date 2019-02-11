/// <reference path="jquery-1.10.2.js" />
$(document).ready(function () {

    //alert('ready');

    $('#btn_Upload').click(function () {
        debugger;
        //alert('btn_Upload');
        var frmData = new FormData();
        var script_file = $("#script_file");
        var scriptfiles = script_file.get(0).files;
        if (scriptfiles.length > 0) {
            frmData.append('scriptfile', scriptfiles[0]);
        }
        $('#btn_Upload').hide();
        $('#tbl_Script').hide();
        $('#img_Loading').show();
        //alert('ajax');
        $.ajax({
            url: "/api/MedicalScript/UploadScript",
            type: "POST",
            contentType: false,
            processData: false,
            data: frmData,
            success: function (result) {
                //alert('success');
                var filePath = result.filePath
                $('#div_lnk').empty();
                var fileLnk = "<a href='" + filePath + "' target='_blank'>uploaded file ...</a>";
                $('#div_lnk').append(fileLnk);
                var medicalScriptObj = result.medicalScriptObj;
                $('#tbl_Script').show();
                var trs = "";
                trs += "<tr><td>Id</td><td>" + medicalScriptObj.Id;
                trs += "</td></tr><tr><td>ClinicAddress</td><td>" + medicalScriptObj.ClinicAddress;
                trs += "</td></tr><tr><td>ClinicTel</td><td>" + medicalScriptObj.ClinicTel;
                trs += "</td></tr><tr><td>ClinicFax</td><td>" + medicalScriptObj.ClinicFax;
                trs += "</td></tr><tr><td>DoctorName</td><td>" + medicalScriptObj.DoctorName;
                trs += "</td></tr><tr><td>ScriptCreationDate</td><td>" + medicalScriptObj.ScriptCreationDate;
                trs += "</td></tr><tr><td>PatientName</td><td>" + medicalScriptObj.PatientName;
                trs += "</td></tr><tr><td>PatientDOB</td><td>" + medicalScriptObj.PatientDOB;
                trs += "</td></tr><tr><td>PatientOHIP</td><td>" + medicalScriptObj.PatientOHIP;
                trs += "</td></tr><tr><td>PatientPhone</td><td>" + medicalScriptObj.PatientPhone;
                trs += "</td></tr><tr><td>PharmacyName</td><td>" + medicalScriptObj.PharmacyName;
                trs += "</td></tr><tr><td>PharmacyAddress</td><td>" + medicalScriptObj.PharmacyAddress;
                trs += "</td></tr><tr><td>PharmacyPhone</td><td>" + medicalScriptObj.PharmacyPhone;
                trs += "</td></tr><tr><td>PharmacyFax</td><td>" + medicalScriptObj.PharmacyFax;
                trs += "</td></tr><tr><td>DoseName</td><td>" + medicalScriptObj.DoseName;
                trs += "</td></tr><tr><td>DoseFromDate</td><td>" + medicalScriptObj.DoseFromDate;
                trs += "</td></tr><tr><td>DoseToDate</td><td>" + medicalScriptObj.DoseToDate;
                trs += "</td></tr><tr><td>DoseTotalDays</td><td>" + medicalScriptObj.DoseTotalDays;
                trs += "</td></tr><tr><td>DoseTotalAmount</td><td>" + medicalScriptObj.DoseTotalAmount;
                trs += "</td></tr><tr><td>PharmacistNurseNotes</td><td>" + medicalScriptObj.PharmacistNurseNotes;
                trs += "</td></tr><tr><td>PharmacistNurseNotesDays</td><td>" + medicalScriptObj.PharmacistNurseNotesDays;
                trs += "</td></tr><tr><td>MD</td><td>" + medicalScriptObj.MD;
                trs += "</td></tr><tr><td>Verification</td><td>" + medicalScriptObj.Verification;
                trs += "</td></tr><tr><td>HealthCard</td><td>" + medicalScriptObj.HealthCard;
                
                trs += "</td></tr>";
                $('#tbl_Script').empty();
                $('#tbl_Script').append(trs);
                $('#btn_Upload').show();
                $('#img_Loading').hide();
            },
            error: function (result) {
                alert(result);
                $('#btn_Upload').show();
            }
        })

    }
)});
