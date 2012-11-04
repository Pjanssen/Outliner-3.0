using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Outliner.MaxUtils
{
   // NOTE:
   // When adding new properties: only add at the end to avoid issues with
   // values stored in scenes by NestedLayers!
   public enum NodeProperty : byte
   {
      None = 0,

      //Display Properties
      IsHidden,
      IsFrozen,
      SeeThrough,
      BoxMode,
      BackfaceCull,
      AllEdges,
      VertexTicks,
      Trajectory,
      IgnoreExtents,
      FrozenInGray,

      //Render Properties
      Renderable,
      InheritVisibility,
      PrimaryVisibility,
      SecondaryVisibility,
      ReceiveShadows,
      CastShadows,
      ApplyAtmospherics,
      RenderOccluded,

      //Other properties
      Name,
      WireColor
   }

   public enum BooleanNodeProperty : byte
   {
      None = 0,
      //Display Properties
      IsHidden = NodeProperty.IsHidden,
      IsFrozen = NodeProperty.IsFrozen,
      SeeThrough = NodeProperty.SeeThrough,
      BoxMode = NodeProperty.BoxMode,
      BackfaceCull = NodeProperty.BackfaceCull,
      AllEdges = NodeProperty.AllEdges,
      VertexTicks = NodeProperty.VertexTicks,
      Trajectory = NodeProperty.Trajectory,
      IgnoreExtents = NodeProperty.IgnoreExtents,
      FrozenInGray = NodeProperty.FrozenInGray,

      //Render Properties
      Renderable = NodeProperty.Renderable,
      InheritVisibility = NodeProperty.InheritVisibility,
      PrimaryVisibility = NodeProperty.PrimaryVisibility,
      SecondaryVisibility = NodeProperty.SecondaryVisibility,
      ReceiveShadows = NodeProperty.ReceiveShadows,
      CastShadows = NodeProperty.CastShadows,
      ApplyAtmospherics = NodeProperty.ApplyAtmospherics,
      RenderOccluded = NodeProperty.RenderOccluded
   }

   public static class NodePropertyHelpers
   {
      public static Boolean IsBooleanProperty(NodeProperty property)
      {
         return Enum.IsDefined(typeof(BooleanNodeProperty), (byte)property);
      }

      public static NodeProperty ToProperty(BooleanNodeProperty property)
      {
         return (NodeProperty)Enum.ToObject(typeof(NodeProperty), property);
      }

      public static BooleanNodeProperty ToBooleanProperty(NodeProperty property)
      {
         return (BooleanNodeProperty)Enum.ToObject(typeof(BooleanNodeProperty), property);
      }

      public static Boolean IsDisplayProperty(NodeProperty property)
      {
         return ((property & NodeProperty.IsHidden)      == NodeProperty.IsHidden)
             || ((property & NodeProperty.IsFrozen)      == NodeProperty.IsFrozen)
             || ((property & NodeProperty.SeeThrough)    == NodeProperty.SeeThrough)
             || ((property & NodeProperty.BoxMode)       == NodeProperty.BoxMode)
             || ((property & NodeProperty.BackfaceCull)  == NodeProperty.BackfaceCull)
             || ((property & NodeProperty.AllEdges)      == NodeProperty.AllEdges)
             || ((property & NodeProperty.VertexTicks)   == NodeProperty.VertexTicks)
             || ((property & NodeProperty.Trajectory)    == NodeProperty.Trajectory)
             || ((property & NodeProperty.IgnoreExtents) == NodeProperty.IgnoreExtents)
             || ((property & NodeProperty.FrozenInGray)  == NodeProperty.FrozenInGray);
      }

      public static Boolean IsDisplayProperty(BooleanNodeProperty property)
      {
         return IsDisplayProperty(ToProperty(property));
      }

      public static Boolean IsRenderProperty(NodeProperty property)
      {
         return ((property & NodeProperty.Renderable)          == NodeProperty.Renderable)
             || ((property & NodeProperty.InheritVisibility)   == NodeProperty.InheritVisibility)
             || ((property & NodeProperty.PrimaryVisibility)   == NodeProperty.PrimaryVisibility)
             || ((property & NodeProperty.SecondaryVisibility) == NodeProperty.SecondaryVisibility)
             || ((property & NodeProperty.ReceiveShadows)      == NodeProperty.ReceiveShadows)
             || ((property & NodeProperty.CastShadows)         == NodeProperty.CastShadows)
             || ((property & NodeProperty.ApplyAtmospherics)   == NodeProperty.ApplyAtmospherics)
             || ((property & NodeProperty.RenderOccluded)      == NodeProperty.RenderOccluded);
      }

      public static Boolean IsRenderProperty(BooleanNodeProperty property)
      {
         return IsRenderProperty(ToProperty(property));
      }
   }
}
