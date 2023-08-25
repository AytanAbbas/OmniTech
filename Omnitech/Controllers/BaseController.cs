using Microsoft.AspNetCore.Mvc;
using Omnitech.Dal.AdoNet;
using Omnitech.Service;
using Omnitech.Utilities;
using System;
using System.Threading.Tasks;

namespace Omnitech.Controllers
{
    public class BaseController : Controller
    {
        private UserMenuManager _userMenuManager;

        public BaseController(UserMenuManager userMenuManager)
        {
            _userMenuManager = userMenuManager;

            //Enums.Tps575UserUrl = _baseService.GetUrlByUsernameAsync().GetAwaiter().GetResult();
        }

        public IActionResult CreateActionResultInstance<T>(T response)
        {
            try
            {
                string username = HttpContext.User.Identity.Name;

                ViewData["Data"] = _userMenuManager.GetSubmenusByUsernameAsync(username).GetAwaiter().GetResult();
            }

            catch (Exception)
            {
            }
            

            return View(response);
        }


        public IActionResult CreateActionResultInstance()
        {
            try
            {
                string username = HttpContext.User.Identity.Name;

                ViewData["Data"] = _userMenuManager.GetSubmenusByUsernameAsync(username).GetAwaiter().GetResult();
            }

            catch (Exception)
            {
            }
           

            return View();
        }
    }
}
