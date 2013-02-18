using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;

namespace Outliner.MaxUtils
{
/// <summary>
/// Provides methods for common IINode operations.
/// </summary>
public static class IINodes
{
   /// <summary>
   /// The name of the 3DxStudio camera.
   /// </summary>
   public const String Cam3DXStudioName = "3DxStudio Perspective";

   /// <summary>
   /// Tests if the supplied IINode is an "invisible" node.
   /// </summary>
   public static Boolean IsInvisibleNode(IINode node)
   {
      if (node == null)
         return false;
      else
         return IsPFHelper(node) || node.Name == IINodes.Cam3DXStudioName;
   }

   /// <summary>
   /// Tests whether the supplied IINode is a bone object.
   /// </summary>
   public static Boolean IsBone(IINode node)
   {
      if (!ClassIDs.IsSuperClass(node, SClass_ID.Geomobject))
         return false;

      return ClassIDs.IsClass(node, BuiltInClassIDA.BONE_OBJ_CLASSID, BuiltInClassIDB.BONE_OBJ_CLASSID)
             || ClassIDs.IsClass(node, ClassIDs.SkelObjClassIDA, 0)
             || ClassIDs.IsClass(node, ClassIDs.BipedClassIDA, 0)
             || ClassIDs.IsClass(node, ClassIDs.CatBoneClassIDA, ClassIDs.CatBoneClassIDB)
             || ClassIDs.IsClass(node, ClassIDs.CatHubClassIDA, ClassIDs.CatHubClassIDB);
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
      return classID_B == ClassIDs.ParticleChannelClassIDB
            || classID_B == ClassIDs.PFActionClassIDB
            || classID_B == ClassIDs.PFActorClassIDB
            || classID_B == ClassIDs.PFMaterialClassIDB;
   }

   /// <summary>
   /// Tests if the supplied IINode is an xref node.
   /// </summary>
   /// <param name="node"></param>
   /// <returns></returns>
   public static Boolean IsXref(IINode node)
   {
      if (!ClassIDs.IsSuperClass(node, SClass_ID.System))
         return false;

      return ClassIDs.IsClass(node, BuiltInClassIDA.XREFOBJ_CLASS_ID) ||
             ClassIDs.IsClass(node, BuiltInClassIDA.XREFMATERIAL_CLASS_ID
                                        , BuiltInClassIDB.XREFMATERIAL_CLASS_ID);
   }


   /// <summary>
   /// Retrieves the IINodes from a ITab of handles.
   /// </summary>
   public static IEnumerable<IINode> NodeKeysToINodeList(this ITab<UIntPtr> handles)
   {
      return IINodes.ITabToIEnumerable(handles)
                          .Select(MaxInterfaces.Global.NodeEventNamespace.GetNodeByKey);
   }


   /// <summary>
   /// Converts the ITab to a more convenient IEnumerable.
   /// </summary>
   public static IEnumerable<T> ITabToIEnumerable<T>(ITab<T> tab)
   {
      Throw.IfArgumentIsNull(tab, "tab");
      
      for (int i = 0; i < tab.Count; i++)
         yield return tab[(IntPtr)i];
   }

   /// <summary>
   /// Converts an IEnumerable of objects into an IINodeTab.
   /// </summary>
   public static IINodeTab ToIINodeTab(IEnumerable<Object> nodes)
   {
      Throw.IfArgumentIsNull(nodes, "nodes");

      IINodeTab tab = MaxInterfaces.Global.INodeTabNS.Create();
      Int32 nodeCount = nodes.Count();
      if (nodes.Count() > 0)
      {
         tab.Resize(nodeCount);
         foreach (Object node in nodes)
         {
            IINode inode = node as IINode;
            if (inode != null)
               tab.AppendNode(inode, true, 0);
         }
      }
      return tab;
   }


   private const int enable = (int)Autodesk.Max.ObjectWrapper.E173.AllEnable;
   private const int nativeType = (int)Autodesk.Max.ObjectWrapper.E172.TriObject;

   /// <summary>
   /// Returns the polycount of an IINode.
   /// </summary>
   public static Int32 GetPolyCount(IINode node)
   {
      int time = MaxInterfaces.COREInterface.Time;
      IObjectWrapper objWrapperX = MaxInterfaces.Global.ObjectWrapper.Create();
      objWrapperX.Init(time, node.EvalWorldState(time, true), false, enable, nativeType);

      int xNumFaces = objWrapperX.NumFaces;
      objWrapperX.Release();
      return xNumFaces;
   }
}
}
