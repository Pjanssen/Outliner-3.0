using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Controls.Tree;
using Outliner.MaxUtils;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Outliner.NodeSorters
{
   public enum SortOrder
   {
      Ascending,
      Descending
   }

   public abstract class NodeSorter : IComparer<TreeNode>
   {
      protected NodeSorter() : this(SortOrder.Ascending) { }
      protected NodeSorter(SortOrder sortOrder)
      {
         this.SortOrder = sortOrder;

         if (!(this is AlphabeticalSorter))
            this.SecondarySorter = new AlphabeticalSorter(sortOrder);
      }

      private Boolean invert;

      [XmlElement("sortOrder")]
      [DefaultValue(SortOrder.Ascending)]
      public SortOrder SortOrder
      {
         get 
         { 
            return this.invert ? SortOrder.Descending 
                               : SortOrder.Ascending; 
         }
         set 
         {
            this.invert = (value == SortOrder.Ascending) ? false : true; 
         }
      }


      [XmlElement("secondarySorter")]
      public NodeSorter SecondarySorter { get; set; }

      public int Compare(TreeNode x, TreeNode y)
      {
         ExceptionHelpers.ThrowIfArgumentIsNull(x, "x");
         ExceptionHelpers.ThrowIfArgumentIsNull(y, "y");

         int compareResult = this.invert ? this.InternalCompare(y, x) 
                                         : this.InternalCompare(x, y);
         
         if (compareResult != 0)
            return compareResult;
         else if (this.SecondarySorter != null)
            return this.SecondarySorter.Compare(x, y);
         else
            return 0;
      }

      protected abstract int InternalCompare(TreeNode x, TreeNode y);

      public Boolean ContainsSorterType(Type t)
      {
         return this.GetType().Equals(t) 
                || (this.SecondarySorter != null && this.SecondarySorter.ContainsSorterType(t));
      }
   }
}
