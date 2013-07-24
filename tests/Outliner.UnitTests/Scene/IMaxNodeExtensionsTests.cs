using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Outliner.Scene;

namespace Outliner.Tests.Scene
{
   [TestClass]
   public class IMaxNodeExtensionsTests
   {
      [TestMethod]
      [ExpectedException(typeof(ArgumentNullException))]
      public void GetBaseObjects_Null_ThrowsException()
      {
         IEnumerable<IMaxNode> nodes = null;
         nodes.GetBaseObjects();
      }

      [TestMethod]
      public void GetBaseObjects()
      {
         Mock<IMaxNode> mockNodeA = new Mock<IMaxNode>();
         mockNodeA.SetupGet(n => n.BaseObject).Returns(1);
         Mock<IMaxNode> mockNodeB = new Mock<IMaxNode>();
         mockNodeB.SetupGet(n => n.BaseObject).Returns(42);

         List<IMaxNode> nodes = new List<IMaxNode>() { mockNodeA.Object, mockNodeB.Object };
         IEnumerable<Object> result = nodes.GetBaseObjects();

         CollectionAssert.AreEqual(new List<Object>() { 1, 42 }, result.ToList());
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentNullException))]
      public void IsParentOfSelected_Null_ThrowsException()
      {
         IMaxNode node = null;
         node.IsParentOfSelected();
      }

      private static IMaxNode SetupSelectedHierarchy(Boolean parentSelected, Boolean childSelected)
      {
         Mock<IMaxNode> mockParent = new Mock<IMaxNode>();
         Mock<IMaxNode> mockChild = new Mock<IMaxNode>();
         mockParent.SetupGet(n => n.IsSelected).Returns(parentSelected);
         mockChild.SetupGet(n => n.IsSelected).Returns(childSelected);
         mockParent.SetupGet(n => n.ChildNodes)
                   .Returns(Enumerable.Repeat(mockChild.Object, 1));
         mockChild.SetupGet(n => n.Parent)
                  .Returns(mockParent.Object);
         return mockParent.Object;
      }

      [TestMethod]
      public void IsParentOfSelected_NonSelected_ReturnsFalse()
      {
         IMaxNode node = SetupSelectedHierarchy(false, false);

         Assert.IsFalse(node.IsParentOfSelected());
      }

      [TestMethod]
      public void IsParentOfSelected_Selected_ReturnsTrue()
      {
         IMaxNode node = SetupSelectedHierarchy(true, false);

         Assert.IsTrue(node.IsParentOfSelected());
      }

      [TestMethod]
      public void IsParentOfSelected_SelectedChild_ReturnsTrue()
      {
         IMaxNode node = SetupSelectedHierarchy(false, true);

         Assert.IsTrue(node.IsParentOfSelected());
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentNullException))]
      public void IsChildOfSelected_Null_ThrowsException()
      {
         IMaxNode node = null;
         node.IsChildOfSelected();
      }

      [TestMethod]
      public void IsChildOfSelected_NonSelected_ReturnsFalse()
      {
         IMaxNode node = SetupSelectedHierarchy(false, false);

         Assert.IsFalse(node.ChildNodes.First().IsChildOfSelected());
      }

      [TestMethod]
      public void IsChildOfSelected_Selected_ReturnsTrue()
      {
         IMaxNode node = SetupSelectedHierarchy(false, true);

         Assert.IsTrue(node.ChildNodes.First().IsChildOfSelected());
      }

      [TestMethod]
      public void IsChildOfSelected_SelectedParent_ReturnsTrue()
      {
         IMaxNode node = SetupSelectedHierarchy(true, false);

         Assert.IsTrue(node.ChildNodes.First().IsChildOfSelected());
      }
   }
}
