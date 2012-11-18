﻿using System;
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
   public abstract class IMaxNodeWrapper
   {
      public abstract Object WrappedNode { get; }
      public override abstract bool Equals(object obj);
      public override abstract int GetHashCode();

      public virtual IMaxNodeWrapper Parent { get { return null; }  }
      public abstract IEnumerable<Object> ChildNodes { get; }
      public virtual IEnumerable<IMaxNodeWrapper> WrappedChildNodes 
      {
         get { return this.ChildNodes.Select(IMaxNodeWrapper.Create); }
      }

      public virtual Boolean CanAddChildNode(IMaxNodeWrapper node)
      {
         return false;
      }
      public virtual Boolean CanAddChildNodes(IEnumerable<IMaxNodeWrapper> nodes)
      {
         return nodes.Any(this.CanAddChildNode);
      }
      public virtual void AddChildNode(IMaxNodeWrapper node) { }
      public virtual void AddChildNodes(IEnumerable<IMaxNodeWrapper> nodes)
      {
         nodes.ForEach(this.AddChildNode);
      }

      public virtual Boolean CanRemoveChildNode(IMaxNodeWrapper node) 
      {
         return false; 
      }
      public virtual Boolean CanRemoveChildNodes(IEnumerable<IMaxNodeWrapper> nodes)
      {
         return nodes.Any(this.CanRemoveChildNode);
      }
      public virtual void RemoveChildNode(IMaxNodeWrapper node) { }
      public virtual void RemoveChildNodes(IEnumerable<IMaxNodeWrapper> nodes)
      {
         nodes.ForEach(this.RemoveChildNode);
      }

      

      public abstract String Name { get; set; }
      public virtual String DisplayName { get { return this.Name; } }
      public virtual Boolean CanEditName { get { return true; } }
      public abstract SClass_ID SuperClassID { get; }
      public abstract IClass_ID ClassID { get; }
      public abstract Boolean IsNodeType(MaxNodeTypes types);
      public abstract Boolean Selected { get; } //TODO check if set should be added?


      public virtual Color WireColor
      {
         get { return Color.Empty; }
         set { }
      }

      public virtual Object GetProperty(NodeProperty property)
      {
         if (NodePropertyHelpers.IsBooleanProperty(property))
            return this.GetProperty(NodePropertyHelpers.ToBooleanProperty(property));
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

      public virtual Boolean GetProperty(BooleanNodeProperty property)
      {
         return false;
      }

      public virtual void SetProperty(NodeProperty property, Object value)
      {
         if (NodePropertyHelpers.IsBooleanProperty(property))
            this.SetProperty(NodePropertyHelpers.ToBooleanProperty(property), (Boolean)value);
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

      public virtual void SetProperty(BooleanNodeProperty property, Boolean value) { }

      /// <summary>
      /// Returns true if the supplied property is inherited from another object, 
      /// e.g. a layer.
      /// </summary>
      public virtual Boolean IsPropertyInherited(NodeProperty property)
      {
         return false;
      }

      /// <summary>
      /// Returns true if the supplied property is inherited from another object, 
      /// e.g. a layer.
      /// </summary>
      public virtual Boolean IsPropertyInherited(BooleanNodeProperty property)
      {
         return this.IsPropertyInherited(NodePropertyHelpers.ToProperty(property));
      }


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

      public static IMaxNodeWrapper Create(Object node)
      {
         ExceptionHelpers.ThrowIfArgumentIsNull(node, "node");

         if (node is IMaxNodeWrapper)
            return (IMaxNodeWrapper)node;

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