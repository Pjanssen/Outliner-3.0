using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Controls.Tree;
using Outliner.Scene;
using Outliner.LayerTools;
using Autodesk.Max;
using Outliner.Plugins;
using Outliner.NodeSorters;
using Outliner.Modes;

namespace Outliner.ColorTags
{
   [OutlinerPlugin(OutlinerPluginType.NodeSorter)]
   [LocalizedDisplayName(typeof(Resources), "Sorter_ColorTag")]
   public class ColorTagSorter : NodeSorter
   {
      public ColorTagSorter() : base() { }
      public ColorTagSorter(SortOrder sortOrder) : base(sortOrder) { }

      protected override int InternalCompare(IMaxNode nodeX, IMaxNode nodeY)
      {
         ColorTag tagX = ColorTags.GetTag(nodeX.BaseObject as IAnimatable);
         ColorTag tagY = ColorTags.GetTag(nodeY.BaseObject as IAnimatable);

         if (tagX == tagY)
            return 0;
         else if (tagX == ColorTag.None)
            return 1;
         else if (tagY == ColorTag.None)
            return -1;
         else
            return tagX.CompareTo(tagY);
      }
   }
}
