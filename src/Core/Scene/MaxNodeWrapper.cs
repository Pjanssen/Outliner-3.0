using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.MaxUtils;
using PJanssen;

namespace Outliner.Scene
{
   /// <summary>
   /// Provides a base wrapper class for a 3dsMax object, implementing IMaxNode.
   /// </summary>
   public abstract class MaxNodeWrapper : IMaxNode
   {
      /// <summary>
      /// Gets the wrapped base object.
      /// </summary>
      public abstract Object BaseObject { get; }

      public virtual Boolean IsAggregate
      {
         get { return false; }
      }

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

      public virtual String NodeTypeDisplayName
      {
         get { return this.BaseObject.GetType().Name; }
      }

      #endregion


      #region Node Properties

      public virtual Object GetNodeProperty(NodeProperty property)
      {
         if (NodeProperties.IsBooleanProperty(property))
            return this.GetNodeProperty(NodeProperties.ToBooleanProperty(property));
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
         if (NodeProperties.IsBooleanProperty(property))
            this.SetNodeProperty(NodeProperties.ToBooleanProperty(property), (Boolean)value);
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
         return this.IsNodePropertyInherited(NodeProperties.ToProperty(property));
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



      #region Factory

      private static List<IMaxNodeFactory> maxNodeFactories;

      /// <summary>
      /// Gets or sets the factories used to create new IMaxNodes.
      /// </summary>
      public static IEnumerable<IMaxNodeFactory> Factories
      {
         get { return maxNodeFactories; }
         set { maxNodeFactories = value.ToList(); }
      }

      /// <summary>
      /// Registers a new IMaxNodeFactory. The factory can be used to create a wrapper
      /// for any kind of node in the scene. When the factory can't create an appropriate
      /// wrapper, it should return null.
      /// </summary>
      public static void RegisterMaxNodeFactory(IMaxNodeFactory factory)
      {
         Throw.IfNull(factory, "factory");

         if (maxNodeFactories == null)
            maxNodeFactories = new List<IMaxNodeFactory>();

         maxNodeFactories.Add(factory);
      }

      /// <summary>
      /// Attempts to create a new IMaxNode wrapper using the registered factories.
      /// </summary>
      /// <exception cref="ArgumentNullException" />
      /// <exception cref="NotSupportedException" />
      public static IMaxNode Create(object baseNode)
      {
         Throw.IfNull(baseNode, "baseNode");

         if (Factories != null)
         {
            foreach (IMaxNodeFactory factory in Factories)
            {
               IMaxNode node = factory.CreateMaxNode(baseNode);
               if (node != null)
                  return node;
            }
         }

         throw new NotSupportedException("Cannot create a wrapper for object of type " + baseNode.GetType().Name);         
      }

      #endregion
   }
}
