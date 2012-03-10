using System;
using Autodesk.Max;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Outliner.Controls.FiltersBase;
using Outliner.Filters;
using Outliner.Scene;

namespace Outliner_Tests
{
   [TestClass()]
   public class FrozenFilterTest
   {
      /// <summary>
      ///A test for ShowNode
      ///</summary>
      [TestMethod()]
      public void ShowNodeNullTest()
      {
         FrozenFilter target = new FrozenFilter();
         IMaxNodeWrapper data = null;
         FilterResult expected = FilterResult.Show;
         FilterResult actual = target.ShowNode(data);
         Assert.AreEqual(expected, actual);
      }
   }
}
