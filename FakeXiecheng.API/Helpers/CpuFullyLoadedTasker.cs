using System;
using System.Diagnostics;

namespace FakeXiecheng.API.Helpers
{
    public static class CpuFullyLoadedTasker
    {
        /// <summary>
        /// Full CPU load for 5 seconds
        /// </summary>
        public static int ComplicatCalculation()
        {
            var watch = new Stopwatch();
            watch.Start();
            while (true)
            {
                if (watch.ElapsedMilliseconds > 5000)
                {
                    break;
                }
            }

            return 0; // 随便return个东西，这不重要
        }
    }
}
