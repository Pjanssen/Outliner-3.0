using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;

namespace Outliner.MaxUtils
{
/// <summary>
/// Provides methods for common NodeProperty operations.
/// </summary>
public static class NodeProperties
{
   /// <summary>
   /// Tests if the supplied NodeProperty is also a BooleanNodeProperty.
   /// </summary>
   /// <param name="property">The NodeProperty to test.</param>
   public static Boolean IsBooleanProperty(NodeProperty property)
   {
      return Enum.IsDefined(typeof(BooleanNodeProperty), (int)property);
   }

   /// <summary>
   /// Converts a NodeProperty to a BooleanNodeProperty.
   /// </summary>
   /// <param name="property">The NodeProperty to convert.</param>
   public static BooleanNodeProperty ToBooleanProperty(NodeProperty property)
   {
      return (BooleanNodeProperty)Enum.ToObject(typeof(BooleanNodeProperty), property);
   }

   /// <summary>
   /// Converts a BooleanNodeProperty to a NodeProperty.
   /// </summary>
   /// <param name="property">The BooleanNodeProperty to convert.</param>
   public static NodeProperty ToProperty(BooleanNodeProperty property)
   {
      return (NodeProperty)Enum.ToObject(typeof(NodeProperty), property);
   }

   /// <summary>
   /// Gets all display NodeProperties.
   /// </summary>
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

   /// <summary>
   /// Tests if the given NodeProperty is a display property.
   /// </summary>
   /// <param name="property">The NodeProperty to test.</param>
   public static Boolean IsDisplayProperty(NodeProperty property)
   {
      return (DisplayProperties & property) != 0;
   }

   /// <summary>
   /// Tests if the given BooleanNodeProperty is a display property.
   /// </summary>
   /// <param name="property">The BooleanNodeProperty to test.</param>
   public static Boolean IsDisplayProperty(BooleanNodeProperty property)
   {
      return IsDisplayProperty(ToProperty(property));
   }

   /// <summary>
   /// Gets all render NodeProperties.
   /// </summary>
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

   /// <summary>
   /// Tests if the given NodeProperty is a render property.
   /// </summary>
   /// <param name="property">The NodeProperty to test.</param>
   public static Boolean IsRenderProperty(NodeProperty property)
   {
      return (RenderProperties & property) != 0;
   }

   /// <summary>
   /// Tests if the given BooleanNodeProperty is a render property.
   /// </summary>
   /// <param name="property">The BooleanNodeProperty to test.</param>
   public static Boolean IsRenderProperty(BooleanNodeProperty property)
   {
      return IsRenderProperty(ToProperty(property));
   }
}
}
