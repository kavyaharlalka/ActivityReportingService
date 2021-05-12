using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

using Shouldly;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

using ActivityReportingService.BusinessLayer;

namespace ActivityReportingService.Tests.BusinessLayer
{
    /// <summary>
    /// This was not a part of the requirement document. But in production, Unit tests are a necessity, hence added an example for the class ActivityEventBusinessLayer.
    /// These UTs are without Shims since it requires Enterprise, hence the accidental coverage is high. With Shims, it would be much lower.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ActivityEventBusinessLayerTests
    {
        private const string DummyKey = "DummyKey";
        private const int DummyValue = 1;

        ActivityEventBusinessLayer _activityEventBusinessLayer;
        PrivateType _privateType;
        PrivateObject _privateObject;

        [SetUp]
        public void Initialize()
        {
            _activityEventBusinessLayer = new ActivityEventBusinessLayer();
            _privateType = new PrivateType(typeof(ActivityEventBusinessLayer));
            _privateObject = new PrivateObject(_activityEventBusinessLayer);
        }

        [Test]
        public void AddActivityEvent_CalledWithNewData_ShouldBeAddedToDictionary()
        {
            // Arrange & Act
            _activityEventBusinessLayer.AddActivityEvent(DummyKey, DummyValue, DateTime.Now);
            _activityEventBusinessLayer.AddActivityEvent(DummyKey, DummyValue + 1, DateTime.Now);

            // Assert
            object resultObject = _privateType.GetStaticFieldOrProperty("_activityKeyToActivityHandlerMap");
            resultObject.ShouldBeOfType(typeof(ConcurrentDictionary<string, ActivityEventHandler>));
            ConcurrentDictionary<string, ActivityEventHandler> result = (ConcurrentDictionary<string, ActivityEventHandler>)resultObject;
            result.ShouldContainKey(DummyKey);
            result[DummyKey].GetTotal().ShouldBe(DummyValue + DummyValue + 1);
        }

        [Test]
        public void GetActivityEventTotal_NoData_ShouldReturnNegativeOne()
        {
            // Arrange
            var activityKeyToActivityHandlerMapDummyValue = new ConcurrentDictionary<string, ActivityEventHandler>();
            _privateType.SetStaticFieldOrProperty("_activityKeyToActivityHandlerMap", activityKeyToActivityHandlerMapDummyValue);

            // Act & Assert
            _activityEventBusinessLayer.GetActivityEventTotal(DummyKey).ShouldBe(-1);
        }

        [Test]
        public void GetActivityEventTotal_CalledWithNewData_ShouldReturnValue()
        {
            // Arrange
            var activityEventHandler = new ActivityEventHandler();
            activityEventHandler.AddActivityEvent(DummyValue, DateTime.Now);
            var activityKeyToActivityHandlerMapDummyValue = new ConcurrentDictionary<string, ActivityEventHandler>();
            activityKeyToActivityHandlerMapDummyValue.TryAdd(DummyKey, activityEventHandler);
            _privateType.SetStaticFieldOrProperty("_activityKeyToActivityHandlerMap", activityKeyToActivityHandlerMapDummyValue);
            
            // Act & Assert
            _activityEventBusinessLayer.GetActivityEventTotal(DummyKey).ShouldBe(DummyValue);
        }
    }
}
