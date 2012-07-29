using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using System.Runtime.InteropServices;
using System.Drawing;
// Import System.Windows.Forms with alias to avoid ambiguity 
// between System.Windows.TreeNode and Outliner.Controls.TreeNode.
using WinForms = System.Windows.Forms;
using Outliner.Controls;
using Outliner.Controls.Tree;
using Outliner.Scene;
using Outliner.Commands;
using MaxUtils;


namespace Outliner
{

public static class HelperMethods
{
   /// <summary>
   /// Returns the NodeWrapper from the Tag of a TreeNode.
   /// </summary>
   public static IMaxNodeWrapper GetMaxNode(TreeNode tn)
   {
      if (tn == null)
         return null;

      return tn.Tag as IMaxNodeWrapper;
   }

   /// <summary>
   /// Maps GetMaxNode to a list of TreeNodes, returning a list of NodeWrappers.
   /// </summary>
   public static IEnumerable<IMaxNodeWrapper> GetMaxNodes(IEnumerable<TreeNode> treeNodes)
   {
      return treeNodes.Select(HelperMethods.GetMaxNode);
   }

   /// <summary>
   /// Extracts all wrapped nodes of type T from a collection of TreeNodes
   /// </summary>
   /// <typeparam name="T">The type of node to select from the IMaxNodeWrapper.</typeparam>
   public static IEnumerable<T> GetWrappedNodes<T>(IEnumerable<TreeNode> treeNodes)
   {
      return GetWrappedNodes<T>(HelperMethods.GetMaxNodes(treeNodes));
   }

   /// <summary>
   /// Extracts all wrapped nodes of type T from a collection of IMaxNodeWrappers
   /// </summary>
   /// <typeparam name="T">The type of node to select from the IMaxNodeWrapper.</typeparam>
   public static IEnumerable<T> GetWrappedNodes<T>(IEnumerable<IMaxNodeWrapper> wrappers)
   {
      return wrappers.Where(w => w.WrappedNode is T).Select(n => (T)n.WrappedNode);
   }



   public static IINodeTab ToIINodeTab(IEnumerable<IMaxNodeWrapper> nodes)
   {
      if (nodes == null)
         throw new ArgumentNullException("nodes");

      IINodeTab tab = MaxInterfaces.Global.INodeTabNS.Create();
      Int32 nodeCount = nodes.Count();
      if (nodes.Count() > 0)
      {
         tab.Resize(nodeCount);
         foreach (IMaxNodeWrapper node in nodes)
         {
            if (node is IINodeWrapper)
               tab.AppendNode((IINode)node.WrappedNode, true, 0);
         }
      }
      return tab;
   }


   /// <summary>
   /// Returns true if the supplied node is a selected node, or a parent of a selected node.
   /// </summary>
   public static Boolean IsParentOfSelected(IMaxNodeWrapper node)
   {
      if (node.Selected)
         return true;

      foreach (IMaxNodeWrapper child in node.WrappedChildNodes)
      {
         if (child.Selected || HelperMethods.IsParentOfSelected(child))
            return true;
      }
      
      return false;
   }

   /// <summary>
   /// Opens or closes a group head.
   /// </summary>
   /// <param name="groupHead">The group head node to open or close.</param>
   public static void OpenCloseGroup(IINodeWrapper groupHead, Boolean open)
   {
      if (groupHead == null)
         return;

      if (groupHead.IINode.IsGroupHead)
         groupHead.IINode.SetGroupHeadOpen(open);

      foreach (IMaxNodeWrapper child in groupHead.WrappedChildNodes)
      {
         IINodeWrapper inodeChild = child as IINodeWrapper;
         if (inodeChild != null && !inodeChild.IINode.IsGroupHead && inodeChild.IINode.IsGroupMember)
         {
            inodeChild.IINode.SetGroupMemberOpen(open);
            OpenCloseGroup(inodeChild, open);
         }
      }
   }


   /// <summary>
   /// Iterates over all elements in the collection with the supplied function.
   /// </summary>
   public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
   {
      if (items == null)
         throw new ArgumentNullException("items");
      if (action == null)
         throw new ArgumentNullException("action");

      foreach (T item in items)
         action(item);
   }


   /// <summary>
   /// Returns true if the Control key is pressed, possibly in combination with other keys.
   /// </summary>
   public static Boolean ControlPressed
   {
      get { return WinForms::Control.ModifierKeys.HasFlag(WinForms::Keys.Control); }
   }

   /// <summary>
   /// Returns true if the Alt key is pressed, possibly in combination with other keys.
   /// </summary>
   public static Boolean AltPressed
   {
      get { return WinForms::Control.ModifierKeys.HasFlag(WinForms::Keys.Alt); }
   }

   /// <summary>
   /// Returns true if the Shift key is pressed, possibly in combination with other keys.
   /// </summary>
   public static Boolean ShiftPressed
   {
      get { return WinForms::Control.ModifierKeys.HasFlag(WinForms::Keys.Shift); }
   }


   public static Double Distance(Point pt1, Point pt2)
   {
      double a = Math.Pow(pt1.X - pt2.X, 2);
      double b = Math.Pow(pt1.Y - pt2.Y, 2);
      return Math.Sqrt(a + b);
   }
}

}
