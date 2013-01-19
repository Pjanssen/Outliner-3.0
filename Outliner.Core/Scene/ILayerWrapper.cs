using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.LayerTools;
using Outliner.MaxUtils;

namespace Outliner.Scene
{
   public class ILayerWrapper : MaxNodeWrapper
   {
      private IILayer ilayer;
      private IILayerProperties ilayerProperties;

      public ILayerWrapper(IILayer ilayer)
      {
         Throw.IfArgumentIsNull(ilayer, "ilayer");

         this.ilayer = ilayer;
         this.ilayerProperties = MaxInterfaces.IIFPLayerManager.GetLayer(ilayer.Name);
      }

      public ILayerWrapper(IILayerProperties ilayerProperties)
      {
         Throw.IfArgumentIsNull(ilayerProperties, "IILayerProperties");

         this.ilayerProperties = ilayerProperties;
         String layerName = ilayerProperties.Name;
         this.ilayer = MaxInterfaces.IILayerManager.GetLayer(ref layerName);
      }

      public ILayerWrapper(IILayer ilayer, IILayerProperties ilayerProperties)
      {
         this.ilayer = ilayer;
         this.ilayerProperties = ilayerProperties;
      }


      public override object BaseObject
      {
         get { return this.ilayer; }
      }

      public override bool IsValid
      {
         get
         {
            if (!base.IsValid)
               return false;

            try
            {
               return !this.ILayer.TestAFlag(AnimatableFlags.IsDeleted);
            }
            catch
            {
               return false;
            }
         }
      }

      public override IMaxNode Parent
      {
         get
         {
            IILayer parent = NestedLayers.GetParent(this.ILayer);
            if (parent != null)
               return new ILayerWrapper(parent);
            else
               return null;
         }
         set
         {
            throw new NotImplementedException();
         }
      }


      #region ILayerWrapper Specific

      public IILayer ILayer
      {
         get { return this.ilayer; }
      }

      public IILayerProperties ILayerProperties
      {
         get { return this.ilayerProperties; }
      }

      /// <summary>
      /// Gets whether this layer is the default (0) layer.
      /// </summary>
      public Boolean IsDefault
      {
         get
         {
            return MaxInterfaces.IILayerManager.RootLayer.Handle == this.ilayer.Handle;
         }
      }

      /// <summary>
      /// Gets or sets if this layer is the currently active layer.
      /// </summary>
      public Boolean IsCurrent
      {
         get { return this.ilayerProperties.Current; }
         set
         {
            if (!value)
               throw new ArgumentException("Cannot set IsCurrent to false. Instead, use IsCurrent = true on the new current layer.");

            this.ilayerProperties.Current = true;
         }
      }

      #endregion


      #region Equality

      public override bool Equals(object obj)
      {
         ILayerWrapper otherObj = obj as ILayerWrapper;
         return otherObj != null && this.ILayer.Handle == otherObj.ILayer.Handle;
      }

      public override int GetHashCode()
      {
         return this.BaseObject.GetHashCode();
      }

      #endregion


      #region Delete

      public override bool CanDelete
      {
         get { return !this.IsDefault; }
      }

      public override void Delete()
      {
         if (!this.CanDelete)
            return;
         
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

      #endregion


      #region Childnodes

      private IEnumerable<IILayer> GetChildLayers()
      {
         return NestedLayers.GetChildren(this.ILayer, false);
      }

      private IEnumerable<IINode> GetChildNodes()
      {
         ITab<IINode> nodes = MaxInterfaces.Global.INodeTabNS.Create();
         this.ilayerProperties.Nodes(nodes);
         return nodes.ToIEnumerable();
      }

      public override IEnumerable<object> ChildBaseObjects
      {
         get
         {
            List<Object> childNodes = new List<object>();
            childNodes.AddRange(this.GetChildLayers());
            childNodes.AddRange(this.GetChildNodes());

            return childNodes;
         }
      }

      public override bool CanAddChildNode(IMaxNode node)
      {
         if (node == null)
            return false;

         if (node is INodeWrapper)
         {
            IINode n = (IINode)node.BaseObject;
            IILayer l = (IILayer)n.GetReference((int)ReferenceNumbers.NodeLayerRef);
            return this.ILayer.Handle != l.Handle;
         }
         else if (node is ILayerWrapper)
         {
            return !this.IsInParentChain(node) && (node.Parent == null || !node.Parent.Equals(this));
         }
         else if (node is SelectionSetWrapper)
            return this.CanAddChildNodes(node.ChildNodes);
         else
            return false;
      }

      private Boolean IsInParentChain(IMaxNode node)
      {
         return this.IsInParentChain(node, this);
      }

      private Boolean IsInParentChain(IMaxNode node, IMaxNode currentParent)
      {
         if (currentParent == null)
            return false;
         if (node.Equals(currentParent))
            return true;

         return IsInParentChain(node, currentParent.Parent);
      }

      public override void AddChildNode(IMaxNode node)
      {
         Throw.IfArgumentIsNull(node, "node");

         if (!this.CanAddChildNode(node))
            return;

         if (node is INodeWrapper)
            this.ILayer.AddToLayer(((INodeWrapper)node).INode);
         else if (node is ILayerWrapper)
            NestedLayers.SetParent(((ILayerWrapper)node).ILayer, this.ILayer);
         else if (node is SelectionSetWrapper)
            this.AddChildNodes(node.ChildNodes);
      }

      public override void RemoveChildNode(IMaxNode node)
      {
         Throw.IfArgumentIsNull(node, "node");

         if (node is INodeWrapper)
         {
            IILayer defaultLayer = MaxInterfaces.IILayerManager.GetLayer(0);
            defaultLayer.AddToLayer(((INodeWrapper)node).INode);
         }
         else if (node is ILayerWrapper)
         {
            NestedLayers.SetParent(((ILayerWrapper)node).ILayer, null);
         }
      }

      #endregion


      #region Node Type

      protected override MaxNodeType MaxNodeType
      {
         get { return MaxNodeType.Layer; }
      }

      public override SClass_ID SuperClassID
      {
         get { return this.ilayer.SuperClassID; }
      }

      public override IClass_ID ClassID
      {
         get { return this.ilayer.ClassID; }
      }

      #endregion


      #region Name

      public override string Name
      {
         get { return this.ilayer.Name; }
         set
         {
            Throw.IfArgumentIsNull(value, "value");
            if (!this.CanEditName)
               throw new InvalidOperationException("The name of this node cannot be edited.");
            
            this.ilayer.SetName(ref value);
         }
      }

      public override bool CanEditName
      {
         get { return !this.IsDefault; }
      }

      public override string DisplayName
      {
         get
         {
            if (this.IsDefault)
               return this.Name + OutlinerResources.Str_LayerDefault;
            else
               return this.Name;
         }
      }

      #endregion


      #region Node Properties

      public override System.Drawing.Color WireColor
      {
         get { return ColorHelpers.FromMaxColor(this.ILayer.WireColor); }
         set
         {
            Throw.IfArgumentIsNull(value, "value");

            this.ILayer.WireColor = value;
            MaxInterfaces.Global.BroadcastNotification(NestedLayers.LayerPropertyChanged, this.ILayer);
         }
      }

      public override Boolean GetNodeProperty(BooleanNodeProperty property)
      {
         return NestedLayers.GetProperty(this.ILayer, property);
      }

      public override void SetNodeProperty(BooleanNodeProperty property, bool value)
      {
         NestedLayers.SetProperty(this.ILayer, property, value);
      }

      public override bool IsNodePropertyInherited(NodeProperty property)
      {
         if (!NodePropertyHelpers.IsBooleanProperty(property))
            return false;

         return NestedLayers.IsPropertyInherited(this.ILayer, NodePropertyHelpers.ToBooleanProperty(property));
      }

      #endregion


      #region ImageKey

      public const String ImgKeyLayer = "layer";
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

      #endregion
      

      public override string ToString()
      {
         return String.Format("ILayerWrapper ({0})", this.Name);
      }
   }
}
