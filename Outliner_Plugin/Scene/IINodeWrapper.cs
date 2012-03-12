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
      set { this.node.WireColor = value; }
   }

   public override bool Renderable
   {
      get { return this.node.Renderable != 0; }
      set { this.node.SetRenderable(value); }
   }


   public override bool IsValid
   {
      get
      {
         if (!base.IsValid)
            return false;

         try { return !this.node.TestAFlag(AnimatableFlags.IsDeleted); }
         catch { return false; }
      }
   }

   public const String IMGKEY_BONE      = "bone";
   public const String IMGKEY_CAMERA    = "camera";
   public const String IMGKEY_GEOMETRY  = "geometry";
   public const String IMGKEY_GROUPHEAD = "group";
   public const String IMGKEY_HELPER    = "helper";
   public const String IMGKEY_LIGHT     = "light";
   public const String IMGKEY_MATERIAL  = "material";
   public const String IMGKEY_NURBS     = "nurbs";
   public const String IMGKEY_PARTICLE  = "particle";
   public const String IMGKEY_SHAPE     = "shape";
   public const String IMGKEY_SPACEWARP = "spacewarp";
   public const String IMGKEY_TARGET    = "helper";
   public const String IMGKEY_XREF      = "xref";
   

   public override string ImageKey
   {
      get
      {
         if (this.node == null || this.node.ObjectRef == null)
            return base.ImageKey;

         SClass_ID superClass = this.SuperClassID;
         switch (superClass)
         {
            case SClass_ID.Camera: return IMGKEY_CAMERA;
            case SClass_ID.Light: return IMGKEY_LIGHT;
            case SClass_ID.Material: return IMGKEY_MATERIAL;
            case SClass_ID.Shape: return IMGKEY_SHAPE;
            case SClass_ID.WsmObject: return IMGKEY_SPACEWARP;
         }

         if (superClass == SClass_ID.System && HelperMethods.IsXref(node))
            return IMGKEY_XREF;

         if (superClass == SClass_ID.Helper)
         {
            if (this.node.IsGroupHead)
               return IMGKEY_GROUPHEAD;
            else
               return IMGKEY_HELPER;
         }

         if (superClass == SClass_ID.Geomobject)
         {
            //Target objects (for light/camera target)
            if (node.IsTarget)
               return IMGKEY_TARGET;

            IObject objRef = node.ObjectRef;
            if (objRef == null)
               return IMGKEY_GEOMETRY;

            //Nurbs / Shape objects.
            if (objRef.IsShapeObject)
               return IMGKEY_NURBS;

            //Particle objects.
            if (objRef.IsParticleSystem)
               return IMGKEY_PARTICLE;

            //Bone and biped objects have Geomobject as a superclass.
            if (HelperMethods.IsBone(node))
               return IMGKEY_BONE;

            //All other geometry objects.
            return IMGKEY_GEOMETRY;
         }

         return base.ImageKey;
      }
   }
}
}
