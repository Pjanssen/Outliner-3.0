using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PJanssen.Outliner.NodeSorters;
using PJanssen.Outliner.Controls.Tree;
using PJanssen.Outliner.MaxUtils;
using Moq;
using Moq.Protected;
using PJanssen.Outliner.Scene;

namespace PJanssen.Outliner.Tests.NodeSorters
{
[TestClass]
public class NodeSorterTests
{
   private IMaxNode CreateMaxNode()
   {
      Mock<IMaxNode> node = new Mock<IMaxNode>();
      node.SetupGet(n => n.IsValid).Returns(true);

      return node.Object;
   }

   private MaxTreeNode CreateMaxTreeNode()
   {
      IMaxNode node = CreateMaxNode();
      Mock<MaxTreeNode> treeNode = new Mock<MaxTreeNode>(node);
      treeNode.SetupGet(tn => tn.MaxNode).Returns(node);

      return treeNode.Object;
   }

   [TestMethod]
   public void Compare_CallsInternalCompare()
   {
      Mock<NodeSorter> sorter = new Mock<NodeSorter>();
      sorter.CallBase = true;

      MaxTreeNode treeNodeX = CreateMaxTreeNode();
      MaxTreeNode treeNodeY = CreateMaxTreeNode();

      sorter.Object.Compare(treeNodeX, treeNodeY);

      sorter.Protected().Verify("InternalCompare", Times.Once(), treeNodeX.MaxNode, treeNodeY.MaxNode);
   }

   [TestMethod]
   public void SortOrder_Descending_InvertsResult()
   {
      Mock<NodeSorter> sorter = new Mock<NodeSorter>();
      sorter.CallBase = true;
      sorter.Object.SortOrder = SortOrder.Descending;

      MaxTreeNode treeNodeX = CreateMaxTreeNode();
      MaxTreeNode treeNodeY = CreateMaxTreeNode();

      int result = sorter.Object.Compare(treeNodeX, treeNodeY);

      sorter.Protected().Verify("InternalCompare", Times.Once(), treeNodeY.MaxNode, treeNodeX.MaxNode);
   }

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
