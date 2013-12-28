using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Controls.Tree;
using Outliner.MaxUtils;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Reflection;
using Outliner.Scene;
using Outliner.Modes;
using PJanssen;

namespace Outliner.NodeSorters
{
/// <summary>
/// Defines a sorting order.
/// </summary>
public enum SortOrder
{
   /// <summary>
   /// Sort ascending: smallest to largest value.
   /// </summary>
   Ascending,

   /// <summary>
   /// Sort descending: largest to smallest value.
   /// </summary>
   Descending
}

/// <summary>
/// A base class for an object that sorts TreeNodes.
/// </summary>
public abstract class NodeSorter : IComparer<TreeNode>
{
   private Boolean invert;

   /// <summary>
   /// Initializes a new instance of the NodeSorter class. The sortorder will be set to Ascending.
   /// </summary>
   protected NodeSorter() : this(SortOrder.Ascending) { }

   /// <summary>
   /// Initializes a new instance of the NodeSorter class.
   /// </summary>
   /// <param name="sortOrder">The sortorder for the sorter.</param>
   protected NodeSorter(SortOrder sortOrder)
   {
      this.SortOrder = sortOrder;
   }

   /// <summary>
   /// Gets or sets the sort order (ascending or descending).
   /// </summary>
   [XmlElement("sortOrder")]
   [DefaultValue(SortOrder.Ascending)]
   public virtual SortOrder SortOrder
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
   [Browsable(false)]
   public virtual NodeSorter SecondarySorter { get; set; }


   /// <summary>
   /// Compares two TreeNodes to determine their relative sorting.
   /// </summary>
   /// <returns>1 if TreeNode x should come before TreeNode y, -1 if TreeNode y should come before TreeNode x.
   /// If the two TreeNodes are equal, 0 will be returned.</returns>
   public int Compare(TreeNode x, TreeNode y)
   {
      Throw.IfNull(x, "x");
      Throw.IfNull(y, "y");

      if (x == y)
         return 0;

      IMaxNode nodeX = TreeMode.GetMaxNode(x);
      if (nodeX == null || !nodeX.IsValid) return 0;

      IMaxNode nodeY = TreeMode.GetMaxNode(y);
      if (nodeY == null || !nodeY.IsValid) return 0;

      int compareResult = this.invert ? this.InternalCompare(nodeY, nodeX) 
                                      : this.InternalCompare(nodeX, nodeY);
         
      if (compareResult != 0)
         return compareResult;
      else if (this.SecondarySorter != null)
         return this.SecondarySorter.Compare(x, y);
      else
         return 0;
   }

   /// <summary>
   /// Classes inheriting from NodeSorter implement this method to do the actual comparison.
   /// </summary>
   /// <returns>1 if TreeNode x should come before TreeNode y, -1 if TreeNode y should come before TreeNode x.
   /// If the two TreeNodes are equal, 0 will be returned.</returns>
   protected abstract int InternalCompare(IMaxNode x, IMaxNode y);

   
   /// <summary>
   /// Compares two NodeSorter objects for equality.
   /// </summary>
   /// <param name="obj">The other object to compare this object with.</param>
   public override bool Equals(object obj)
   {
      if (obj == null)
         return false;

      Type thisType = this.GetType();
      if (!thisType.Equals(obj.GetType()))
         return false;

      NodeSorter otherSorter = (NodeSorter)obj;

      foreach (PropertyInfo prop in thisType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
      {
         Object ownValue = prop.GetValue(this, null);
         Object otherValue = prop.GetValue(otherSorter, null);
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

   /// <summary>
   /// Gets the hashcode for this NodeSorter.
   /// </summary>
   public override int GetHashCode()
   {
      Type thisType = this.GetType();
      int hash = thisType.GetHashCode();
      int m = 7;
      foreach (PropertyInfo prop in thisType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
      {
         Object ownValue = prop.GetValue(this, null);
         if (ownValue != null)
            hash = (hash * m) + ownValue.GetHashCode();
         else
            hash = hash * m;

         m++;
      }

      return hash;
   }

   /// <summary>
   /// Tests whether the supplied sorter chain contains a sorter of the supplied 
   /// type. This can be used to determine whether sorter is required.
   /// </summary>
   public static Boolean RequiresSort(NodeSorter sorter, Type sorterType)
   {
      Throw.IfNull(sorterType, "sorterType");

      if (sorter == null)
         return false;

      if (sorter.GetType().Equals(sorterType))
         return true;
      else
         return NodeSorter.RequiresSort(sorter.SecondarySorter, sorterType);
   }

   /// <summary>
   /// Tests whether the supplied sorter chain contains a NodePropertySorter
   /// that sorts using the supplied NodeProperty.
   /// </summary>
   public static Boolean RequiresSort(NodeSorter sorter, NodeProperty prop)
   {
      if (sorter == null)
         return false;

      NodePropertySorter propertySorter = sorter as NodePropertySorter;
      if (propertySorter != null && (propertySorter.Property & prop) != 0)
         return true;
      else
         return NodeSorter.RequiresSort(sorter.SecondarySorter, prop);
   }
}
}
