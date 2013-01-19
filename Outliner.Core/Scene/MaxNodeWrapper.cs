using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.MaxUtils;

namespace Outliner.Scene
{
   public abstract class MaxNodeWrapper : IMaxNode
   {
      public abstract Object BaseObject { get; }

      public virtual IMaxNode Parent
      {
         get { return null; }
         set { }
      }

      public virtual Boolean IsSelected
      {
         get { return false; }
         set { }
      }

      public virtual Boolean IsValid
      {
         get { return this.BaseObject != null; }
      }


      #region Equality

      public override abstract Boolean Equals(Object obj);
      public override abstract int GetHashCode();

      #endregion


      #region Delete
      
      public virtual Boolean CanDelete
      {
         get { return false; }
      }

      public virtual void Delete() { }

      #endregion


      #region Childnodes
      
      public virtual Int32 ChildNodeCount
      {
         get { return this.ChildBaseObjects.Count(); }
      }

      public virtual IEnumerable<Object> ChildBaseObjects
      {
         get { return Enumerable.Empty<Object>(); }
      }

      public virtual IEnumerable<IMaxNode> ChildNodes
      {
         get { return ChildBaseObjects.Select(MaxNodeWrapper.Create); }
      }

      public virtual Boolean CanAddChildNode(IMaxNode node)
      {
         return false;
      }

      public virtual Boolean CanAddChildNodes(IEnumerable<IMaxNode> nodes)
      {
         return nodes.Any(this.CanAddChildNode);
      }

      public virtual void AddChildNode(IMaxNode node) { }

      public virtual void AddChildNodes(IEnumerable<IMaxNode> nodes)
      {
         nodes.ForEach(this.AddChildNode);
      }

      public virtual Boolean CanRemoveChildNode(IMaxNode node)
      {
         return this.ChildBaseObjects.Contains(node.BaseObject);
      }

      public virtual Boolean CanRemoveChildNodes(IEnumerable<IMaxNode> nodes)
      {
         return nodes.Any(this.CanRemoveChildNode);
      }

      public virtual void RemoveChildNode(IMaxNode node) { }

      public virtual void RemoveChildNodes(IEnumerable<IMaxNode> nodes)
      {
         nodes.ForEach(this.RemoveChildNode);
      }

      #endregion


      #region Node Type

      public virtual SClass_ID SuperClassID
      {
         get { return (SClass_ID)0; }
      }

      public virtual IClass_ID ClassID
      {
         get { return null; }
      }

      protected abstract MaxNodeType MaxNodeType { get; }
      public virtual Boolean IsNodeType(MaxNodeType types)
      {
         return (types & MaxNodeType) == MaxNodeType;
      }

      #endregion


      #region Name

      public virtual String Name
      {
         get { return ""; }
         set { }
      }

      public virtual Boolean CanEditName
      {
         get { return false; }
      }

      public virtual String DisplayName
      {
         get { return this.Name; }
      }

      #endregion


      #region Node Properties

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

      public virtual void SetNodeProperty(NodeProperty property, object value) 
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

      public virtual void SetNodeProperty(BooleanNodeProperty property, bool value) { }

      public virtual Boolean IsNodePropertyInherited(NodeProperty property)
      {
         return false;
      }

      public virtual Boolean IsNodePropertyInherited(BooleanNodeProperty property)
      {
         return this.IsNodePropertyInherited(NodePropertyHelpers.ToProperty(property));
      }

      public virtual Color WireColor 
      {
         get { return Color.Empty; }
         set { }
      }

      #endregion


      #region ImageKey
      
      public virtual String ImageKey
      {
         get { return "unknown"; }
      }

      #endregion


      public override String ToString()
      {
         return "MaxNodeWrapper (" + this.Name + ")";
      }


      public static MaxNodeWrapper Create(Object obj)
      {
         Throw.IfArgumentIsNull(obj, "obj");

         //INodeWrapper
         IINode inode = obj as IINode;
         if (inode != null)
            return new INodeWrapper(inode);

         //ILayerWrapper
         IILayer ilayer = obj as IILayer;
         if (ilayer != null)
            return new ILayerWrapper(ilayer);
         IILayerProperties ilayerProperties = obj as IILayerProperties;
         if (ilayerProperties != null)
            return new ILayerWrapper(ilayerProperties);

         //MaterialWrapper
         IMtl imtl = obj as IMtl;
         if (imtl != null)
            return new MaterialWrapper(imtl);

         throw new NotSupportedException("Cannot create a wrapper for object of type " + obj.GetType().Name);
      }
   }
}
