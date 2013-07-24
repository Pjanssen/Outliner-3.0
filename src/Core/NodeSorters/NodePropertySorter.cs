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
using Outliner.Modes;

namespace Outliner.NodeSorters
{
/// <summary>
/// Sorts TreeNodes based on given NodeProperty.
/// </summary>
[OutlinerPlugin(OutlinerPluginType.NodeSorter)]
[LocalizedDisplayName(typeof(OutlinerResources), "Sorter_NodeProperty")]
public class NodePropertySorter : NodeSorter
{
   /// <summary>
   /// Gets or sets the property to sort by.
   /// </summary>
   [XmlElement("property")]
   [DefaultValue(NodeProperty.None)]
   public NodeProperty Property { get; set; }

   /// <summary>
   /// Initializes a new instance of the NodePropertySorter class, with NodeProperty.None and SortOrder.Ascending.
   /// </summary>
   public NodePropertySorter() 
      : this(NodeProperty.None, SortOrder.Ascending) { }
   
   /// <summary>
   /// Initializes a new instance of the NodePropertySorter class, with NodeProperty.None.
   /// </summary>
   /// <param name="sortOrder">The order to sort by.</param>
   public NodePropertySorter(SortOrder sortOrder) 
      : this(NodeProperty.None, sortOrder) { }

   /// <summary>
   /// Initializes a new instance of the NodePropertySorter class, with SortOrder.Ascending.
   /// </summary>
   /// <param name="property">The property to sort by.</param>
   public NodePropertySorter(NodeProperty property) 
      : this(property, SortOrder.Ascending) { }
   
   /// <summary>
   /// Initializes a new instance of the NodePropertySorter class.
   /// </summary>
   /// <param name="property">The property to sort by.</param>
   /// <param name="sortOrder">The order to sort by.</param>
   public NodePropertySorter(NodeProperty property, SortOrder sortOrder)
      : base(sortOrder)
   {
      this.Property = property;
   }


   protected override int InternalCompare(IMaxNode nodeX, IMaxNode nodeY)
   {
      if (nodeX == nodeY)
            return 0;

      Object propValueX = nodeX.GetNodeProperty(this.Property);
      Object propValueY = nodeY.GetNodeProperty(this.Property);

      IComparable iCompX = propValueX as IComparable;
      if (iCompX != null)
         return iCompX.CompareTo(propValueY);
      else if (propValueX is Color && propValueY is Color)
         return Colors.Compare((Color)propValueX, (Color)propValueY);
      else
         return 0;
   }
}
}
