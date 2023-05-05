using Omnitech.Models;
using System.Data.Common;
using System;
using static Omnitech.Utilities.Enums;

namespace Omnitech.Dal.AdoNet.Queries
{
    public class JobPermissionQueries
    {
        public static string GetAllJobsPermisionByIdQuery(int id)
        {
            return $@"SELECT * FROM RunningJobsPermissions WITH(NOLOCK) WHERE Id={id}";
        }

        public static string ChangePermissionQuery(int jobId, int isRunning, int intervalUnit, string intervalType)
        {
            string query = "";

            DateTime now = DateTime.Now;
            string dateTimeStr = $@"{now.Month}.{now.Day}.{now.Year} {now.Hour}:{now.Minute}:{now.Second}:{now.Millisecond}";

            if (isRunning == 1)
                query = $@"UPDATE RunningJobsPermissions
                        SET IsRunning={isRunning}, 
                            LastRun=CONVERT(datetime,'{dateTimeStr}'), NextRun=DATEADD({intervalType},{intervalUnit},CONVERT(datetime,'{dateTimeStr}'))
                                WHERE Id={jobId}";

            else
                query = $@"UPDATE RunningJobsPermissions
                        SET IsRunning={isRunning}
                                WHERE Id={jobId}";

            return query;
        }

        public static string AddJobExceptionQuery(int jobId, string exception)
        {
            DateTime now = DateTime.Now;
            string dateTimeStr = $@"{now.Month}.{now.Day}.{now.Year} {now.Hour}:{now.Minute}:{now.Second}:{now.Millisecond}";

            string query = $"INSERT INTO JobsHistories VALUES ({jobId}, '{dateTimeStr}', '{exception.Replace("'", "")}')";

            return query;
        }

        public static string ChangePermissionByJobIdQuery(int jobId, int permission)
        {
            string query = $@"UPDATE RunningJobsPermissions SET 
                                HasPermission={permission}
                              WHERE Id= {jobId}";

            return query;
        }
    }
}
