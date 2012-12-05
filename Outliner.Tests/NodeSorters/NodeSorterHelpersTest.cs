using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Outliner.NodeSorters;
using Outliner.Controls.Tree;
using Outliner.MaxUtils;

namespace Outliner.Tests.NodeSorters
{
[TestClass]
public class NodeSorterHelpersTest
{
   private class MockSorter : NodeSorter
   {
      protected override int InternalCompare(TreeNode x, TreeNode y)
      {
         return 0;
      }
   }

   private class MockPropertySorter : NodePropertySorter
   {
      public MockPropertySorter(NodeProperty prop) 
         : base(prop) { }
   }

   [TestMethod]
   public void RequiresSortForTypeTest()
   {
      Assert.IsFalse(NodeSorterHelpers.RequiresSort(null, typeof(MockSorter)), "sorter parameter = null");
      Assert.IsFalse(NodeSorterHelpers.RequiresSort(new MockSorter(), null), "type parameter = null");

      Assert.IsFalse(NodeSorterHelpers.RequiresSort(new MockSorter(), typeof(MockPropertySorter)), "single filter, type not the same");
      Assert.IsTrue(NodeSorterHelpers.RequiresSort(new MockSorter(), typeof(MockSorter)), "single filter, same type");

      MockSorter a = new MockSorter();
      a.SecondarySorter = new MockSorter();
      a.SecondarySorter.SecondarySorter = new MockPropertySorter(NodeProperty.None);
      Assert.IsFalse(NodeSorterHelpers.RequiresSort(a, typeof(Object)));
      Assert.IsTrue(NodeSorterHelpers.RequiresSort(a, typeof(MockSorter)));
      Assert.IsTrue(NodeSorterHelpers.RequiresSort(a, typeof(MockPropertySorter)));
   }

   [TestMethod]
   public void RequiresSortForPropertyTest()
   {
      Assert.IsFalse(NodeSorterHelpers.RequiresSort(null, NodeProperty.AllEdges), "sorter parameter = null");
      Assert.IsFalse(NodeSorterHelpers.RequiresSort(new MockSorter(), NodeProperty.Name), "not a property sorter");

      MockPropertySorter a = new MockPropertySorter(NodeProperty.Name);
      Assert.IsFalse(NodeSorterHelpers.RequiresSort(a, NodeProperty.None), "none property");
      Assert.IsFalse(NodeSorterHelpers.RequiresSort(a, NodeProperty.PrimaryVisibility), "non-matching property");
      Assert.IsTrue(NodeSorterHelpers.RequiresSort(a, NodeProperty.Name), "exact matching property");
      Assert.IsTrue(NodeSorterHelpers.RequiresSort(a, NodeProperty.Name | NodeProperty.IsHidden | NodeProperty.IsFrozen), "combination of properties");

      MockSorter b = new MockSorter();
      b.SecondarySorter = new MockPropertySorter(NodeProperty.IsHidden);
      b.SecondarySorter.SecondarySorter = new MockPropertySorter(NodeProperty.Name);
      Assert.IsTrue(NodeSorterHelpers.RequiresSort(b, NodeProperty.Name), "matching property in nested sorter");
   }
}
}
