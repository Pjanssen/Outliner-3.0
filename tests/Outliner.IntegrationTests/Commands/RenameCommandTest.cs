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
public class RenameCommandTest : MaxIntegrationTest
{
   // TODO test layer renaming.

   [TestMethod]
   public void RenameTest()
   {
      IMaxNode node = MaxNodeWrapper.Create(MaxRemoting.CreateBox());
      Assert.IsNotNull(node);

      String oldName = node.Name;
      String newName = "Test";
      RenameCommand cmd = new RenameCommand(node.ToIEnumerable(), newName);

      cmd.Redo();
      Assert.AreEqual(newName, node.Name);

      cmd.Restore(true);
      Assert.AreEqual(oldName, node.Name);
   }
}
}
