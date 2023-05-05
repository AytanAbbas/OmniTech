using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System;

namespace Omnitech.Dal.AdoNet
{
    public class KNInvoiceSalesLogsRepository : SalesLogsRepository
    {
        private readonly string _connectionStrLogo;
        private readonly string _connectionStrIntegrlo;
        public KNInvoiceSalesLogsRepository(IConfiguration configuration) : base(configuration)
        {
            _connectionStrLogo = configuration["ConnectionStrings:MyConnection"];
            _connectionStrIntegrlo = configuration["ConnectionStrings:IntegrloConnection"];
        }

        public async Task SendKassaAsync(int invId, double mebleg, string accessToken, string url, string faktura)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStrIntegrlo))
            {
                connection.Open();
                using (SqlCommand sqlCommand = new SqlCommand("TPS575_SEND_KASSA_001", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    SqlParameter invIdParameter = new SqlParameter();
                    invIdParameter.ParameterName = "@INV_ID";
                    invIdParameter.SqlDbType = SqlDbType.Int;
                    invIdParameter.Value = invId;

                    SqlParameter meblegParameter = new SqlParameter();
                    meblegParameter.ParameterName = "@MEBLEG";
                    meblegParameter.SqlDbType = SqlDbType.Float;
                    meblegParameter.Value = mebleg;

                    SqlParameter accessTokenParameter = new SqlParameter();
                    accessTokenParameter.ParameterName = "@ACCESS_TOKEN";
                    accessTokenParameter.SqlDbType = SqlDbType.NVarChar;
                    accessTokenParameter.Value = accessToken;

                    SqlParameter urlParameter = new SqlParameter();
                    urlParameter.ParameterName = "@URL";
                    urlParameter.SqlDbType = SqlDbType.NVarChar;
                    urlParameter.Value = url;

                    SqlParameter fakturaParameter = new SqlParameter();
                    fakturaParameter.ParameterName = "@FAKTURA";
                    fakturaParameter.SqlDbType = SqlDbType.NVarChar;
                    fakturaParameter.Value = faktura;

                    sqlCommand.Parameters.Add(invIdParameter);
                    sqlCommand.Parameters.Add(meblegParameter);
                    sqlCommand.Parameters.Add(accessTokenParameter);
                    sqlCommand.Parameters.Add(urlParameter);
                    sqlCommand.Parameters.Add(fakturaParameter);
                   

                    await sqlCommand.ExecuteNonQueryAsync();

                    sqlCommand.Dispose();
                }
                connection.Close();
            }
        }

    }
}
