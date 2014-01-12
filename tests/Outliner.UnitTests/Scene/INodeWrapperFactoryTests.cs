using System;
using Autodesk.Max;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PJanssen.Outliner.Scene;

namespace PJanssen.Outliner.Tests.Scene
{
   [TestClass]
   public class INodeWrapperFactoryTests
   {
      [TestMethod]
      [ExpectedException(typeof(ArgumentNullException))]
      public void CreateMaxNode_Null_ThrowsException()
      {
         INodeWrapperFactory factory = new INodeWrapperFactory();
         factory.CreateMaxNode(null);
      }

      [TestMethod]
      public void CreateMaxNode_NonIINode_ReturnsNull()
      {
         INodeWrapperFactory factory = new INodeWrapperFactory();
         IMaxNode result = factory.CreateMaxNode(42);
         Assert.IsNull(result);
      }

      [TestMethod]
      public void CreateMaxNode_IINode_ReturnsIMaxNode()
      {
         Mock<IINode> mockIINode = new Mock<IINode>();
         INodeWrapperFactory factory = new INodeWrapperFactory();
         IMaxNode result = factory.CreateMaxNode(mockIINode.Object);

         Assert.IsNotNull(result);
         Assert.AreEqual(mockIINode.Object, result.BaseObject);
      }
   }
}
