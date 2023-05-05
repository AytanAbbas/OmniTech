using Microsoft.Extensions.Configuration;
using Omnitech.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System;

namespace Omnitech.Dal.AdoNet
{
    public class KNInvoiceRepository
    {
        private readonly string _connectionStrLogo;
        private readonly string _connectionStrIntegrlo;

        public KNInvoiceRepository(IConfiguration configuration)
        {
            _connectionStrLogo = configuration["ConnectionStrings:MyConnection"];
            _connectionStrIntegrlo = configuration["ConnectionStrings:IntegrloConnection"];
        }
       
        public async Task<List<KNInvoiceInfo>> GetKNInvoiceInfosByStartDateAndEndDateAsync(DateTime startDate, DateTime endDate)
        {
            List<KNInvoiceInfo> invoices = new List<KNInvoiceInfo>();

            await using (SqlConnection connection = new SqlConnection(_connectionStrIntegrlo))
            {
                await connection.OpenAsync();
                await using (SqlCommand sqlCommand = new SqlCommand("TPS575_GET_FAKTURALAR_FROM_TIGER_FOR_001", connection))
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
                        KNInvoiceInfo invoice = new KNInvoiceInfo
                        {
                            INV_ID = dataReader.IsDBNull(dataReader.GetOrdinal("INV_ID")) ? 0 : dataReader.GetInt32(dataReader.GetOrdinal("INV_ID")),
                            FAKTURANO = dataReader.IsDBNull(dataReader.GetOrdinal("FAKTURANO")) ? "" : dataReader.GetString(dataReader.GetOrdinal("FAKTURANO")),
                            TARIX = dataReader.IsDBNull(dataReader.GetOrdinal("TARIX")) ? Convert.ToDateTime("01.01.1900") : dataReader.GetDateTime(dataReader.GetOrdinal("TARIX")),
                            CARI_HESAB_KODU = dataReader.IsDBNull(dataReader.GetOrdinal("CARI_HESAB_KODU")) ? "" : dataReader.GetString(dataReader.GetOrdinal("CARI_HESAB_KODU")),
                            CARI_HESAB_ADI = dataReader.IsDBNull(dataReader.GetOrdinal("CARI_HESAB_ADI")) ? "" : dataReader.GetString(dataReader.GetOrdinal("CARI_HESAB_ADI")),
                            FAKTURA_MEBLEGI = dataReader.IsDBNull(dataReader.GetOrdinal("FAKTURA_MEBLEGI")) ? 0 : dataReader.GetDouble(dataReader.GetOrdinal("FAKTURA_MEBLEGI")),
                            QEBZ_MEBLEGI = dataReader.IsDBNull(dataReader.GetOrdinal("QEBZ_MEBLEGI")) ? 0 : dataReader.GetDouble(dataReader.GetOrdinal("QEBZ_MEBLEGI")),
                            CAP_EDILMIS_MEBLEG = dataReader.IsDBNull(dataReader.GetOrdinal("CAP_EDILMIS_MEBLEG")) ? 0 : dataReader.GetDouble(dataReader.GetOrdinal("CAP_EDILMIS_MEBLEG")),
                            CAP_SAYI = dataReader.IsDBNull(dataReader.GetOrdinal("CAP_SAYI")) ? 0 : dataReader.GetInt32(dataReader.GetOrdinal("CAP_SAYI")),
                            setr_sayi = dataReader.IsDBNull(dataReader.GetOrdinal("setr_sayi")) ? 0 : dataReader.GetInt32(dataReader.GetOrdinal("setr_sayi")),
                            FICSAL_DOCUMENT = dataReader.IsDBNull(dataReader.GetOrdinal("FICSAL_DOCUMENT")) ? "" : dataReader.GetString(dataReader.GetOrdinal("FICSAL_DOCUMENT"))
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

        public async Task AddKNInvoiceDetailsByInvIdAndMeblegAsync(int invId, double mebleg)
        {
            List<KNInvoiceDetail> knInvoiceDetails = new List<KNInvoiceDetail>();

            try
            {
                await using (SqlConnection connection = new SqlConnection(_connectionStrIntegrlo))
                {
                    await connection.OpenAsync();

                    await using (SqlCommand sqlCommand = new SqlCommand("TPS575_ADD_TO_TOTAL_TEMP_001", connection))
                    {
                        SqlParameter invoiceParameter = new SqlParameter();
                        invoiceParameter.ParameterName = "@INV_ID";
                        invoiceParameter.SqlDbType = SqlDbType.Int;
                        invoiceParameter.Value = invId;

                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        SqlParameter meblegParameter = new SqlParameter();
                        meblegParameter.ParameterName = "@MEBLEG";
                        meblegParameter.SqlDbType = SqlDbType.Float;
                        meblegParameter.Value = mebleg;


                        sqlCommand.Parameters.Add(invoiceParameter);
                        sqlCommand.Parameters.Add(meblegParameter);


                        sqlCommand.CommandTimeout = 120;
                        await sqlCommand.ExecuteNonQueryAsync();

                        await sqlCommand.DisposeAsync();
                    }
                    await connection.CloseAsync();
                }
            }

            catch (Exception exp)
            {
                throw new Exception(exp.Message);
            }
        }

        public async Task<List<KNInvoiceDetail>> GetKNInvoiceDetailsByInvIdAndMeblegAsync(int invId, double mebleg)
        {
            List<KNInvoiceDetail> knInvoiceDetails = new List<KNInvoiceDetail>();

            try
            {
                await using (SqlConnection connection = new SqlConnection(_connectionStrIntegrlo))
                {
                    await connection.OpenAsync();

                    await using (SqlCommand sqlCommand = new SqlCommand("TPS575_GET_FAKTURA_DETAIL_001", connection))
                    {
                        SqlParameter invoiceParameter = new SqlParameter();
                        invoiceParameter.ParameterName = "@INV_ID";
                        invoiceParameter.SqlDbType = SqlDbType.Int;
                        invoiceParameter.Value = invId;

                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        SqlParameter meblegParameter = new SqlParameter();
                        meblegParameter.ParameterName = "@MEBLEG";
                        meblegParameter.SqlDbType = SqlDbType.Float;
                        meblegParameter.Value = mebleg;


                        sqlCommand.Parameters.Add(invoiceParameter);
                        sqlCommand.Parameters.Add(meblegParameter);

                        SqlDataReader dataReader = await sqlCommand.ExecuteReaderAsync();
                        while (await dataReader.ReadAsync())
                        {
                            KNInvoiceDetail knInvoiceDetail = new KNInvoiceDetail()
                            {
                                LOGICALREF = dataReader.IsDBNull(dataReader.GetOrdinal("LOGICALREF")) ? 0 : dataReader.GetInt32(dataReader.GetOrdinal("LOGICALREF")),
                                SKU = dataReader.IsDBNull(dataReader.GetOrdinal("SKU")) ? 0 : dataReader.GetInt32(dataReader.GetOrdinal("SKU")),
                                TARIX = dataReader.IsDBNull(dataReader.GetOrdinal("TARIX")) ? Convert.ToDateTime("01.01.1900") : dataReader.GetDateTime(dataReader.GetOrdinal("TARIX")),
                                MIQDAR = dataReader.IsDBNull(dataReader.GetOrdinal("MIQDAR")) ? 0 : dataReader.GetDouble(dataReader.GetOrdinal("MIQDAR")),
                                MEBLEG = dataReader.IsDBNull(dataReader.GetOrdinal("MEBLEG")) ? 0 : dataReader.GetDouble(dataReader.GetOrdinal("MEBLEG")),
                                CLIENT = dataReader.IsDBNull(dataReader.GetOrdinal("CLIENT")) ? 0 : dataReader.GetInt32(dataReader.GetOrdinal("CLIENT")),
                                MEHSULUN_KODU = dataReader.IsDBNull(dataReader.GetOrdinal("MEHSULUN_KODU")) ? "" : dataReader.GetString(dataReader.GetOrdinal("MEHSULUN_KODU")),
                                MEHSULUN_ADI = dataReader.IsDBNull(dataReader.GetOrdinal("MEHSULUN_ADI")) ? "" : dataReader.GetString(dataReader.GetOrdinal("MEHSULUN_ADI")),
                                FIRMA = dataReader.IsDBNull(dataReader.GetOrdinal("FIRMA")) ? "" : dataReader.GetString(dataReader.GetOrdinal("FIRMA")),
                                ISTEHSALCHI = dataReader.IsDBNull(dataReader.GetOrdinal("ISTEHSALCHI")) ? "" : dataReader.GetString(dataReader.GetOrdinal("ISTEHSALCHI")),
                                OLKE = dataReader.IsDBNull(dataReader.GetOrdinal("ISTEHSALCHI")) ? "" : dataReader.GetString(dataReader.GetOrdinal("ISTEHSALCHI")),
                                BARCODE = dataReader.IsDBNull(dataReader.GetOrdinal("BARCODE")) ? "" : dataReader.GetString(dataReader.GetOrdinal("BARCODE")),
                                VAHID = dataReader.IsDBNull(dataReader.GetOrdinal("VAHID")) ? "" : dataReader.GetString(dataReader.GetOrdinal("VAHID")),
                                EDEDSAYI = dataReader.IsDBNull(dataReader.GetOrdinal("EDEDSAYI")) ? 0 : dataReader.GetInt32(dataReader.GetOrdinal("EDEDSAYI")),
                                DELETED = dataReader.IsDBNull(dataReader.GetOrdinal("DELETED")) ? 0 : dataReader.GetInt32(dataReader.GetOrdinal("DELETED")),

                            };

                            knInvoiceDetails.Add(knInvoiceDetail);
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

            return knInvoiceDetails;
        }
    }
}