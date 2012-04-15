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
public class FreezeCommandTest : MaxIntegrationTest
{
   [TestMethod]
   public void FreezeTest()
   {
      IINodeWrapper node = IMaxNodeWrapper.Create(MaxRemoting.CreateBox()) as IINodeWrapper;
      Assert.IsNotNull(node);
      IILayerWrapper layer = IMaxNodeWrapper.Create(MaxRemoting.CreateLayer()) as IILayerWrapper;
      Assert.IsNotNull(layer);

      Boolean nodeFrozen = node.IsFrozen;
      Boolean layerFrozen = layer.IsFrozen;
      List<IMaxNodeWrapper> nodes = new List<IMaxNodeWrapper>(2) { node, layer };
      FreezeCommand cmd = new FreezeCommand(nodes, !nodeFrozen);
      
      cmd.Do();
      Assert.AreEqual(!nodeFrozen, node.IsFrozen);
      Assert.AreEqual(!layerFrozen, layer.IsFrozen);

      cmd.Undo();
      Assert.AreEqual(nodeFrozen, node.IsFrozen);
      Assert.AreEqual(layerFrozen, layer.IsFrozen);
   }
}
}
