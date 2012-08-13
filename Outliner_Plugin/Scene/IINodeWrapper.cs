using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using System.Drawing;
using Outliner.Controls;
using MaxUtils;

namespace Outliner.Scene
{
public class IINodeWrapper : IMaxNodeWrapper
{
   private IINode iinode;
   public IINodeWrapper(IINode node)
   {
      this.iinode = node;
   }

   public override object WrappedNode
   {
      get { return this.iinode; }
   }

   public override bool Equals(object obj)
   {
      IINodeWrapper otherObj = obj as IINodeWrapper;
      return otherObj != null && this.iinode.Handle == otherObj.iinode.Handle;
   }

   public override int GetHashCode()
   {
      return this.WrappedNode.GetHashCode();
   }

   public IINode IINode
   {
      get { return this.iinode; }
   }

   public  IINodeLayerProperties NodeLayerProperties
   {
      get
      {
         IBaseInterface baseInterface = this.iinode.GetInterface(MaxInterfaces.NodeLayerProperties);
         return baseInterface as IINodeLayerProperties;
      }
   }

   public IILayer IILayer
   {
      get
      {
         return this.IINode.GetReference((int)ReferenceNumbers.NodeLayerRef) as IILayer;
      }
   }

   public override IMaxNodeWrapper Parent
   {
      get
      {
         return new IINodeWrapper(this.iinode.ParentNode);
      }
   }

   public override IEnumerable<Object> ChildNodes
   {
      get 
      {
         int numChildren = this.iinode.NumberOfChildren;
         List<IINode> nodes = new List<IINode>(numChildren);
         for (int i = 0; i < numChildren; i++)
            nodes.Add(this.iinode.GetChildNode(i));
         return nodes;
      }
   }

   public override bool CanAddChildNode(IMaxNodeWrapper node)
   {
      IINodeWrapper iinodeWrapper = node as IINodeWrapper;
      if (iinodeWrapper == null)
         return false;

      if (node.Parent.Equals(this))
         return false;

      RefResult loop = this.iinode.TestForLoop( MaxInterfaces.Interval_Forever
                                              , iinodeWrapper.IINode as IReferenceMaker);

      return loop == RefResult.Succeed;
   }
   public override bool CanAddChildNodes(IEnumerable<IMaxNodeWrapper> nodes)
   {
      return nodes.All(this.CanAddChildNode);
   }

   public override void AddChildNode(IMaxNodeWrapper node)
   {
      if (node == null)
         throw new ArgumentNullException("node");

      if (node.WrappedNode is IINode)
         this.iinode.AttachChild((IINode)node.WrappedNode, true);
   }

   public override void RemoveChildNode(IMaxNodeWrapper node)
   {
      if (node is IINodeWrapper)
         ((IINodeWrapper)node).iinode.Detach(0, true);
   }

   public override String Name
   {
      get { return this.iinode.Name; }
      set { this.iinode.Name = value; }
   }

   public override string DisplayName
   {
      get
      {
         String name = this.Name;

         if (String.IsNullOrEmpty(name))
            return "-unnamed-";

         Boolean isGroupedNode = this.iinode.IsGroupHead || this.iinode.IsGroupMember;
         if (IINodeHelpers.IsXref(this.iinode))
         {
            if (isGroupedNode)
               return "{[" + name + "]}";
            else
               return "{" + name + "}";
         }
         else if (isGroupedNode)
            return "[" + name + "]";
         else
            return name;
      }
   }

   public override IClass_ID ClassID
   {
      get { return this.iinode.ObjectRef.ClassID; }
   }

   public override SClass_ID SuperClassID
   {
      get { return this.iinode.ObjectRef.FindBaseObject().SuperClassID; }
   }

   public override bool Selected
   {
      get { return this.iinode.Selected; }
   }

   public override bool IsNodeType(MaxNodeTypes types)
   {
      return (types & MaxNodeTypes.Object) == MaxNodeTypes.Object;
   }

   public override bool IsHidden
   {
      get { return this.iinode.IsObjectHidden; }
      set { this.iinode.Hide(value); }
   }

   public override bool IsFrozen
   {
      get { return this.iinode.IsObjectFrozen; }
      set { this.iinode.IsFrozen = value; }
   }

   public override bool BoxMode
   {
      get { return this.iinode.BoxMode_ != 0; }
      set { this.iinode.BoxMode(value); }
   }

   public override Color WireColor
   {
      get { return ColorHelpers.FromMaxColor(this.iinode.WireColor); }
      set { this.iinode.WireColor = value; }
   }

   public override bool Renderable
   {
      get { return this.iinode.Renderable != 0; }
      set { this.iinode.SetRenderable(value); }
   }

   public bool IsInstance
   {
      get
      {
         IINodeTab instances = MaxInterfaces.Global.INodeTabNS.Create();
         uint numInstances = MaxInterfaces.InstanceMgr.GetInstances(this.iinode, instances);
         return numInstances > 1;
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
         if (this.iinode == null || this.iinode.ObjectRef == null)
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

         if (superClass == SClass_ID.System && IINodeHelpers.IsXref(iinode))
            return IMGKEY_XREF;

         if (superClass == SClass_ID.Helper)
         {
            if (this.iinode.IsGroupHead)
               return IMGKEY_GROUPHEAD;
            else
               return IMGKEY_HELPER;
         }

         if (superClass == SClass_ID.Geomobject)
         {
            //Target objects (for light/camera target)
            if (iinode.IsTarget)
               return IMGKEY_TARGET;

            IObject objRef = iinode.ObjectRef;
            if (objRef == null)
               return IMGKEY_GEOMETRY;

            //Nurbs / Shape objects.
            if (objRef.IsShapeObject)
               return IMGKEY_NURBS;

            //Particle objects.
            if (objRef.IsParticleSystem)
               return IMGKEY_PARTICLE;

            //Bone and biped objects have Geomobject as a superclass.
            if (IINodeHelpers.IsBone(iinode))
               return IMGKEY_BONE;

            //All other geometry objects.
            return IMGKEY_GEOMETRY;
         }

         return base.ImageKey;
      }
   }
}
}
