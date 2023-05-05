using System;

namespace Omnitech.Dal.AdoNet.Queries
{
    public class PharmacyInvoiceQueries
    {
        public static string GetAllFoodSupplementItemsQuery()
        {
            string query = $@"SELECT
					ITM.LOGICALREF SKU,
					ITM.CODE	   KODU,
					ITM.NAME	   ADI,
					ITM.SPECODE    FIRMA,
					ITM.STGRPCODE  ISTEHSALCHI,
					ITM.CYPHCODE   OLKE,	
					CASE 
						WHEN ITM.SPECODE3 = 'VQIYMET' THEN (SELECT TOP 1 PRC_1.PRICE FROM LG_001_PRCLIST PRC_1 WITH (NOLOCK) WHERE PRC_1.CARDREF = ITM.LOGICALREF AND PRC_1.ACTIVE= 0 AND PRC_1.PTYPE = 2 AND PRC_1.CLSPECODE4 = 'TEMPER' ORDER BY LOGICALREF DESC)
						ELSE (SELECT TOP 1 PRC_1.PRICE FROM LG_001_PRCLIST PRC_1 WITH (NOLOCK) WHERE PRC_1.CARDREF = ITM.LOGICALREF AND PRC_1.ACTIVE= 0 AND PRC_1.PTYPE = 2 AND PRC_1.CLSPECODE5 = 'AP100' ORDER BY LOGICALREF DESC) 
					END QIYMET
				FROM LG_001_ITEMS ITM WITH (NOLOCK)
				WHERE 
					ITM.CARDTYPE = 1 
				AND ITM.ACTIVE = 0
				AND ITM.LOGICALREF in (92608	,86788	,59735	,76351	,46934	,34945	,115785	,116366	,92609	,107636	,57731	,76352	,92612	,
						59241	,713	,4237	,33681	,92610	,59734	,57725	,59242	,719	,20391	,106814	,48853	,
						109030	,17664	,17667	,24590	,24591	,24588	,89076	,111999	,112002	
			 
)
                          ";

            return query;


        }

        public static string GetFoodSupplementPriceByItemRefQuery(int itemRef)
        {
            string query = $@"
							SELECT
						CASE 
							WHEN ITM.SPECODE3 = 'VQIYMET' THEN (SELECT TOP 1 PRC_1.PRICE FROM LG_001_PRCLIST PRC_1 WITH (NOLOCK) WHERE PRC_1.CARDREF = ITM.LOGICALREF AND PRC_1.ACTIVE= 0 AND PRC_1.PTYPE = 2 AND PRC_1.CLSPECODE4 = 'TEMPER' ORDER BY LOGICALREF DESC)
							ELSE (SELECT TOP 1 PRC_1.PRICE FROM LG_001_PRCLIST PRC_1 WITH (NOLOCK) WHERE PRC_1.CARDREF = ITM.LOGICALREF AND PRC_1.ACTIVE= 0 AND PRC_1.PTYPE = 2 AND PRC_1.CLSPECODE5 = 'AP100' ORDER BY LOGICALREF DESC) 
						END QIYMET
					FROM LG_001_ITEMS ITM WITH (NOLOCK)
					WHERE 
						ITM.CARDTYPE = 1 
					AND ITM.ACTIVE = 0
					AND ITM.LOGICALREF ={itemRef}";

            return query;


        }

        public static string DeletePharmacyInvoiceDetailLine(int logicalref)
        {
            string query = $@"
                            DELETE FROM TPS575_TOTAL_TEMPS
                            WHERE LOGICALREF={logicalref}
                            ";

            return query;


        }

        public static string GetPharmacyInvoiceAddingItemSumByDateAndSourceIndexQuery(int sourceIndex, DateTime datetime)
        {
            string query = $@"SELECT 
                                 ISNULL(SUM(MEBLEG),0) MEBLEG
                              FROM INTEGRLO.dbo.TPS575_TOTAL_TEMPS
                              WHERE TARIX='{datetime.ToString("yyyy-MM-dd")}' AND ANBAR={sourceIndex} AND DELETED=3";

            return query;


        }
    }
}
