using Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Omnitech.DTOs
{
    public class UserForRegisterDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
