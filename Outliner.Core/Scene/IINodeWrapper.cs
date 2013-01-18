using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using System.Drawing;
using Outliner.Controls;
using Outliner.MaxUtils;

namespace Outliner.Scene
{
public class IINodeWrapper : MaxNodeWrapper
{
   private IINode iinode;
   public IINodeWrapper(IINode node)
   {
      Throw.IfArgumentIsNull(node, "node");
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

   public bool IsInstance
   {
      get
      {
         IINodeTab instances = MaxInterfaces.Global.INodeTabNS.Create();
         uint numInstances = MaxInterfaces.InstanceMgr.GetInstances(this.iinode, instances);
         return numInstances > 1;
      }
   }

   public override MaxNodeWrapper Parent
   {
      get
      {
         return new IINodeWrapper(this.iinode.ParentNode);
      }
   }

   #region ChildNodes
   
   public override int ChildNodeCount
   {
      get { return this.iinode.NumberOfChildren; }
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

   public override bool CanAddChildNode(MaxNodeWrapper node)
   {
      if (node == null)
         return false;

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

   public override bool CanAddChildNodes(IEnumerable<MaxNodeWrapper> nodes)
   {
      return nodes.All(this.CanAddChildNode);
   }

   public override void AddChildNode(MaxNodeWrapper node)
   {
      Throw.IfArgumentIsNull(node, "node");

      if (!this.CanAddChildNode(node))
         return;

      if (node is SelectionSetWrapper)
         this.AddChildNodes(node.WrappedChildNodes);

      IINode iinode = node.WrappedNode as IINode;
      if (iinode != null)
         this.iinode.AttachChild(iinode, true);
   }

   public override void RemoveChildNode(MaxNodeWrapper node)
   {
      Throw.IfArgumentIsNull(node, "node");

      if (node is IINodeWrapper)
         ((IINodeWrapper)node).iinode.Detach(0, true);
   }

   #endregion

   #region Name
   
   public override String Name
   {
      get { return this.iinode.Name; }
      set
      {
         Throw.IfArgumentIsNull(value, "value");
         this.iinode.Name = value;
      }
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

   #endregion

   #region Type
   
   public override IClass_ID ClassID
   {
      get { return this.iinode.ObjectRef.ClassID; }
   }

   public override SClass_ID SuperClassID
   {
      get { return this.iinode.ObjectRef.FindBaseObject().SuperClassID; }
   }

   public override bool IsNodeType(MaxNodeTypes types)
   {
      return (types & MaxNodeTypes.Object) == MaxNodeTypes.Object;
   }

   #endregion

   public override bool Selected
   {
      get { return this.iinode.Selected; }
      set
      {
         if (value)
            MaxInterfaces.COREInterface.SelectNode(this.IINode, false);
         else
            MaxInterfaces.COREInterface.DeSelectNode(this.IINode);
      }
   }

   public override void Delete()
   {
      if (this.CanDelete)
      {
         MaxInterfaces.COREInterface.DeleteNode(this.IINode, false, false);
      }
   }

   #region NodeProperties

   public override Color WireColor
   {
      get { return ColorHelpers.FromMaxColor(this.iinode.WireColor); }
      set
      {
         Throw.IfArgumentIsNull(value, "value");
         this.iinode.WireColor = value;
      }
   }


   private Boolean IntToBool(int i)
   {
      return i != 0;
   }

   private int BoolToInt(bool b)
   {
      return b ? 1 : 0;
   }


   public override bool GetNodeProperty(BooleanNodeProperty property)
   {
      switch (property)
      {
         case BooleanNodeProperty.IsHidden: return this.iinode.IsObjectHidden;
         case BooleanNodeProperty.IsFrozen: return this.iinode.IsObjectFrozen;
         case BooleanNodeProperty.SeeThrough: return IntToBool(this.iinode.XRayMtl_);
         case BooleanNodeProperty.BoxMode: return IntToBool(this.iinode.BoxMode_);
         case BooleanNodeProperty.BackfaceCull: return IntToBool(this.iinode.BackCull_);
         case BooleanNodeProperty.AllEdges: return IntToBool(this.iinode.AllEdges_);
         case BooleanNodeProperty.VertexTicks: return IntToBool(this.iinode.VertTicks);
         case BooleanNodeProperty.Trajectory: return IntToBool(this.iinode.TrajectoryON);
         case BooleanNodeProperty.IgnoreExtents: return IntToBool(this.iinode.IgnoreExtents_);
         case BooleanNodeProperty.FrozenInGray: return IntToBool(this.iinode.ShowFrozenWithMtl);
         case BooleanNodeProperty.Renderable: return IntToBool(this.iinode.Renderable);
         case BooleanNodeProperty.InheritVisibility: return this.iinode.InheritVisibility;
         case BooleanNodeProperty.PrimaryVisibility: return IntToBool(this.iinode.PrimaryVisibility);
         case BooleanNodeProperty.SecondaryVisibility: return IntToBool(this.iinode.SecondaryVisibility);
         case BooleanNodeProperty.ReceiveShadows: return IntToBool(this.iinode.RcvShadows);
         case BooleanNodeProperty.CastShadows: return IntToBool(this.iinode.CastShadows);
         case BooleanNodeProperty.ApplyAtmospherics: return IntToBool(this.iinode.ApplyAtmospherics);
         case BooleanNodeProperty.RenderOccluded: return this.iinode.RenderOccluded;
         default: return base.GetNodeProperty(property);
      }
   }

   public override void SetNodeProperty(BooleanNodeProperty property, bool value)
   {
      switch (property)
      {
         case BooleanNodeProperty.IsHidden:
            this.iinode.Hide(value);
            break;
         case BooleanNodeProperty.IsFrozen:
            this.iinode.IsFrozen = value;
            break;
         case BooleanNodeProperty.SeeThrough:
            this.iinode.XRayMtl(value);
            break;
         case BooleanNodeProperty.BoxMode:
            this.iinode.BoxMode(value);
            break;
         case BooleanNodeProperty.BackfaceCull:
            this.iinode.BackCull(value);
            break;
         case BooleanNodeProperty.AllEdges:
            this.iinode.AllEdges(value);
            break;
         case BooleanNodeProperty.VertexTicks:
            this.iinode.VertTicks = BoolToInt(value);
            break;
         case BooleanNodeProperty.Trajectory:
            this.iinode.SetTrajectoryON(value);
            break;
         case BooleanNodeProperty.IgnoreExtents:
            this.iinode.IgnoreExtents(value);
            break;
         case BooleanNodeProperty.FrozenInGray:
            this.iinode.SetShowFrozenWithMtl(value);
            break;
         case BooleanNodeProperty.Renderable:
            this.iinode.SetRenderable(value);
            break;
         case BooleanNodeProperty.InheritVisibility:
            this.iinode.InheritVisibility = value;
            break;
         case BooleanNodeProperty.PrimaryVisibility:
            this.iinode.SetPrimaryVisibility(value);
            break;
         case BooleanNodeProperty.SecondaryVisibility:
            this.iinode.SetSecondaryVisibility(value);
            break;
         case BooleanNodeProperty.ReceiveShadows:
            this.iinode.SetRcvShadows(value);
            break;
         case BooleanNodeProperty.CastShadows:
            this.iinode.SetCastShadows(value);
            break;
         case BooleanNodeProperty.ApplyAtmospherics:
            this.iinode.SetApplyAtmospherics(value);
            break;
         case BooleanNodeProperty.RenderOccluded:
            this.iinode.RenderOccluded = value;
            break;
         default:
            base.SetNodeProperty(property, value);
            break;
      }
   }

   public override bool IsNodePropertyInherited(NodeProperty property)
   {
      IILayer layer = this.IILayer;
      IINodeLayerProperties layerProperties = this.NodeLayerProperties;

      if (property == NodeProperty.IsHidden)
         return layer != null && layer.IsHidden;
      else if (property == NodeProperty.IsFrozen)
         return layer != null && layer.IsFrozen;
      else if (property == NodeProperty.WireColor)
         return this.NodeLayerProperties.ColorByLayer;
      else if (NodePropertyHelpers.IsDisplayProperty(property))
         return layerProperties.DisplayByLayer;
      else if (NodePropertyHelpers.IsRenderProperty(property))
         return layerProperties.RenderByLayer;
      else
         return false;
   }

   #endregion
   


   public const String ImgKeyBone      = "bone";
   public const String ImgKeyCamera    = "camera";
   public const String ImgKeyContainer = "container";
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
            else if (MaxInterfaces.ContainerManager.IsContainerNode(this.iinode) != null)
               return ImgKeyContainer;
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

   public override string ToString()
   {
      return String.Format("IINodeWrapper ({0})", this.Name);
   }
}
}
