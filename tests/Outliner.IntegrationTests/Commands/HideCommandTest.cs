using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PJanssen.Outliner.Scene;
using PJanssen.Outliner.Commands;
using PJanssen.Outliner.MaxUtils;

namespace PJanssen.Outliner.IntegrationTests.Commands
{
[TestClass]
public class HideCommandTest : MaxIntegrationTest
{
   //[TestMethod]
   //public void HideTest()
   //{
   //   INodeWrapper node = MaxNodeWrapper.Create(MaxRemoting.CreateBox()) as INodeWrapper;
   //   Assert.IsNotNull(node);
   //   ILayerWrapper layer = MaxNodeWrapper.Create(MaxRemoting.CreateLayer()) as ILayerWrapper;
   //   Assert.IsNotNull(layer);

   //   Boolean nodeHidden = node.GetNodeProperty(BooleanNodeProperty.IsHidden);
   //   Boolean layerHidden = layer.GetNodeProperty(BooleanNodeProperty.IsHidden);
   //   List<MaxNodeWrapper> nodes = new List<MaxNodeWrapper>(2) { node, layer };
   //   HideCommand cmd = new HideCommand(nodes, !nodeHidden);

   //   cmd.Redo();
   //   Assert.AreEqual(!nodeHidden, node.GetNodeProperty(BooleanNodeProperty.IsHidden));
   //   Assert.AreEqual(!layerHidden, layer.GetNodeProperty(BooleanNodeProperty.IsHidden));

   //   cmd.Restore(true);
   //   Assert.AreEqual(nodeHidden, node.GetNodeProperty(BooleanNodeProperty.IsHidden));
   //   Assert.AreEqual(layerHidden, layer.GetNodeProperty(BooleanNodeProperty.IsHidden));
   //}
}
}
