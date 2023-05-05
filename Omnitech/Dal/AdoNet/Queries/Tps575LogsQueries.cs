using Omnitech.Models;

namespace Omnitech.Dal.AdoNet.Queries
{
    public class Tps575LogsQueries
    {
        public static string AddTps575LogsQuery ( Tps575Logs tps575Logs)
         
        {
            string query = $@"INSERT INTO TPS575_LOGS(
                                  FAKTURA_NAME
                                  ,request
                                  ,response
                                  ,insert_date
                              )
                              VALUES (
	                            '{tps575Logs.FAKTURA_NAME}',
	                            '{tps575Logs.request}',
	                            '{tps575Logs.response}',
	                            GETDATE()
                              )";
            return query;

        }
    }
}
