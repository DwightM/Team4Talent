using System.Web.Mvc;

namespace Quartz.Management.Presentation.Mvc.Controllers
{
    public class SchedulerController
       : Controller
    {
        // Methods.
        public ActionResult Overview()
        {
            var _service = new WcfServiceLayer.SchedulerReadFacadeServiceClient();
            var _overviewData = _service.ReadOverviewData();

            return View(_overviewData);
        }


        public ActionResult Detail(string name, string groupName)
        {
            //name = "MyJob";
            //groupName = "MyOwnGroup";
            var _service = new WcfServiceLayer.SchedulerReadFacadeServiceClient();
            var _jobDetail = _service.ReadJobDetail(name, groupName);

            return View(_jobDetail);
        }
    }
}