using System.Web;
using System.Web.Mvc;

namespace ProvaDeConceito2FA
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
