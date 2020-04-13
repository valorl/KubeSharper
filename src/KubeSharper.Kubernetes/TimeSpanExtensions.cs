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
            return timeSpan + timeSpan.GetJitter(maxFactor);
        }
        /// <summary>
        /// Gets random jitter based on <paramref name="timeSpan"/>,
        /// returning a <c>TimeSpan</c> between <c>TimeSpan.Zero</c>
        /// and <c><paramref name="timeSpan"/>*<paramref name="maxFactor"/></c>
        /// </summary>
        ///
        public static TimeSpan GetJitter(this TimeSpan timeSpan, double maxFactor)
        {
            var ms = timeSpan.TotalMilliseconds;
            var rand = new Random();
            var jitter = rand.NextDouble() * maxFactor * ms;
            return TimeSpan.FromMilliseconds(jitter);
        }
    }
}
