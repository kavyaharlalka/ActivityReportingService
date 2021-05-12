using System;

namespace ActivityReportingService.BusinessLayer
{
    public interface IActivityEventBusinessLayer
    {
        void AddActivityEvent(string key, int value, DateTime timeStamp);
        int GetActivityEventTotal(string key);
    }
}
