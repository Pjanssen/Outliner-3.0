using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Controls.Tree;
using Outliner.MaxUtils;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Reflection;

namespace Outliner.NodeSorters
{
public enum SortOrder
{
   Ascending,
   Descending
}

public abstract class NodeSorter : IComparer<TreeNode>
{
   private Boolean invert;

   protected NodeSorter() : this(SortOrder.Ascending) { }
   protected NodeSorter(SortOrder sortOrder)
   {
      this.SortOrder = sortOrder;

      if (!(this is AlphabeticalSorter))
         this.SecondarySorter = new AlphabeticalSorter(sortOrder);
   }


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


   /// <summary>
   /// The sorter which will determine the order of two items if the Compare
   /// method returns 0.
   /// </summary>
   [XmlElement("secondarySorter")]
   public NodeSorter SecondarySorter { get; set; }



   public int Compare(TreeNode x, TreeNode y)
   {
      Throw.IfArgumentIsNull(x, "x");
      Throw.IfArgumentIsNull(y, "y");

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

   

   public override bool Equals(object obj)
   {
      if (obj == null)
         return false;

      Type thisType = this.GetType();
      if (!thisType.Equals(obj.GetType()))
         return false;

      NodeSorter otherSorter = (NodeSorter)obj;

      foreach (FieldInfo field in thisType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
      {
         Object ownValue = field.GetValue(this);
         Object otherValue = field.GetValue(otherSorter);
         if (ownValue == null)
         {
            if (otherValue != null)
               return false;
         }
         else if (!ownValue.Equals(otherValue))
            return false;
      }
      return true;
   }

   public override int GetHashCode()
   {
      Type thisType = this.GetType();
      int hash = thisType.GetHashCode();
      int m = 7;
      foreach (FieldInfo field in thisType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
      {
         Object ownValue = field.GetValue(this);
         if (ownValue != null)
            hash = (hash * m) + ownValue.GetHashCode();
         else
            hash = hash * m;

         m++;
      }

      return hash;
   }
}
}
