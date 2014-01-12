using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PJanssen.Outliner.Scene;
using PJanssen.Outliner.Commands;

namespace PJanssen.Outliner.IntegrationTests.Commands
{
[TestClass]
public class SelectCommandTest : MaxIntegrationTest
{
   //[TestMethod]
   //public void SelectTest()
   //{
   //   IMaxNode nodeA = MaxNodeWrapper.Create(MaxRemoting.CreateBox());
   //   IMaxNode nodeB = MaxNodeWrapper.Create(MaxRemoting.CreateBox());
   //   IMaxNode nodeC = MaxNodeWrapper.Create(MaxRemoting.CreateBox());

   //   Assert.AreEqual(false, nodeA.IsSelected);
   //   Assert.AreEqual(false, nodeB.IsSelected);
   //   Assert.AreEqual(false, nodeC.IsSelected);

   //   List<IMaxNode> nodes = new List<IMaxNode>(2) { nodeA, nodeB };
   //   SelectCommand cmd = new SelectCommand(nodes, false);

   //   cmd.Redo();
   //   Assert.AreEqual(true, nodeA.IsSelected);
   //   Assert.AreEqual(true, nodeB.IsSelected);
   //   Assert.AreEqual(false, nodeC.IsSelected);

   //   cmd.Restore(true);
   //   Assert.AreEqual(false, nodeA.IsSelected);
   //   Assert.AreEqual(false, nodeB.IsSelected);
   //   Assert.AreEqual(false, nodeC.IsSelected);
   //}

   //[TestMethod]
   //public void ChangeSelectionTest()
   //{
   //   IMaxNode nodeA = MaxNodeWrapper.Create(MaxRemoting.CreateBox());
   //   IMaxNode nodeB = MaxNodeWrapper.Create(MaxRemoting.CreateBox());
   //   IMaxNode nodeC = MaxNodeWrapper.Create(MaxRemoting.CreateBox());

   //   Assert.AreEqual(false, nodeA.IsSelected);
   //   Assert.AreEqual(false, nodeB.IsSelected);
   //   Assert.AreEqual(false, nodeC.IsSelected);

   //   List<IMaxNode> nodes1 = new List<IMaxNode>(2) { nodeA, nodeB };
   //   SelectCommand cmd = new SelectCommand(nodes1, false);

   //   cmd.Redo();
   //   Assert.AreEqual(true, nodeA.IsSelected);
   //   Assert.AreEqual(true, nodeB.IsSelected);
   //   Assert.AreEqual(false, nodeC.IsSelected);

   //   List<MaxNodeWrapper> nodes2 = new List<IMaxNode>(1) { nodeC };
   //   SelectCommand cmd2 = new SelectCommand(nodes2, false);
      
   //   cmd2.Redo();
   //   Assert.AreEqual(false, nodeA.IsSelected);
   //   Assert.AreEqual(false, nodeB.IsSelected);
   //   Assert.AreEqual(true, nodeC.IsSelected);

   //   cmd2.Restore(true);
   //   Assert.AreEqual(true, nodeA.IsSelected);
   //   Assert.AreEqual(true, nodeB.IsSelected);
   //   Assert.AreEqual(false, nodeC.IsSelected);
   //}

   //[TestMethod]
   //public void ClearSelectionTest()
   //{
   //   IMaxNode nodeA = MaxNodeWrapper.Create(MaxRemoting.CreateBox());
   //   IMaxNode nodeB = MaxNodeWrapper.Create(MaxRemoting.CreateBox());
   //   IMaxNode nodeC = MaxNodeWrapper.Create(MaxRemoting.CreateBox());

   //   Assert.AreEqual(false, nodeA.IsSelected);
   //   Assert.AreEqual(false, nodeB.IsSelected);
   //   Assert.AreEqual(false, nodeC.IsSelected);

   //   List<IMaxNode> nodes = new List<IMaxNode>(2) { nodeA, nodeB };
   //   SelectCommand cmd = new SelectCommand(nodes, false);

   //   cmd.Redo();
   //   Assert.AreEqual(true, nodeA.IsSelected);
   //   Assert.AreEqual(true, nodeB.IsSelected);
   //   Assert.AreEqual(false, nodeC.IsSelected);

   //   SelectCommand clearCmd = new SelectCommand(new List<IMaxNode>(), false);

   //   clearCmd.Redo();
   //   Assert.AreEqual(false, nodeA.IsSelected);
   //   Assert.AreEqual(false, nodeB.IsSelected);
   //   Assert.AreEqual(false, nodeC.IsSelected);

   //   clearCmd.Restore(true);
   //   Assert.AreEqual(true, nodeA.IsSelected);
   //   Assert.AreEqual(true, nodeB.IsSelected);
   //   Assert.AreEqual(false, nodeC.IsSelected);
   //}
}
}
