using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Outliner.Scene;
using Autodesk.Max;
using System.Runtime.InteropServices;
using Outliner;

namespace Outliner.Tests
{
[TestClass]
public class GroupHelpersTest : MaxIntegrationTest
{
   private IINode GroupNodes(IEnumerable<IMaxNodeWrapper> nodes)
   {
      IINode groupHead = MaxRemoting.CreateDummy();
      groupHead.SetGroupHead(true);
      ((IDummyObject)groupHead.ObjectRef).Box = MaxRemoting.Global.Box3.Create(MaxRemoting.Global.Point3.Create(0, 0, 0), MaxRemoting.Global.Point3.Create(0, 0, 0));

      foreach (IMaxNodeWrapper node in nodes)
      {
         IINodeWrapper inode = node as IINodeWrapper;
         if (inode != null && inode.IsValid)
         {
            inode.IINode.SetGroupMember(true);
            if (!nodes.Contains(node.Parent))
               groupHead.AttachChild(inode.IINode, true);
         }
      }

      return groupHead;
   }

   [TestMethod]
   public void OpenSelectedGroupHeadsTest()
   {
      Assert.Inconclusive("Fix remoting exception.");
      IINode node = MaxRemoting.CreateBox();
      IINodeWrapper nodeWrapper = IMaxNodeWrapper.Create(node) as IINodeWrapper;

      IINode groupHead = this.GroupNodes(new List<IMaxNodeWrapper>(1) { nodeWrapper });
      IINodeWrapper groupWrapper = IMaxNodeWrapper.Create(groupHead) as IINodeWrapper;

      Assert.IsNotNull(nodeWrapper);
      Assert.IsTrue(nodeWrapper.IsValid);
      Assert.IsNotNull(groupWrapper);
      Assert.IsTrue(groupWrapper.IsValid);
      Assert.IsTrue(node.IsGroupMember);

      OutlinerGUP gup = new OutlinerGUP();
      Assert.IsNotNull(gup);

      Assert.IsFalse(node.IsOpenGroupMember);
      GroupHelpers.OpenSelectedGroupHeads(new List<IMaxNodeWrapper>(1) { nodeWrapper });
      Assert.IsTrue(node.IsOpenGroupMember);

      GroupHelpers.CloseUnselectedGroupHeads();
      Assert.IsFalse(node.IsOpenGroupMember);
   }
}
}
