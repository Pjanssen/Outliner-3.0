using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Outliner.Controls;
using Outliner.Scene;
using System.Drawing;

namespace Outliner
{

public static class HelperMethods
{
   /// <summary>
   /// Creates a new TreeNode object with a correct TreeNodeData object as its Tag.
   /// </summary>
   public static TreeNode CreateTreeNode(IINode node)
   {
      return HelperMethods.CreateTreeNode(IMaxNodeWrapper.Create(node));
   }

   /// <summary>
   /// Creates a new TreeNode object with a correct TreeNodeData object as its Tag.
   /// </summary>
   public static TreeNode CreateTreeNode(IMaxNodeWrapper node)
   {
      TreeNode tn = new TreeNode(node.Name);
      tn.Tag = new OutlinerTreeNodeData(tn, node);
      tn.ImageKey = node.ImageKey;
      return tn;
   }


   /// <summary>
   /// Returns the NodeWrapper from the Tag of a TreeNode.
   /// </summary>
   public static IMaxNodeWrapper GetMaxNode(TreeNode tn)
   {
      if (tn == null)
         return null;

      OutlinerTreeNodeData tnData = tn.Tag as OutlinerTreeNodeData;
      if (tnData == null)
         return null;
      else
         return tnData.Node;
   }

   /// <summary>
   /// Maps GetMaxNode to a list of TreeNodes, returning a list of NodeWrappers.
   /// </summary>
   public static IEnumerable<IMaxNodeWrapper> GetMaxNodes(IEnumerable<TreeNode> tns)
   {
      return tns.Select(HelperMethods.GetMaxNode);
   }

   public static IEnumerable<T> GetWrappedNodes<T>(IEnumerable<TreeNode> tns)
   {
      return GetWrappedNodes<T>(HelperMethods.GetMaxNodes(tns));
   }
   public static IEnumerable<T> GetWrappedNodes<T>(IEnumerable<IMaxNodeWrapper> wrappers)
   {
      return wrappers.Where(w => w.WrappedNode is T).Select(n => (T)n.WrappedNode);
   }

   /// <summary>
   /// Marshals the INotifyInfo object from a pointer sent by a general event callback.
   /// </summary>
   public static INotifyInfo GetNotifyInfo(IntPtr infoPtr)
   {
      return GlobalInterface.Instance.NotifyInfo.Marshal(infoPtr);
   }

   /// <summary>
   /// Converts the ITab to a more convenient IEnumerable.
   /// </summary>
   public static IEnumerable<T> ToIEnumerable<T>(this ITab<T> tab)
   {
      List<T> lst = new List<T>(tab.Count);
      for (int i = 0; i < tab.Count; i++)
         lst.Add(tab[(IntPtr)i]);

      return lst;
   }

   /// <summary>
   /// Retrieves the IINodes from a ITab of handles.
   /// </summary>
   public static IEnumerable<IINode> NodeKeysToINodeList(this ITab<UIntPtr> handles)
   {
      return handles.ToIEnumerable().Select(GlobalInterface.Instance.NodeEventNamespace.GetNodeByKey);
   }


   public static bool ClassIDEquals(IClass_ID cid, BuiltInClassIDA cidA)
   {
      return ClassIDEquals(cid, (uint)cidA);
   }

   public static bool ClassIDEquals(IClass_ID cid, BuiltInClassIDA cidA, BuiltInClassIDB cidB)
   {
      return ClassIDEquals(cid, (uint)cidA, (uint)cidB);
   }

   public static bool ClassIDEquals(IClass_ID cid, uint cidA)
   {
      return ClassIDEquals(cid, cidA, 0);
   }

   public static bool ClassIDEquals(IClass_ID cid, uint cidA, uint cidB)
   {
      return cid.PartA == cidA && cid.PartB == cidB;
   }

   
   public const uint PARTICLECHANNELCLASSID_PARTB = 0x1eb34100;
   public const uint PFACTIONCLASSID_PARTB        = 0x1eb34200;
   public const uint PFACTORCLASSID_PARTB         = 0x1eb34300;
   public const uint PFMATERIALCLASSID_PARTB      = 0x1eb34400;

   public static bool IsPFHelper(IINode node)
   {
      if (node == null)
         return false;

      IObject objRef = node.ObjectRef;
      if (objRef == null)
         return false;

      uint classID_B = objRef.ClassID.PartB;
      return classID_B == PARTICLECHANNELCLASSID_PARTB
          || classID_B == PFACTIONCLASSID_PARTB
          || classID_B == PFACTORCLASSID_PARTB
          || classID_B == PFMATERIALCLASSID_PARTB;
   }

   /// <summary>
   /// Workaround 3dsMax Color issues. (Alpha + flipped components)
   /// </summary>
   /// <param name="c">The color value from 3dsMax.</param>
   /// <returns>A correct color value.</returns>
   public static Color FromMaxColor(Color c)
   {
      return Color.FromArgb(255, c.B, c.G, c.R);
   }

   /// <summary>
   /// Workaround 3dsMax Color issues. (Alpha + flipped components)
   /// </summary>
   /// <param name="c">The .NET color to change.</param>
   /// <returns>A color that the 3dsMax SDK can do something with.</returns>
   public static Color ToMaxColor(Color c)
   {
      return Color.FromArgb(0, c.B, c.G, c.R);
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
}

}
