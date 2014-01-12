using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PJanssen.Outliner.Filters;
using Moq;
using Moq.Protected;

namespace PJanssen.Outliner.Tests.Filters
{
[TestClass]
public class FilterTest
{
   [TestMethod]
   public void Constructor_SetsDefaultValues()
   {
      Mock<Filter<Boolean>> filter = new Mock<Filter<Boolean>>();
      filter.CallBase = true;
      Assert.IsTrue(filter.Object.Enabled, "Initial enabled value.");
      Assert.IsFalse(filter.Object.Invert, "Initial invert value.");
   }

   [TestMethod]
   public void Enabled_SetNewValue_RaisesFilterChangedEvent()
   {
      Boolean filterChangedRaised = false;
      Mock<Filter<Boolean>> filter = new Mock<Filter<bool>>();
      filter.CallBase = true;
      filter.Object.FilterChanged += (sender, args) => filterChangedRaised = true;

      filter.Object.Enabled = false;

      Assert.IsFalse(filter.Object.Enabled);
      Assert.IsTrue(filterChangedRaised);
   }

   [TestMethod]
   public void Enabled_SetFalse_ShowNodeAlwaysReturnsTrue()
   {
      Mock<Filter<Boolean>> filter = new Mock<Filter<bool>>();
      filter.CallBase = true;
      filter.Protected().Setup<Boolean>("ShowNodeInternal", false)
                        .Returns(false);
      filter.Protected().Setup<Boolean>("ShowNodeInternal", true)
                        .Returns(true);

      Assert.IsTrue(filter.Object.ShowNode(true));
      Assert.IsFalse(filter.Object.ShowNode(false));

      filter.Object.Enabled = false;

      Assert.IsTrue(filter.Object.ShowNode(true));
      Assert.IsTrue(filter.Object.ShowNode(false));
   }

   [TestMethod]
   public void Invert_SetNewValue_RaisesFilterChangedEvent()
   {
      Boolean filterChangedRaised = false;
      Mock<Filter<Boolean>> filter = new Mock<Filter<bool>>();
      filter.CallBase = true;
      filter.Object.FilterChanged += (sender, args) => filterChangedRaised = true;

      filter.Object.Invert = true;

      Assert.IsTrue(filter.Object.Invert);
      Assert.IsTrue(filterChangedRaised);
   }

   [TestMethod]
   public void Invert_SetTrue_InversesShowNode()
   {
      Mock<Filter<Boolean>> filter = new Mock<Filter<bool>>();
      filter.CallBase = true;
      filter.Protected().Setup<Boolean>("ShowNodeInternal", false)
                        .Returns(false);
      filter.Protected().Setup<Boolean>("ShowNodeInternal", true)
                        .Returns(true);

      Assert.IsTrue(filter.Object.ShowNode(true));
      Assert.IsFalse(filter.Object.ShowNode(false));

      filter.Object.Invert = true;

      Assert.IsFalse(filter.Object.ShowNode(true));
      Assert.IsTrue(filter.Object.ShowNode(false));
   }
}
}
