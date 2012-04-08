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

   /// <summary>
   /// Marshals the INotifyInfo object from a pointer sent by a general event callback.
   /// </summary>
   public static INotifyInfo GetNotifyInfo(IntPtr info)
   {
      return GlobalInterface.Instance.NotifyInfo.Marshal(info);
   }

   /// <summary>
   /// Converts the ITab to a more convenient IEnumerable.
   /// </summary>
   public static IEnumerable<T> ToIEnumerable<T>(this ITab<T> tab)
   {
      if (tab == null)
         throw new ArgumentNullException("tab");

      List<T> lst = new List<T>(tab.Count);
      for (int i = 0; i < tab.Count; i++)
         lst.Add(tab[(IntPtr)i]);

      return lst;
   }

   public static IINodeTab ToIINodeTab(IEnumerable<IMaxNodeWrapper> nodes)
   {
      if (nodes == null)
         throw new ArgumentNullException("nodes");

      IINodeTab tab = GlobalInterface.Instance.INodeTabNS.Create();
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
      if (cid == null)
         return false;
      else
         return cid.PartA == cidA && cid.PartB == cidB;
   }


   public const uint BIPED_CLASSIDA               = 0x9155;
   public const uint SKELOBJ_CLASSIDA             = 0x9125;
   public const uint CATBONE_CLASSIDA             = 0x2E6A0C09;
   public const uint CATBONE_CLASSIDB             = 0x43D5C9C0;
   public const uint CATHUB_CLASSIDA              = 0x73DC4833;
   public const uint CATHUB_CLASSIDB              = 0x65C93CAA;
   public const uint PARTICLECHANNELCLASSID_PARTB = 0x1eb34100;
   public const uint PFACTIONCLASSID_PARTB        = 0x1eb34200;
   public const uint PFACTORCLASSID_PARTB         = 0x1eb34300;
   public const uint PFMATERIALCLASSID_PARTB      = 0x1eb34400;
   public const String CAM_3DXSTUDIO_NAME         = "3DxStudio Perspective";

   /// <summary>
   /// Tests if the supplied IINode is an "invisible" node.
   /// </summary>
   public static Boolean IsInvisibleNode(IINode node)
   {
      if (node == null)
         return false;
      else
         return IsPFHelper(node) || node.Name == CAM_3DXSTUDIO_NAME;
   }

   /// <summary>
   /// Tests whether the supplied IINode is a bone object.
   /// </summary>
   public static Boolean IsBone(IINode node)
   {
      if (node != null && node.ObjectRef != null && node.ObjectRef.SuperClassID == SClass_ID.Geomobject)
      {
         IClass_ID classID = node.ObjectRef.ClassID;
         return ClassIDEquals(classID, BuiltInClassIDA.BONE_OBJ_CLASSID, BuiltInClassIDB.BONE_OBJ_CLASSID)
             || ClassIDEquals(classID, SKELOBJ_CLASSIDA)
             || ClassIDEquals(classID, BIPED_CLASSIDA)
             || ClassIDEquals(classID, CATBONE_CLASSIDA, CATBONE_CLASSIDB)
             || ClassIDEquals(classID, CATHUB_CLASSIDA, CATHUB_CLASSIDB);
      }
      return false;
   }

   /// <summary>
   /// Tests whether the supplied IINode is a particle flow helper object.
   /// </summary>
   public static bool IsPFHelper(IINode node)
   {
      if (node == null || node.ObjectRef == null)
         return false;

      IObject objRef = node.ObjectRef;

      uint classID_B = objRef.ClassID.PartB;
      return classID_B == PARTICLECHANNELCLASSID_PARTB
          || classID_B == PFACTIONCLASSID_PARTB
          || classID_B == PFACTORCLASSID_PARTB
          || classID_B == PFMATERIALCLASSID_PARTB;
   }

   /// <summary>
   /// Tests if the supplied IINode is an xref node.
   /// </summary>
   /// <param name="node"></param>
   /// <returns></returns>
   public static Boolean IsXref(IINode node)
   {
      if (node == null || node.ObjectRef == null)
         return false;

      IObject objRef = node.ObjectRef;
      if (objRef.SuperClassID != SClass_ID.System)
         return false;

      IClass_ID cID = objRef.ClassID;
      return ClassIDEquals(cID, BuiltInClassIDA.XREFOBJ_CLASS_ID)
          || ClassIDEquals(cID, BuiltInClassIDA.XREFMATERIAL_CLASS_ID, BuiltInClassIDB.XREFMATERIAL_CLASS_ID);
   }


   private static IInterface_ID nodeLayerProperties;
   public static IInterface_ID NodeLayerProperties
   {
      get
      {
         if (nodeLayerProperties == null)
            nodeLayerProperties = GlobalInterface.Instance.Interface_ID.Create(0x44e025f8, 0x6b071e44);

         return nodeLayerProperties;
      }
   }

   public static IIInstanceMgr InstanceMgr
   {
      get
      {
         return GlobalInterface.Instance.IInstanceMgr.InstanceMgr;
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
