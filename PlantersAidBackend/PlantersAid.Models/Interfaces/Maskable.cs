using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantersAid.Models.Interfaces
{
    public abstract class Maskable
    {
        public String Mask()
        {
            return "XXXXXXXXXX";
        }
        public override abstract String ToString();
    }
}
