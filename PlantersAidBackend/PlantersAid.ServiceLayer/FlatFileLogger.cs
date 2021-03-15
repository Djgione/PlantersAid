using PlantersAid.ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantersAid.ServiceLayer
{
    public class FlatFileLogger : ILogger
    {
        private readonly String _logFolderLocation;

        public FlatFileLogger()
        {
            _logFolderLocation = Environment.GetEnvironmentVariable()
        }

        public void Log(string str)
        {
            
        }

        public void Log(object obj)
        {
            
        }

        private String CreateTimestamp()
        {

        }
    }
}
