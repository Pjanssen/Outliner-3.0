using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Outliner.Scene;
using Outliner.Commands;

namespace Outliner.IntegrationTests.Commands
{
[TestClass]
public class SelectCommandTest : MaxIntegrationTest
{
   [TestMethod]
   public void SelectTest()
   {
      MaxNodeWrapper nodeA = MaxNodeWrapper.Create(MaxRemoting.CreateBox());
      MaxNodeWrapper nodeB = MaxNodeWrapper.Create(MaxRemoting.CreateBox());
      MaxNodeWrapper nodeC = MaxNodeWrapper.Create(MaxRemoting.CreateBox());

      Assert.AreEqual(false, nodeA.Selected);
      Assert.AreEqual(false, nodeB.Selected);
      Assert.AreEqual(false, nodeC.Selected);

      List<MaxNodeWrapper> nodes = new List<MaxNodeWrapper>(2) { nodeA, nodeB };
      SelectCommand cmd = new SelectCommand(nodes, false);

      cmd.Redo();
      Assert.AreEqual(true, nodeA.Selected);
      Assert.AreEqual(true, nodeB.Selected);
      Assert.AreEqual(false, nodeC.Selected);

      cmd.Restore(true);
      Assert.AreEqual(false, nodeA.Selected);
      Assert.AreEqual(false, nodeB.Selected);
      Assert.AreEqual(false, nodeC.Selected);
   }

   [TestMethod]
   public void ChangeSelectionTest()
   {
      MaxNodeWrapper nodeA = MaxNodeWrapper.Create(MaxRemoting.CreateBox());
      MaxNodeWrapper nodeB = MaxNodeWrapper.Create(MaxRemoting.CreateBox());
      MaxNodeWrapper nodeC = MaxNodeWrapper.Create(MaxRemoting.CreateBox());

      Assert.AreEqual(false, nodeA.Selected);
      Assert.AreEqual(false, nodeB.Selected);
      Assert.AreEqual(false, nodeC.Selected);

      List<MaxNodeWrapper> nodes1 = new List<MaxNodeWrapper>(2) { nodeA, nodeB };
      SelectCommand cmd = new SelectCommand(nodes1, false);

      cmd.Redo();
      Assert.AreEqual(true, nodeA.Selected);
      Assert.AreEqual(true, nodeB.Selected);
      Assert.AreEqual(false, nodeC.Selected);

      List<MaxNodeWrapper> nodes2 = new List<MaxNodeWrapper>(1) { nodeC };
      SelectCommand cmd2 = new SelectCommand(nodes2, false);
      
      cmd2.Redo();
      Assert.AreEqual(false, nodeA.Selected);
      Assert.AreEqual(false, nodeB.Selected);
      Assert.AreEqual(true, nodeC.Selected);

      cmd2.Restore(true);
      Assert.AreEqual(true, nodeA.Selected);
      Assert.AreEqual(true, nodeB.Selected);
      Assert.AreEqual(false, nodeC.Selected);
   }

   [TestMethod]
   public void ClearSelectionTest()
   {
      MaxNodeWrapper nodeA = MaxNodeWrapper.Create(MaxRemoting.CreateBox());
      MaxNodeWrapper nodeB = MaxNodeWrapper.Create(MaxRemoting.CreateBox());
      MaxNodeWrapper nodeC = MaxNodeWrapper.Create(MaxRemoting.CreateBox());

      Assert.AreEqual(false, nodeA.Selected);
      Assert.AreEqual(false, nodeB.Selected);
      Assert.AreEqual(false, nodeC.Selected);

      List<MaxNodeWrapper> nodes = new List<MaxNodeWrapper>(2) { nodeA, nodeB };
      SelectCommand cmd = new SelectCommand(nodes, false);

      cmd.Redo();
      Assert.AreEqual(true, nodeA.Selected);
      Assert.AreEqual(true, nodeB.Selected);
      Assert.AreEqual(false, nodeC.Selected);

      SelectCommand clearCmd = new SelectCommand(new List<MaxNodeWrapper>(), false);

      clearCmd.Redo();
      Assert.AreEqual(false, nodeA.Selected);
      Assert.AreEqual(false, nodeB.Selected);
      Assert.AreEqual(false, nodeC.Selected);

      clearCmd.Restore(true);
      Assert.AreEqual(true, nodeA.Selected);
      Assert.AreEqual(true, nodeB.Selected);
      Assert.AreEqual(false, nodeC.Selected);
   }
}
}
