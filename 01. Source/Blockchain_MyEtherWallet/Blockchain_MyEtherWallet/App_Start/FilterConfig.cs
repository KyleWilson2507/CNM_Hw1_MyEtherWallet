using System.Web;
using System.Web.Mvc;

namespace Blockchain_MyEtherWallet
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
