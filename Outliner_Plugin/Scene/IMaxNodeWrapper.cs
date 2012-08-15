using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using System.Drawing;
using MaxUtils;

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

      public virtual Boolean IsHidden 
      {
         get { return false; }
         set { }
      }
      public virtual Boolean IsFrozen 
      {
         get { return false; }
         set { }
      }
      public virtual Boolean BoxMode
      {
         get { return false; }
         set { }
      }
      public virtual Color WireColor
      {
         get { return Color.Empty; }
         set { }
      }
      public virtual Boolean Renderable
      {
         get { return false; }
         set { }
      }
      public virtual Boolean XRayMtl 
      {
         get { return false; }
         set { }
      }

      public virtual Object GetProperty(AnimatableProperty prop)
      {
         if (AnimatablePropertyHelpers.IsBooleanProperty(prop))
            return this.GetProperty(AnimatablePropertyHelpers.ToBooleanProperty(prop));
         else
         {
            switch (prop)
            {
               case AnimatableProperty.WireColor: return this.WireColor;
               case AnimatableProperty.Name: return this.Name;
               default: return null;
            }
         }
      }

      public virtual Boolean GetProperty(AnimatableBooleanProperty prop)
      {
         switch (prop)
         {
            case AnimatableBooleanProperty.BoxMode: return this.BoxMode;
            case AnimatableBooleanProperty.IsFrozen: return this.IsFrozen;
            case AnimatableBooleanProperty.IsHidden: return this.IsHidden;
            case AnimatableBooleanProperty.Renderable: return this.Renderable;
            case AnimatableBooleanProperty.XRayMtl: return this.XRayMtl;
            default: return false;
         }  
      }

      public virtual void SetProperty(AnimatableBooleanProperty prop, Boolean newValue)
      {
         switch (prop)
         {
            case AnimatableBooleanProperty.BoxMode: this.BoxMode = newValue; break;
            case AnimatableBooleanProperty.IsFrozen: this.IsFrozen = newValue; break;
            case AnimatableBooleanProperty.IsHidden: this.IsHidden = newValue; break;
            case AnimatableBooleanProperty.Renderable: this.Renderable = newValue; break;
            case AnimatableBooleanProperty.XRayMtl: this.XRayMtl = newValue; break;
         } 
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
         get { return Outliner.Controls.IconHelperMethods.IMGKEY_UNKNOWN; }
      }

      public static IMaxNodeWrapper Create(Object node)
      {
         if (node == null)
            throw new ArgumentNullException("node");

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
