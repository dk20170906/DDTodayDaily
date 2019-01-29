using DDTodayDaily.EF;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;

namespace DDTodayDaily
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            Database.SetInitializer<DailyDBContext>(new DropCreateDatabaseIfModelChanges<DailyDBContext>());

        }
    }
}
