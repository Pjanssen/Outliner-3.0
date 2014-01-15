using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PJanssen.UnitTesting;

namespace PJanssen.Outliner.UnitTests
{
   [TestClass]
   public class OutlinerVersionTests
   {
      [TestMethod]
      public void CompareTo()
      {
         ParameterizedTest<OutlinerVersion, OutlinerVersion, int> tester = new ParameterizedTest<OutlinerVersion, OutlinerVersion, int>();
         tester.Method = delegate(OutlinerVersion vX, OutlinerVersion vY)
         {
            return vX.CompareTo(vY);
         };

         tester.Test(new OutlinerVersion(), null, 1);
         tester.Test(new OutlinerVersion(), new OutlinerVersion(), 0);
         tester.Test(new OutlinerVersion(1, 0, 0), new OutlinerVersion(2, 0, 0), -1);
         tester.Test(new OutlinerVersion(1, 0, 0), new OutlinerVersion(1, 1, 0), -1);
         tester.Test(new OutlinerVersion(1, 0, 0), new OutlinerVersion(1, 0, 1), -1);
         tester.Test(new OutlinerVersion(1, 0, 0, ReleaseStage.Alpha), new OutlinerVersion(1, 0, 0, ReleaseStage.Beta), -1);
         tester.Test(new OutlinerVersion(1, 0, 0, ReleaseStage.Beta), new OutlinerVersion(1, 0, 0, ReleaseStage.Release), -1);
         tester.Test(new OutlinerVersion(2, 0, 0), new OutlinerVersion(1, 0, 0, 0), 1);
      }

      [TestMethod]
      public void ToString_FormatsAllComponents()
      {
         OutlinerVersion version = new OutlinerVersion(1, 2, 3);

         Assert.AreEqual("1.2.3", version.ToString());
      }

      [TestMethod]
      public void ToString_AlphaVersion()
      {
         OutlinerVersion version = new OutlinerVersion(1, 2, 3, ReleaseStage.Alpha);

         Assert.AreEqual("1.2.3 alpha", version.ToString());
      }

      [TestMethod]
      public void ToString_BetaVersion()
      {
         OutlinerVersion version = new OutlinerVersion(1, 2, 3, ReleaseStage.Beta);

         Assert.AreEqual("1.2.3 beta", version.ToString());
      }
   }
}
