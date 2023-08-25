using DocumentFormat.OpenXml.Office2010.ExcelAc;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Extensions.Configuration;
using Omnitech.Models;
using System.Data;
using System;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Collections.Generic;
using Omnitech.Dal.AdoNet.Queries;

namespace Omnitech.Dal.AdoNet
{
    public class UserMenuRepository : UserMenuQueries
    {
        private readonly string _connectionStrLogo;
        private readonly string _connectionStrIntegrlo;

        public UserMenuRepository(IConfiguration configuration)
        {
            _connectionStrLogo = configuration["ConnectionStrings:MyConnection"];
            _connectionStrIntegrlo = configuration["ConnectionStrings:Integrlo"];
        }

        public async Task<List<Submenu>> GetSubmenusByUserIdAsync(int userId)
        {
            List<Submenu> submenus = new List<Submenu>();

            try
            {
                await using (SqlConnection connection = new SqlConnection(_connectionStrIntegrlo))
                {
                    await connection.OpenAsync();
                    await using (SqlCommand sqlCommand = new SqlCommand(GetSubmenusByUserIdQuery(userId), connection))
                    {
                        SqlDataReader dataReader = await sqlCommand.ExecuteReaderAsync();

                        while (await dataReader.ReadAsync())
                        {
                            Submenu submenu = new Submenu
                            {
                                ID = dataReader.IsDBNull(dataReader.GetOrdinal("ID")) ? 0 : dataReader.GetInt32(dataReader.GetOrdinal("ID")),
                                Controller = dataReader.IsDBNull(dataReader.GetOrdinal("Controller")) ? "" : dataReader.GetString(dataReader.GetOrdinal("Controller")),
                                Action = dataReader.IsDBNull(dataReader.GetOrdinal("Action")) ? "" : dataReader.GetString(dataReader.GetOrdinal("Action")),
                                Name = dataReader.IsDBNull(dataReader.GetOrdinal("Name")) ? "" : dataReader.GetString(dataReader.GetOrdinal("Name")),
                                MenuId = dataReader.IsDBNull(dataReader.GetOrdinal("MenuId")) ? 0 : dataReader.GetInt32(dataReader.GetOrdinal("MenuId")),
                            };
                            submenus.Add(submenu);
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

            return submenus;
        }

        public async Task<List<Submenu>> GetSubmenusByUsernameAsync(string username)
        {
            List<Submenu> submenus = new List<Submenu>();

            try
            {
                await using (SqlConnection connection = new SqlConnection(_connectionStrIntegrlo))
                {
                    await connection.OpenAsync();
                    await using (SqlCommand sqlCommand = new SqlCommand(GetSubmenusByUsernameQuery(username), connection))
                    {
                        SqlDataReader dataReader = await sqlCommand.ExecuteReaderAsync();

                        while (await dataReader.ReadAsync())
                        {
                            Submenu submenu = new Submenu
                            {
                                ID = dataReader.IsDBNull(dataReader.GetOrdinal("ID")) ? 0 : dataReader.GetInt32(dataReader.GetOrdinal("ID")),
                                Controller = dataReader.IsDBNull(dataReader.GetOrdinal("Controller")) ? "" : dataReader.GetString(dataReader.GetOrdinal("Controller")),
                                Action = dataReader.IsDBNull(dataReader.GetOrdinal("Action")) ? "" : dataReader.GetString(dataReader.GetOrdinal("Action")),
                                Name = dataReader.IsDBNull(dataReader.GetOrdinal("Name")) ? "" : dataReader.GetString(dataReader.GetOrdinal("Name")),
                                MenuId = dataReader.IsDBNull(dataReader.GetOrdinal("MenuId")) ? 0 : dataReader.GetInt32(dataReader.GetOrdinal("MenuId")),
                            };
                            submenus.Add(submenu);
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

            return submenus;
        }
    }
}
