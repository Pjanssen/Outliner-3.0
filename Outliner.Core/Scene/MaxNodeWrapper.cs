using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using System.Drawing;
using Outliner.MaxUtils;
using Outliner.LayerTools;

namespace Outliner.Scene
{
   /// <summary>
   /// IMaxNodeWrapper is an adapter class which provides a common interface
   /// for INodes, ILayers and other 3dsmax objects.
   /// </summary>
   public abstract class MaxNodeWrapper
   {
      public abstract Object WrappedNode { get; }
      public override abstract bool Equals(object obj);
      public override abstract int GetHashCode();

      public virtual MaxNodeWrapper Parent 
      { 
         get { return null; }
         set 
         {
            if (value == null)
               MaxScene.SceneRoot.AddChildNode(this);
            else
               value.AddChildNode(this);
         }
      }

      #region ChildNodes

      public abstract Int32 ChildNodeCount 
      { 
         get;
      }

      public abstract IEnumerable<Object> ChildNodes 
      { 
         get;
      }

      public virtual IEnumerable<MaxNodeWrapper> WrappedChildNodes 
      {
         get { return this.ChildNodes.Select(MaxNodeWrapper.Create); }
      }

      public virtual Boolean CanAddChildNode(MaxNodeWrapper node)
      {
         return false;
      }
      
      public virtual Boolean CanAddChildNodes(IEnumerable<MaxNodeWrapper> nodes)
      {
         return nodes.Any(this.CanAddChildNode);
      }

      public virtual void AddChildNode(MaxNodeWrapper node) { }
      
      public virtual void AddChildNodes(IEnumerable<MaxNodeWrapper> nodes)
      {
         nodes.ForEach(this.AddChildNode);
      }

      public virtual Boolean CanRemoveChildNode(MaxNodeWrapper node) 
      {
         return false; 
      }
      
      public virtual Boolean CanRemoveChildNodes(IEnumerable<MaxNodeWrapper> nodes)
      {
         return nodes.Any(this.CanRemoveChildNode);
      }
      
      public virtual void RemoveChildNode(MaxNodeWrapper node) { }
      
      public virtual void RemoveChildNodes(IEnumerable<MaxNodeWrapper> nodes)
      {
         nodes.ForEach(this.RemoveChildNode);
      }

      #endregion

      #region Name
      
      public abstract String Name { get; set; }
      public virtual String DisplayName { get { return this.Name; } }
      public virtual Boolean CanEditName { get { return true; } }

      #endregion

      #region Node Type

      public abstract SClass_ID SuperClassID { get; }
      public abstract IClass_ID ClassID { get; }
      public abstract Boolean IsNodeType(MaxNodeTypes types);

      #endregion

      public abstract Boolean Selected { get; set; }

      public virtual Boolean CanDelete { get { return true; } }
      public virtual void Delete() { }

      #region NodeProperties
      
      public virtual Color WireColor
      {
         get { return Color.Empty; }
         set { }
      }

      public virtual Object GetNodeProperty(NodeProperty property)
      {
         if (NodePropertyHelpers.IsBooleanProperty(property))
            return this.GetNodeProperty(NodePropertyHelpers.ToBooleanProperty(property));
         else
         {
            switch (property)
            {
               case NodeProperty.Name: return this.Name;
               case NodeProperty.WireColor: return this.WireColor;
               default: return null;
            }
         }
      }

      public virtual Boolean GetNodeProperty(BooleanNodeProperty property)
      {
         return false;
      }

      public virtual void SetNodeProperty(NodeProperty property, Object value)
      {
         if (NodePropertyHelpers.IsBooleanProperty(property))
            this.SetNodeProperty(NodePropertyHelpers.ToBooleanProperty(property), (Boolean)value);
         else
         {
            switch (property)
            {
               case NodeProperty.Name: 
                  this.Name = (String)value; 
                  break;
               case NodeProperty.WireColor: 
                  this.WireColor = (Color)value; 
                  break;
               default:
                  break;
            }
         }
      }

      public virtual void SetNodeProperty(BooleanNodeProperty property, Boolean value) { }

      /// <summary>
      /// Returns true if the supplied property is inherited from another object, 
      /// e.g. a layer.
      /// </summary>
      public virtual Boolean IsNodePropertyInherited(NodeProperty property)
      {
         return false;
      }

      /// <summary>
      /// Returns true if the supplied property is inherited from another object, 
      /// e.g. a layer.
      /// </summary>
      public virtual Boolean IsNodePropertyInherited(BooleanNodeProperty property)
      {
         return this.IsNodePropertyInherited(NodePropertyHelpers.ToProperty(property));
      }

      #endregion

      /// <summary>
      /// Tests if the wrapped node is still a valid scene node and hasn't been deleted.
      /// </summary>
      public virtual Boolean IsValid
      {
         get { return this.WrappedNode != null; }
      }

      public virtual String ImageKey
      {
         get { return "unknown"; }
      }

      public static MaxNodeWrapper Create(Object node)
      {
         Throw.IfArgumentIsNull(node, "node");

         if (node is MaxNodeWrapper)
            return (MaxNodeWrapper)node;

         if (node is IINode)
            return new IINodeWrapper((IINode)node);
         else if (node is IILayer)
            return new IILayerWrapper((IILayer)node);
         else if (node is IILayerProperties)
            return new IILayerWrapper((IILayerProperties)node);
         else
            throw new ArgumentException("Cannot create wrapper for type " + node.GetType().ToString());
      }
   }
}
