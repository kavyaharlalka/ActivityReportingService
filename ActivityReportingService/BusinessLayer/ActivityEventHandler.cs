using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Caching;

namespace ActivityReportingService.BusinessLayer
{
    internal class ActivityEventHandler
    {
        private const int ExpirationTimeInHours = 12;

        private Cache _activityEventValueCache;
        private List<int> _activityEventValueList;

        private readonly object _lockList = new object();

        internal ActivityEventHandler()
        {
            _activityEventValueList = new List<int>();
            _activityEventValueCache = new Cache();
        }

        internal void AddActivityEvent(int value, DateTime timeStamp)
        {
            lock (_lockList)
            {
                _activityEventValueList.Add(value);
            }

            _activityEventValueCache.Add(Guid.NewGuid().ToString(), value, null, timeStamp.AddMinutes(1), Cache.NoSlidingExpiration, CacheItemPriority.Default, RemoveItemFromListOnExpiration);
        }

        internal int GetTotal()
        {
            lock (_lockList)
            {
                return _activityEventValueList.Sum();
            }
        }

        private void RemoveItemFromListOnExpiration(string key, object value, CacheItemRemovedReason reason)
        {
            lock (_lockList)
            {
                _activityEventValueList.Remove((int)value);
            }
        }
    }
}