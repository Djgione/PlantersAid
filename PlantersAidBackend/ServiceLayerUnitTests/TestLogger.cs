using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlantersAid.ServiceLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayerUnitTests
{
    [TestClass]
    public class TestLogger
    {
        [TestMethod]
        public void TestLogCreation_Success()
        {
            FlatFileLogger log = new FlatFileLogger();
            log.Log("f");
            DateTime current = DateTime.UtcNow;
            var storage = Environment.GetEnvironmentVariable("plantersAidLogFiles");

            Assert.IsTrue(File.Exists(storage + current.Year + "-" + current.Month + "-" + current.Day + ".txt"));

        }
    }
}
