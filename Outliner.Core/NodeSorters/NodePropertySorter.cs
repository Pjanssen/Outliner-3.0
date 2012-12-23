using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.MaxUtils;
using Outliner.Controls.Tree;
using Outliner.Scene;
using System.Drawing;
using Outliner.Plugins;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Outliner.NodeSorters
{
[OutlinerPlugin(OutlinerPluginType.NodeSorter)]
[LocalizedDisplayName(typeof(OutlinerResources), "Sorter_NodeProperty")]
public class NodePropertySorter : NodeSorter
{
   [XmlElement("property")]
   [DefaultValue(NodeProperty.None)]
   public NodeProperty Property { get; set; }

   public NodePropertySorter() 
      : this(NodeProperty.None, SortOrder.Ascending) { }
   
   public NodePropertySorter(SortOrder sortOrder) 
      : this(NodeProperty.None, sortOrder) { }
   
   public NodePropertySorter(NodeProperty property) 
      : this(property, SortOrder.Ascending) { }
   
   public NodePropertySorter(NodeProperty property, SortOrder sortOrder)
      : base(sortOrder)
   {
      this.Property = property;
   }


   protected override int InternalCompare(TreeNode x, TreeNode y)
   {
      if (x == y)
         return 0;

      IMaxNodeWrapper nodeX = HelperMethods.GetMaxNode(x);
      if (nodeX == null || !nodeX.IsValid) return 0;

      IMaxNodeWrapper nodeY = HelperMethods.GetMaxNode(y);
      if (nodeY == null || !nodeY.IsValid) return 0;

      Object propValueX = nodeX.GetNodeProperty(this.Property);
      Object propValueY = nodeY.GetNodeProperty(this.Property);

      IComparable iCompX = propValueX as IComparable;
      if (iCompX != null)
         return iCompX.CompareTo(propValueY);
      else if (propValueX is Color && propValueY is Color)
         return ColorHelpers.Compare((Color)propValueX, (Color)propValueY);
      else
         return 0;
   }
}
}
