using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Outliner.Scene;
using Outliner.Commands;
using Outliner.MaxUtils;

namespace Outliner.Tests.Commands
{
[TestClass]
public class FreezeCommandTest : MaxIntegrationTest
{
   [TestMethod]
   public void FreezeTest()
   {
      IINodeWrapper node = IMaxNodeWrapper.Create(MaxRemoting.CreateBox()) as IINodeWrapper;
      Assert.IsNotNull(node);
      IILayerWrapper layer = IMaxNodeWrapper.Create(MaxRemoting.CreateLayer()) as IILayerWrapper;
      Assert.IsNotNull(layer);

      Boolean nodeFrozen = node.GetNodeProperty(BooleanNodeProperty.IsFrozen);
      Boolean layerFrozen = layer.GetNodeProperty(BooleanNodeProperty.IsFrozen);
      List<IMaxNodeWrapper> nodes = new List<IMaxNodeWrapper>(2) { node, layer };
      FreezeCommand cmd = new FreezeCommand(nodes, !nodeFrozen);
      
      cmd.Redo();
      Assert.AreEqual(!nodeFrozen, node.GetNodeProperty(BooleanNodeProperty.IsFrozen));
      Assert.AreEqual(!layerFrozen, layer.GetNodeProperty(BooleanNodeProperty.IsFrozen));

      cmd.Restore(true);
      Assert.AreEqual(nodeFrozen, node.GetNodeProperty(BooleanNodeProperty.IsFrozen));
      Assert.AreEqual(layerFrozen, layer.GetNodeProperty(BooleanNodeProperty.IsFrozen));
   }
}
}
