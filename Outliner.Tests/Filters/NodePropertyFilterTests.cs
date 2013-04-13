using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Outliner.Filters;
using Outliner.MaxUtils;
using Outliner.Scene;

namespace Outliner.Tests.Filters
{
   [TestClass]
   public class NodePropertyFilterTests
   {
      [TestMethod]
      public void Property_SetNewValue_RaisesFilterChangedEvent()
      {
         Boolean filterChangedEventRaised = false;
         NodePropertyFilter filter = new NodePropertyFilter();
         filter.FilterChanged += (sender, args) => filterChangedEventRaised = true;

         filter.Property = BooleanNodeProperty.BoxMode;

         Assert.IsTrue(filterChangedEventRaised);
      }

      [TestMethod]
      public void ShowNode_GetNodePropertyReturningTrue_ReturnsTrue()
      {
         NodePropertyFilter filter = new NodePropertyFilter(BooleanNodeProperty.BoxMode);
         Mock<IMaxNode> node = new Mock<IMaxNode>();
         node.Setup(n => n.GetNodeProperty(BooleanNodeProperty.BoxMode)).Returns(true);

         Boolean result = filter.ShowNode(node.Object);

         Assert.IsTrue(result);
         node.Verify(n => n.GetNodeProperty(BooleanNodeProperty.BoxMode), Times.Once);
      }

      [TestMethod]
      public void ShowNode_GetNodePropertyReturningFalse_ReturnsFalse()
      {
         NodePropertyFilter filter = new NodePropertyFilter(BooleanNodeProperty.BoxMode);
         Mock<IMaxNode> node = new Mock<IMaxNode>();
         node.Setup(n => n.GetNodeProperty(BooleanNodeProperty.BoxMode)).Returns(false);

         Boolean result = filter.ShowNode(node.Object);

         Assert.IsFalse(result);
         node.Verify(n => n.GetNodeProperty(BooleanNodeProperty.BoxMode), Times.Once);
      }
   }
}
