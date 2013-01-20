using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.MaxUtils;
using Outliner.Scene;

namespace Outliner.Modes.SelectionSet
{
   public class AllObjectsSelectionSetWrapper : MaxNodeWrapper
   {
      public const String SelSetName = "--Internal_AllObjects--";
      public AllObjectsSelectionSetWrapper() { }

      public override object BaseObject
      {
         get { return SelSetName; }
      }

      public override bool IsSelected
      {
         get { return false; }
         set
         {
            this.ChildNodes.ForEach(n => n.IsSelected = value);
         }
      }


      #region Equality

      public override bool Equals(object obj)
      {
         return obj is AllObjectsSelectionSetWrapper;
      }

      public override int GetHashCode()
      {
         return SelSetName.GetHashCode();
      }

      #endregion


      #region Delete

      public override bool CanDelete
      {
         get { return false; }
      }

      #endregion


      #region Childnodes

      public override IEnumerable<object> ChildBaseObjects
      {
         get
         {
            return this.getChildren(MaxInterfaces.COREInterface.RootNode);
         }
      }

      private IEnumerable<IINode> getChildren(IINode node)
      {
         List<IINode> nodes = new List<IINode>();
         for (int i = 0; i < node.NumberOfChildren; i++)
         {
            IINode child = node.GetChildNode(i);
            nodes.Add(child);
            nodes.AddRange(getChildren(child));
         }
         return nodes;
      }

      public override bool CanAddChildNode(IMaxNode node)
      {
         return false;
      }

      public override bool CanRemoveChildNode(IMaxNode node)
      {
         return false;
      }

      #endregion


      #region Name
      
      public override string Name
      {
         get { return Resources.Str_AllObjectsSelSet; }
         set { throw new NotSupportedException(); }
      }

      public override bool CanEditName
      {
         get { return false; }
      }

      #endregion


      #region Type

      protected override MaxNodeType MaxNodeType
      {
         get { return MaxNodeType.SelectionSet; }
      }

      #endregion


      public override string ImageKey
      {
         get
         {
            return "selectionset";
         }
      }

      public override string ToString()
      {
         return "AllObjectsSelectionSetWrapper";
      }
   }
}
