using System;
using System.Collections.Generic;
using System.Text;

namespace KubeSharper.Utils
{
    public static class TimeSpanExtensions
    {
        /// <summary>
        /// Adds random jitter to <paramref name="timeSpan"/>,
        /// returning a new <c>TimeSpan</c> between <paramref name="timeSpan"/>
        /// and <c><paramref name="timeSpan"/>*<paramref name="maxFactor"/></c>
        /// </summary>
        ///
        public static TimeSpan WithJitter(this TimeSpan timeSpan, double maxFactor)
        {
            var ms = timeSpan.TotalMilliseconds;
            var rand = new Random();
            var jitter = rand.NextDouble() * maxFactor * ms;
            return TimeSpan.FromMilliseconds(ms + jitter);
        }
    }
}
