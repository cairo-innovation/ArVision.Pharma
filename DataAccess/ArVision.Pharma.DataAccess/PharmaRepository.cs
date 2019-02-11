using ArVision.Core.Logging;
using ArVision.Pharma.Shared.DataModels;
using ArVision.Service.Pharma.Shared.DTO;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArVision.Pharma.DataAccess
{
    internal class TransactionBlock
    {
        public string SqlStatement { get; set; }
        public object SqlParameters { get; set; }
    }

    public class PharmaRepository : IPharmaRepository
    {
        //private const string DATA_BASE_FOLDER_PATH = @"D:\work\Assem\Pharma\Ver2\SampleService_ver2\ArVision.Pharma.Api\Database";//".\\..\\..\\Database\\";
        private const string DATA_BASE_FOLDER_PATH = ".\\..\\..\\Database\\";
        private const string CLASS_NAME = nameof(PharmaRepository);


        private SqliteDapperDbContext _databaseContext;

        public PharmaRepository()
        {
            //var s = Path.GetFullPath(DATA_BASE_FOLDER_PATH);
            string methodName = LogManager.GetCurrentMethodName(nameof(CLASS_NAME));

            var databaseFilePath = Path.Combine(DATA_BASE_FOLDER_PATH, "Database.sqlite");
            //var databaseFilePath = Path.Combine(DBFolderPath, DBFileName);
            var path = Path.GetDirectoryName(databaseFilePath);

            if (!Directory.Exists(path))
            {
                LogManager.Logger.Trace($@"{methodName}: Directory created: ({path})");
                Directory.CreateDirectory(path);
            }

            var builder = new SqliteConnectionStringBuilder
            {
                DataSource = databaseFilePath,
            };
            var connectionString = builder.ConnectionString;
            SqliteDapperDbContext databaseContext = new SqliteDapperDbContext(connectionString);
            _databaseContext = databaseContext;
        }
        public PharmaRepository(string DBFolderPath, string DBFileName)
        {
            //var s = Path.GetFullPath(DATA_BASE_FOLDER_PATH);
            string methodName = LogManager.GetCurrentMethodName(nameof(CLASS_NAME));

            //var databaseFilePath = Path.Combine(DATA_BASE_FOLDER_PATH, "Database.sqlite");
            var databaseFilePath = Path.Combine(DBFolderPath, DBFileName);
            var path = Path.GetDirectoryName(databaseFilePath);

            if (!Directory.Exists(path))
            {
                LogManager.Logger.Trace($@"{methodName}: Directory created: ({path})");
                Directory.CreateDirectory(path);
            }

            var builder = new SqliteConnectionStringBuilder
            {
                DataSource = databaseFilePath,
            };
            var connectionString = builder.ConnectionString;
            SqliteDapperDbContext databaseContext = new SqliteDapperDbContext(connectionString);
            _databaseContext = databaseContext;
        }
        public List<LookUpDto> GetList(string table)
        {
            string sql = string.Format(@"
                SELECT
                  Id as Id,
                  Name as Name
                
                FROM {0};
            ", table);

            var List = _databaseContext.Query<LookUpDto>(sql, null).ToList();

            return List;
        }
        public PatientDto GetPatientWithRX(int id)
        {
            string patientsql = @"
                                select
                                    *
                                from Patient
                                where Id=@pid;
                                ";
            var patientparam = new { pid = id };
            var patient = _databaseContext.Query<PatientDto>(patientsql, patientparam).FirstOrDefault();
            var lastrxsql = @"select * from RX where PatientId=@pid order by CreatedDate LIMIT 1";
            var lastrx = _databaseContext.Query<RXDto>(lastrxsql, patientparam).FirstOrDefault();
            patient.RX = new List<RXDto>();
            if (lastrx != null) patient.RX.Add(lastrx);
            var last5rxsql = @"select id, createdDate from RX where PatientId=@pid order by CreatedDate LIMIT 5";
            var last5rx = _databaseContext.Query<RXDto>(last5rxsql, patientparam).ToList();
            if (last5rx.Count != 0)
            {
                last5rx.RemoveAt(0);
                for (int i = 0; i < last5rx.Count; i++)
                {
                    patient.RX.Add(last5rx[i]);
                }
            }
            return patient;
        }
        public List<Juices> GetJuiceList()
        {
            const string sql = @"
                SELECT
                  Id as Id,
                  JuiceName as JuiceName
                
                FROM Juice;
            ";

            var juiceList = _databaseContext.Query<Juices>(sql, null).ToList();

            return juiceList;
        }
        public List<Juices> GetDoctorList()
        {
            const string sql = @"
                SELECT
                  Id as Id,
                  DoctorName as DoctorName
                
                FROM Doctor;
            ";

            var juiceList = _databaseContext.Query<Juices>(sql, null).ToList();

            return juiceList;
        }
        public PatientDto AddPatient(PatientDto patient)
        {
            //check if name of patient exist
            var sql = "SELECT COUNT(*) FROM patient WHERE replace(trim(lower(Name)),' ','')=replace(trim(lower(@_name)),' ','')";
            var param = new { _name = patient.Name };
            var count = _databaseContext.Query<int>(sql, param).First();
            if (count>0)
            {
                return null;
            }
            using (var transaction = _databaseContext.BeginTransaction())
            {
                var createPatientSql = @"
                                        INSERT INTO patient (Name, DOB, Address, Phone, DoctorId, PatientIMG, PatientIdenficationIMG, CreatedUser)
                                        VALUES (@_name, @_dob, @_address, @_phone, @_doctorid, @_patientimg, @_patientidentificationimg, @_createduser)
                                        ;
                                        SELECT last_insert_rowid();";
                var createPatientSqlParams = new
                {
                    _name = patient.Name,
                    _dob = patient.DOB,
                    _address = patient.Address,
                    _phone = patient.Phone,
                    _doctorid = patient.DoctorId,
                    _patientimg = patient.PatientIMG,
                    _patientidentificationimg = patient.PatientIdenficationIMG,
                    _createduser = patient.CreatedUser,
                };
                var patientId = _databaseContext.Query<int>(createPatientSql, createPatientSqlParams).First();
                patient.Id = patientId;
                var createRXSql = @"
                                        INSERT INTO RX (StartDate, EndDate, PatientId, MedicineId, Dose, JuiceId, IMG, Notes, SunDay, MonDay, TusDay, WedDay, ThrDay, FriDay, SatDay,CreatedUser)
                                        VALUES (@_startdate, @_enddate, @_patientid, @_medicineid, @_dose, @_juiceid, @_img, @_notes, @_sun, @_mon, @_tus, @_wed, @_thr, @_fri, @_sat, @_createduser)
                                        ;
                                        SELECT last_insert_rowid();";
                var createRXSqlParams = new
                {
                    _startdate = patient.RX[0].StartDate,
                    _enddate = patient.RX[0].EndDate,
                    _patientid = patient.Id,
                    _medicineid = patient.RX[0].MedicineId,
                    _dose = patient.RX[0].Dose,
                    _juiceid = patient.RX[0].JuiceId,
                    _img = patient.RX[0].IMG,
                    _notes = patient.RX[0].Notes,
                    _sun = patient.RX[0].SunDay,
                    _mon = patient.RX[0].MonDay,
                    _tus = patient.RX[0].TusDay,
                    _wed = patient.RX[0].WedDay,
                    _thr = patient.RX[0].ThrDay,
                    _fri = patient.RX[0].FriDay,
                    _sat = patient.RX[0].SatDay,
                    _createduser = patient.RX[0].CreatedUser,
                };
                var rxId = _databaseContext.Query<int>(createRXSql, createRXSqlParams).First();
                patient.RX[0].Id = rxId;
                transaction.Commit();
            }
            return patient;
        }
        public PatientDto EditPatient(PatientDto patient)
        {
            var createPatientSql = @"
                                        UPDATE patient SET Name=@_name, DOB=@_dob, Address=@_address, Phone=@_phone, DoctorId=@_doctorid, PatientIMG=@_patientimg, PatientIdenficationIMG=@_patientidentificationimg, UpdatedUser=@_updateduser,UpdatedDate=@_updateddate
                                        where id=@_id;
                                        SELECT id from patient where id=@_id;";
            var createPatientSqlParams = new
            {
                _id=patient.Id,
                _name = patient.Name,
                _dob = patient.DOB,
                _address = patient.Address,
                _phone = patient.Phone,
                _doctorid = patient.DoctorId,
                _patientimg = patient.PatientIMG,
                _patientidentificationimg = patient.PatientIdenficationIMG,
                _updateduser = patient.UpdatedUser,
                _updateddate = DateTime.Now,
            };
            var patientId = _databaseContext.Query<int>(createPatientSql, createPatientSqlParams).First();
            var createRXSql = @"
                                        UPDATE RX SET StartDate=@_startdate, EndDate=@_enddate, MedicineId=@_medicineid, Dose=@_dose, JuiceId=@_juiceid, IMG=@_img, Notes=@_notes, SunDay=@_sun, MonDay=@_mon, TusDay=@_tus, WedDay=@_wed, ThrDay=@_thr, FriDay=@_fri, SatDay=@_sat,UpdatedUser=@_updateduser,UpdatedDate=@_updateddate
                                        WHERE PatientId=@_patientid and Id=@_id;
                                        SELECT id from rx where id=@_id;";
            var createRXSqlParams = new
            {
                _id = patient.RX[0].Id,
                _startdate = patient.RX[0].StartDate,
                _enddate = patient.RX[0].EndDate,
                _patientid = patient.Id,
                _medicineid = patient.RX[0].MedicineId,
                _dose = patient.RX[0].Dose,
                _juiceid = patient.RX[0].JuiceId,
                _img = patient.RX[0].IMG,
                _notes = patient.RX[0].Notes,
                _sun = patient.RX[0].SunDay,
                _mon = patient.RX[0].MonDay,
                _tus = patient.RX[0].TusDay,
                _wed = patient.RX[0].WedDay,
                _thr = patient.RX[0].ThrDay,
                _fri = patient.RX[0].FriDay,
                _sat = patient.RX[0].SatDay,
                _updateduser = patient.UpdatedUser,
                _updateddate = DateTime.Now,
            };
            var rxId = _databaseContext.Query<int>(createRXSql, createRXSqlParams).First();
            return patient;
        }
        public RXDto AddRXToPatient(RXDto rx)
        {
            using (var transaction = _databaseContext.BeginTransaction())
            {
                var createRXSql = @"
                                        INSERT INTO RX (StartDate, EndDate, PatientId, MedicineId, Dose, JuiceId, IMG, Notes, SunDay, MonDay, TusDay, WedDay, ThrDay, FriDay, SatDay,CreatedUser)
                                        VALUES (@_startdate, @_enddate, @_patientid, @_medicineid, @_dose, @_juiceid, @_img, @_notes, @_sun, @_mon, @_tus, @_wed, @_thr, @_fri, @_sat, @_createduser)
                                        ;
                                        SELECT last_insert_rowid();";
                var createRXSqlParams = new
                {
                    _startdate = rx.StartDate,
                    _enddate = rx.EndDate,
                    _patientid = rx.PatientId,
                    _medicineid = rx.MedicineId,
                    _dose = rx.Dose,
                    _juiceid = rx.JuiceId,
                    _img = rx.IMG,
                    _notes = rx.Notes,
                    _sun = rx.SunDay,
                    _mon = rx.MonDay,
                    _tus = rx.TusDay,
                    _wed = rx.WedDay,
                    _thr = rx.ThrDay,
                    _fri = rx.FriDay,
                    _sat = rx.SatDay,
                    _createduser = rx.CreatedUser,
                };
                var rxId = _databaseContext.Query<int>(createRXSql, createRXSqlParams).First();
                rx.Id = rxId;
                transaction.Commit();
            }
            return rx;
        }
        public VisitDto AddVisitToPatient(VisitDto visit)
        {
            using (var transaction = _databaseContext.BeginTransaction())
            {
                var createVisitSql = @"
                                        INSERT INTO VISIT (PatientId, RXId, UserId, CreatedUser, CreatedDate)
                                        VALUES (@_patientid, @_rxid, @_userid,  @_createduser,@_createddate)
                                        ;
                                        SELECT last_insert_rowid();";
                var createVisitSqlParams = new
                {
                    _patientid = visit.PatientId,
                    _rxid = visit.RXId,
                    _userid = visit.UserId,
                    _createddate = DateTime.Now,
                    _createduser = visit.CreatedUser,
                };
                var visitId = _databaseContext.Query<int>(createVisitSql, createVisitSqlParams).First();
                visit.Id = visitId;
                transaction.Commit();
            }
            return visit;
        }
        //public List<VisitDto> GetAllVisits()
        //{
        //    var sql = "select * from visits";
        //}
        /*
            public ModuleDto Insert(ModuleDto moduleDto)
            {
                var sql = @"
                INSERT INTO module (module_id, is_available, created_on)
                VALUES (@moduleid, @isAvailable, @createdOn);

                SELECT last_insert_rowid();
            ";

                var sqlParams = new
                {
                    moduleId = moduleDto.ModuleId,
                    isAvailable = moduleDto.IsAvailable,
                    createdOn = DateTime.UtcNow
                };

                _databaseContext.Query<int>(sql, sqlParams);
                BackupUpdate(sql, sqlParams);

                return moduleDto;
            }

            public AreaDto AssignAreaToModule(AreaDto areaDto)
            {
                var sql = @"
                INSERT INTO area (module_id, area_number, side, is_load, x_position, y_position, width, height, created_on)
                VALUES (@moduleId, @areaNumber, @moduleSide, @isLoad, @xPosition, @yPosition, @width, @height, @createdOn);

                SELECT last_insert_rowid();
            ";

                var sqlParams = new
                {
                    moduleId = areaDto.ModuleId,
                    areaNumber = areaDto.AreaNumber,
                    moduleSide = areaDto.Side.ToString(),
                    isLoad = areaDto.IsLoad,
                    xPosition = areaDto.XPosition,
                    yPosition = areaDto.YPosition,
                    width = areaDto.Width,
                    height = areaDto.Height,
                    createdOn = DateTime.UtcNow
                };

                var areaId = _databaseContext.Query<int>(sql, sqlParams).First();
                BackupUpdate(sql, sqlParams);

                areaDto.AreaId = areaId;

                return areaDto;
            }

            public void Remove(int binId)
            {
                var sql = @"
                BEGIN TRANSACTION;

                DELETE
                FROM slot
                WHERE slot.bin_id = @binId;

                DELETE 
                FROM bin
                WHERE bin_id = @binId;

                COMMIT;
            ";

                var sqlParams = new
                {
                    binId = binId
                };

                _databaseContext.Execute(sql, sqlParams);
                BackupUpdate(sql, sqlParams);
            }

            private TransientLocationDto AddTransientLocationDto(TransientLocationDto transientLocationDto, string transientLocationType)
            {

                using (var transaction = _databaseContext.BeginTransaction())
                {
                    List<TransactionBlock> backupTransactionBlocks = new List<TransactionBlock>();

                    var createLocationSql = @"
                    INSERT INTO location (location_type_id, created_on)
                    VALUES (
                      (SELECT location_type_id FROM location_type WHERE name = 'Transient'),
                      @createdOn
                    );

                    SELECT last_insert_rowid();
                ";

                    var createLocationSqlParams = new
                    {
                        createdOn = DateTime.UtcNow
                    };

                    var locationId = _databaseContext.Query<int>(createLocationSql, createLocationSqlParams).First();

                    backupTransactionBlocks.Add(new TransactionBlock
                    {
                        SqlStatement = createLocationSql,
                        SqlParameters = createLocationSqlParams
                    });

                    transientLocationDto.LocationId = locationId;

                    var createTransientLocationSql = @"
                    INSERT INTO transient_location (location_id, transient_location_type_id, identifier, module_id, side, is_available, created_on)
                    VALUES (
                      @locationId,
                      (SELECT transient_location_type_id FROM transient_location_type WHERE name = @transientLocationType),
                      @transientLocationIdentifier,
                      @moduleId,
                      @side,
                      @isAvailable,
                      @createdOn
                    );
                ";

                    var createTransientLocationSqlParams = new
                    {
                        createdOn = DateTime.UtcNow,
                        locationId = locationId,
                        transientLocationIdentifier = transientLocationDto.Identifier,
                        moduleId = transientLocationDto.ModuleId,
                        side = transientLocationDto.Side?.ToString(),
                        isAvailable = transientLocationDto.IsAvailable,
                        transientLocationType = transientLocationType
                    };

                    _databaseContext.Execute(createTransientLocationSql, createTransientLocationSqlParams);
                    backupTransactionBlocks.Add(new TransactionBlock
                    {
                        SqlStatement = createTransientLocationSql,
                        SqlParameters = createTransientLocationSqlParams
                    });

                    transaction.Commit();

                    backupTransactionBlocks.ForEach(p => BackupUpdate(p.SqlStatement, p.SqlParameters));
                }

                return transientLocationDto;
            }
*/


        public void Dispose()
        {
            _databaseContext?.Dispose();
        }

        public List<Patient> GetPatientList()
        {
            const string sql = @"
                SELECT
                  id as Id,
                  name as Name
                
                FROM Patient;
            ";

            var patientList = _databaseContext.Query<Patient>(sql, null).ToList();

            return patientList;
        }
    }

}
