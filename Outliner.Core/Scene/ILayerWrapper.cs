﻿using System;
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
      }

      public ILayerWrapper(IILayerProperties ilayerProperties)
      {
         Throw.IfArgumentIsNull(ilayerProperties, "IILayerProperties");

         this.ilayerProperties = ilayerProperties;
      }

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

      public override int ChildNodeCount
      {
         get
         {
            if (!this.ILayer.Used)
               return 0;
            else
               return base.ChildNodeCount;
         }
      }
      private IEnumerable<IILayer> GetChildLayers()
      {
         return NestedLayers.GetChildren(this.ILayer, false);
      }

      private IEnumerable<IINode> GetChildNodes()
      {
         ITab<IINode> nodes = MaxInterfaces.Global.INodeTabNS.Create();
         IILayerProperties layerProperties = this.ILayerProperties;
         if (layerProperties != null)
            layerProperties.Nodes(nodes);
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

         SelectionSetWrapper selSetWrapper = node as SelectionSetWrapper;
         if (selSetWrapper != null)
         {
            return this.CanAddChildNodes(node.ChildNodes);
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
         Throw.IfArgumentIsNull(node, "node");

         if (!this.CanAddChildNode(node))
            return;

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

         SelectionSetWrapper selSetWrapper = node as SelectionSetWrapper;
         if (selSetWrapper != null)
         {
            this.AddChildNodes(node.ChildNodes);
            return;
         }
      }

      public override void RemoveChildNode(IMaxNode node)
      {
         Throw.IfArgumentIsNull(node, "node");

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
            Throw.IfArgumentIsNull(value, "value");
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