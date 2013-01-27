using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;

namespace Outliner.MaxUtils
{
public static class NodeProperties
{
   public static Boolean IsBooleanProperty(NodeProperty property)
   {
      return Enum.IsDefined(typeof(BooleanNodeProperty), (int)property);
   }

   public static NodeProperty ToProperty(BooleanNodeProperty property)
   {
      return (NodeProperty)Enum.ToObject(typeof(NodeProperty), property);
   }

   public static BooleanNodeProperty ToBooleanProperty(NodeProperty property)
   {
      return (BooleanNodeProperty)Enum.ToObject(typeof(BooleanNodeProperty), property);
   }

   public static NodeProperty DisplayProperties
   {
      get
      {
         return NodeProperty.IsHidden
                | NodeProperty.IsFrozen
                | NodeProperty.SeeThrough
                | NodeProperty.BoxMode
                | NodeProperty.BackfaceCull
                | NodeProperty.AllEdges
                | NodeProperty.VertexTicks
                | NodeProperty.Trajectory
                | NodeProperty.IgnoreExtents
                | NodeProperty.FrozenInGray;
      }
   }

   public static Boolean IsDisplayProperty(NodeProperty property)
   {
      return (DisplayProperties & property) != 0;
   }

   public static Boolean IsDisplayProperty(BooleanNodeProperty property)
   {
      return IsDisplayProperty(ToProperty(property));
   }

   public static NodeProperty RenderProperties
   {
      get
      {
         return NodeProperty.Renderable
                | NodeProperty.InheritVisibility
                | NodeProperty.PrimaryVisibility
                | NodeProperty.SecondaryVisibility
                | NodeProperty.ReceiveShadows
                | NodeProperty.CastShadows
                | NodeProperty.ApplyAtmospherics
                | NodeProperty.RenderOccluded;
      }
   }
   public static Boolean IsRenderProperty(NodeProperty property)
   {
      return (RenderProperties & property) != 0;
   }

   public static Boolean IsRenderProperty(BooleanNodeProperty property)
   {
      return IsRenderProperty(ToProperty(property));
   }
}
}
