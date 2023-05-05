using Omnitech.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Configuration;
using Omnitech.Dal.AdoNet.Queries;

namespace Omnitech.Dal.AdoNet
{
    public class JobPermissionRepository
    {
        private readonly string _connectionStrLogo;
        private readonly string _connectionStrTestRep;

        public JobPermissionRepository(IConfiguration configuration)
        {
            _connectionStrLogo = configuration["ConnectionStrings:MyConnection"];
            _connectionStrTestRep = configuration["ConnectionStrings:TestRepConnection"];

        }

        public async Task<bool> ChangePermissionAsync(int jobId, int isRunning, int intervalUnit, string intervalType)
        {
            bool result = false;
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_connectionStrTestRep))
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand sqlCommand = new SqlCommand(JobPermissionQueries.ChangePermissionQuery(jobId, isRunning, intervalUnit, intervalType), sqlConnection))
                    {
                        await sqlCommand.ExecuteNonQueryAsync();
                        await sqlCommand.DisposeAsync();
                    }
                    await sqlConnection.CloseAsync();
                }
                result = true;
            }

            catch (Exception exp)
            {
            }

            finally
            {
                GC.Collect();
            }

            return result;
        }

        public async Task AddJobExceptionAsync(int jobId, string exception)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_connectionStrTestRep))
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand sqlCommand = new SqlCommand(JobPermissionQueries.AddJobExceptionQuery(jobId, exception), sqlConnection))
                    {
                        await sqlCommand.ExecuteNonQueryAsync();
                        await sqlCommand.DisposeAsync();
                    }
                    await sqlConnection.CloseAsync();
                }
            }

            catch (Exception exp)
            {

            }

            finally
            {
                GC.Collect();
            }
        }

        public async Task<JobPermission> GetJobPermissionByIdAsync(int id)
        {
            JobPermission jobPermission = null;
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_connectionStrTestRep))
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand sqlCommand = new SqlCommand(JobPermissionQueries.GetAllJobsPermisionByIdQuery(id), sqlConnection))
                    {
                        SqlDataReader dataReader = await sqlCommand.ExecuteReaderAsync();
                        while (await dataReader.ReadAsync())
                        {
                            jobPermission = new JobPermission
                            {
                                Id = dataReader.IsDBNull(dataReader.GetOrdinal("Id")) ? 0 : dataReader.GetInt32(dataReader.GetOrdinal("Id")),
                                JobName = dataReader.IsDBNull(dataReader.GetOrdinal("JobName")) ? "" : dataReader.GetString(dataReader.GetOrdinal("JobName")),
                                HasPermission = dataReader.IsDBNull(dataReader.GetOrdinal("HasPermission")) ? 0 : dataReader.GetInt32(dataReader.GetOrdinal("HasPermission")),
                                IsRunning = dataReader.IsDBNull(dataReader.GetOrdinal("IsRunning")) ? 0 : dataReader.GetInt32(dataReader.GetOrdinal("IsRunning")),
                                LastRun = dataReader.IsDBNull(dataReader.GetOrdinal("LastRun")) ? DateTime.Now : dataReader.GetDateTime(dataReader.GetOrdinal("LastRun")),
                                NextRun = dataReader.IsDBNull(dataReader.GetOrdinal("NextRun")) ? DateTime.Now : dataReader.GetDateTime(dataReader.GetOrdinal("NextRun")),
                                IntervalUnit = dataReader.IsDBNull(dataReader.GetOrdinal("IntervalUnit")) ? 0 : dataReader.GetInt32(dataReader.GetOrdinal("IntervalUnit")),
                                IntervalType = dataReader.IsDBNull(dataReader.GetOrdinal("IntervalType")) ? "" : dataReader.GetString(dataReader.GetOrdinal("IntervalType")),
                                DayCount = dataReader.IsDBNull(dataReader.GetOrdinal("DayCount")) ? 0 : dataReader.GetInt16(dataReader.GetOrdinal("DayCount")),
                                DateBetween = dataReader.IsDBNull(dataReader.GetOrdinal("DateBetween")) ? 0 : dataReader.GetInt16(dataReader.GetOrdinal("DateBetween")),
                            };
                        }
                        await sqlCommand.DisposeAsync();
                    }
                    await sqlConnection.CloseAsync();
                }
            }

            catch (Exception exp)
            {
                throw new Exception(exp.Message);
            }

            return jobPermission;
        }

        public async Task<int> GetHasPermissionById(int id)
        {
            int hasPermission = 0;
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_connectionStrTestRep))
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand sqlCommand = new SqlCommand(JobPermissionQueries.GetAllJobsPermisionByIdQuery(id), sqlConnection))
                    {
                        SqlDataReader dataReader = await sqlCommand.ExecuteReaderAsync();
                        while (await dataReader.ReadAsync())
                        {
                            hasPermission = dataReader.IsDBNull(dataReader.GetOrdinal("HasPermission")) ? 0 : dataReader.GetInt32(dataReader.GetOrdinal("HasPermission"));
                        }
                        await sqlCommand.DisposeAsync();
                    }
                    await sqlConnection.CloseAsync();
                }
            }

            catch (Exception exp)
            {
                throw new Exception(exp.Message);
            }

            return hasPermission;
        }

        public async Task ChangePermissionByJobIdAsync(int jobId, int permission)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_connectionStrTestRep))
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand sqlCommand = new SqlCommand(JobPermissionQueries.ChangePermissionByJobIdQuery(jobId, permission), sqlConnection))
                    {
                        await sqlCommand.ExecuteNonQueryAsync();
                        await sqlCommand.DisposeAsync();
                    }
                    await sqlConnection.CloseAsync();
                }
            }

            catch (Exception exp)
            {

            }

            finally
            {
                GC.Collect();
            }
        }
    }
}
