using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityReportingService.BusinessLayer
{
    public interface IActivityEventBusinessLayer
    {
        void AddActivityEvent(string key, int value, DateTime timeStamp);
        int GetActivityEventTotal(string key);
    }
}
