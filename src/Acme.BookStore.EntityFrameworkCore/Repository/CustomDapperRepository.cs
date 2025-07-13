using Acme.BookStore.IRepository;
using Acme.BookStore.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Acme.BookStore.Repository
{
    public class CustomDapperRepository: ICustomDapperRepository,ITransientDependency
    {
        private readonly IConfiguration _configuration;

        public CustomDapperRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IDbConnection CreateConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("Default"));
        }

        public async Task<string> GetActiveItemAsync()
        {
            const string sql = "SELECT top 1 Name FROM Authors";

            using (var connection = CreateConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<string>(sql);
                return result;
            }
        }
    }
}
