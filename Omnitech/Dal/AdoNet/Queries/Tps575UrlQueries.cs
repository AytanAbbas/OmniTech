namespace Omnitech.Dal.AdoNet.Queries
{
    public class Tps575UrlQueries
    {
        public static string GetActiveUrlQuery
        {
            get
            {
                return $@"SELECT TOP 1
	                        ID,
	                        URL,
	                        ACTIVE
                        FROM TPS575_URLS
                        WHERE ACTIVE = 1
                        ORDER BY ID DESC";
            }
        }
    }
}
