using Microsoft.Extensions.Configuration;
using Omnitech.Models;
using System.Collections.Generic;
using System.Data;
using System;
using System.Data.SqlClient;
using Persistence.Concrete.AdoNet.Queries;
using DocumentFormat.OpenXml.Drawing.Charts;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Omnitech.Dal.AdoNet
{
    public class UserRepository
    {
        private readonly IConfiguration _configuration;
        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task AddAsync(User user)
        {
            //await ExecuteCommandAsync(UserQueries.AddUserQuery(user), _configuration["ConnectionStrings:INTEGRLO"]);
            try
            {
                await using (SqlConnection sqlConnection = new SqlConnection(_configuration["ConnectionStrings:Integrlo"]))
                {
                    await sqlConnection.OpenAsync();
                    await using (SqlCommand sqlCommand = new SqlCommand(UserQueries.AddUserQuery(user), sqlConnection))
                    {
                        SqlParameter passwordSaltParam = sqlCommand.Parameters.AddWithValue("@PasswordSalt", user.PasswordSalt);
                        passwordSaltParam.DbType = DbType.Binary;

                        SqlParameter passwordHashParam = sqlCommand.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                        passwordHashParam.DbType = DbType.Binary;

                        string tt = sqlCommand.CommandText;

                        await sqlCommand.ExecuteNonQueryAsync();
                        await sqlCommand.DisposeAsync();
                    }
                    await sqlConnection.CloseAsync();
                }


            }

            catch (Exception exp)
            {
                string msg = exp.Message;
            }
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            User user = null;
            try
            {
                await using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:Integrlo"]))
                {
                    await connection.OpenAsync();

                    await using (SqlCommand sqlCommand = new SqlCommand(UserQueries.GetUserByUsernameQuery(username), connection))
                    {
                        SqlDataReader dataReader = await sqlCommand.ExecuteReaderAsync();
                        while (await dataReader.ReadAsync())
                        {
                            user = new User
                            {
                                ID = dataReader.IsDBNull(dataReader.GetOrdinal("ID")) ? 0 : dataReader.GetInt32(dataReader.GetOrdinal("ID")),
                                FirstName = dataReader.IsDBNull(dataReader.GetOrdinal("FirstName")) ? "" : dataReader.GetString(dataReader.GetOrdinal("FirstName")),
                                LastName = dataReader.IsDBNull(dataReader.GetOrdinal("LastName")) ? "" : dataReader.GetString(dataReader.GetOrdinal("LastName")),
                                Username = dataReader.IsDBNull(dataReader.GetOrdinal("Username")) ? "" : dataReader.GetString(dataReader.GetOrdinal("Username")),
                                PasswordHash = dataReader["PasswordHash"] == null ? new byte[0] : (byte[])dataReader["PasswordHash"],
                                PasswordSalt = dataReader["PasswordSalt"] == null ? new byte[0] : (byte[])dataReader["PasswordSalt"],
                                Status = dataReader.IsDBNull(dataReader.GetOrdinal("Status")) ? false : dataReader.GetBoolean(dataReader.GetOrdinal("Status")),
                            };

                        }

                        await dataReader.DisposeAsync();
                        await sqlCommand.DisposeAsync();
                    }
                    await connection.CloseAsync();
                }
            }

            catch (Exception exp)
            {
                throw new Exception(exp.Message);
            }

            return user;
        }

        public async Task<List<OperationClaim>> GetClaimsAsync(User user)
        {
            List<OperationClaim> operationClaims = new List<OperationClaim>();
            try
            {
                await using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:Integrlo"]))
                {
                    await connection.OpenAsync();

                    await using (SqlCommand sqlCommand = new SqlCommand(UserQueries.GetClaimsByUserQuery(user), connection))
                    {
                        SqlDataReader dataReader = await sqlCommand.ExecuteReaderAsync();
                        while (await dataReader.ReadAsync())
                        {
                            OperationClaim operationClaim = new OperationClaim
                            {
                                ID = dataReader.IsDBNull(dataReader.GetOrdinal("ID")) ? 0 : dataReader.GetInt32(dataReader.GetOrdinal("ID")),
                                Name = dataReader.IsDBNull(dataReader.GetOrdinal("Name")) ? "" : dataReader.GetString(dataReader.GetOrdinal("Name")),
                            };

                            operationClaims.Add(operationClaim);
                        }

                        await dataReader.DisposeAsync();
                        await sqlCommand.DisposeAsync();
                    }
                    await connection.CloseAsync();
                }
            }

            catch (Exception exp)
            {
                throw new Exception(exp.Message);
            }

            return operationClaims;
        }
    }
}
