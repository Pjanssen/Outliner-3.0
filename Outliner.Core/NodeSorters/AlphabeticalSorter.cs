using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Controls.Tree;
using Autodesk.Max;
using Outliner.Scene;
using Outliner.Plugins;
using Outliner.MaxUtils;
using Outliner.Modes;

namespace Outliner.NodeSorters
{
   /// <summary>
   /// Sorts TreeNodes alphabetically by the name of the IMaxNode attached to the TreeNode.
   /// </summary>
   [OutlinerPlugin(OutlinerPluginType.NodeSorter)]
   [LocalizedDisplayName(typeof(OutlinerResources), "Sorter_Alphabetical")]
   public class AlphabeticalSorter : NodePropertySorter
   {
      /// <summary>
      /// Initializes a new instance of the AlphabeticalSorter class, using SortOrder.Ascending.
      /// </summary>
      public AlphabeticalSorter() : this(SortOrder.Ascending) { }

      /// <summary>
      /// Initializes a new instance of the AlphabeticalSorter class
      /// </summary>
      /// <param name="sortOrder">The order to sort by.</param>
      public AlphabeticalSorter(SortOrder sortOrder) : base(NodeProperty.Name, sortOrder) { }

      protected override int InternalCompare(TreeNode x, TreeNode y)
      {
         if (x == y)
            return 0;

         IMaxNode nodeX = TreeMode.GetMaxNode(x);
         if (nodeX == null || !nodeX.IsValid) return 0;

         IMaxNode nodeY = TreeMode.GetMaxNode(y);
         if (nodeY == null || !nodeY.IsValid) return 0;

         return NativeMethods.StrCmpLogicalW(nodeX.Name, nodeY.Name);
      }
   }
}
