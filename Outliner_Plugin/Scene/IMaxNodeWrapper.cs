using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using System.Drawing;

namespace Outliner.Scene
{
   public abstract class IMaxNodeWrapper
   {
      public abstract Object UnderlyingNode { get; }

      public abstract int NumChildren { get; }
      public abstract IEnumerable<IMaxNodeWrapper> ChildNodes { get; }
      public virtual void AddChildNode(IMaxNodeWrapper node) { }
      public virtual void AddChildNodes(IEnumerable<IMaxNodeWrapper> nodes)
      {
         nodes.ForEach(this.AddChildNode);
      }

      public virtual Boolean CanAddChildNode(IMaxNodeWrapper node) 
      {
         return false; 
      }
      public virtual Boolean CanAddChildNodes(IEnumerable<IMaxNodeWrapper> nodes) 
      {
         return nodes.Any(this.CanAddChildNode);
      }

      public abstract String Name { get; set; }

      public abstract SClass_ID SuperClassID { get; }
      public abstract IClass_ID ClassID { get; }

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


      /// <summary>
      /// Tests if the wrapped node is still a valid scene node and hasn't been deleted.
      /// </summary>
      public virtual Boolean IsValid
      {
         get
         {
            Object node = this.UnderlyingNode;
            if (node == null)
               return false;

            try
            {
               if (node is IAnimatable)
                  return ((IAnimatable)node).TestAFlag(AnimatableFlags.IsDeleted);
               else
                  return true;
            }
            catch
            {
               return false;
            }
         }
      }

      public const String IMGKEY_UNKNOWN = "unknown";
      public virtual String ImageKey
      {
         get { return IMGKEY_UNKNOWN;}
      }

      public static IMaxNodeWrapper Create(Object node)
      {
         if (node is IINode)
            return new IINodeWrapper((IINode)node);
         else if (node is IILayer)
            return new IILayerWrapper((IILayer)node);
         else if (node is KeyValuePair<IINamedSelectionSetManager, int>)
         {
            KeyValuePair<IINamedSelectionSetManager, int> kvp = (KeyValuePair<IINamedSelectionSetManager, int>)node;
            return new SelectionSetWrapper(kvp.Key, kvp.Value);
         }
         else
            throw new ArgumentException("Cannot create wrapper for type " + node.GetType().ToString());
      }
   }
}
