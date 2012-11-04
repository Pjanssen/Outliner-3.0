using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;

namespace Outliner.MaxUtils
{
public static class ClassIDHelpers
{
   /// <summary>
   /// Tests if an object is a plugin with the supplied SuperClass.
   /// </summary>
   public static bool IsSuperClass(Object node, SClass_ID scid)
   {
      IINode inode = node as IINode;
      if (inode == null)
         return false;
      else
         return IsSuperClass(inode, scid);
   }

   /// <summary>
   /// Tests if an object is a plugin with the supplied SuperClass.
   /// </summary>
   public static bool IsSuperClass(IINode node, SClass_ID scid)
   {
      if (node == null)
         return false;

      return IsSuperClass(node.ObjectRef, scid);
   }

   /// <summary>
   /// Tests if an object is a plugin with the supplied SuperClass.
   /// </summary>
   public static bool IsSuperClass(IObject objectRef, SClass_ID scid)
   {
      if (objectRef == null)
         return false;

      return objectRef.SuperClassID == scid;
   }


   /// <summary>
   /// Tests if an object is a plugin with the supplied BuildInClassID
   /// </summary>
   public static bool IsClass(Object node, BuiltInClassIDA cidA)
   {
      return IsClass(node, (uint)cidA, 0);
   }

   /// <summary>
   /// Tests if an object is a plugin with the supplied BuildInClassID
   /// </summary>
   public static bool IsClass(Object node, BuiltInClassIDA cidA, BuiltInClassIDB cidB)
   {
      return IsClass(node, (uint)cidA, (uint)cidB);
   }

   /// <summary>
   /// Tests if an object is a plugin with the supplied ClassID.
   /// </summary>
   public static bool IsClass(Object node, uint cidA, uint cidB)
   {
      IINode iinode = node as IINode;
      if (iinode == null)
         return false;
      else
         return IsClass(iinode, cidA, cidB);
   }

   /// <summary>
   /// Tests if an inode is a plugin with the supplied ClassID.
   /// </summary>
   public static bool IsClass(IINode node, uint cidA, uint cidB)
   {
      if (node == null)
         return false;

      return IsClass(node.ObjectRef, cidA, cidB);
   }

   /// <summary>
   /// Tests if an object reference is a plugin with the supplied ClassID.
   /// </summary>
   public static bool IsClass(IObject objectRef, uint cidA, uint cidB)
   {
      return Equals(objectRef.ClassID, cidA, cidB);
   }


   public static bool Equals(IClass_ID cid, BuiltInClassIDA cidA)
   {
      return ClassIDHelpers.Equals(cid, (uint)cidA, 0);
   }

   public static bool Equals(IClass_ID cid, BuiltInClassIDA cidA, BuiltInClassIDB cidB)
   {
      return ClassIDHelpers.Equals(cid, (uint)cidA, (uint)cidB);
   }

   public static bool Equals(IClass_ID cid, uint cidA, uint cidB)
   {
      if (cid == null)
         return false;
      else
         return cid.PartA == cidA && cid.PartB == cidB;
   }


   public const uint BipedClassIDA = 0x9155;
   public const uint SkelObjClassIDA = 0x9125;
   public const uint CatBoneClassIDA = 0x2E6A0C09;
   public const uint CatBoneClassIDB = 0x43D5C9C0;
   public const uint CatHubClassIDA = 0x73DC4833;
   public const uint CatHubClassIDB = 0x65C93CAA;
   public const uint ParticleChannelClassIDB = 0x1eb34100;
   public const uint PFActionClassIDB = 0x1eb34200;
   public const uint PFActorClassIDB = 0x1eb34300;
   public const uint PFMaterialClassIDB = 0x1eb34400;  
}
}
