using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Outliner.NodeSorters;
using Outliner.Controls.Tree;
using Outliner.MaxUtils;
using Moq;

namespace Outliner.Tests.NodeSorters
{
[TestClass]
public class NodeSorterTests
{
   [TestMethod]
   public void RequiresSort_NullSorter_ReturnsFalse()
   {
      Assert.IsFalse(NodeSorter.RequiresSort(null, typeof(int)));
   }

   [TestMethod]
   [ExpectedException(typeof(ArgumentNullException))]
   public void RequiresSort_NullType_ThrowsException()
   {
      Mock<NodeSorter> sorter = new Mock<NodeSorter>();
      NodeSorter.RequiresSort(sorter.Object, null);
   }

   [TestMethod]
   public void RequiresSort_SorterType_ReturnsTrue()
   {
      NodeSorter sorter = new Mock<NodeSorter>().Object;
      
      Boolean result = NodeSorter.RequiresSort(sorter, sorter.GetType());

      Assert.IsTrue(result);
   }

   [TestMethod]
   public void RequiresSort_NonSorterType_ReturnsFalse()
   {
      NodeSorter sorter = new Mock<NodeSorter>().Object;

      Boolean result = NodeSorter.RequiresSort(sorter, typeof(int));

      Assert.IsFalse(result);
   }

   [TestMethod]
   public void RequiresSort_SecondarySorterType_ReturnsTrue()
   {
      Mock<NodeSorter> sorter = new Mock<NodeSorter>();
      sorter.SetupGet(s => s.SecondarySorter).Returns(new NodePropertySorter());

      Boolean result = NodeSorter.RequiresSort(sorter.Object, typeof(NodePropertySorter));

      Assert.IsTrue(result);
   }

   [TestMethod]
   public void RequiresSort_NodeProperty()
   {
      Mock<NodeSorter> sorter = new Mock<NodeSorter>();
      sorter.SetupGet(s => s.SecondarySorter).Returns(new NodePropertySorter(NodeProperty.BoxMode));

      Assert.IsFalse(NodeSorter.RequiresSort(sorter.Object, NodeProperty.None));
      Assert.IsFalse(NodeSorter.RequiresSort(sorter.Object, NodeProperty.IsHidden));
      Assert.IsTrue(NodeSorter.RequiresSort(sorter.Object, NodeProperty.BoxMode));
      Assert.IsTrue(NodeSorter.RequiresSort(sorter.Object, NodeProperty.BackfaceCull | NodeProperty.BoxMode));
   }
}
}
