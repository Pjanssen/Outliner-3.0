using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PJanssen.Outliner.Controls.Tree;
using System.Drawing;

namespace PJanssen.Outliner.Tests.Controls.Tree
{
[TestClass]
public class TreeViewTest
{
   public TreeView CreateTree()
   {
      TreeView tree = new TreeView();
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

   [TestMethod]
   public void GetNodeAtNormalTest()
   {
      TreeView tree = this.CreateTree();
      TreeNode expected = tree.Nodes[0];
      TreeNode actual = tree.GetNodeAt(new Point(1, 1));
      Assert.AreEqual(expected, actual, "First node");

      expected = tree.Nodes[0].Nodes[0];
      actual = tree.GetNodeAt(new Point(1, tree.TreeNodeLayout.ItemHeight + 1));
      Assert.AreEqual(expected, actual, "First childnode");
   }

   [TestMethod]
   public void GetNodeAtExceptionalTest()
   {
      TreeView tree = new TreeView();
      tree.Width = 100;
      tree.Height = 100;
      Assert.IsNull(tree.GetNodeAt(0, 0), "Empty tree");

      Assert.IsNull(tree.GetNodeAt(-5, 0), "Outside bounds left");
      Assert.IsNull(tree.GetNodeAt(200, 0), "Outside bounds right");
      Assert.IsNull(tree.GetNodeAt(0, -5), "Outside bounds top");
      Assert.IsNull(tree.GetNodeAt(0, 200), "Outside bounds bottom");
   }
}
}
