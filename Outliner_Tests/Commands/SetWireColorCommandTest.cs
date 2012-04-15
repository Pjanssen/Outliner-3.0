using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Outliner.Scene;
using System.Drawing;
using Outliner.Commands;

namespace Outliner_Tests.Commands
{
[TestClass]
public class SetWireColorCommandTest : MaxIntegrationTest
{
   [TestMethod]
   public void WireColorTest()
   {
      IINodeWrapper node = IMaxNodeWrapper.Create(MaxRemoting.CreateBox()) as IINodeWrapper;
      Assert.IsNotNull(node);
      IILayerWrapper layer = IMaxNodeWrapper.Create(MaxRemoting.CreateLayer()) as IILayerWrapper;
      Assert.IsNotNull(layer);

      Color nodeColor = node.WireColor;
      Color layerColor = layer.WireColor;
      Random r = new Random();
      Color newColor = Color.FromArgb(r.Next(255), r.Next(255), r.Next(255));
      List<IMaxNodeWrapper> nodes = new List<IMaxNodeWrapper>(2) { node, layer };
      SetWireColorCommand cmd = new SetWireColorCommand(nodes, newColor);

      cmd.Do();
      Assert.AreEqual(newColor, node.WireColor);
      Assert.AreEqual(newColor, layer.WireColor);

      cmd.Undo();
      Assert.AreEqual(nodeColor, node.WireColor);
      Assert.AreEqual(layerColor, layer.WireColor);
   }
}
}
