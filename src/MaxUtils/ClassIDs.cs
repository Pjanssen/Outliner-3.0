using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;

namespace PJanssen.Outliner.MaxUtils
{
/// <summary>
/// Provides constants and methods for common IINode operations.
/// </summary>
public static class ClassIDs
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

   /// <summary>
   /// Determines if the ClassID and the BuiltInClassID are equivalent.
   /// </summary>
   public static bool Equals(IClass_ID cid, BuiltInClassIDA cidA)
   {
      return ClassIDs.Equals(cid, (uint)cidA, 0);
   }

   /// <summary>
   /// Determines if the ClassID and the BuiltInClassID pair are equivalent.
   /// </summary>
   public static bool Equals(IClass_ID cid, BuiltInClassIDA cidA, BuiltInClassIDB cidB)
   {
      return ClassIDs.Equals(cid, (uint)cidA, (uint)cidB);
   }

   /// <summary>
   /// Determines if the ClassID and the uints are equivalent.
   /// </summary>
   public static bool Equals(IClass_ID cid, uint cidA, uint cidB)
   {
      if (cid == null)
         return false;
      else
         return cid.PartA == cidA && cid.PartB == cidB;
   }

   /// <summary>
   /// ClassID part A for the biped class.
   /// </summary>
   public const uint BipedClassIDA = 0x9155;

   /// <summary>
   /// ClassID part A for the skeleton object class.
   /// </summary>
   public const uint SkelObjClassIDA = 0x9125;

   /// <summary>
   /// ClassID part A for the CAT bone class.
   /// </summary>
   public const uint CatBoneClassIDA = 0x2E6A0C09;

   /// <summary>
   /// ClassID part B for the CAT bone class.
   /// </summary>
   public const uint CatBoneClassIDB = 0x43D5C9C0;

   /// <summary>
   /// ClassID part A for the CAT Hub class.
   /// </summary>
   public const uint CatHubClassIDA = 0x73DC4833;

   /// <summary>
   /// ClassID part B for the CAT Hub class.
   /// </summary>
   public const uint CatHubClassIDB = 0x65C93CAA;

   /// <summary>
   /// ClassID part B for the ParticleChannel class.
   /// </summary>
   public const uint ParticleChannelClassIDB = 0x1eb34100;

   /// <summary>
   /// ClassID part B for the ParticleFlow Action class.
   /// </summary>
   public const uint PFActionClassIDB = 0x1eb34200;

   /// <summary>
   /// ClassID part B for the ParticleFlow Actor class.
   /// </summary>
   public const uint PFActorClassIDB = 0x1eb34300;
   
   /// <summary>
   /// ClassID part B for the ParticleFlow Material class.
   /// </summary>
   public const uint PFMaterialClassIDB = 0x1eb34400;  
}
}
