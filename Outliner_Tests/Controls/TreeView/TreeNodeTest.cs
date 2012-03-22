using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Outliner.Controls;
using System.Drawing;

namespace Outliner_Tests.Controls.TreeView
{
[TestClass]
public class TreeNodeTest
{
   private Outliner.Controls.TreeView CreateTree()
   {
      Outliner.Controls.TreeView tree = new Outliner.Controls.TreeView();
      tree.Width = 100;
      tree.Height = 100;
      TreeNode tn1 = new TreeNode("tn1");
      tn1.IsExpanded = true;
      tree.Nodes.Add(tn1);
      tn1.Nodes.Add(new TreeNode("tn2"));
      tn1.Nodes.Add(new TreeNode("tn3"));
      tree.Nodes.Add(new TreeNode("tn4"));
      return tree;
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
   public void SetParentTest()
   {
      TreeNode tn = new TreeNode();
      TreeNode tn1 = new TreeNode();
      Nodes.Add(tn1);
      TreeNode tn2 = new TreeNode();
      Nodes.Add(tn2);

      tn1.Nodes.Add(tn);
      Assert.AreEqual(tn1, tn.Parent, "Initial parent");
      tn.Parent = tn2;
      Assert.AreEqual(tn2, tn.Parent, "New parent");
      Assert.IsFalse(tn1.Nodes.Contains(tn), "Initial parent nodes should no longer contain tn");
      Assert.IsTrue(tn2.Nodes.Contains(tn), "New parent nodes contains tn");

      tn.Parent = null;
      Assert.IsNull(tn.Parent);
      Assert.IsFalse(tn2.Nodes.Contains(tn), "New parent nodes should no longer contain tn");
   }


   [TestMethod]
   public void LevelTest()
   {
      TreeNode tn1 = new TreeNode();
      TreeNode tn2 = new TreeNode();
      Nodes.Add(tn1);
      tn1.Nodes.Add(tn2);
      Assert.AreEqual(0, tn1.Level, "tn1 level");
      Assert.AreEqual(1, tn2.Level, "tn2 level");
   }

   [TestMethod]
   public void IsVisibleTest()
   {
      TreeNode root = new TreeNode();
      Nodes.Add(root);
      Assert.IsTrue(root.IsVisible, "Root visibility");

      TreeNode tn1 = new TreeNode();
      root.Nodes.Add(tn1);
      root.IsExpanded = false;
      Assert.IsFalse(tn1.IsVisible, "Childnode of collapsed parent");
      root.IsExpanded = true;
      Assert.IsTrue(tn1.IsVisible, "Childnode of expanded parent.");
      
      TreeNode tn2 = new TreeNode();
      tn1.Nodes.Add(tn2);
      tn1.IsExpanded = true;
      Assert.IsTrue(tn2.IsVisible, "Childnode of expanded parent chain.");
      root.IsExpanded = false;
      Assert.IsFalse(tn2.IsVisible, "Childnode of expanded parent, but collapsed root.");
   }

   [TestMethod]
   public void NextNodeTest()
   {
      TreeNode tn = new TreeNode("tn");
      Assert.IsNull(tn.NextNode, "NextNode in TreeNode without parent");

      TreeNode tn2 = new TreeNode("tn2");
      tn.Nodes.Add(tn2);
      Assert.IsNull(tn2.NextNode, "NextNode for single treenode in parent collection.");

      TreeNode tn3 = new TreeNode("tn3");
      tn.Nodes.Add(tn3);
      Assert.IsNull(tn3.NextNode, "NextNode for last treenode in parent collection.");
      Assert.AreEqual(tn3, tn2.NextNode, "NextNode for first node in parent collection.");
   }

   [TestMethod]
   public void PreviousNodeTest()
   {
      TreeNode tn = new TreeNode("tn");
      Assert.IsNull(tn.PreviousNode, "PreviousNode in TreeNode without parent");

      TreeNode tn2 = new TreeNode("tn2");
      tn.Nodes.Add(tn2);
      Assert.IsNull(tn2.PreviousNode, "PreviousNode for single treenode in parent collection.");

      TreeNode tn3 = new TreeNode("tn3");
      tn.Nodes.Add(tn3);
      Assert.IsNull(tn2.PreviousNode, "PreviousNode for first treenode in parent collection.");
      Assert.AreEqual(tn2, tn3.PreviousNode, "NextNode for first node in parent collection.");
   }

   [TestMethod]
   public void NextVisibleNodeTest()
   {
      TreeNode root = new TreeNode("root");
      Nodes.Add(root);
      Assert.IsNull(root.NextVisibleNode, "NextVisibleNode in TreeNode without parent.");

      TreeNode tn1 = new TreeNode("tn1");
      root.Nodes.Add(tn1);
      Assert.IsNull(tn1.NextVisibleNode, "NextVisibleNode for last treenode in chain.");

      root.IsExpanded = false;
      Assert.IsNull(root.NextVisibleNode, "NextVisibleNode for collapsed rootnode.");

      root.IsExpanded = true;
      Assert.AreEqual(tn1, root.NextVisibleNode, "NextVisibleNode for expanded rootnode.");
      
      TreeNode tn2 = new TreeNode("tn2");
      root.Nodes.Add(tn2);
      Assert.AreEqual(tn2, tn1.NextVisibleNode);

      TreeNode tn3 = new TreeNode("tn3");
      tn1.Nodes.Add(tn3);
      tn1.IsExpanded = true;
      Assert.AreEqual(tn2, tn3.NextVisibleNode);

      root.IsExpanded = false;
      Assert.IsNull(tn3.NextVisibleNode, "NextVisibleNode on non-visible node");
   }

   [TestMethod]
   public void PreviousVisibleNodeTest()
   {
      TreeNode root = new TreeNode("root");
      Nodes.Add(root);
      Assert.IsNull(root.PreviousVisibleNode, "PreviousVisibleNode for root.");

      TreeNode tn1 = new TreeNode("tn1");
      root.Nodes.Add(tn1);
      root.IsExpanded = true;
      Assert.AreEqual(root, tn1.PreviousVisibleNode);

      TreeNode tn2 = new TreeNode("tn2");
      tn1.Nodes.Add(tn2);
      tn1.IsExpanded = true;
      TreeNode tn3 = new TreeNode("tn3");
      root.Nodes.Add(tn3);
      Assert.AreEqual(tn2, tn3.PreviousVisibleNode);
      tn1.IsExpanded = false;
      Assert.AreEqual(tn1, tn3.PreviousVisibleNode);

      root.IsExpanded = false;
      Assert.IsNull(tn1.PreviousVisibleNode, "PreviousVisibleNode on non-visible node");
   }

   [TestMethod]
   public void TreeViewTest()
   {
      Outliner.Controls.TreeView tree = new Outliner.Controls.TreeView();
      TreeNode tn1 = new TreeNode("tn1");
      Assert.IsNull(tn1.TreeView);

      tree.Nodes.Add(tn1);
      Assert.AreEqual(tree, tn1.TreeView);

      TreeNode tn2 = new TreeNode("tn2");
      tn1.Nodes.Add(tn2);
      Assert.AreEqual(tree, tn2.TreeView);
   }

   [TestMethod]
   public void BoundsTest()
   {
      Outliner.Controls.TreeView tree = this.CreateTree();
      Rectangle bounds = tree.Nodes[0].Bounds;
      Assert.AreEqual(new Rectangle(0, 0, tree.Width, tree.TreeNodeLayout.ItemHeight), bounds, "first rootnode");

      bounds = tree.Nodes[1].Bounds;
      Assert.AreEqual(new Rectangle(0, tree.TreeNodeLayout.ItemHeight * 3, tree.Width, tree.TreeNodeLayout.ItemHeight), bounds, "second rootnode");

      bounds = tree.Nodes[0].Nodes[0].Bounds;
      Assert.AreEqual(new Rectangle(0, tree.TreeNodeLayout.ItemHeight, tree.Width, tree.TreeNodeLayout.ItemHeight), bounds, "first childnode");

      tree.Nodes[0].IsExpanded = false;
      bounds = tree.Nodes[0].Nodes[0].Bounds;
      Assert.AreEqual(Rectangle.Empty, bounds, "first childnode with collapsed parent");
   }
}
}
