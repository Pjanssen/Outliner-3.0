using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PJanssen.Outliner.IntegrationTests
{
   /// <summary>
   /// A generic test baseclass, which automates 3dsMax scene cleanup after each test.
   /// </summary>
   [TestClass]
   public abstract class MaxIntegrationTest
   {
      [TestCleanup]
      public void TestCleanup()
      {
         MaxRemoting.ResetScene();
      }
   }
}
