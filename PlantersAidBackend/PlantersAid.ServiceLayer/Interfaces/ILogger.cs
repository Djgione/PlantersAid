using PlantersAid.ServiceLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantersAid.ServiceLayer.Interfaces
{
    public interface ILogger
    {
        /// <summary>
        /// Log String  
        /// </summary>
        /// <param name="str"></param>
        public Task Log(String str);
        public Task Log(String str, LogLevel level);
        public Task Log(Exception ex);
    }
}
