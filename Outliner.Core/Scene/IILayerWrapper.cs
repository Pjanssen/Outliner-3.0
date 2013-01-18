using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.Controls;
using Outliner.MaxUtils;
using Outliner.LayerTools;

namespace Outliner.Scene
{
   public class IILayerWrapper : MaxNodeWrapper
   {
      private IILayer layer;
      private IILayerProperties layerProperties;

      public IILayerWrapper(IILayer layer)
      {
         Throw.IfArgumentIsNull(layer, "layer");

         this.layer = layer;
         this.layerProperties = MaxInterfaces.IIFPLayerManager.GetLayer(layer.Name);
      }

      public IILayerWrapper(IILayerProperties layerProperties)
      {
         Throw.IfArgumentIsNull(layerProperties, "layerProperties");

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

      #region Childnodes
      
      public override MaxNodeWrapper Parent
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

      public override int ChildNodeCount
      {
         get { return this.ChildNodes.Count(); }
      }

      private IEnumerable<IILayer> GetChildLayers()
      {
         return NestedLayers.GetChildren(this.IILayer, false);
      }

      private IEnumerable<IINode> GetChildNodes()
      {
         ITab<IINode> nodes = MaxInterfaces.Global.INodeTabNS.Create();
         this.layerProperties.Nodes(nodes);
         return nodes.ToIEnumerable();
      }

      public override IEnumerable<Object> ChildNodes
      {
         get
         {
            List<Object> childNodes = new List<object>();
            childNodes.AddRange(this.GetChildLayers());
            childNodes.AddRange(this.GetChildNodes());

            return childNodes;
         }
      }

      public override bool CanAddChildNode(MaxNodeWrapper node)
      {
         if (node == null)
            return false;

         if (node is IINodeWrapper)
         {
            IINode n = (IINode)node.WrappedNode;
            IILayer l = (IILayer)n.GetReference((int)ReferenceNumbers.NodeLayerRef);
            return this.layer.Handle != l.Handle;
         }
         else if (node is IILayerWrapper)
         {
            return !this.IsInParentChain(node) && (node.Parent == null || !node.Parent.Equals(this));
            //return !node.Equals(this) && (node.Parent == null || !node.Parent.Equals(this));
         }
         else if (node is SelectionSetWrapper)
            return this.CanAddChildNodes(node.WrappedChildNodes);
         else
            return false;
      }

      private Boolean IsInParentChain(MaxNodeWrapper node)
      {
         return this.IsInParentChain(node, this);
      }

      private Boolean IsInParentChain(MaxNodeWrapper node, MaxNodeWrapper currentParent)
      {
         if (currentParent == null)
            return false;
         if (node.Equals(currentParent))
            return true;

         return IsInParentChain(node, currentParent.Parent);
      }

      public override void AddChildNode(MaxNodeWrapper node)
      {
         Throw.IfArgumentIsNull(node, "node");

         if (!this.CanAddChildNode(node))
            return;

         if (node is IINodeWrapper)
            this.IILayer.AddToLayer(((IINodeWrapper)node).IINode);
         else if (node is IILayerWrapper)
            NestedLayers.SetParent(((IILayerWrapper)node).IILayer, this.IILayer);
         else if (node is SelectionSetWrapper)
            this.AddChildNodes(node.WrappedChildNodes);
      }

      public override void RemoveChildNode(MaxNodeWrapper node)
      {
         Throw.IfArgumentIsNull(node, "node");

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

      #region Name
      
      public override string Name 
      {
         get { return layer.Name; }
         set 
         {
            Throw.IfArgumentIsNull(value, "value");
            layer.SetName(ref value); 
         }
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

      #endregion

      #region Type

      public override IClass_ID ClassID 
      {
         get { return layer.ClassID; }
      }

      public override SClass_ID SuperClassID 
      {
         get { return layer.SuperClassID; }
      }

      public override bool IsNodeType(MaxNodeTypes types) 
      {
         return types.HasFlag(MaxNodeTypes.Layer);
      }

      #endregion

      public override bool Selected 
      {
         get { return false; }
         set 
         {
            this.WrappedChildNodes.ForEach(n => n.Selected = value);
         }
      }

      #region Delete
      
      public override bool CanDelete
      {
         get { return !this.IsDefault; }
      }

      public override void Delete()
      {
         if (CanDelete)
         {
            IILayer defaultLayer = MaxInterfaces.IILayerManager.GetLayer(0);
            foreach (IINode node in this.GetChildNodes())
            {
               defaultLayer.AddToLayer(node);
            }
            foreach (IILayer layer in this.GetChildLayers())
            {
               NestedLayers.SetParent(layer, null);
            }

            String name = this.Name;
            MaxInterfaces.IILayerManager.DeleteLayer(ref name);
         }
      }

      #endregion

      #region NodeProperties
      
      public override System.Drawing.Color WireColor 
      {
         get { return ColorHelpers.FromMaxColor(this.layer.WireColor); }
         set 
         {
            Throw.IfArgumentIsNull(value, "value");

            this.layer.WireColor = value;
            MaxInterfaces.Global.BroadcastNotification(NestedLayers.LayerPropertyChanged, this.IILayer);
         }
      }


      public override Boolean GetNodeProperty(BooleanNodeProperty property)
      {
         return NestedLayers.GetProperty(this.layer, property);
      }

      public override void SetNodeProperty(BooleanNodeProperty property, bool value)
      {
         NestedLayers.SetProperty(this.layer, property, value);
      }

      public override bool IsNodePropertyInherited(NodeProperty property)
      {
         if (!NodePropertyHelpers.IsBooleanProperty(property))
            return false;
         
         return NestedLayers.IsPropertyInherited(layer, NodePropertyHelpers.ToBooleanProperty(property));
      }

      #endregion

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

      public override string ToString()
      {
         return String.Format("IILayerWrapper ({0})", this.Name);
      }
   }
}
