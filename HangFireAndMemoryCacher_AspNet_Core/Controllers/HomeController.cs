using HangFireAndMemoryCacher_AspNet_Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HangFireAndMemoryCacher_AspNet_Core.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            string cacheKey = typeof(Vendor).ToString();
            IList<Vendor> vendorList = new List<Vendor>();
            IList<Vendor> items = MemoryCacher.GetValue(cacheKey) as IList<Vendor>;
            if (items == null || items.Count == 0)
            {
                #region old code
                //connectionFactory = ConnectionHelper.GetConnection(username, password);
                //using (var context = new DbContext(connectionFactory))
                //{
                //    var rep = new VendorRepository(context);
                //    var data = rep.GetList(companyCode, storeCode);
                //    foreach (var d in data)
                //    {
                //        d.VendorAdditionInfo = new VendorAdditionInfoRepository(context).Get(d.COMPANY_CODE, d.VENDOR_CODE);
                //    }
                //    items = data;
                //    MemoryCacher.Add(cacheKey, items, DateTimeOffset.UtcNow.AddDays(10));
                //}
                #endregion

                Vendor vendor = new Vendor{ VENDOR_CODE = "1001", VENDOR_NAME = "ACI LTD", ADDRESS = "Dhaka", POSTAL_CODE = "1235" };
                vendorList.Add(vendor);
                Vendor vendor1 = new Vendor { VENDOR_CODE = "1001", VENDOR_NAME = "ACI LTD", ADDRESS = "Dhaka", POSTAL_CODE = "1235" };
                vendorList.Add(vendor1);
                items = vendorList;
                MemoryCacher.Add(cacheKey, items, DateTimeOffset.UtcNow.AddDays(10));
            }
            var dt = items;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}


// step 1 : install nuget pakage: using System.Runtime.Caching;
// step 1 :  Add MemoryCacher Class