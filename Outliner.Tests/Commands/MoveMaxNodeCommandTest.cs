using Outliner.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Outliner.Scene;
using System.Collections.Generic;

namespace Outliner.Tests.Commands
{
[TestClass]
public class MoveMaxNodeCommandTest : MaxIntegrationTest
{
   [TestMethod]
   public void LinkIINodeTest()
   {
      IMaxNodeWrapper nodeA = IMaxNodeWrapper.Create(MaxRemoting.CreateBox());
      IMaxNodeWrapper nodeB = IMaxNodeWrapper.Create(MaxRemoting.CreateBox());
      Assert.IsNotNull(nodeA);
      Assert.IsNotNull(nodeB);

      IMaxNodeWrapper nodeAParent = nodeA.Parent;
      IMaxNodeWrapper nodeBParent = nodeB.Parent;

      List<IMaxNodeWrapper> nodes = new List<IMaxNodeWrapper>(1) { nodeA };
      MoveMaxNodeCommand cmd = new MoveMaxNodeCommand(nodes, nodeB, "", "");

      cmd.Redo();
      Assert.AreNotEqual(nodeAParent, nodeA.Parent);
      Assert.AreEqual(nodeB, nodeA.Parent);
      Assert.AreEqual(nodeBParent, nodeB.Parent);

      cmd.Restore(true);
      Assert.AreEqual(nodeAParent, nodeA.Parent);
      Assert.AreEqual(nodeBParent, nodeB.Parent);
   }

   [TestMethod]
   public void UnlinkIINodeTest()
   {
      IMaxNodeWrapper nodeA = IMaxNodeWrapper.Create(MaxRemoting.CreateBox());
      IMaxNodeWrapper nodeB = IMaxNodeWrapper.Create(MaxRemoting.CreateBox());
      IMaxNodeWrapper root  = IMaxNodeWrapper.Create(MaxRemoting.SceneRoot);
      Assert.IsNotNull(nodeA);
      Assert.IsNotNull(nodeB);
      
      nodeA.AddChildNode(nodeB);
      Assert.AreEqual(root, nodeA.Parent);
      Assert.AreEqual(nodeA, nodeB.Parent);

      List<IMaxNodeWrapper> nodes = new List<IMaxNodeWrapper>(1) { nodeB };
      MoveMaxNodeCommand cmd = new MoveMaxNodeCommand(nodes, null, "", "");

      cmd.Redo();
      Assert.AreNotEqual(nodeA, nodeB.Parent);
      Assert.AreEqual(root, nodeA.Parent);
      Assert.AreEqual(root, nodeB.Parent);
   }
}
}
