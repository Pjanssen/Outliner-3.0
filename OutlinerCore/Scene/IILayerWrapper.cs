using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.Controls;
using MaxUtils;
using Outliner.LayerTools;

namespace Outliner.Scene
{
   public class IILayerWrapper : IMaxNodeWrapper
   {
      private IILayer layer;
      private IILayerProperties layerProperties;

      public IILayerWrapper(IILayer layer)
      {
         ExceptionHelpers.ThrowIfArgumentIsNull(layer, "layer");

         this.layer = layer;
         this.layerProperties = MaxInterfaces.IIFPLayerManager.GetLayer(layer.Name);
      }

      public IILayerWrapper(IILayerProperties layerProperties)
      {
         ExceptionHelpers.ThrowIfArgumentIsNull(layerProperties, "layerProperties");

         this.layerProperties = layerProperties;

         String layerName = layerProperties.Name;
         this.layer = MaxInterfaces.IILayerManager.GetLayer(ref layerName);
      }

      public IILayerWrapper(IILayer layer, IILayerProperties layerProperties)
      {
         this.layer = layer;
         this.layerProperties = layerProperties;
      }


      public override object WrappedNode
      {
         get { return this.layer; }
      }

      public override bool Equals(object obj)
      {
         IILayerWrapper otherObj = obj as IILayerWrapper;
         return otherObj != null && this.layer.Handle == otherObj.layer.Handle;
      }

      public override int GetHashCode()
      {
         return this.WrappedNode.GetHashCode();
      }

      public IILayer IILayer
      {
         get { return this.layer; }
      }

      public IILayerProperties IILayerProperties
      {
         get { return this.layerProperties; }
      }

      #region Nested Layers & Adding IINodes
      
      public override IMaxNodeWrapper Parent
      {
         get
         {
            IILayer parent = NestedLayers.GetParent(this.IILayer);
            if (parent != null)
               return new IILayerWrapper(parent);
            else
               return null;
         }
      }

      public override IEnumerable<Object> ChildNodes
      {
         get
         {
            IEnumerable<IILayer> childLayers = NestedLayers.GetChildren(this.IILayer, false);

            ITab<IINode> nodes = MaxInterfaces.Global.INodeTabNS.Create();
            this.layerProperties.Nodes(nodes);

            List<Object> childNodes = new List<object>();
            childNodes.AddRange(childLayers);
            childNodes.AddRange(nodes.ToIEnumerable());

            return childNodes;
         }
      }

      public override bool CanAddChildNode(IMaxNodeWrapper node)
      {
         if (node is IINodeWrapper)
         {
            IINode n = (IINode)node.WrappedNode;
            IILayer l = (IILayer)n.GetReference((int)ReferenceNumbers.NodeLayerRef);
            return this.layer.Handle != l.Handle;
         }
         else if (node is IILayerWrapper)
         {
            //TODO check if node is parent of this, etc.
            return node != this;
         }
         else if (node is SelectionSetWrapper)
            return this.CanAddChildNodes(node.WrappedChildNodes);
         else
            return false;
      }

      public override void AddChildNode(IMaxNodeWrapper node)
      {
         if (!this.CanAddChildNode(node))
            return;

         if (node is IINodeWrapper)
            this.IILayer.AddToLayer(((IINodeWrapper)node).IINode);
         else if (node is IILayerWrapper)
            NestedLayers.SetParent(((IILayerWrapper)node).IILayer, this.IILayer);
         else if (node is SelectionSetWrapper)
            this.AddChildNodes(node.WrappedChildNodes);
      }

      public override void RemoveChildNode(IMaxNodeWrapper node)
      {
         if (node is IINodeWrapper)
         {
            IILayer defaultLayer = MaxInterfaces.IILayerManager.GetLayer(0);
            defaultLayer.AddToLayer(((IINodeWrapper)node).IINode);
         }
         else if (node is IILayerWrapper)
         {
            NestedLayers.SetParent(((IILayerWrapper)node).IILayer, null);
         }
      }
      
      #endregion

      /// <summary>
      /// Gets whether this layer is the default (0) layer.
      /// </summary>
      public Boolean IsDefault 
      {
         get
         {
            return MaxInterfaces.IILayerManager.RootLayer.Handle == this.layer.Handle;
         }
      }

      /// <summary>
      /// Gets or sets if this layer is the currently active layer.
      /// </summary>
      public Boolean IsCurrent 
      {
         get { return this.layerProperties.Current; }
         set
         {
            if (!value)
               throw new ArgumentException("Cannot set IsCurrent to false. Instead, use IsCurrent = true on the new current layer.");

            this.layerProperties.Current = true;
         }
      }

      public override string Name 
      {
         get { return layer.Name; }
         set { layer.SetName(ref value); }
      }

      public override string DisplayName 
      {
         get
         {
            if (this.IsDefault)
               return this.Name + " (default)";
            else
               return this.Name;
         }
      }

      public override bool CanEditName 
      {
         get { return !this.IsDefault; }
      }

      public override IClass_ID ClassID 
      {
         get { return layer.ClassID; }
      }

      public override SClass_ID SuperClassID 
      {
         get { return layer.SuperClassID; }
      }


      public override bool Selected 
      {
         get { return false; }
      }

      public override bool IsNodeType(MaxNodeTypes types) 
      {
         return types.HasFlag(MaxNodeTypes.Layer);
      }


      public override bool IsHidden 
      {
         get { return this.layer.IsHidden; }
         set 
         { 
            NestedLayers.SetProperty(this.layer, AnimatableBooleanProperty.IsHidden, value);
         }
      }

      public override bool IsFrozen 
      {
         get { return this.layer.IsFrozen; }
         set 
         {
            NestedLayers.SetProperty(this.layer, AnimatableBooleanProperty.IsFrozen, value);
         }
      }

      public override bool BoxMode 
      {
         get { return this.layer.BoxMode; }
         set 
         {
            NestedLayers.SetProperty(this.layer, AnimatableBooleanProperty.BoxMode, value);
         }
      }

      public override System.Drawing.Color WireColor 
      {
         get { return ColorHelpers.FromMaxColor(this.layer.WireColor); }
         set 
         { 
            this.layer.WireColor = value;
            MaxInterfaces.Global.BroadcastNotification(NestedLayers.LayerPropertyChanged, this.IILayer);
         }
      }

      public override bool Renderable 
      {
         get { return this.layer.Renderable; }
         set
         {
            NestedLayers.SetProperty(this.layer, AnimatableBooleanProperty.Renderable, value);
         }
      }

      public override bool IsValid 
      {
         get
         {
            if (!base.IsValid)
               return false;

            try { return !this.layer.TestAFlag(AnimatableFlags.IsDeleted); }
            catch { return false; }
         }
      }


      public const String ImgKeyLayer       = "layer";
      public const String ImgKeyLayerActive = "layer_active";
      public override string ImageKey
      {
         get
         {
            if (this.IsCurrent)
               return ImgKeyLayerActive;
            else
               return ImgKeyLayer;
         }
      }
   }
}
