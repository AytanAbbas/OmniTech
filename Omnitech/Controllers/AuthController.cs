
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Omnitech.Dtos;
using Omnitech.DTOs;
using Omnitech.Models;
using Omnitech.Service;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace Omnitech.Controllers
{
   // [Authorize(Roles = "Admin")]
    public class AuthController : BaseController
    {
        private readonly AuthManager _authService;
        private readonly UserManager _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserMenuManager _userMenuManager;

        public AuthController(AuthManager authService, UserManager userManager, IHttpContextAccessor contextAccessor, UserMenuManager userMenuManager) : base(userMenuManager)
        {
            _authService = authService;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
            _userMenuManager= userMenuManager;


        }

        //[Authorize]
        public async Task<IActionResult> Register()
        {
            return View(new UserForRegisterDto());
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            if (!ModelState.IsValid)
            {
                return View(userForRegisterDto);
            }

            var userExists = await _authService.UserExists(userForRegisterDto.Username);

            if (!userExists)
            {
                ModelState.AddModelError("Email", "User allready exist!");
            }

            var registerResult = await _authService.Register(userForRegisterDto, userForRegisterDto.Password);

            if (registerResult != null || registerResult != default || registerResult.ID != 0)
            {
                return RedirectToAction("Login", "Auth");
            }
            return View(registerResult);
        }

        public async Task<IActionResult> Login()
        {
            UserForLoginDto rr = new UserForLoginDto();
            return CreateActionResultInstance(rr);
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            bool happenExp = false;

            try
            {
                var user = await _authService.LoginAsync(userForLoginDto);

                List<OperationClaim> operationClaims = await _userManager.GetClaimsAsync(user);

                HttpContext.Session.SetString(user.Username, JsonConvert.SerializeObject(user));

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username.ToString()),
                    //new Claim("FullName", user.FullName),
                };

                foreach (var operationClaim in operationClaims)
                {
                    claims.Add(new Claim(ClaimTypes.Role, operationClaim.Name));
                }

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                };

                await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);

                ViewData["Data"] = "";
            }

            catch (Exception exp)
            {
                happenExp = true;
                ModelState.AddModelError("Username", exp.Message);
            }

            if (happenExp)
                return View(userForLoginDto);

            else
                return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            _contextAccessor.HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}