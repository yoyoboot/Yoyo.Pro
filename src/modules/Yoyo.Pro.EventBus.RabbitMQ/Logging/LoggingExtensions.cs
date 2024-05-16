using System;
using Microsoft.Extensions.Logging;

namespace Yoyo.Pro.Logging
{
    public static class LoggingExtensions
    {
        public static void Trace(this ILogger logger, string message, Exception ex = null)
        {
            if (logger == null)
            {
                Console.WriteLine($"LOG Trace: {message}  Exception:{ex?.ToString()}");
                return;
            }
            logger.LogTrace(ex, message);
        }


        public static void Debug(this ILogger logger, string message, Exception ex = null)
        {
            if (logger == null)
            {
                Console.WriteLine($"LOG Debug: {message}  Exception:{ex?.ToString()}");
                return;
            }
            logger.LogDebug(ex, message);
        }




        public static void Warn(this ILogger logger, string message, Exception ex = null)
        {
            if (logger == null)
            {
                Console.WriteLine($"LOG Warn: {message}  Exception:{ex?.ToString()}");
                return;
            }
            logger.LogWarning(ex, message);
        }


        public static void Error(this ILogger logger, string message, Exception ex = null)
        {
            if (logger == null)
            {
                Console.WriteLine($"LOG Error: {message}  Exception:{ex?.ToString()}");
                return;
            }
            logger.LogError(ex, message);
        }


        public static void Critical(this ILogger logger, string message, Exception ex = null)
        {
            if (logger == null)
            {
                Console.WriteLine($"LOG Critical: {message}  Exception:{ex?.ToString()}");
                return;
            }
            logger.LogCritical(ex, message);
        }
    }
}
