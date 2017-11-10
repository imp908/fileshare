using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;

namespace NewsAPI.Interfaces
{
   public interface IUserAuthenticator
    {
        string AuthenticateUser(IPrincipal principal);
    }
}
