using System;
using System.Web.Http;
using ActivityReportingService.BusinessLayer;
using ActivityReportingService.Models;

namespace ActivityReportingService.Controllers
{
    public class ValuesController : ApiController
    {
        private readonly IActivityEventBusinessLayer _activityEventBusinessLayer;

        public ValuesController()
        {
            this._activityEventBusinessLayer = new ActivityEventBusinessLayer();
        }

        [Route("activity/{key}")]
        [HttpPost]
        public IHttpActionResult PostActivityEvent(string key, [FromBody]ActivityEventModel value)
        {
            try
            {
                if (value.value < 0)
                {
                    throw new InvalidOperationException("Value cannot be negative");
                }

                _activityEventBusinessLayer.AddActivityEvent(key, value.value, DateTime.Now);
                return Json("{}");
            }
            catch (Exception oEx)
            {
                return InternalServerError(oEx);
            }
        }

        [Route("activity/{key}/total")]
        [HttpGet]
        public IHttpActionResult GetActivityEventsTotal(string key)
        {
            try
            {
                int eventTotal = _activityEventBusinessLayer.GetActivityEventTotal(key);
                if (eventTotal == -1)
                {
                    return NotFound();
                }

                var result = new ActivityEventModel { value = eventTotal };
                return Json(result);
            }
            catch (Exception oEx)
            {
                return InternalServerError(oEx);
            }
        }
    }
}
