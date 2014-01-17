using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PJanssen.UnitTesting;
using PJanssen.Outliner;

namespace UnitTestProject1
{
   [TestClass]
   public class OutlinerVersionConverterTests
   {
      [TestMethod]
      public void ConvertFrom_String()
      {
         ParameterizedTest<string, OutlinerVersion> tester = new ParameterizedTest<string, OutlinerVersion>();
         tester.Method = delegate(string input)
         {
            OutlinerVersionConverter converter = new OutlinerVersionConverter();
            return converter.ConvertFrom(input) as OutlinerVersion;
         };

         tester.Test("1.0.0", new OutlinerVersion(1, 0, 0));
         tester.Test("1.0.0    ", new OutlinerVersion(1, 0, 0));
         tester.Test("1.0.0 alpha", new OutlinerVersion(1, 0, 0, ReleaseStage.Alpha));
         tester.Test("1.0.0 beta", new OutlinerVersion(1, 0, 0, ReleaseStage.Beta));
         tester.Test("2.3.4", new OutlinerVersion(2, 3, 4));
      }
   }
}
