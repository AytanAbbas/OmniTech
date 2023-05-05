using Microsoft.Extensions.Configuration;
using Omnitech.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System;
using Omnitech.Dal.AdoNet.Queries;

namespace Omnitech.Dal.AdoNet
{
    public class Tps575UrlRepository
    {
        private readonly string _connectionStrLogo;
        private readonly string _connectionStrIntegrlo;

        public Tps575UrlRepository(IConfiguration configuration)
        {
            _connectionStrLogo = configuration["ConnectionStrings:MyConnection"];
            _connectionStrIntegrlo = configuration["ConnectionStrings:IntegrloConnection"];
        }


        public async Task<Tps575Url> GetActiveUrlAsync()
        {
            Tps575Url tps575Url = null;

            try
            {
                await using (SqlConnection connection = new SqlConnection(_connectionStrIntegrlo))
                {
                    await connection.OpenAsync();

                    await using (SqlCommand sqlCommand = new SqlCommand(Tps575UrlQueries.GetActiveUrlQuery, connection))
                    {
                        SqlDataReader dataReader = await sqlCommand.ExecuteReaderAsync();
                        while (await dataReader.ReadAsync())
                        {
                            tps575Url = new Tps575Url()
                            {
                                 ID = dataReader.IsDBNull(dataReader.GetOrdinal("ID")) ? 0 : dataReader.GetInt32(dataReader.GetOrdinal("ID")),
                                 URL = dataReader.IsDBNull(dataReader.GetOrdinal("URL")) ? "" : dataReader.GetString(dataReader.GetOrdinal("URL")),
                                 ACTIVE = dataReader.IsDBNull(dataReader.GetOrdinal("ACTIVE")) ? 0 : dataReader.GetInt16(dataReader.GetOrdinal("ACTIVE")),
                            };
                        }

                        await dataReader.DisposeAsync();
                        await sqlCommand.DisposeAsync();
                    }
                    await connection.CloseAsync();
                }
            }
            catch (Exception exp)
            {
                throw new Exception(exp.Message);
            }

            return tps575Url;
        }
    }
}
