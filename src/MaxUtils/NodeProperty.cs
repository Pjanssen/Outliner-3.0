using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PJanssen.Outliner.MaxUtils
{
   // NOTE:
   // When adding new properties: only add at the end to avoid issues with
   // values stored in scenes by NestedLayers!
   [Flags]
   public enum NodeProperty : int
   {
      None                = 0x00000,

      //Display Properties
      IsHidden            = 0x00001,
      IsFrozen            = 0x00002,
      SeeThrough          = 0x00004,
      BoxMode             = 0x00008,
      BackfaceCull        = 0x00010,
      AllEdges            = 0x00020,
      VertexTicks         = 0x00040,
      Trajectory          = 0x00080,
      IgnoreExtents       = 0x00100,
      FrozenInGray        = 0x00200,

      //Render Properties
      Renderable          = 0x00400,
      InheritVisibility   = 0x00800,
      PrimaryVisibility   = 0x01000,
      SecondaryVisibility = 0x02000,
      ReceiveShadows      = 0x04000,
      CastShadows         = 0x08000,
      ApplyAtmospherics   = 0x10000,
      RenderOccluded      = 0x20000,

      //Other properties
      Name                = 0x40000,
      WireColor           = 0x80000
   }

   [Flags]
   public enum BooleanNodeProperty : int
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

}
