using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Outliner.Controls.Tree;

namespace Outliner.Tests.Controls.Tree
{
   [TestClass]
   public class TreeNodeCollectionTest
   {
      private TreeNodeCollection Nodes
      {
         get
         {
            TreeNode root = new TreeNode("root");
            return root.Nodes;
         }
      }

      [TestMethod]
      public void AddTest()
      {
         TreeNode tn = new TreeNode("tn");
         Nodes.Add(tn);

         Assert.IsNull(tn.Parent, "Node added to root collection should have null as parent.");

         TreeNode tn2 = new TreeNode("tn2");
         tn.Nodes.Add(tn2);
         Assert.AreEqual(tn, tn2.Parent, "Add should set added node's parent.");
      }

      [TestMethod]
      public void RemoveTest()
      {
         TreeNode tn1 = new TreeNode("tn1");
         Nodes.Add(tn1);
         TreeNode tn2 = new TreeNode("tn");
         tn1.Nodes.Add(tn2);
         tn1.Nodes.Remove(tn2);

         Assert.IsNull(tn2.Parent, "Remove should set node's parent to null");
      }

      [TestMethod]
      public void ClearTest()
      {
         TreeNode root = new TreeNode("root");
         Nodes.Add(root);
         TreeNode tn1 = new TreeNode("tn1");
         TreeNode tn2 = new TreeNode("tn2");
         root.Nodes.Add(tn1);
         root.Nodes.Add(tn2);
         root.Nodes.Clear();

         Assert.AreEqual(0, root.Nodes.Count, "Clear should set Count to 0.");
         Assert.IsNull(tn1.Parent, "Clear should set node's parent to null");
         Assert.IsNull(tn2.Parent, "Clear should set node's parent to null");
      }
   }
}
