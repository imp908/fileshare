using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class WCF_consumer
    {

        public void WCF_check()
        {
            WCF_UOW.WCFClient wcf = new WCF_UOW.WCFClient();

            wcf.SetCurrentUser(0);
            var kk = wcf.GetKKByUserId();
        }

    }
}