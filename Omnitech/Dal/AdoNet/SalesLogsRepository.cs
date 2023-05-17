using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System;
using Omnitech.Models;
using System.Collections.Generic;
using Omnitech.Dal.AdoNet.Queries;

namespace Omnitech.Dal.AdoNet
{
    public class SalesLogsRepository
    {
        private readonly string _connectionStrLogo;
        private readonly string _connectionStrIntegrlo;

        public SalesLogsRepository(IConfiguration configuration)
        {
            _connectionStrLogo = configuration["ConnectionStrings:MyConnection"];
            _connectionStrIntegrlo = configuration["ConnectionStrings:IntegrloConnection"];
        }

        public async Task<List<SalesLogs>> GetSalesLogsByFakturaNameAsync(string fakturaName)
        {
            List<SalesLogs> salesLogsList = new List<SalesLogs>();
            await using (SqlConnection sqlConnection = new SqlConnection(_connectionStrIntegrlo))
            {
                await sqlConnection.OpenAsync();
                await using (SqlCommand sqlCommand = new SqlCommand(SaleLogsQueries.GetSalesLogsByFakturaNameQuery(fakturaName), sqlConnection))
                {
                    SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                    while (await sqlDataReader.ReadAsync())
                    {
                        SalesLogs salesLogs = new SalesLogs
                        {
                            RECNO = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("RECNO")) ? 0 : sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("RECNO")),
                            FAKTURA_NO = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("FAKTURA_NO")) ? "" : sqlDataReader.GetString(sqlDataReader.GetOrdinal("FAKTURA_NO")),
                            FAKTURA_NAME = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("FAKTURA_NAME")) ? "" : sqlDataReader.GetString(sqlDataReader.GetOrdinal("FAKTURA_NAME")),
                            REQUEST_TPS575 = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("REQUEST_TPS575")) ? "" : sqlDataReader.GetString(sqlDataReader.GetOrdinal("REQUEST_TPS575")),
                            RESPONSE_TPS575 = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("RESPONSE_TPS575")) ? "" : sqlDataReader.GetString(sqlDataReader.GetOrdinal("RESPONSE_TPS575")),
                            FICSAL_DOCUMENT = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("FICSAL_DOCUMENT")) ? "" : sqlDataReader.GetString(sqlDataReader.GetOrdinal("FICSAL_DOCUMENT")),
                            TIPI = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("TIPI")) ? 0 : sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("TIPI")),
                            INSERT_DATE = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("INSERT_DATE")) ? Convert.ToDateTime("01.01.1900") : sqlDataReader.GetDateTime(sqlDataReader.GetOrdinal("INSERT_DATE")),
                            PC_NAME = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("PC_NAME")) ? 0 : sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("PC_NAME")),
                            IP_REQUEST = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("IP_REQUEST")) ? "" : sqlDataReader.GetString(sqlDataReader.GetOrdinal("IP_REQUEST")),
                            FICSAL_DOCUMENT_LONG = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("FICSAL_DOCUMENT_LONG")) ? "" : sqlDataReader.GetString(sqlDataReader.GetOrdinal("FICSAL_DOCUMENT_LONG")),
                            FIRMA = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("FIRMA")) ? 0 : sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("FIRMA")),
                        };
                        salesLogsList.Add(salesLogs);
                    }
                    await sqlDataReader.DisposeAsync();

                    await sqlCommand.DisposeAsync();
                }

                await sqlConnection.CloseAsync();
            }
            return salesLogsList;
        }

        public async Task<List<SalesLogs>> GetAllSalesLogsForPrintAsync()
        {
            List<SalesLogs> salesLogsList = new List<SalesLogs>();
            await using (SqlConnection sqlConnection = new SqlConnection(_connectionStrIntegrlo))
            {
                await sqlConnection.OpenAsync();
                await using (SqlCommand sqlCommand = new SqlCommand(SaleLogsQueries.GetAllSalesLogsForPrintQuery(), sqlConnection))
                {
                    SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                    while (await sqlDataReader.ReadAsync())
                    {
                        SalesLogs salesLogs = new SalesLogs
                        {
                            RECNO = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("RECNO")) ? 0 : sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("RECNO")),
                            FAKTURA_NO = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("FAKTURA_NO")) ? "" : sqlDataReader.GetString(sqlDataReader.GetOrdinal("FAKTURA_NO")),
                            FAKTURA_NAME = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("FAKTURA_NAME")) ? "" : sqlDataReader.GetString(sqlDataReader.GetOrdinal("FAKTURA_NAME")),
                            REQUEST_TPS575 = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("REQUEST_TPS575")) ? "" : sqlDataReader.GetString(sqlDataReader.GetOrdinal("REQUEST_TPS575")),
                            RESPONSE_TPS575 = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("RESPONSE_TPS575")) ? "" : sqlDataReader.GetString(sqlDataReader.GetOrdinal("RESPONSE_TPS575")),
                            FICSAL_DOCUMENT = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("FICSAL_DOCUMENT")) ? "" : sqlDataReader.GetString(sqlDataReader.GetOrdinal("FICSAL_DOCUMENT")),
                            TIPI = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("TIPI")) ? 0 : sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("TIPI")),
                            INSERT_DATE = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("INSERT_DATE")) ? Convert.ToDateTime("01.01.1900") : sqlDataReader.GetDateTime(sqlDataReader.GetOrdinal("INSERT_DATE")),
                            PC_NAME = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("PC_NAME")) ? 0 : sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("PC_NAME")),
                            IP_REQUEST = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("IP_REQUEST")) ? "" : sqlDataReader.GetString(sqlDataReader.GetOrdinal("IP_REQUEST")),
                            FICSAL_DOCUMENT_LONG = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("FICSAL_DOCUMENT_LONG")) ? "" : sqlDataReader.GetString(sqlDataReader.GetOrdinal("FICSAL_DOCUMENT_LONG")),
                            FIRMA = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("FIRMA")) ? 0 : sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("FIRMA")),
                        };
                        salesLogsList.Add(salesLogs);
                    }
                    await sqlDataReader.DisposeAsync();

                    await sqlCommand.DisposeAsync();
                }

                await sqlConnection.CloseAsync();
            }
            return salesLogsList;
        }

        public async Task UpdateResponseAsync(int recno, string json)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStrIntegrlo))
            {
                connection.Open();
                using (SqlCommand sqlCommand = new SqlCommand("TPS575_UPDATE_RESPONSE", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    SqlParameter recnoParameter = new SqlParameter();
                    recnoParameter.ParameterName = "@RECNO";
                    recnoParameter.SqlDbType = SqlDbType.Int;
                    recnoParameter.Value = recno;


                    SqlParameter jsonParameter = new SqlParameter();
                    jsonParameter.ParameterName = "@json";
                    jsonParameter.SqlDbType = SqlDbType.NVarChar;
                    jsonParameter.Value = json;



                    sqlCommand.Parameters.Add(recnoParameter);
                    sqlCommand.Parameters.Add(jsonParameter);


                    await sqlCommand.ExecuteNonQueryAsync();

                    sqlCommand.Dispose();
                }
                connection.Close();
            }
        }

        public async Task AddLogAsync(int recno,string invName,string request, string json)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStrIntegrlo))
            {
                connection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(SaleLogsQueries.AddLogQuery(recno,invName,request, json), connection))
                {
                    await sqlCommand.ExecuteNonQueryAsync();

                    sqlCommand.Dispose();
                }
                connection.Close();
            }
        }

        public async Task<SalesLogs> GetSalesLogsByRecnoAsync(int recno)
        {
            SalesLogs salesLogs = null;
            await using (SqlConnection sqlConnection = new SqlConnection(_connectionStrIntegrlo))
            {
                await sqlConnection.OpenAsync();
                await using (SqlCommand sqlCommand = new SqlCommand(SaleLogsQueries.GetSalesLogsByRecnoAsync(recno), sqlConnection))
                {
                    SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                    while (await sqlDataReader.ReadAsync())
                    {
                        salesLogs = new SalesLogs
                        {
                            RECNO = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("RECNO")) ? 0 : sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("RECNO")),
                            FAKTURA_NO = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("FAKTURA_NO")) ? "" : sqlDataReader.GetString(sqlDataReader.GetOrdinal("FAKTURA_NO")),
                            FAKTURA_NAME = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("FAKTURA_NAME")) ? "" : sqlDataReader.GetString(sqlDataReader.GetOrdinal("FAKTURA_NAME")),
                            REQUEST_TPS575 = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("REQUEST_TPS575")) ? "" : sqlDataReader.GetString(sqlDataReader.GetOrdinal("REQUEST_TPS575")),
                            RESPONSE_TPS575 = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("RESPONSE_TPS575")) ? "" : sqlDataReader.GetString(sqlDataReader.GetOrdinal("RESPONSE_TPS575")),
                            FICSAL_DOCUMENT = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("FICSAL_DOCUMENT")) ? "" : sqlDataReader.GetString(sqlDataReader.GetOrdinal("FICSAL_DOCUMENT")),
                            TIPI = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("TIPI")) ? 0 : sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("TIPI")),
                            INSERT_DATE = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("INSERT_DATE")) ? Convert.ToDateTime("01.01.1900") : sqlDataReader.GetDateTime(sqlDataReader.GetOrdinal("INSERT_DATE")),
                            PC_NAME = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("PC_NAME")) ? 0 : sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("PC_NAME")),
                            IP_REQUEST = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("IP_REQUEST")) ? "" : sqlDataReader.GetString(sqlDataReader.GetOrdinal("IP_REQUEST")),
                            FICSAL_DOCUMENT_LONG = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("FICSAL_DOCUMENT_LONG")) ? "" : sqlDataReader.GetString(sqlDataReader.GetOrdinal("FICSAL_DOCUMENT_LONG")),
                            FIRMA = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("FIRMA")) ? 0 : sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("FIRMA")),
                        };
                    }
                    await sqlDataReader.DisposeAsync();

                    await sqlCommand.DisposeAsync();
                }

                await sqlConnection.CloseAsync();
            }
            return salesLogs;
        }

        public async Task<bool> InvoiceHasExists(string ficheno)
        {
            bool invHasExist = true;
            await using (SqlConnection sqlConnection = new SqlConnection(_connectionStrIntegrlo))
            {
                await sqlConnection.OpenAsync();
                await using (SqlCommand sqlCommand = new SqlCommand(SaleLogsQueries.InvoiceHasExistsQuery(ficheno), sqlConnection))
                {
                    SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                    while (await sqlDataReader.ReadAsync())
                    {
                        invHasExist = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("invHasExist")) ? true : sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("invHasExist"));
                    }
                    await sqlDataReader.DisposeAsync();

                    await sqlCommand.DisposeAsync();
                }

                await sqlConnection.CloseAsync();
            }
            return invHasExist;
        }


        public async Task<List<SalesLogs>> GetProblemicSalesLogsForPrintAsync()
        {
            List<SalesLogs> salesLogsList = new List<SalesLogs>();
            await using (SqlConnection sqlConnection = new SqlConnection(_connectionStrIntegrlo))
            {
                await sqlConnection.OpenAsync();
                await using (SqlCommand sqlCommand = new SqlCommand(SaleLogsQueries.GetProblemicSalesLogsQuery(), sqlConnection))
                {
                    SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                    while (await sqlDataReader.ReadAsync())
                    {
                        SalesLogs salesLogs = new SalesLogs
                        {
                            RECNO = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("RECNO")) ? 0 : sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("RECNO")),
                            FAKTURA_NO = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("FAKTURA_NO")) ? "" : sqlDataReader.GetString(sqlDataReader.GetOrdinal("FAKTURA_NO")),
                            RESPONSE_TPS575 = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("RESPONSE_TPS575")) ? "" : sqlDataReader.GetString(sqlDataReader.GetOrdinal("RESPONSE_TPS575")),
                            QEBZMEBLEG = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("QEBZMEBLEG")) ? 0 : sqlDataReader.GetDouble(sqlDataReader.GetOrdinal("QEBZMEBLEG")),
                            FICSAL_DOCUMENT = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("FICSAL_DOCUMENT")) ? "" : sqlDataReader.GetString(sqlDataReader.GetOrdinal("FICSAL_DOCUMENT")),
                            INSERT_DATE = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("INSERT_DATE")) ? Convert.ToDateTime("01.01.1900") : sqlDataReader.GetDateTime(sqlDataReader.GetOrdinal("INSERT_DATE")),
                            FIRMA = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("FIRMA")) ? 0 : sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("FIRMA")),

                        };
                        salesLogsList.Add(salesLogs);

                    }
                    await sqlDataReader.DisposeAsync();

                    await sqlCommand.DisposeAsync();
                }

                await sqlConnection.CloseAsync();
            }
            return salesLogsList;
        }

        //public async Task UpdateProblemicResponseAsync(string fakturaNo)
        //{
        //   SalesLogs salesLogsList = new SalesLogs();
        //    await using (SqlConnection sqlConnection = new SqlConnection(_connectionStrIntegrlo))
        //    {
        //        await sqlConnection.OpenAsync();
        //        await using (SqlCommand sqlCommand = new SqlCommand(SaleLogsQueries.ChangeProblemicSalesLogsQuery(fakturaNo), sqlConnection))
        //        {
        //            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
        //            while (await sqlDataReader.ReadAsync())
        //            {
        //                SalesLogs salesLogs = new SalesLogs
        //                {
        //                    FAKTURA_NO = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("FAKTURA_NO")) ? "" : sqlDataReader.GetString(sqlDataReader.GetOrdinal("FAKTURA_NO")),
        //                    RESPONSE_TPS575 = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("RESPONSE_TPS575")) ? "" : sqlDataReader.GetString(sqlDataReader.GetOrdinal("RESPONSE_TPS575")),
        //                    QEBZMEBLEG = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("QEBZMEBLEG")) ? 0 : sqlDataReader.GetDouble(sqlDataReader.GetOrdinal("QEBZMEBLEG")),
        //                    FICSAL_DOCUMENT = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("FICSAL_DOCUMENT")) ? "" : sqlDataReader.GetString(sqlDataReader.GetOrdinal("FICSAL_DOCUMENT")),
        //                    INSERT_DATE = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("INSERT_DATE")) ? Convert.ToDateTime("01.01.1900") : sqlDataReader.GetDateTime(sqlDataReader.GetOrdinal("INSERT_DATE")),
        //                    FIRMA = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("FIRMA")) ? 0 : sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("FIRMA")),

        //                };
        //                salesLogsList.Add(salesLogs);

        //            }
        //            await sqlDataReader.DisposeAsync();

        //            await sqlCommand.DisposeAsync();
        //        }

        //        await sqlConnection.CloseAsync();
        //    }
        //    return salesLogsList;
        //}
    }
}
