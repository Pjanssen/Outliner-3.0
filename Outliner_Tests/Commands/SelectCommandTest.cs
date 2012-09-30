using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Outliner.Scene;
using Outliner.Commands;

namespace Outliner.Tests.Commands
{
[TestClass]
public class SelectCommandTest : MaxIntegrationTest
{
   [TestMethod]
   public void SelectTest()
   {
      IMaxNodeWrapper nodeA = IMaxNodeWrapper.Create(MaxRemoting.CreateBox());
      IMaxNodeWrapper nodeB = IMaxNodeWrapper.Create(MaxRemoting.CreateBox());
      IMaxNodeWrapper nodeC = IMaxNodeWrapper.Create(MaxRemoting.CreateBox());

      Assert.AreEqual(false, nodeA.Selected);
      Assert.AreEqual(false, nodeB.Selected);
      Assert.AreEqual(false, nodeC.Selected);

      List<IMaxNodeWrapper> nodes = new List<IMaxNodeWrapper>(2) { nodeA, nodeB };
      SelectCommand cmd = new SelectCommand(nodes);

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
      IMaxNodeWrapper nodeA = IMaxNodeWrapper.Create(MaxRemoting.CreateBox());
      IMaxNodeWrapper nodeB = IMaxNodeWrapper.Create(MaxRemoting.CreateBox());
      IMaxNodeWrapper nodeC = IMaxNodeWrapper.Create(MaxRemoting.CreateBox());

      Assert.AreEqual(false, nodeA.Selected);
      Assert.AreEqual(false, nodeB.Selected);
      Assert.AreEqual(false, nodeC.Selected);

      List<IMaxNodeWrapper> nodes1 = new List<IMaxNodeWrapper>(2) { nodeA, nodeB };
      SelectCommand cmd = new SelectCommand(nodes1);

      cmd.Redo();
      Assert.AreEqual(true, nodeA.Selected);
      Assert.AreEqual(true, nodeB.Selected);
      Assert.AreEqual(false, nodeC.Selected);

      List<IMaxNodeWrapper> nodes2 = new List<IMaxNodeWrapper>(1) { nodeC };
      SelectCommand cmd2 = new SelectCommand(nodes2);
      
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
      IMaxNodeWrapper nodeA = IMaxNodeWrapper.Create(MaxRemoting.CreateBox());
      IMaxNodeWrapper nodeB = IMaxNodeWrapper.Create(MaxRemoting.CreateBox());
      IMaxNodeWrapper nodeC = IMaxNodeWrapper.Create(MaxRemoting.CreateBox());

      Assert.AreEqual(false, nodeA.Selected);
      Assert.AreEqual(false, nodeB.Selected);
      Assert.AreEqual(false, nodeC.Selected);

      List<IMaxNodeWrapper> nodes = new List<IMaxNodeWrapper>(2) { nodeA, nodeB };
      SelectCommand cmd = new SelectCommand(nodes);

      cmd.Redo();
      Assert.AreEqual(true, nodeA.Selected);
      Assert.AreEqual(true, nodeB.Selected);
      Assert.AreEqual(false, nodeC.Selected);

      SelectCommand clearCmd = new SelectCommand(new List<IMaxNodeWrapper>());

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
