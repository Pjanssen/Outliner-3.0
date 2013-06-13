using Outliner.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Outliner.Scene;
using System.Collections.Generic;

namespace Outliner.IntegrationTests.Commands
{
[TestClass]
public class MoveMaxNodeCommandTest : MaxIntegrationTest
{
   //[TestMethod]
   //public void LinkIINodeTest()
   //{
   //   IMaxNode nodeA = MaxNodeWrapper.Create(MaxRemoting.CreateBox());
   //   IMaxNode nodeB = MaxNodeWrapper.Create(MaxRemoting.CreateBox());
   //   Assert.IsNotNull(nodeA);
   //   Assert.IsNotNull(nodeB);

   //   IMaxNode nodeAParent = nodeA.Parent;
   //   IMaxNode nodeBParent = nodeB.Parent;

   //   MoveMaxNodeCommand cmd = new MoveMaxNodeCommand(nodeA.ToIEnumerable(), nodeB, "", "");

   //   cmd.Redo();
   //   Assert.AreNotEqual(nodeAParent, nodeA.Parent);
   //   Assert.AreEqual(nodeB, nodeA.Parent);
   //   Assert.AreEqual(nodeBParent, nodeB.Parent);

   //   cmd.Restore(true);
   //   Assert.AreEqual(nodeAParent, nodeA.Parent);
   //   Assert.AreEqual(nodeBParent, nodeB.Parent);
   //}

   //[TestMethod]
   //public void UnlinkIINodeTest()
   //{
   //   IMaxNode nodeA = MaxNodeWrapper.Create(MaxRemoting.CreateBox());
   //   IMaxNode nodeB = MaxNodeWrapper.Create(MaxRemoting.CreateBox());
   //   IMaxNode root  = MaxNodeWrapper.Create(MaxRemoting.SceneRoot);
   //   Assert.IsNotNull(nodeA);
   //   Assert.IsNotNull(nodeB);
      
   //   nodeA.AddChildNode(nodeB);
   //   Assert.AreEqual(root, nodeA.Parent);
   //   Assert.AreEqual(nodeA, nodeB.Parent);

   //   MoveMaxNodeCommand cmd = new MoveMaxNodeCommand(nodeB.ToIEnumerable(), null, "", "");

   //   cmd.Redo();
   //   Assert.AreNotEqual(nodeA, nodeB.Parent);
   //   Assert.AreEqual(root, nodeA.Parent);
   //   Assert.AreEqual(root, nodeB.Parent);
   //}
}
}
