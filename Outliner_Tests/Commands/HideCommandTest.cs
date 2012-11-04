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
public class HideCommandTest : MaxIntegrationTest
{
   [TestMethod]
   public void HideTest()
   {
      IINodeWrapper node = IMaxNodeWrapper.Create(MaxRemoting.CreateBox()) as IINodeWrapper;
      Assert.IsNotNull(node);
      IILayerWrapper layer = IMaxNodeWrapper.Create(MaxRemoting.CreateLayer()) as IILayerWrapper;
      Assert.IsNotNull(layer);

      Boolean nodeHidden = node.GetProperty(BooleanNodeProperty.IsHidden);
      Boolean layerHidden = layer.GetProperty(BooleanNodeProperty.IsHidden);
      List<IMaxNodeWrapper> nodes = new List<IMaxNodeWrapper>(2) { node, layer };
      HideCommand cmd = new HideCommand(nodes, !nodeHidden);

      cmd.Redo();
      Assert.AreEqual(!nodeHidden, node.GetProperty(BooleanNodeProperty.IsHidden));
      Assert.AreEqual(!layerHidden, layer.GetProperty(BooleanNodeProperty.IsHidden));

      cmd.Restore(true);
      Assert.AreEqual(nodeHidden, node.GetProperty(BooleanNodeProperty.IsHidden));
      Assert.AreEqual(layerHidden, layer.GetProperty(BooleanNodeProperty.IsHidden));
   }
}
}
