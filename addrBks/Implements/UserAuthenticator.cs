using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewsAPI.Interfaces;
using System.Security.Principal;
using System.Web.Http;


namespace NewsAPI.Implements
{
    public class UserAuthenticator : IUserAuthenticator
    {
        public string AuthenticateUser(IPrincipal principal)
        {
            string name = string.Empty;
            try
            {
                name = principal.Identity.Name.Split('\\')[1];
            }catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }
            return name;
        }
    }
}