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
public class SetRenderableCommandTest
{
   [TestMethod]
   public void RenderableTest()
   {
      IINodeWrapper node = IMaxNodeWrapper.Create(MaxRemoting.CreateBox()) as IINodeWrapper;
      Assert.IsNotNull(node);
      IILayerWrapper layer = IMaxNodeWrapper.Create(MaxRemoting.CreateLayer()) as IILayerWrapper;
      Assert.IsNotNull(layer);

      Boolean nodeRenderable = node.Renderable;
      Boolean layerRenderable = layer.Renderable;
      List<IMaxNodeWrapper> nodes = new List<IMaxNodeWrapper>(2) { node, layer };
      SetRenderableCommand cmd = new SetRenderableCommand(nodes, !nodeRenderable);

      cmd.Do();
      Assert.AreEqual(!nodeRenderable, node.Renderable);
      Assert.AreEqual(!layerRenderable, layer.Renderable);

      cmd.Undo();
      Assert.AreEqual(nodeRenderable, node.Renderable);
      Assert.AreEqual(layerRenderable, layer.Renderable);
   }
}
}
