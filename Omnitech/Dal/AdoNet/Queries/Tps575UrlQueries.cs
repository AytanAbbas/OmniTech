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
        public static string GetUrlByUsernameQuery(string username)
        {
            return $@"SELECT TOP 1
	                    usr_url.ID,
	                    usr_url.URL,
	                    usr_url.ACTIVE
                    FROM TPS575_USER_URLS usr_url
                    INNER JOIN Users usr on usr_url.USERID=usr.ID
                    WHERE ACTIVE = 1 AND usr.Username='{username}'
                    ORDER BY ID DESC";
        }
    }
}
