using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;

namespace MaxUtils
{
public static class IINodeHelpers
{
   public const String CAM_3DXSTUDIO_NAME = "3DxStudio Perspective";

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
         return ClassIDHelpers.Equals(classID, BuiltInClassIDA.BONE_OBJ_CLASSID, BuiltInClassIDB.BONE_OBJ_CLASSID)
               || ClassIDHelpers.Equals(classID, ClassIDHelpers.SKELOBJ_CLASSIDA)
               || ClassIDHelpers.Equals(classID, ClassIDHelpers.BIPED_CLASSIDA)
               || ClassIDHelpers.Equals(classID, ClassIDHelpers.CATBONE_CLASSIDA, ClassIDHelpers.CATBONE_CLASSIDB)
               || ClassIDHelpers.Equals(classID, ClassIDHelpers.CATHUB_CLASSIDA, ClassIDHelpers.CATHUB_CLASSIDB);
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
      return classID_B == ClassIDHelpers.PARTICLECHANNEL_CLASSIDB
            || classID_B == ClassIDHelpers.PFACTION_CLASSIDB
            || classID_B == ClassIDHelpers.PFACTOR_CLASSIDB
            || classID_B == ClassIDHelpers.PFMATERIAL_CLASSIDB;
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
      return ClassIDHelpers.Equals(cID, BuiltInClassIDA.XREFOBJ_CLASS_ID)
            || ClassIDHelpers.Equals(cID, BuiltInClassIDA.XREFMATERIAL_CLASS_ID, BuiltInClassIDB.XREFMATERIAL_CLASS_ID);
   }


   /// <summary>
   /// Retrieves the IINodes from a ITab of handles.
   /// </summary>
   public static IEnumerable<IINode> NodeKeysToINodeList(this ITab<UIntPtr> handles)
   {
      return handles.ToIEnumerable().Select(MaxInterfaces.Global.NodeEventNamespace.GetNodeByKey);
   }

}
}
