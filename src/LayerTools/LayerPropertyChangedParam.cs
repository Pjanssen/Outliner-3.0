using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using PJanssen.Outliner.MaxUtils;

namespace PJanssen.Outliner.LayerTools
{
   /// <summary>
   /// Provides a structure containing information about a property change on a layer.
   /// </summary>
   public struct LayerPropertyChangedParam
   {
      private IILayer layer;
      private NodeProperty property;

      /// <summary>
      /// The layer of which a property has changed.
      /// </summary>
      public IILayer Layer { get { return layer; } }

      /// <summary>
      /// The property that changed.
      /// </summary>
      public NodeProperty Property { get { return property; } }

      /// <summary>
      /// Initializes a new instance of the LayerPropertyChangedParam struct.
      /// </summary>
      /// <param name="layer">The layer of which a property has changed.</param>
      /// <param name="property">The property that changed.</param>
      public LayerPropertyChangedParam(IILayer layer, NodeProperty property)
      {
         this.layer = layer;
         this.property = property;
      }
   }
}
