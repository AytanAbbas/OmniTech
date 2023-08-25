namespace Omnitech.Dal.AdoNet.Queries
{
    public class UserMenuQueries
    {
        protected string GetSubmenusByUserIdQuery(int userId)
        {
            string query = $@"SELECT 
	                           sbm.ID
                              ,sbm.Name
                              ,sbm.Controller
                              ,sbm.Action
                              ,sbm.MenuId
                          FROM TPS575_Submenus sbm
                          INNER JOIN TPS575_UserSubmenus usbm ON sbm.ID=usbm.SubmenuId
                          WHERE usbm.UserId={userId}";

            return query;
        }

        protected string GetSubmenusByUsernameQuery(string username)
        {
            string query = $@"SELECT 
	                           sbm.ID
                              ,sbm.Name
                              ,sbm.Controller
                              ,sbm.Action
                              ,sbm.MenuId
                          FROM TPS575_Submenus sbm
                          INNER JOIN TPS575_UserSubmenus usbm ON sbm.ID = usbm.SubmenuId
                          INNER JOIN Users us on usbm.UserId=us.ID
                          WHERE us.username='{username}'";

            return query;
        }
    }
}
