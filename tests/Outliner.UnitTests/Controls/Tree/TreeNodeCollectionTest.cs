using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Outliner.Controls.Tree;
using Moq;

namespace Outliner.Tests.Controls.Tree
{
   [TestClass]
   public class TreeNodeCollectionTest
   {
      private TreeView Tree;
      private TreeNode Root;
      private TreeNodeCollection CreateCollection()
      {
         Tree = new TreeView();
         Root = new TreeNode(Tree, "root");
         Tree.Nodes.Add(Root);

         return Root.Nodes;
      }

      private TreeNodeCollection Nodes
      {
         get
         {
            TreeNode root = new TreeNode("root");
            return root.Nodes;
         }
      }

      [TestMethod]
      public void Add_TreeNode_SetsTreeView()
      {
         TreeNodeCollection nodes = CreateCollection();
         
         TreeNode child = new TreeNode();
         nodes.Add(child);

         Assert.AreEqual(Tree, child.TreeView);
      }

      [TestMethod]
      public void Add_TreeNode_SetsParent()
      {
         TreeNodeCollection nodes = CreateCollection();

         TreeNode tn = new TreeNode();
         nodes.Add(tn);

         Assert.AreEqual(Root, tn.Parent);
      }

      [TestMethod]
      public void Add_TreeNode_ContainsAddedNode()
      {
         TreeNodeCollection nodes = CreateCollection();
         TreeNode tn = new TreeNode();

         nodes.Add(tn);

         Assert.IsTrue(nodes.Contains(tn));
      }

      [TestMethod]
      public void Add_FromOtherCollection_RemovesFromOldCollection()
      {
         TreeNode oldParent = new TreeNode();
         TreeNode newParent = new TreeNode();
         TreeNode child = new TreeNode();

         oldParent.Nodes.Add(child);
         newParent.Nodes.Add(child);

         Assert.IsFalse(oldParent.Nodes.Contains(child));
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentNullException))]
      public void Add_Null_ThrowsException()
      {
         TreeNodeCollection nodes = CreateCollection();
         nodes.Add(null);
      }

      [TestMethod]
      [ExpectedException(typeof(InvalidOperationException))]
      public void Add_Owner_ThrowsException()
      {
         TreeNode tn = new TreeNode();
         tn.Nodes.Add(tn);
      }

      [TestMethod]
      [ExpectedException(typeof(InvalidOperationException))]
      public void Add_DependencyLoop_ThrowsException()
      {
         TreeNode tnA = new TreeNode("A");
         TreeNode tnB = new TreeNode("B");
         TreeNode tnC = new TreeNode("C");
         tnA.Nodes.Add(tnB);
         tnB.Nodes.Add(tnC);
         tnC.Nodes.Add(tnA);
      }

      [TestMethod]
      public void TreeNodeShowNode_AffectsNodesInCollection()
      {
         TreeNodeCollection nodes = CreateCollection();
         TreeNode tnVisible = new TreeNode() { ShowNode = true };
         TreeNode tnInvisible = new TreeNode() { ShowNode = false };
         nodes.Add(tnVisible);
         nodes.Add(tnInvisible);

         List<TreeNode> foreachedNodes = new List<TreeNode>();
         foreach (TreeNode tn in nodes)
         {
            foreachedNodes.Add(tn);
         }
         Assert.AreEqual(1, foreachedNodes.Count);
         Assert.AreEqual(tnVisible, foreachedNodes[0]);
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
