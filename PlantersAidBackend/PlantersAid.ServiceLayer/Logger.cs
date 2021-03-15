using PlantersAid.ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantersAid.ServiceLayer
{
    public static class Logger
    {
        private static IEnumerable<ILogger> _loggers;

        static Logger()
        {
            _loggers = new List<ILogger> 
            {
                new FlatFileLogger()
            };
        }

        public static void Log(string str)
        {
            foreach(ILogger logger in _loggers)
            {
                logger.Log(str);
            }
        }
    }
}
