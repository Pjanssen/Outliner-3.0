using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.MaxUtils;

namespace Outliner.NodeSorters
{
public static class NodeSorterHelpers
{
   /// <summary>
   /// Tests whether the supplied sorter chain contains a sorter of the supplied 
   /// type. This can be used to determine whether sorter is required.
   /// </summary>
   public static Boolean RequiresSort(NodeSorter sorter, Type t)
   {
      if (sorter == null)
         return false;

      if (sorter.GetType().Equals(t))
         return true;
      else
         return NodeSorterHelpers.RequiresSort(sorter.SecondarySorter, t);
   }

   /// <summary>
   /// Tests whether the supplied sorter chain contains a NodePropertySorter
   /// that sorts using the supplied NodeProperty.
   /// </summary>
   public static Boolean RequiresSort(NodeSorter sorter, NodeProperty prop)
   {
      if (sorter == null)
         return false;

      //if (prop == NodeProperty.Name && sorter is AlphabeticalSorter)
      //   return true;

      NodePropertySorter propertySorter = sorter as NodePropertySorter;
      if (propertySorter != null && (propertySorter.Property & prop) != 0)
         return true;
      else
         return NodeSorterHelpers.RequiresSort(sorter.SecondarySorter, prop);
   }
}
}
