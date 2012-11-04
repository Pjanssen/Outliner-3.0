using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Outliner.Filters;

namespace Outliner.Tests.Filters
{
internal class MockFilterTrue : Filter<Boolean>
{
   protected override bool ShowNodeInternal(bool data)
   {
      return data;
   }
}

internal class MockFilterFalse : Filter<Boolean>
{
   protected override bool ShowNodeInternal(bool data)
   {
      return !data;
   }
}

[TestClass]
public class FilterTest
{
   private Boolean filterChangedEventFired;

   [TestInitialize]
   public void TestInit()
   {
      this.filterChangedEventFired = false;
   }

   [TestMethod]
   public void ConstructorTest()
   {
      Filter<Boolean> f = new MockFilterTrue();
      Assert.IsTrue(f.Enabled, "Initial enabled value.");
      Assert.IsFalse(f.Invert, "Initial invert value.");
   }

   [TestMethod]
   public void EnabledTest()
   {
      Filter<Boolean> f = new MockFilterTrue();

      f.FilterChanged += new EventHandler(f_FilterChanged);

      f.Enabled = false;
      Assert.IsFalse(f.Enabled, "Setting enabled false should work as expected.");
      Assert.IsTrue(filterChangedEventFired, "Setting enabled value should fire FilterChanged event. 1");
      this.filterChangedEventFired = false;

      Assert.IsTrue(f.ShowNode(true), "Disabled filter should return true for ShowNode(true).");
      Assert.IsTrue(f.ShowNode(false), "Disabled filter should return true for ShowNode(false).");

      f.Enabled = true;
      Assert.IsTrue(f.Enabled, "Setting enabled true should work as expected.");
      Assert.IsTrue(filterChangedEventFired, "Setting enabled value should fire FilterChanged event. 2");

      Assert.IsTrue(f.ShowNode(true), "Enabled filter should return true for ShowNode(true).");
      Assert.IsFalse(f.ShowNode(false), "Enabled filter should return false for ShowNode(false).");
   }

   void f_FilterChanged(object sender, EventArgs e)
   {
      this.filterChangedEventFired = true;
   }

   [TestMethod]
   public void InvertTest()
   {
      Filter<Boolean> f = new MockFilterTrue();
      f.FilterChanged += new EventHandler(f_FilterChanged);

      f.Invert = true;
      Assert.IsTrue(f.Invert, "Setting invert to true should work as expected.");
      Assert.IsTrue(this.filterChangedEventFired, "Setting invert should fire FilterChanged event. 1");
      this.filterChangedEventFired = false;

      Assert.IsFalse(f.ShowNode(true), "Inverted filter should return false for ShowNode(true).");
      Assert.IsTrue(f.ShowNode(false), "Inverted filter should return true for ShowNode(false).");

      f.Invert = false;
      Assert.IsFalse(f.Invert, "Setting invert to false should work as expected.");
      Assert.IsTrue(this.filterChangedEventFired, "Setting invert should fire FilterChanged event. 2");

      Assert.IsTrue(f.ShowNode(true), "Not inverted filter should return true for ShowNode(true).");
      Assert.IsFalse(f.ShowNode(false), "Not inverted filter should return false for ShowNode(false).");

   }
}
}
