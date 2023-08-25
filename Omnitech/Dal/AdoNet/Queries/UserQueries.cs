using Omnitech.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Concrete.AdoNet.Queries
{
    internal static class UserQueries
    {
        internal static string AddUserQuery(User user)
        {
            int status = 0;

            if (user.Status == true)
                status = 1;
            string query = $@"INSERT INTO Users(
									FirstName,
									LastName,
									Username,
									PasswordSalt,
									PasswordHash,
									Status
								)
								VALUES 
								(
									'{user.FirstName}',
									'{user.LastName}',
									'{user.Username}',
									@PasswordSalt,
									@PasswordHash,
									{status}
								)";

            return query;
        }

        internal static string GetUserByUsernameQuery(string username)
        {
            string query = $@"SELECT 
								ID,
								FirstName,
								LastName,
								Username,
								PasswordSalt,
								PasswordHash,
								Status
							FROM Users
							WHERE Username='{username}'";

            return query;
        }

        internal static string GetClaimsByUserQuery(User user)
        {
            string query = $@"SELECT 
									 OP_CL.ID
									,OP_CL.Name
								FROM UserOperationClaims US_OP_CL
								INNER JOIN OperationClaims OP_CL ON US_OP_CL.OperationClaimId=OP_CL.ID
								WHERE US_OP_CL.UserId={user.ID}";

            return query;
        }
    }
}
