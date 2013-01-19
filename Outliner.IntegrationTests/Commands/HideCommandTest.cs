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
public class HideCommandTest : MaxIntegrationTest
{
   [TestMethod]
   public void HideTest()
   {
      IINodeWrapper node = MaxNodeWrapper.Create(MaxRemoting.CreateBox()) as IINodeWrapper;
      Assert.IsNotNull(node);
      IILayerWrapper layer = MaxNodeWrapper.Create(MaxRemoting.CreateLayer()) as IILayerWrapper;
      Assert.IsNotNull(layer);

      Boolean nodeHidden = node.GetNodeProperty(BooleanNodeProperty.IsHidden);
      Boolean layerHidden = layer.GetNodeProperty(BooleanNodeProperty.IsHidden);
      List<MaxNodeWrapper> nodes = new List<MaxNodeWrapper>(2) { node, layer };
      HideCommand cmd = new HideCommand(nodes, !nodeHidden);

      cmd.Redo();
      Assert.AreEqual(!nodeHidden, node.GetNodeProperty(BooleanNodeProperty.IsHidden));
      Assert.AreEqual(!layerHidden, layer.GetNodeProperty(BooleanNodeProperty.IsHidden));

      cmd.Restore(true);
      Assert.AreEqual(nodeHidden, node.GetNodeProperty(BooleanNodeProperty.IsHidden));
      Assert.AreEqual(layerHidden, layer.GetNodeProperty(BooleanNodeProperty.IsHidden));
   }
}
}
