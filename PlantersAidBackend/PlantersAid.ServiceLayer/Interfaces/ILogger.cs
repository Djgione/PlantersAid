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
        public void Log(String str);
        public void Log(Object obj);
    }
}
