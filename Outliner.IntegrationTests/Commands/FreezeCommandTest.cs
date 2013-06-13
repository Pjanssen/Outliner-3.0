using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Outliner.Scene;
using Outliner.Commands;
using Outliner.MaxUtils;

namespace Outliner.IntegrationTests.Commands
{
[TestClass]
public class FreezeCommandTest : MaxIntegrationTest
{
   //[TestMethod]
   //public void FreezeTest()
   //{
   //   INodeWrapper node = MaxNodeWrapper.Create(MaxRemoting.CreateBox()) as INodeWrapper;
   //   Assert.IsNotNull(node);
   //   ILayerWrapper layer = MaxNodeWrapper.Create(MaxRemoting.CreateLayer()) as ILayerWrapper;
   //   Assert.IsNotNull(layer);

   //   Boolean nodeFrozen = node.GetNodeProperty(BooleanNodeProperty.IsFrozen);
   //   Boolean layerFrozen = layer.GetNodeProperty(BooleanNodeProperty.IsFrozen);
   //   List<MaxNodeWrapper> nodes = new List<MaxNodeWrapper>(2) { node, layer };
   //   FreezeCommand cmd = new FreezeCommand(nodes, !nodeFrozen);
      
   //   cmd.Redo();
   //   Assert.AreEqual(!nodeFrozen, node.GetNodeProperty(BooleanNodeProperty.IsFrozen));
   //   Assert.AreEqual(!layerFrozen, layer.GetNodeProperty(BooleanNodeProperty.IsFrozen));

   //   cmd.Restore(true);
   //   Assert.AreEqual(nodeFrozen, node.GetNodeProperty(BooleanNodeProperty.IsFrozen));
   //   Assert.AreEqual(layerFrozen, layer.GetNodeProperty(BooleanNodeProperty.IsFrozen));
   //}
}
}
