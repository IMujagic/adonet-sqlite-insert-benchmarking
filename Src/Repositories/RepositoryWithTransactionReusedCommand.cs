using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;

namespace Sqlite.Benchmarking.Repositories
{
    public class RepositoryWithTransactionReusedCommand : BaseRepository, IRepository
    {
        public RepositoryWithTransactionReusedCommand(string connectionString)
            : base(connectionString)
        {
        }

        public void Insert(List<SampleDataModel> models)
        {
            CreateEmptyDb();

            using var connection = new SqliteConnection(_connString);
            connection.Open();

            using var trans = connection.BeginTransaction();
            using var insertCommand = connection.CreateCommand();

            insertCommand.CommandText = @"INSERT INTO SampleTable VALUES(@Id, @Date, @IsActive, @LongText, @RandomNumber)";

            var IdParameter = insertCommand.CreateParameter();
            IdParameter.ParameterName = "@Id";
            insertCommand.Parameters.Add(IdParameter);

            var DateParameter = insertCommand.CreateParameter();
            DateParameter.ParameterName = "@Date";
            insertCommand.Parameters.Add(DateParameter);

            var IsActiveParameter = insertCommand.CreateParameter();
            IsActiveParameter.ParameterName = "@IsActive";
            insertCommand.Parameters.Add(IsActiveParameter);

            var LongTextParameter = insertCommand.CreateParameter();
            LongTextParameter.ParameterName = "@LongText";
            insertCommand.Parameters.Add(LongTextParameter);

            var RandomNumberParameter = insertCommand.CreateParameter();
            RandomNumberParameter.ParameterName = "@RandomNumber";
            insertCommand.Parameters.Add(RandomNumberParameter);

            foreach (var model in models)
            {
                IdParameter.Value =  model.Id;
                DateParameter.Value = model.Date;
                IsActiveParameter.Value = model.IsActive;
                LongTextParameter.Value = model.LongText;
                RandomNumberParameter.Value = model.RandomNumber;

                insertCommand.ExecuteNonQuery();
            }

            trans.Commit();
            connection.Close();
        }
    }
}
