using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System;

namespace Omnitech.Dal.AdoNet
{
    public class PharmacyInvoiceSalesLogsRepository : SalesLogsRepository
    {
        private readonly string _connectionStrLogo;
        private readonly string _connectionStrIntegrlo;
        public PharmacyInvoiceSalesLogsRepository(IConfiguration configuration) : base(configuration)
        {
            _connectionStrLogo = configuration["ConnectionStrings:MyConnection"];
            _connectionStrIntegrlo = configuration["ConnectionStrings:IntegrloConnection"];
        }

        public async Task SendKassaAsync(int anbar, DateTime tarix, string accessToken, string url, string faktura, int chekSayi,double mebleg)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStrIntegrlo))
            {
                connection.Open();
                using (SqlCommand sqlCommand = new SqlCommand("TPS575_SEND_KASSA_NEW", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    SqlParameter anbarParameter = new SqlParameter();
                    anbarParameter.ParameterName = "@ANBAR";
                    anbarParameter.SqlDbType = SqlDbType.Int;
                    anbarParameter.Value = anbar;

                    SqlParameter tarixParameter = new SqlParameter();
                    tarixParameter.ParameterName = "@TARIX";
                    tarixParameter.SqlDbType = SqlDbType.DateTime;
                    tarixParameter.Value = tarix;

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

                    SqlParameter chekSayiParameter = new SqlParameter();
                    chekSayiParameter.ParameterName = "@CHEK_SAYI";
                    chekSayiParameter.SqlDbType = SqlDbType.Int;
                    chekSayiParameter.Value = chekSayi;

                    SqlParameter meblegParameter = new SqlParameter();
                    meblegParameter.ParameterName = "@MEBLEG";
                    meblegParameter.SqlDbType = SqlDbType.Float;
                    meblegParameter.Value = mebleg;

                    sqlCommand.Parameters.Add(anbarParameter);
                    sqlCommand.Parameters.Add(tarixParameter);
                    sqlCommand.Parameters.Add(accessTokenParameter);
                    sqlCommand.Parameters.Add(urlParameter);
                    sqlCommand.Parameters.Add(fakturaParameter);
                    sqlCommand.Parameters.Add(chekSayiParameter);
                    sqlCommand.Parameters.Add(meblegParameter);

                    await sqlCommand.ExecuteNonQueryAsync();

                    sqlCommand.Dispose();
                }
                connection.Close();
            }
        }

    }
}
