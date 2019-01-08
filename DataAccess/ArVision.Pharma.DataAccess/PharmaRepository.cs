using ArVision.Core.Logging;
using ArVision.Pharma.Shared.DataModels;
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
        private const string DATA_BASE_FOLDER_PATH = ".\\..\\..\\Database\\";
        private const string CLASS_NAME = nameof(PharmaRepository);


        private SqliteDapperDbContext _databaseContext;

        public PharmaRepository()
        {
            string methodName = LogManager.GetCurrentMethodName(nameof(CLASS_NAME));

            var databaseFilePath = Path.Combine(DATA_BASE_FOLDER_PATH, "Database.sqlite");
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
        public List<Juice> GetJuiceList()
        {
            const string sql = @"
                SELECT
                  id as Id,
                  name as Name
                
                FROM Juice;
            ";

            var juiceList = _databaseContext.Query<Juice>(sql, null).ToList();

            return juiceList;
        }

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
