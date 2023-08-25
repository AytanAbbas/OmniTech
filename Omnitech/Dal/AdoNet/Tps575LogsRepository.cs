using Microsoft.Extensions.Configuration;
using Omnitech.Dal.AdoNet.Queries;
using Omnitech.Models;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Omnitech.Dal.AdoNet
{
    public class Tps575LogsRepository
    {
        private readonly string _connectionStrLogo;
        private readonly string _connectionStrIntegrlo;

        public Tps575LogsRepository(IConfiguration configuration)
        {
            _connectionStrLogo = configuration["ConnectionStrings:MyConnection"];
            _connectionStrIntegrlo = configuration["ConnectionStrings:IntegrloConnection"];
        }

        public async Task AddTps575LogsAsync(Tps575Logs tps575Logs)
        {
            await using (SqlConnection sqlConnection = new SqlConnection(_connectionStrIntegrlo))
            {
                await sqlConnection.OpenAsync();
                await using (SqlCommand sqlCommand = new SqlCommand(Tps575LogsQueries.AddTps575LogsQuery(tps575Logs), sqlConnection))
                {
                    await sqlCommand.ExecuteNonQueryAsync();

                    await sqlCommand.DisposeAsync();
                }

                await sqlConnection.CloseAsync();
            }
        }

    }
}
