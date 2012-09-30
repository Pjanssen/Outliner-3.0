using Outliner.Filters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Outliner.Scene;
using System;
using System.Collections.Generic;

namespace Outliner.Tests.Filters
{
[TestClass()]
public class NodeFilterCollectionTest
{
   public class MockFilter : Filter<Boolean>
   {
      public override Boolean ShowNode(bool data)
      {
         return data ? true : false;
      }
   }

   public class MockPermanentFilter : MockFilter
   {
      public override bool AlwaysEnabled
      {
         get { return true; }
      }
   }

   public List<Boolean> GetChildNodesFalse(Boolean b)
   {
      if (b)
         return new List<Boolean>() { false, false };
      else
         return new List<Boolean>();
   }

   public List<Boolean> GetChildNodesTrue(Boolean b)
   {
      if (!b)
         return new List<Boolean>() { true, true };
      else
         return new List<Boolean>();
   }

   [TestMethod()]
   public void ShowNodeTest()
   {
      FilterCollection<Boolean> target = new FilterCollection<Boolean>();
      target.Enabled = true;

      Assert.AreEqual(false, target.ShowNode(true), "No filters, true value");
      Assert.AreEqual(false, target.ShowNode(false), "No filters, false value");

      target.Add(new MockFilter());
      Assert.AreEqual(false, target.ShowNode(true), "MockFilter, enabled, true");
      Assert.AreEqual(false, target.ShowNode(false), "MockFilter, enabled, false");

      target.Enabled = false;
      Assert.AreEqual(false, target.ShowNode(true), "MockFilter, !enabled, true");
      Assert.AreEqual(false, target.ShowNode(false), "MockFilter, !enabled, false");

      target = new FilterCollection<Boolean>();
      target.Enabled = true;
      target.Add(new MockPermanentFilter());
      Assert.AreEqual(false, target.ShowNode(true), "MockPermanentFilter, enabled, true");
      Assert.AreEqual(false, target.ShowNode(false), "MockPermanentFilter, enabled, false");

      target.Enabled = false;
      Assert.AreEqual(false, target.ShowNode(true), "MockPermanentFilter, !enabled, true");
      Assert.AreEqual(false, target.ShowNode(false), "MockPermanentFilter, !enabled, false");
   }

   [TestMethod()]
   public void AddContainsTest()
   {
      FilterCollection<Boolean> target = new FilterCollection<Boolean>();
      MockFilter filter = new MockFilter();
      Assert.IsFalse(target.Contains(filter), "Initially doesn't contain filter");
      target.Add(filter);
      Assert.IsTrue(target.Contains(filter), "Contains filter after adding");
   }

   [TestMethod()]
   public void RemoveTest()
   {
      FilterCollection<Boolean> target = new FilterCollection<Boolean>();
      MockFilter filterA = new MockFilter();
      MockFilter filterB = new MockFilter();
      target.Add(filterA);
      Assert.IsTrue(target.Contains(filterA), "FilterA added");
      target.Add(filterB);
      Assert.IsTrue(target.Contains(filterB), "FilterB added");

      target.Remove(filterA);
      Assert.IsFalse(target.Contains(filterA), "FilterA removed");
      Assert.IsTrue(target.Contains(filterB), "FilterB not removed");
   }

   [TestMethod()]
   public void RemoveTypeTest()
   {
      FilterCollection<Boolean> target = new FilterCollection<Boolean>();
      MockFilter filterA = new MockFilter();
      MockPermanentFilter filterB = new MockPermanentFilter();
      target.Add(filterA);
      Assert.IsTrue(target.Contains(filterA), "FilterA added");
      target.Add(filterB);
      Assert.IsTrue(target.Contains(filterB), "FilterB added");

      target.Remove(typeof(MockFilter));
      Assert.IsFalse(target.Contains(filterA), "FilterA removed");
      Assert.IsTrue(target.Contains(filterB), "FilterB not removed");
   }

   [TestMethod()]
   public void ClearTest()
   {
      FilterCollection<Boolean> target = new FilterCollection<Boolean>();
      Assert.AreEqual(0, target.Count, "Initial count");
      target.Clear();
      Assert.AreEqual(0, target.Count, "Count after Clear 1");

      MockFilter filter = new MockFilter();
      target.Add(filter);
      Assert.AreEqual(1, target.Count, "Count after adding filter 2.");
      Assert.IsTrue(target.Contains(filter));
      target.Clear();
      Assert.AreEqual(0, target.Count, "Count after clear 2");
      Assert.IsFalse(target.Contains(filter));

      MockPermanentFilter permanentFilter = new MockPermanentFilter();
      target.Add(filter);
      target.Add(permanentFilter);
      Assert.AreEqual(2, target.Count, "Count after adding filters 3.");
      Assert.IsTrue(target.Contains(filter));
      Assert.IsTrue(target.Contains(permanentFilter));
      target.Clear(false);
      Assert.AreEqual(1, target.Count, "Count after clear 3");
      Assert.IsFalse(target.Contains(filter));
      Assert.IsTrue(target.Contains(permanentFilter));

      target.Add(filter);
      Assert.AreEqual(2, target.Count, "Count after adding filters 4.");
      Assert.IsTrue(target.Contains(filter));
      Assert.IsTrue(target.Contains(permanentFilter));
      target.Clear(true);
      Assert.AreEqual(0, target.Count, "Count after clear 4");
      Assert.IsFalse(target.Contains(filter));
      Assert.IsFalse(target.Contains(permanentFilter));
   }

   [TestMethod()]
   public void FilterCollectionConstructorTest()
   {
      FilterCollection<Boolean> target = new FilterCollection<Boolean>();
      Assert.IsFalse(target.Enabled);
   }

   [TestMethod()]
   public void FilterCollectionCopyConstructorTest()
   {
      FilterCollection<Boolean> collection = new FilterCollection<Boolean>();
      collection.Enabled = false;
      MockFilter filter1 = new MockFilter();
      collection.Add(filter1);
      MockPermanentFilter filter2 = new MockPermanentFilter();
      collection.Add(filter2);

      FilterCollection<Boolean> target = new FilterCollection<Boolean>(collection);
      Assert.IsFalse(target.Enabled);
      Assert.AreEqual(2, target.Count);
      Assert.IsTrue(target.Contains(filter1));
      Assert.IsTrue(target.Contains(filter2));
   }
}
}
