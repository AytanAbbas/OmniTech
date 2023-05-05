using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System;
using Omnitech.Models;
using Omnitech.Dal.AdoNet.Queries;

namespace Omnitech.Dal.AdoNet
{
    public class PharmacyInvoiceRepository
    {
        private readonly string _connectionStrLogo;
        private readonly string _connectionStrIntegrlo;

        public PharmacyInvoiceRepository(IConfiguration configuration)
        {
            _connectionStrLogo = configuration["ConnectionStrings:MyConnection"];
            _connectionStrIntegrlo = configuration["ConnectionStrings:IntegrloConnection"];
        }

        public async Task<List<PharmacyInvoiceInfo>> GetPharmacyInvoiceInfosByStartDateAndEndDateAsync(DateTime startDate, DateTime endDate)
        {
            List<PharmacyInvoiceInfo> invoices = new List<PharmacyInvoiceInfo>();

            await using (SqlConnection connection = new SqlConnection(_connectionStrIntegrlo))
            {
                await connection.OpenAsync();
                await using (SqlCommand sqlCommand = new SqlCommand("TPS575_GET_FAKTURALAR_FROM_TIGER", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    SqlParameter startParameter = new SqlParameter();
                    startParameter.ParameterName = "@BEGIN_DATE";
                    startParameter.SqlDbType = SqlDbType.DateTime;
                    startParameter.Value = startDate;

                    SqlParameter endParameter = new SqlParameter();
                    endParameter.ParameterName = "@END_DATE";
                    endParameter.SqlDbType = SqlDbType.DateTime;
                    endParameter.Value = endDate;

                    sqlCommand.Parameters.Add(startParameter);
                    sqlCommand.Parameters.Add(endParameter);

                    SqlDataReader dataReader = await sqlCommand.ExecuteReaderAsync();

                    while (await dataReader.ReadAsync())
                    {
                        PharmacyInvoiceInfo invoice = new PharmacyInvoiceInfo
                        {
                            TARIX = dataReader.IsDBNull(dataReader.GetOrdinal("TARIX")) ? Convert.ToDateTime("01.01.1900") : dataReader.GetDateTime(dataReader.GetOrdinal("TARIX")),
                            ANBAR = dataReader.IsDBNull(dataReader.GetOrdinal("ANBAR")) ? 0 : dataReader.GetInt32(dataReader.GetOrdinal("ANBAR")),
                            FAKTURA = dataReader.IsDBNull(dataReader.GetOrdinal("FAKTURA")) ? "" : dataReader.GetString(dataReader.GetOrdinal("FAKTURA")),
                            SETR_SAY = dataReader.IsDBNull(dataReader.GetOrdinal("SETR_SAY")) ? 0 : dataReader.GetInt32(dataReader.GetOrdinal("SETR_SAY")),
                            CEMI_MAL_SAYI_IADE_CIXILMIS = dataReader.IsDBNull(dataReader.GetOrdinal("CEMI_MAL_SAYI_IADE_CIXILMIS")) ? 0 : dataReader.GetDouble(dataReader.GetOrdinal("CEMI_MAL_SAYI_IADE_CIXILMIS")),
                            CEMI_MEBLEG_IADE_CIXILMIS = dataReader.IsDBNull(dataReader.GetOrdinal("CEMI_MEBLEG_IADE_CIXILMIS")) ? 0 : dataReader.GetDouble(dataReader.GetOrdinal("CEMI_MEBLEG_IADE_CIXILMIS")),
                            IADE_MEBLEG_CEMI = dataReader.IsDBNull(dataReader.GetOrdinal("IADE_MEBLEG_CEMI")) ? 0 : dataReader.GetDouble(dataReader.GetOrdinal("IADE_MEBLEG_CEMI")),
                            IADE_MEBLEG_CEMI_GUNLUK_SATISH = dataReader.IsDBNull(dataReader.GetOrdinal("IADE_MEBLEG_CEMI_GUNLUK_SATISH")) ? 0 : dataReader.GetDouble(dataReader.GetOrdinal("IADE_MEBLEG_CEMI_GUNLUK_SATISH")),
                            QADAGA_SATISH = dataReader.IsDBNull(dataReader.GetOrdinal("QADAGA_SATISH")) ? 0 : dataReader.GetDouble(dataReader.GetOrdinal("QADAGA_SATISH")),
                            QADAGA_IADE = dataReader.IsDBNull(dataReader.GetOrdinal("QADAGA_IADE")) ? 0 : dataReader.GetDouble(dataReader.GetOrdinal("QADAGA_IADE")),
                            KASSA_GONDERILME = dataReader.IsDBNull(dataReader.GetOrdinal("KASSA_GONDERILME")) ? "" : dataReader.GetString(dataReader.GetOrdinal("KASSA_GONDERILME")),
                            APTEKIN_ADI = dataReader.IsDBNull(dataReader.GetOrdinal("APTEKIN_ADI")) ? "" : dataReader.GetString(dataReader.GetOrdinal("APTEKIN_ADI")),
                            fiscal = dataReader.IsDBNull(dataReader.GetOrdinal("fiscal")) ? "" : dataReader.GetString(dataReader.GetOrdinal("fiscal")),

                        };
                        invoices.Add(invoice);
                    }

                    await dataReader.DisposeAsync();
                    await sqlCommand.DisposeAsync();
                }
                await connection.CloseAsync();
            }
            return invoices;
        }

        public async Task<List<PharmacyInvoiceDetail>> GetPharmacyInvoiceDetailsBySourceIndexAndDateAsync(int sourceIndex, DateTime date)
        {
            List<PharmacyInvoiceDetail> pharmacyInvoiceDetails = new List<PharmacyInvoiceDetail>();

            try
            {
                await using (SqlConnection connection = new SqlConnection(_connectionStrIntegrlo))
                {
                    await connection.OpenAsync();

                    await using (SqlCommand sqlCommand = new SqlCommand("TPS575_GET_FAKTURA_DETAIL", connection))
                    {
                        SqlParameter anbarParameter = new SqlParameter();
                        anbarParameter.ParameterName = "@ANBAR";
                        anbarParameter.SqlDbType = SqlDbType.Int;
                        anbarParameter.Value = sourceIndex;

                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        SqlParameter dateParameter = new SqlParameter();
                        dateParameter.ParameterName = "@TARIX";
                        dateParameter.SqlDbType = SqlDbType.DateTime;
                        dateParameter.Value = date;


                        sqlCommand.Parameters.Add(anbarParameter);
                        sqlCommand.Parameters.Add(dateParameter);

                        SqlDataReader dataReader = await sqlCommand.ExecuteReaderAsync();
                        while (await dataReader.ReadAsync())
                        {
                            PharmacyInvoiceDetail pharmacyInvoiceDetail = new PharmacyInvoiceDetail()
                            {
                                LOGICALREF = dataReader.IsDBNull(dataReader.GetOrdinal("LOGICALREF")) ? 0 : dataReader.GetInt32(dataReader.GetOrdinal("LOGICALREF")),
                                SKU = dataReader.IsDBNull(dataReader.GetOrdinal("SKU")) ? 0 : dataReader.GetInt32(dataReader.GetOrdinal("SKU")),
                                TARIX = dataReader.IsDBNull(dataReader.GetOrdinal("TARIX")) ? Convert.ToDateTime("01.01.1900") : dataReader.GetDateTime(dataReader.GetOrdinal("TARIX")),
                                MIQDAR = dataReader.IsDBNull(dataReader.GetOrdinal("MIQDAR")) ? 0 : dataReader.GetDouble(dataReader.GetOrdinal("MIQDAR")),
                                MEBLEG = dataReader.IsDBNull(dataReader.GetOrdinal("MEBLEG")) ? 0 : dataReader.GetDouble(dataReader.GetOrdinal("MEBLEG")),
                                ANBAR = dataReader.IsDBNull(dataReader.GetOrdinal("ANBAR")) ? 0 : dataReader.GetInt32(dataReader.GetOrdinal("ANBAR")),
                                MEHSULUN_KODU = dataReader.IsDBNull(dataReader.GetOrdinal("MEHSULUN_KODU")) ? "" : dataReader.GetString(dataReader.GetOrdinal("MEHSULUN_KODU")),
                                MEHSULUN_ADI = dataReader.IsDBNull(dataReader.GetOrdinal("MEHSULUN_ADI")) ? "" : dataReader.GetString(dataReader.GetOrdinal("MEHSULUN_ADI")),
                                FIRMA = dataReader.IsDBNull(dataReader.GetOrdinal("FIRMA")) ? "" : dataReader.GetString(dataReader.GetOrdinal("FIRMA")),
                                ISTEHSALCHI = dataReader.IsDBNull(dataReader.GetOrdinal("ISTEHSALCHI")) ? "" : dataReader.GetString(dataReader.GetOrdinal("ISTEHSALCHI")),
                                BARCODE = dataReader.IsDBNull(dataReader.GetOrdinal("BARCODE")) ? "" : dataReader.GetString(dataReader.GetOrdinal("BARCODE")),
                                VAHID = dataReader.IsDBNull(dataReader.GetOrdinal("VAHID")) ? "" : dataReader.GetString(dataReader.GetOrdinal("VAHID")),
                                EDEDSAYI = dataReader.IsDBNull(dataReader.GetOrdinal("EDEDSAYI")) ? 0 : dataReader.GetInt32(dataReader.GetOrdinal("EDEDSAYI")),
                                DELETED = dataReader.IsDBNull(dataReader.GetOrdinal("DELETED")) ? 0 : dataReader.GetInt32(dataReader.GetOrdinal("DELETED")),

                            };

                            pharmacyInvoiceDetails.Add(pharmacyInvoiceDetail);
                        }

                        await dataReader.DisposeAsync();
                        await sqlCommand.DisposeAsync();
                    }
                    await connection.CloseAsync();
                }
            }
            catch (Exception exp)
            {
                string msg = exp.Message;
            }

            return pharmacyInvoiceDetails;
        }

        public async Task<List<FoodSupplementItem>> GetAllFoodSuppLementItemsAsync()
        {
            List<FoodSupplementItem> foodItems = new List<FoodSupplementItem>();

            await using (SqlConnection sqlConnection = new SqlConnection(_connectionStrLogo))
            {
                await sqlConnection.OpenAsync();
                await using (SqlCommand sqlCommand = new SqlCommand(PharmacyInvoiceQueries.GetAllFoodSupplementItemsQuery(), sqlConnection))
                {
                    SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

                    while (await sqlDataReader.ReadAsync())
                    {
                        FoodSupplementItem foodItem = new FoodSupplementItem
                        {
                            Logicalref = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("SKU")) ? 0 : sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("SKU")),
                            Code = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("KODU")) ? "" : sqlDataReader.GetString(sqlDataReader.GetOrdinal("KODU")),
                            Name = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("ADI")) ? "" : sqlDataReader.GetString(sqlDataReader.GetOrdinal("ADI")),
                            Specode = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("FIRMA")) ? "" : sqlDataReader.GetString(sqlDataReader.GetOrdinal("FIRMA")),
                            Stgrpcode = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("ISTEHSALCHI")) ? "" : sqlDataReader.GetString(sqlDataReader.GetOrdinal("ISTEHSALCHI")),
                            Cyphcode = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("OLKE")) ? "" : sqlDataReader.GetString(sqlDataReader.GetOrdinal("OLKE")),
                            Price = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("QIYMET")) ? 0 : sqlDataReader.GetDouble(sqlDataReader.GetOrdinal("QIYMET")),

                        };


                        foodItems.Add(foodItem);
                    }
                    await sqlDataReader.DisposeAsync();
                    await sqlCommand.DisposeAsync();
                }

                await sqlConnection.CloseAsync();
            }
            return foodItems;
        }

        public async Task<double> GetFoodSupplementItemPriceByItemRefAsync(int itemRef)
        {
            double price = 0;

            await using (SqlConnection sqlConnection = new SqlConnection(_connectionStrLogo))
            {
                await sqlConnection.OpenAsync();
                await using (SqlCommand sqlCommand = new SqlCommand(PharmacyInvoiceQueries.GetFoodSupplementPriceByItemRefQuery(itemRef), sqlConnection))
                {

                    SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

                    while (await sqlDataReader.ReadAsync())
                    {
                        price = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("QIYMET")) ? 0 : sqlDataReader.GetDouble(sqlDataReader.GetOrdinal("QIYMET"));
                    }
                    await sqlDataReader.DisposeAsync();

                    await sqlCommand.DisposeAsync();
                }

                await sqlConnection.CloseAsync();
            }

            return price;
        }

        public async Task AddFoodSupplementItemAsync(int sku, DateTime date, int sourceIndex, double quantity, double price)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStrIntegrlo))
            {
                connection.Open();
                using (SqlCommand sqlCommand = new SqlCommand("ADD_TO_TOTAL_TEMP", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    SqlParameter skuParameter = new SqlParameter();
                    skuParameter.ParameterName = "@SKU";
                    skuParameter.SqlDbType = SqlDbType.Int;
                    skuParameter.Value = sku;

                    SqlParameter tarixParameter = new SqlParameter();
                    tarixParameter.ParameterName = "@TARIX";
                    tarixParameter.SqlDbType = SqlDbType.DateTime;
                    tarixParameter.Value = date;

                    SqlParameter anbarParameter = new SqlParameter();
                    anbarParameter.ParameterName = "@ANBAR";
                    anbarParameter.SqlDbType = SqlDbType.Int;
                    anbarParameter.Value = sourceIndex;

                    SqlParameter miqdarParameter = new SqlParameter();
                    miqdarParameter.ParameterName = "@MIQDAR";
                    miqdarParameter.SqlDbType = SqlDbType.Float;
                    miqdarParameter.Value = quantity;

                    SqlParameter qiymetParameter = new SqlParameter();
                    qiymetParameter.ParameterName = "@QIYMET";
                    qiymetParameter.SqlDbType = SqlDbType.Float;
                    qiymetParameter.Value = price;

                    sqlCommand.Parameters.Add(skuParameter);
                    sqlCommand.Parameters.Add(tarixParameter);
                    sqlCommand.Parameters.Add(anbarParameter);
                    sqlCommand.Parameters.Add(miqdarParameter);
                    sqlCommand.Parameters.Add(qiymetParameter);

                    await sqlCommand.ExecuteNonQueryAsync();

                    sqlCommand.Dispose();
                }
                connection.Close();
            }
        }

        public async Task<PharmacyInvoiceItemReplaceResponse> GetPharmacyInvoiceItemReplaceResponseByDateAndSourceIndex(int sourceIndex, DateTime datetime)
        {
            PharmacyInvoiceItemReplaceResponse reverseResponse = null;

            try
            {
                await using (SqlConnection connection = new SqlConnection(_connectionStrIntegrlo))
                {
                    await connection.OpenAsync();

                    await using (SqlCommand sqlCommand = new SqlCommand("TPS575_EVEZLEME_SELECT", connection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        SqlParameter dateParameter = new SqlParameter();
                        dateParameter.ParameterName = "@TARIX";
                        dateParameter.SqlDbType = SqlDbType.DateTime;
                        dateParameter.Value = datetime;

                        SqlParameter sourceIndexParameter = new SqlParameter();
                        sourceIndexParameter.ParameterName = "@ANBAR";
                        sourceIndexParameter.SqlDbType = SqlDbType.Int;
                        sourceIndexParameter.Value = sourceIndex;

                        sqlCommand.Parameters.Add(sourceIndexParameter);
                        sqlCommand.Parameters.Add(dateParameter);

                        SqlDataReader dataReader = await sqlCommand.ExecuteReaderAsync();
                        while (await dataReader.ReadAsync())
                        {
                            reverseResponse = new PharmacyInvoiceItemReplaceResponse()
                            {
                                SILINEN = dataReader.IsDBNull(dataReader.GetOrdinal("SILINEN")) ? 0 : dataReader.GetDouble(dataReader.GetOrdinal("SILINEN")),
                                FORMALASDIRILAN = dataReader.IsDBNull(dataReader.GetOrdinal("FORMALASDIRILAN")) ? 0 : dataReader.GetDouble(dataReader.GetOrdinal("FORMALASDIRILAN")),
                                FERQ = dataReader.IsDBNull(dataReader.GetOrdinal("FERQ")) ? 0 : dataReader.GetDouble(dataReader.GetOrdinal("FERQ")),
                            };
                        }
                        await sqlCommand.DisposeAsync();
                    }
                    await connection.CloseAsync();
                }
            }
            catch (Exception exp)
            {
                throw new Exception(exp.Message);
            }

            return reverseResponse;
        }

        public async Task<PharmacyInvoiceItemReplaceResponse> ReplacePharmacyInvoiceItemAsync(int sourceIndex, DateTime date)
        {
            PharmacyInvoiceItemReplaceResponse pharmacyInvoiceItemReplaceResponse = null;

            try
            {
                await using (SqlConnection connection = new SqlConnection(_connectionStrIntegrlo))
                {
                    await connection.OpenAsync();

                    //TPS575_GET_FAKTURA_DETAIL
                    await using (SqlCommand sqlCommand = new SqlCommand("TPS575_EVEZLEME", connection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.CommandTimeout = 180;
                        SqlParameter dateParameter = new SqlParameter();
                        dateParameter.ParameterName = "@TARIX";
                        dateParameter.SqlDbType = SqlDbType.DateTime;
                        dateParameter.Value = date;

                        SqlParameter sourceIndexParameter = new SqlParameter();
                        sourceIndexParameter.ParameterName = "@ANBAR";
                        sourceIndexParameter.SqlDbType = SqlDbType.Int;
                        sourceIndexParameter.Value = sourceIndex;

                        sqlCommand.Parameters.Add(sourceIndexParameter);
                        sqlCommand.Parameters.Add(dateParameter);

                        SqlDataReader dataReader = await sqlCommand.ExecuteReaderAsync();
                        while (await dataReader.ReadAsync())
                        {
                            pharmacyInvoiceItemReplaceResponse = new PharmacyInvoiceItemReplaceResponse()
                            {
                                SILINEN = dataReader.IsDBNull(dataReader.GetOrdinal("SILINEN")) ? 0 : dataReader.GetDouble(dataReader.GetOrdinal("SILINEN")),
                                FORMALASDIRILAN = dataReader.IsDBNull(dataReader.GetOrdinal("FORMALASDIRILAN")) ? 0 : dataReader.GetDouble(dataReader.GetOrdinal("FORMALASDIRILAN")),
                                FERQ = dataReader.IsDBNull(dataReader.GetOrdinal("FERQ")) ? 0 : dataReader.GetDouble(dataReader.GetOrdinal("FERQ")),
                            };
                        }
                        await sqlCommand.DisposeAsync();
                    }
                    await connection.CloseAsync();
                }
            }
            catch (Exception exp)
            {
                throw new Exception(exp.Message);
            }

            return pharmacyInvoiceItemReplaceResponse;
        }

        public async Task DeletePharmacyInvoiceDetailLine(int logicalref)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStrIntegrlo))
            {
                await connection.OpenAsync();
                await using (SqlCommand sqlCommand = new SqlCommand(PharmacyInvoiceQueries.DeletePharmacyInvoiceDetailLine(logicalref), connection))
                {
                    await sqlCommand.ExecuteNonQueryAsync();

                    await sqlCommand.DisposeAsync();
                }
                await connection.CloseAsync();
            }
        }

        public async Task<double> GetPharmacyInvoiceAddingItemSumByDateAndSourceIndexAsync(int sourceIndex, DateTime datetime)
        {
            double result = 0;

            await using (SqlConnection sqlConnection = new SqlConnection(_connectionStrLogo))
            {
                await sqlConnection.OpenAsync();
                await using (SqlCommand sqlCommand = new SqlCommand(PharmacyInvoiceQueries.GetPharmacyInvoiceAddingItemSumByDateAndSourceIndexQuery(sourceIndex, datetime), sqlConnection))
                {
                    SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

                    while (await sqlDataReader.ReadAsync())
                    {
                        result = sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("MEBLEG")) ? 0 : sqlDataReader.GetDouble(sqlDataReader.GetOrdinal("MEBLEG"));
                    }
                    await sqlDataReader.DisposeAsync();
                    await sqlCommand.DisposeAsync();
                }

                await sqlConnection.CloseAsync();
            }
            return result;
        }
    }
}