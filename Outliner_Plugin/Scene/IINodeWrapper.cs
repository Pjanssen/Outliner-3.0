using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using System.Drawing;

namespace Outliner.Scene
{
public class IINodeWrapper : IMaxNodeWrapper
{
   private IINode node;
   public IINodeWrapper(IINode node)
   {
      this.node = node;
   }

   public override object WrappedNode
   {
      get { return this.node; }
   }

   public override int NumChildren
   {
      get { return this.node.NumberOfChildren; }
   }

   public override IEnumerable<IMaxNodeWrapper> ChildNodes
   {
      get 
      {
         int numChildren = this.NumChildren;
         List<IMaxNodeWrapper> nodes = new List<IMaxNodeWrapper>(numChildren);
         for (int i = 0; i < numChildren; i++)
            nodes.Add(IMaxNodeWrapper.Create(this.node.GetChildNode(i)));
         return nodes;
      }
   }

   public override String Name
   {
      get { return this.node.Name; }
      set { this.node.Name = value; }
   }

   public override IClass_ID ClassID
   {
      get { return this.node.ObjectRef.ClassID; }
   }

   public override SClass_ID SuperClassID
   {
      get { return this.node.ObjectRef.SuperClassID; }
   }

   public override bool IsHidden
   {
      get { return this.node.IsObjectHidden; }
      set { this.node.Hide(value); }
   }

   public override bool IsFrozen
   {
      get { return this.node.IsObjectFrozen; }
      set { this.node.IsFrozen = value; }
   }

   public override bool BoxMode
   {
      get { return this.node.BoxMode_ != 0; }
      set { this.node.BoxMode(value); }
   }

   public override Color WireColor
   {
      get { return HelperMethods.FromMaxColor(this.node.WireColor); }
      set { this.node.WireColor = HelperMethods.ToMaxColor(value); }
   }


   public const String IMGKEY_BONE      = "bone";
   public const String IMGKEY_CAMERA    = "camera";
   public const String IMGKEY_GEOMETRY  = "geometry";
   public const String IMGKEY_HELPER    = "helper";
   public const String IMGKEY_LIGHT     = "light";
   public const String IMGKEY_MATERIAL  = "material";
   public const String IMGKEY_PARTICLE  = "particle";
   public const String IMGKEY_SHAPE     = "shape";
   public const String IMGKEY_SPACEWARP = "spacewarp";
   public const String IMGKEY_TARGET    = "helper";

   public const uint BIPED_CLASSIDA      = 0x9155;
   public const uint SKELOBJ_CLASSIDA    = 0x9125;
   public const uint PFSOURCE_CLASSIDA   = 0x50320C9A;
   public const uint SPRAY_CLASSIDA      = 0x9BD61AA0;
   public const uint SUPERSPRAY_CLASSIDA = 0x74F811E3;
   public const uint SUPERSPRAY_CLASSIDB = 0x21fb7b57;
   public const uint PARRAY_CLASSIDA     = 0xE3C25B5;
   public const uint PARRAY_CLASSIDB     = 0x109D1659;
   public const uint PCLOUD_CLASSIDA     = 0x1C0F3D2F;
   public const uint PCLOUD_CLASSIDB     = 0x30310AF9;
   public const uint BLIZZARD_CLASSIDA   = 0x5835054D;
   public const uint BLIZZARD_CLASSIDB   = 0x564B40ED;

   public override string ImageKey
   {
      get
      {
         if (this.node == null || this.node.ObjectRef == null)
            return base.ImageKey;

         SClass_ID superClass = this.node.ObjectRef.SuperClassID;
         switch (superClass)
         {
            case SClass_ID.Camera: return IMGKEY_CAMERA;
            case SClass_ID.Helper: return IMGKEY_HELPER;
            case SClass_ID.Light: return IMGKEY_LIGHT;
            case SClass_ID.Material: return IMGKEY_MATERIAL;
            case SClass_ID.Shape: return IMGKEY_SHAPE;
            case SClass_ID.WsmObject: return IMGKEY_SPACEWARP;
         }

         if (superClass == SClass_ID.Geomobject)
         {
            IClass_ID classID = node.ObjectRef.ClassID;

            //Target objects (for light/camera target)
            if (HelperMethods.ClassIDEquals(classID, BuiltInClassIDA.TARGET_CLASS_ID))
               return IMGKEY_TARGET;

            //Bone and biped objects have Geomobject as a superclass.
            if (HelperMethods.ClassIDEquals(classID, BuiltInClassIDA.BONE_OBJ_CLASSID, BuiltInClassIDB.BONE_OBJ_CLASSID)
                  || HelperMethods.ClassIDEquals(classID, SKELOBJ_CLASSIDA)
                  || HelperMethods.ClassIDEquals(classID, BIPED_CLASSIDA))
               return IMGKEY_BONE;

            //Particle objects.
            if (HelperMethods.ClassIDEquals(classID, PFSOURCE_CLASSIDA)
                || HelperMethods.ClassIDEquals(classID, SPRAY_CLASSIDA)
                || HelperMethods.ClassIDEquals(classID, BuiltInClassIDA.SNOW_CLASS_ID)
                || HelperMethods.ClassIDEquals(classID, SUPERSPRAY_CLASSIDA, SUPERSPRAY_CLASSIDB)
                || HelperMethods.ClassIDEquals(classID, BLIZZARD_CLASSIDA, BLIZZARD_CLASSIDB)
                || HelperMethods.ClassIDEquals(classID, PARRAY_CLASSIDA, PARRAY_CLASSIDB)
                || HelperMethods.ClassIDEquals(classID, PCLOUD_CLASSIDA, PCLOUD_CLASSIDB))
               return IMGKEY_PARTICLE;

            //All other geometry objects.
            return IMGKEY_GEOMETRY;
         }

         return base.ImageKey;
      }
   }
}
}
