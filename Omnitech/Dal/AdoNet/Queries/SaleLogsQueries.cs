using DocumentFormat.OpenXml.Spreadsheet;

namespace Omnitech.Dal.AdoNet.Queries
{
    public class SaleLogsQueries
    {

        public static string GetSalesLogsByFakturaNameQuery(string fakturaName)
        {
            string query = $@"
                                DECLARE @FAKTURA NVARCHAR(305)='{fakturaName}'
                                --DECLARE @seprateNumIndex int = CHARINDEX('_',@FAKTURA)
                                --DECLARE @FAKTURA_BASE NVARCHAR(30) = SUBSTRING(@FAKTURA,1,@seprateNumIndex-1)


                                  SELECT 
                                       [RECNO]
                                      ,[FAKTURA_NO]
                                      ,[FAKTURA_NAME]
                                      ,[REQUEST_TPS575]
                                      ,[RESPONSE_TPS575]
                                      ,[FICSAL_DOCUMENT]
                                      ,[TIPI]
                                      ,[INSERT_DATE]
                                      ,[PC_NAME]
                                      ,[IP_REQUEST]
                                      ,[FICSAL_DOCUMENT_LONG]
                                  FROM [INTEGRLO].[dbo].[TPS575_SaleLogs]
                                  --WHERE FAKTURA_NO LIKE @FAKTURA_BASE+'%'
                                  WHERE FAKTURA_NO LIKE @FAKTURA +'%'";
            return query;

        }

        public static string GetAllSalesLogsForPrintQuery()
        {
            string query = $@"
                                  SELECT 
                                       [RECNO]
                                      ,[FAKTURA_NO]
                                      ,[FAKTURA_NAME]
                                      ,[REQUEST_TPS575]
                                      ,[RESPONSE_TPS575]
                                      ,[FICSAL_DOCUMENT]
                                      ,[TIPI]
                                      ,[INSERT_DATE]
                                      ,[PC_NAME]
                                      ,[IP_REQUEST]
                                      ,[FICSAL_DOCUMENT_LONG]
                                     ,[FIRMA]
                                  FROM [TPS575_SaleLogs]
                                  where ISNULL(RESPONSE_TPS575,'')='' --and recno>47
								  order by recno";
            return query;

        }

        public static string GetAllSalesLogsByRecnoAsync(int recno)
        {
            string query = $@"
                                  SELECT 
                                       [RECNO]
                                      ,[FAKTURA_NO]
                                      ,[FAKTURA_NAME]
                                      ,[REQUEST_TPS575]
                                      ,[RESPONSE_TPS575]
                                      ,[FICSAL_DOCUMENT]
                                      ,[TIPI]
                                      ,[INSERT_DATE]
                                      ,[PC_NAME]
                                      ,[IP_REQUEST]
                                      ,[FICSAL_DOCUMENT_LONG]
                                     ,[FIRMA]
                                  FROM [TPS575_SaleLogs]
                                  where RECNO={recno}
								  order by recno";
            return query;

        }

        public static string GetProblemicSalesLogsQuery()
        {
            string query = $@"
                                     SELECT 
                                      [FAKTURA_NO]
                                     ,[RESPONSE_TPS575]
                                      ,VP.QEBZMEBLEG  
                                      ,[FICSAL_DOCUMENT]                                     
                                      ,[INSERT_DATE]                                     
                                      ,sl.[FIRMA]
                                  FROM [TPS575_SaleLogs] sl
								  LEFT OUTER JOIN INTEGRLO.dbo.TPS575_VPUL vp with (nolock) on vp.FAKTURANAME =sl.FAKTURA_NO
                                  where ISNULL(FICSAL_DOCUMENT,'')='' and ISNULL(RESPONSE_TPS575,'')<>''
								  order by sl.RECNO";
            return query;

        }

        public static string ChangeProblemicSalesLogsQuery(int recno) 
        {
            string query = $@"
                                 
								  update [TPS575_SaleLogs] 
								  set RESPONSE_TPS575 = 'NULL'
								  where recno ={recno} ";
            return query;
        }
        public static string InvoiceHasExistsQuery(string ficheno)
        {
            string query = $@"DECLARE @invHasExist bit = 0,  @existInvCount int
                            SELECT @existInvCount= COUNT(*)  

                            FROM TPS575_SaleLogs
                            where FAKTURA_NO LIKE '{ficheno}%'

                            if(@existInvCount>0) 
	                            BEGIN
		                            set @invHasExist = 1
	                            END

                            SELECT  @invHasExist invHasExist";
            return query;
        }
    }
}
