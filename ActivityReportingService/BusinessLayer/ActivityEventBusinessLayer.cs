using System;
using System.Collections.Concurrent;

namespace ActivityReportingService.BusinessLayer
{
    public class ActivityEventBusinessLayer : IActivityEventBusinessLayer
    {
        private static ConcurrentDictionary<string, ActivityEventHandler> _activityKeyToActivityHandlerMap = new ConcurrentDictionary<string, ActivityEventHandler>();

        public void AddActivityEvent(string key, int value, DateTime timeStamp)
        {
            ActivityEventHandler activityEventHandler;
            if (!_activityKeyToActivityHandlerMap.TryGetValue(key, out activityEventHandler))
            {
                activityEventHandler = new ActivityEventHandler();
                _activityKeyToActivityHandlerMap[key] = activityEventHandler;
            }

            activityEventHandler.AddActivityEvent(value, timeStamp);
        }

        public int GetActivityEventTotal(string key)
        {
            ActivityEventHandler activityEventHandler;
            if (!_activityKeyToActivityHandlerMap.TryGetValue(key, out activityEventHandler))
            {
                return -1;
            }

            return activityEventHandler.GetTotal();
        }
    }
}