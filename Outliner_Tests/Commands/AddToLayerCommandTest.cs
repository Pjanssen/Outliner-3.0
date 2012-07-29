
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Autodesk.Max;
using Outliner;
using Outliner.Scene;
using Outliner.Commands;
using MaxUtils;

namespace Outliner_Tests.Commands
{
[TestClass]
public class AddToLayerCommandTest : MaxIntegrationTest
{
   [TestMethod]
   public void AddToLayerTest()
   {
      IILayerManager man = MaxInterfaces.IILayerManager;
      //IILayer defaultLayer = man.GetLayer(0);
      IINodeWrapper node = IMaxNodeWrapper.Create(MaxRemoting.CreateBox()) as IINodeWrapper;
      Assert.IsNotNull(node);

      Assert.Inconclusive("Fix.");
      /*
      String layerName = node.Layer.Name;
      IILayer oldLayer = man.GetLayer(ref layerName);
      //IILayer oldLayer = node.IINode.GetReference((int)ReferenceNumbers.NodeLayerRef) as IILayer;
      IILayer newLayer = MaxRemoting.CreateLayer();

      IEnumerable<IINode> nodes = HelperMethods.GetWrappedNodes<IINode>(new List<IMaxNodeWrapper>(1) { node });
      AddToLayerCommand cmd = new AddToLayerCommand(nodes, newLayer);

      cmd.Do();
      layerName = node.Layer.Name;
      Assert.AreEqual(newLayer, man.GetLayer(ref layerName));

      cmd.Undo();
      layerName = node.Layer.Name;
      Assert.AreEqual(oldLayer, man.GetLayer(ref layerName));
      */
   }
}
}
