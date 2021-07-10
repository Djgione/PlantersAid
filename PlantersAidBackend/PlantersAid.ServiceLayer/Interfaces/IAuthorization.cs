using PlantersAid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantersAid.ServiceLayer.Interfaces
{
    public interface IAuthorization
    {
        public void Authorize(in AuthzRequest request);
    }
}
