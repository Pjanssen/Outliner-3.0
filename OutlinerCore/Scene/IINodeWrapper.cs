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
      //SelectionSet
      if (node is SelectionSetWrapper)
         return this.CanAddChildNodes(node.WrappedChildNodes);

      //IINode
      IINodeWrapper iinodeWrapper = node as IINodeWrapper;
      if (iinodeWrapper == null)
         return false;

      if (node.Parent.Equals(this))
         return false;

      RefResult loop = this.iinode.TestForLoop( MaxInterfaces.IntervalForever
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

      if (!this.CanAddChildNode(node))
         return;

      if (node is SelectionSetWrapper)
         this.AddChildNodes(node.WrappedChildNodes);

      IINode iinode = node.WrappedNode as IINode;
      if (iinode != null)
         this.iinode.AttachChild(iinode, true);
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

   public const String ImgKeyBone      = "bone";
   public const String ImgKeyCamera    = "camera";
   public const String ImgKeyGeometry  = "geometry";
   public const String ImgKeyGroupHead = "group";
   public const String ImgKeyHelper    = "helper";
   public const String ImgKeyLight     = "light";
   public const String ImgKeyMaterial  = "material";
   public const String ImgKeyNurbs     = "nurbs";
   public const String ImgKeyParticle  = "particle";
   public const String ImgKeyShape     = "shape";
   public const String ImgKeySpaceWarp = "spacewarp";
   public const String ImgKeyTarget    = "helper";
   public const String ImgKeyXref      = "xref";
   

   public override string ImageKey
   {
      get
      {
         if (this.iinode == null || this.iinode.ObjectRef == null)
            return base.ImageKey;

         SClass_ID superClass = this.SuperClassID;
         switch (superClass)
         {
            case SClass_ID.Camera: return ImgKeyCamera;
            case SClass_ID.Light: return ImgKeyLight;
            case SClass_ID.Material: return ImgKeyMaterial;
            case SClass_ID.Shape: return ImgKeyShape;
            case SClass_ID.WsmObject: return ImgKeySpaceWarp;
         }

         if (superClass == SClass_ID.System && IINodeHelpers.IsXref(iinode))
            return ImgKeyXref;

         if (superClass == SClass_ID.Helper)
         {
            if (this.iinode.IsGroupHead)
               return ImgKeyGroupHead;
            else
               return ImgKeyHelper;
         }

         if (superClass == SClass_ID.Geomobject)
         {
            //Target objects (for light/camera target)
            if (iinode.IsTarget)
               return ImgKeyTarget;

            IObject objRef = iinode.ObjectRef;
            if (objRef == null)
               return ImgKeyGeometry;

            //Nurbs / Shape objects.
            if (objRef.IsShapeObject)
               return ImgKeyNurbs;

            //Particle objects.
            if (objRef.IsParticleSystem)
               return ImgKeyParticle;

            //Bone and biped objects have Geomobject as a superclass.
            if (IINodeHelpers.IsBone(iinode))
               return ImgKeyBone;

            //All other geometry objects.
            return ImgKeyGeometry;
         }

         return base.ImageKey;
      }
   }
}
}
