using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Outliner.Scene;
using Outliner.Commands;

namespace Outliner_Tests.Commands
{
[TestClass]
public class SetBoxModeCommandTest : MaxIntegrationTest
{
   [TestMethod]
   public void BoxModeTest()
   {
      IINodeWrapper node = IMaxNodeWrapper.Create(MaxRemoting.CreateBox()) as IINodeWrapper;
      Assert.IsNotNull(node);
      IILayerWrapper layer = IMaxNodeWrapper.Create(MaxRemoting.CreateLayer()) as IILayerWrapper;
      Assert.IsNotNull(layer);

      Boolean nodeBoxMode = node.BoxMode;
      Boolean layerBoxMode = layer.BoxMode;
      List<IMaxNodeWrapper> nodes = new List<IMaxNodeWrapper>(2) { node, layer };
      SetBoxModeCommand cmd = new SetBoxModeCommand(nodes, !nodeBoxMode);
      
      cmd.Do();
      Assert.AreEqual(!nodeBoxMode, node.BoxMode);
      Assert.AreEqual(!layerBoxMode, layer.BoxMode);

      cmd.Undo();
      Assert.AreEqual(nodeBoxMode, node.BoxMode);
      Assert.AreEqual(layerBoxMode, layer.BoxMode);
   }
}
}
