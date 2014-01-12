using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using PJanssen.Outliner.LayerTools;
using PJanssen.Outliner.MaxUtils;
using PJanssen.Outliner.Modes.Layer;

namespace PJanssen.Outliner.Scene
{
   /// <summary>
   /// Defines a MaxNodeWrapper for an ILayer object.
   /// </summary>
   public class ILayerWrapper : MaxNodeWrapper
   {
      private IILayer ilayer;
      private IILayerProperties ilayerProperties;

      /// <summary>
      /// Initializes a new instance of the ILayerWrapper class.
      /// </summary>
      /// <param name="ilayer">The ILayer object to wrap.</param>
      public ILayerWrapper(IILayer ilayer)
      {
         Throw.IfNull(ilayer, "ilayer");

         this.ilayer = ilayer;
      }

      /// <summary>
      /// Initializes a new instance of the ILayerWrapper class.
      /// </summary>
      /// <param name="ilayerProperties">The ILayerProperties of the ILayer to wrap.</param>
      public ILayerWrapper(IILayerProperties ilayerProperties)
      {
         Throw.IfNull(ilayerProperties, "IILayerProperties");

         this.ilayerProperties = ilayerProperties;
      }

      /// <summary>
      /// Initializes a new instance of the ILayerWrapper class.
      /// </summary>
      /// <param name="ilayer">The ILayer object to wrap.</param>
      /// <param name="ilayerProperties">The ILayerProperties of the ILayer to wrap.</param>
      public ILayerWrapper(IILayer ilayer, IILayerProperties ilayerProperties)
      {
         this.ilayer = ilayer;
         this.ilayerProperties = ilayerProperties;
      }


      public override object BaseObject
      {
         get { return this.ILayer; }
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

      public override bool IsSelected
      {
         get
         {
            return false;
         }
         set
         {
            this.ChildNodes.ForEach(n => n.IsSelected = value);
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
            IILayer newParent = null;
            ILayerWrapper parentWrapper = value as ILayerWrapper;
            if (parentWrapper != null)
               newParent = parentWrapper.ILayer;
            NestedLayers.SetParent(this.ILayer, newParent);
         }
      }


      #region ILayerWrapper Specific

      /// <summary>
      /// Gets the wrapped ILayer object.
      /// </summary>
      public IILayer ILayer
      {
         get 
         {
            if (this.ilayer == null)
               this.ilayer = this.GetILayer();

            return this.ilayer; 
         }
      }

      private IILayer GetILayer()
      {
         if (this.ilayerProperties != null)
         {
            String layerName = ilayerProperties.Name;
            return MaxInterfaces.IILayerManager.GetLayer(ref layerName);
         }
         else
            return null;
      }

      /// <summary>
      /// Gets the ILayerProperties object of the wrapped ILayer object.
      /// </summary>
      public IILayerProperties ILayerProperties
      {
         get 
         {
            if (this.ilayerProperties == null)
               this.ilayerProperties = this.GetLayerProperties();

            return this.ilayerProperties; 
         }
      }

      private IILayerProperties GetLayerProperties()
      {
         if (this.ilayer != null)
            return MaxInterfaces.IIFPLayerManager.GetLayer(this.ilayer.Name);
         else
            return null;
      }

      /// <summary>
      /// Gets whether this layer is the default (0) layer.
      /// </summary>
      public Boolean IsDefault
      {
         get
         {
            return MaxInterfaces.IILayerManager.RootLayer.Handle == this.ILayer.Handle;
         }
      }

      /// <summary>
      /// Gets or sets if this layer is the currently active layer.
      /// </summary>
      public Boolean IsCurrent
      {
         get { return MaxInterfaces.IILayerManager.CurrentLayer.Handle == this.ILayer.Handle; }
         set
         {
            if (!value)
               throw new ArgumentException("Cannot set IsCurrent to false. Instead, use IsCurrent = true on the new current layer.");

            LayerTools.LayerTools.SetCurrentLayer(this.ILayer);
         }
      }

      /// <summary>
      /// Gets the child layers of this layer.
      /// </summary>
      public IEnumerable<IILayer> ChildILayers
      {
         get
         {
            return NestedLayers.GetChildren(this.ILayer, false);
         }
      }

      /// <summary>
      /// Gets the nodes that belong to this layer.
      /// </summary>
      public IEnumerable<IINode> ChildINodes
      {
         get
         {
            ITab<IINode> nodes = MaxInterfaces.Global.INodeTabNS.Create();
            IILayerProperties layerProperties = this.ILayerProperties;
            if (layerProperties != null)
               layerProperties.Nodes(nodes);
            return IINodes.ITabToIEnumerable(nodes);
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
         foreach (IINode node in this.ChildINodes)
         {
            defaultLayer.AddToLayer(node);
         }
         foreach (IILayer layer in this.ChildILayers)
         {
            NestedLayers.SetParent(layer, null);
         }

         String name = this.Name;
         MaxInterfaces.IILayerManager.DeleteLayer(ref name);
      }

      #endregion


      #region Childnodes

      public override int ChildNodeCount
      {
         get
         {
            if (!this.ILayer.Used && this.ChildILayers.Count() == 0)
               return 0;
            else
               return base.ChildNodeCount;
         }
      }


      public override IEnumerable<object> ChildBaseObjects
      {
         get
         {
            List<Object> childNodes = new List<object>();
            childNodes.AddRange(this.ChildILayers);
            childNodes.AddRange(this.ChildINodes);

            return childNodes;
         }
      }

      public override bool CanAddChildNode(IMaxNode node)
      {
         if (node == null)
            return false;

         //Aggregates (e.g. SelectionSet)
         if (node.IsAggregate)
         {
            return this.CanAddChildNodes(node.ChildNodes);
         }

         INodeWrapper inodeWrapper = node as INodeWrapper;
         if (inodeWrapper != null)
         {
            IILayer l = (IILayer)inodeWrapper.INode.GetReference((int)ReferenceNumbers.NodeLayerRef);
            return this.ILayer.Handle != l.Handle;
         }

         ILayerWrapper ilayerWrapper = node as ILayerWrapper;
         if (ilayerWrapper != null)
         {
            return !this.IsInParentChain(node) && (node.Parent == null || !node.Parent.Equals(this));
         }
         
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
         Throw.IfNull(node, "node");

         if (!this.CanAddChildNode(node))
            return;

         //Aggregates (e.g. SelectionSet)
         if (node.IsAggregate)
         {
            this.AddChildNodes(node.ChildNodes);
            return;
         }

         INodeWrapper inodeWrapper = node as INodeWrapper;
         if (inodeWrapper != null)
         {
            this.ILayer.AddToLayer(inodeWrapper.INode);
            return;
         }

         ILayerWrapper ilayerWrapper = node as ILayerWrapper;
         if (ilayerWrapper != null)
         {
            NestedLayers.SetParent(ilayerWrapper.ILayer, this.ILayer);
            return;
         }
      }

      public override void RemoveChildNode(IMaxNode node)
      {
         Throw.IfNull(node, "node");

         INodeWrapper inodeWrapper = node as INodeWrapper;
         if (inodeWrapper != null)
         {
            IILayer defaultLayer = MaxInterfaces.IILayerManager.GetLayer(0);
            defaultLayer.AddToLayer(inodeWrapper.INode);
            return;
         }

         ILayerWrapper ilayerWrapper = node as ILayerWrapper;
         if (ilayerWrapper != null)
         {
            NestedLayers.SetParent(ilayerWrapper.ILayer, null);
            return;
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
         get { return this.ILayer.SuperClassID; }
      }

      public override IClass_ID ClassID
      {
         get { return this.ILayer.ClassID; }
      }

      #endregion


      #region Name

      public override string Name
      {
         get { return this.ILayer.Name; }
         set
         {
            Throw.IfNull(value, "value");
            if (!this.CanEditName)
               throw new InvalidOperationException("The name of this node cannot be edited.");
            
            this.ILayer.SetName(ref value);
            MaxInterfaces.Global.BroadcastNotification(SystemNotificationCode.LayerRenamed, this.ILayer);
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
               return this.Name + Resources.Str_LayerDefault;
            else
               return this.Name;
         }
      }

      public override string NodeTypeDisplayName
      {
         get { return Resources.Str_ILayer; }
      }

      #endregion


      #region Node Properties

      public override System.Drawing.Color WireColor
      {
         get { return Colors.FromMaxColor(this.ILayer.WireColor); }
         set
         {
            Throw.IfNull(value, "value");

            this.ILayer.WireColor = value;
            LayerPropertyChangedParam parameters = new LayerPropertyChangedParam(this.ILayer, NodeProperty.WireColor);
            MaxInterfaces.Global.BroadcastNotification(LayerNotificationCode.LayerPropertyChanged, parameters);
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
         if (!NodeProperties.IsBooleanProperty(property))
            return false;

         return NestedLayers.IsPropertyInherited(this.ILayer, NodeProperties.ToBooleanProperty(property));
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
