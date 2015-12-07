using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplicationFiltros.Utils;

namespace WebApplicationFiltros.Filtros
{
    public class FiltrosAcceso : ActionFilterAttribute

    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
           //string clientip = filterContext.HttpContext.Request.UserHostAddress;
            string clientip = GetIP.GetIP4Address();
            
            if (!AddIp(clientip.Trim()))
            {
                filterContext.Result = new HttpStatusCodeResult(403);
            }
            base.OnActionExecuting(filterContext);
         }

        public bool AddIp(string clientip)
        {
            string[] clientipconvert = clientip.Trim().Split(new char[] { '.' });
            string iplist = Convert.ToString(ConfigurationManager.AppSettings["AccessList"]);
            string[] ipaccesslist = iplist.Trim().Split(new char[] { ',' });

            foreach (var ipaccesslists in ipaccesslist)
            {
                if (ipaccesslists.Trim() == clientip)
                {
                    return true;
                }

                string[] validclientip = ipaccesslists.Trim().Split(new char[] { '.' });

                bool result = true;

                for (int i = 0; i < validclientip.Length; i++)
                {
                    if (validclientip[i] != "*")
                    {
                        if (validclientip[i] != clientipconvert[i])
                        {
                            result = false;
                            break;
                        }
                    }
                }
                if (result)
                {
                    return true;
                }
            }
            return false;
        }
    }
}