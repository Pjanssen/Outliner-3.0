using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Outliner.Scene;

namespace Outliner.Tests.Scene
{
   [TestClass]
   public class MaxNodeWrapperTests
   {
      [TestCleanup]
      public void Cleanup()
      {
         MaxNodeWrapper.Factories = Enumerable.Empty<IMaxNodeFactory>();
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentNullException))]
      public void RegisterMaxNodeFactory_Null_ThrowsException()
      {
         MaxNodeWrapper.RegisterMaxNodeFactory(null);
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentNullException))]
      public void Create_Null_ThrowsException()
      {
         MaxNodeWrapper.Create(null);
      }

      [TestMethod]
      [ExpectedException(typeof(NotSupportedException))]
      public void Create_UnsupportedObject_ThrowsException()
      {
         MaxNodeWrapper.Create(42);
      }

      [TestMethod]
      public void Create_UsesRegisteredFactory()
      {
         Mock<IMaxNode> mockMaxNode = new Mock<IMaxNode>();
         Mock<IMaxNodeFactory> mockFactory = new Mock<IMaxNodeFactory>();
         mockFactory.Setup(f => f.CreateMaxNode(It.IsAny<Object>()))
                    .Returns(mockMaxNode.Object);

         MaxNodeWrapper.RegisterMaxNodeFactory(mockFactory.Object);
         IMaxNode result = MaxNodeWrapper.Create(42);

         Assert.AreEqual(mockMaxNode.Object, result);
      }
   }
}
