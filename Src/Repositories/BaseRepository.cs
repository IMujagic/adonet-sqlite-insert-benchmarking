using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sqlite.Benchmarking.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly string _connString;
        protected readonly string _dbPath;

        public BaseRepository(string dbPath)
        {
            this._dbPath = dbPath;
            this._connString = string.Format("Data Source={0}", dbPath);
        }

        protected void CreateEmptyDb()
        {
            if (File.Exists(_dbPath))
                File.Delete(_dbPath);

            using var connection = new SqliteConnection(_connString);
            connection.Open();

            using var cmd = new SqliteCommand(CREATE_TABLE_COMMAND, connection);
         
            cmd.ExecuteNonQuery();
        }

        const string CREATE_TABLE_COMMAND = @"
CREATE TABLE SampleTable (Id INTEGER, Date TEXT, IsActive INTEGER, LongText TEXT, RandomNumber INTEGER)";
    }
}
