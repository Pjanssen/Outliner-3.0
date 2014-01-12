using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Max;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PJanssen.Outliner.IntegrationTests
{
   [TestClass]
   public class RemotingTests : MaxIntegrationTest
   {
      /// <summary>
      /// Tests if the remoting to 3dsMax works.
      /// </summary>
      [TestMethod]
      public void RemotingTest()
      {
         IGlobal global = MaxRemoting.Global;
         IInterface ip = global.COREInterface;
         int newSceneNumChildren = ip.RootNode.NumberOfChildren;

         IINode boxNode = MaxRemoting.CreateBox();
         Assert.IsNotNull(boxNode, "Create box");
         Assert.AreEqual(newSceneNumChildren + 1, ip.RootNode.NumberOfChildren, "Add box");

         MaxRemoting.DeleteNode(boxNode);
         Assert.AreEqual(newSceneNumChildren, ip.RootNode.NumberOfChildren, "Delete box");
      }
   }
}
