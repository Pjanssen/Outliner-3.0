using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.MaxUtils;

namespace Outliner.LayerTools
{
   public struct LayerPropertyChangedParam
   {
      private IILayer layer;
      private NodeProperty property;

      public IILayer Layer { get { return layer; } }
      public NodeProperty Property { get { return property; } }

      public LayerPropertyChangedParam(IILayer layer, NodeProperty property)
      {
         this.layer = layer;
         this.property = property;
      }
   }
}
