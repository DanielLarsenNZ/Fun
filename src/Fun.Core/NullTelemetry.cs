using System;
using System.Collections.Generic;

namespace Fun
{
    public class NullTelemetry : ITelemetry
    {
        public void TrackAvailability(string name, DateTimeOffset timeStamp, TimeSpan duration, string runLocation, bool success, string message = null, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            return;
        }

        public void TrackDependency(string dependencyTypeName, string target, string dependencyName, string data, DateTimeOffset startTime, TimeSpan duration, string resultCode, bool success)
        {
            return;
        }

        public void TrackDependency(string dependencyTypeName, string dependencyName, string data, DateTimeOffset startTime, TimeSpan duration, bool success)
        {
            return;
        }

        public void TrackEvent(string eventName, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            return;
        }

        public void TrackException(Exception exception, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            return;
        }

        public void TrackMetric(string name, double value, IDictionary<string, string> properties = null)
        {
            return;
        }

        public void TrackPageView(string name)
        {
            return;
        }

        public void TrackRequest(string name, DateTimeOffset startTime, TimeSpan duration, string responseCode, bool success)
        {
            return;
        }

        public void TrackTrace(string message, SeverityLevel severityLevel, IDictionary<string, string> properties)
        {
            return;
        }

        public void TrackTrace(string message, IDictionary<string, string> properties)
        {
            return;
        }

        public void TrackTrace(string message, SeverityLevel severityLevel)
        {
            return;
        }

        public void TrackTrace(string message)
        {
            return;
        }
    }
}
