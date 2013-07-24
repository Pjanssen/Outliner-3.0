using System;
using Autodesk.Max;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Outliner.ColorTags;

namespace Outliner.UnitTests.Plugins.ColorTagsPlugin
{
   [TestClass]
   public class ColorTagsTests
   {
      private byte[] AppData;

      private Mock<IAnimatable> CreateMockNode()
      {
         Mock<IAnimatable> node = new Mock<IAnimatable>();
         node.Setup(n => n.AddAppDataChunk(It.IsAny<IClass_ID>(), It.IsAny<SClass_ID>(), It.IsAny<uint>(), It.IsAny<byte[]>()))
             .Callback((IClass_ID cid, SClass_ID scid, uint sbid, byte[] data) => AppData = data);
         node.Setup(n => n.GetAppDataChunk(It.IsAny<IClass_ID>(), It.IsAny<SClass_ID>(), It.IsAny<uint>()))
             .Returns((IClass_ID cid, SClass_ID scid, uint sbid) => CreateAppDataChunk(AppData));

         return node;
      }

      private IAppDataChunk CreateAppDataChunk(byte[] AppData)
      {
         Mock<IAppDataChunk> mockChunk = new Mock<IAppDataChunk>();
         mockChunk.SetupGet(c => c.Data)
                  .Returns(AppData);

         return mockChunk.Object;
      }

      #region HasTag
      
      [TestMethod]
      public void HasTag_Null_ReturnsFalse()
      {
         Boolean result = Outliner.ColorTags.ColorTags.HasTag(null);
         Assert.IsFalse(result);
      }

      [TestMethod]
      public void HasTag_UntaggedNode_ReturnsFalse()
      {
         Mock<IAnimatable> node = CreateMockNode();
         
         Boolean result = Outliner.ColorTags.ColorTags.HasTag(node.Object);

         Assert.IsFalse(result);
      }

      [TestMethod]
      public void HasTag_ReturnsTrue_ForSetTags()
      {
         Mock<IAnimatable> node = CreateMockNode();

         ColorTags.ColorTags.SetTag(node.Object, ColorTag.Blue);

         Assert.IsFalse(ColorTags.ColorTags.HasTag(node.Object, ColorTag.Green));
         Assert.IsTrue(ColorTags.ColorTags.HasTag(node.Object, ColorTag.Blue));
      }

      [TestMethod]
      public void HasTag_ColorTagAll_ReturnsTrue()
      {
         Mock<IAnimatable> node = CreateMockNode();

         ColorTags.ColorTags.SetTag(node.Object, ColorTag.All);

         Assert.IsTrue(ColorTags.ColorTags.HasTag(node.Object, ColorTag.Green));
         Assert.IsTrue(ColorTags.ColorTags.HasTag(node.Object, ColorTag.Blue));
      }

      #endregion

      #region GetTag / SetTag

      [TestMethod]
      [ExpectedException(typeof(ArgumentNullException))]
      public void GetTag_NodeNull_ThrowsException()
      {
         ColorTags.ColorTags.GetTag(null);
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentNullException))]
      public void SetTag_NodeNull_ThrowsException()
      {
         ColorTags.ColorTags.SetTag(null, ColorTag.All);
      }

      [TestMethod]
      public void SetTag_SetsAppData()
      {
         Mock<IAnimatable> node = CreateMockNode();

         ColorTags.ColorTags.SetTag(node.Object, ColorTag.All);

         node.Verify(n => n.AddAppDataChunk(It.IsAny<IClass_ID>(), It.IsAny<SClass_ID>(), It.IsAny<uint>(), It.IsNotNull<byte[]>()));
      }

      [TestMethod]
      public void GetTag_ReturnsSetTag()
      {
         ColorTag expected= ColorTag.Blue;
         Mock<IAnimatable> node = CreateMockNode();

         ColorTags.ColorTags.SetTag(node.Object, expected);
         ColorTag tag = ColorTags.ColorTags.GetTag(node.Object);

         Assert.AreEqual(expected, tag);
      }

      [TestMethod]
      public void SetTag_DifferentTags_ReplacesTag()
      {
         Mock<IAnimatable> node = CreateMockNode();

         ColorTags.ColorTags.SetTag(node.Object, ColorTag.Blue);
         ColorTags.ColorTags.SetTag(node.Object, ColorTag.Green);
         ColorTag tag = ColorTags.ColorTags.GetTag(node.Object);

         Assert.AreEqual(ColorTag.Green, tag);
      }

      #endregion
   }
}
