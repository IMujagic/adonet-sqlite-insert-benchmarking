using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;

namespace Sqlite.Benchmarking.Repositories
{
    public class RepositoryWithTransaction : BaseRepository, IRepository
    {
        public RepositoryWithTransaction(string connectionString)
            : base(connectionString)
        {
        }

        public void Insert(List<SampleDataModel> models)
        {
            CreateEmptyDb();

            using var connection = new SqliteConnection(_connString);
            connection.Open();

            using var trans = connection.BeginTransaction();

            foreach (var model in models)
            {
                using var insertCommand = connection.CreateCommand();

                insertCommand.CommandText = @"INSERT INTO SampleTable(Id, Date, IsActive, LongText, RandomNumber) VALUES(@Id, @Date, @IsActive, @LongText, @RandomNumber)";

                insertCommand.Parameters.AddWithValue("Id", model.Id);
                insertCommand.Parameters.AddWithValue("Date", model.Date);
                insertCommand.Parameters.AddWithValue("IsActive", model.IsActive);
                insertCommand.Parameters.AddWithValue("LongText", model.LongText);
                insertCommand.Parameters.AddWithValue("RandomNumber", model.RandomNumber);

                insertCommand.ExecuteNonQuery();
            }

            trans.Commit();
            connection.Close();
        }
    }
}
