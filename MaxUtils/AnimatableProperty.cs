using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MaxUtils
{

   public enum AnimatableBooleanProperty : byte
   {
      IsHidden = AnimatableProperty.IsHidden,
      IsFrozen = AnimatableProperty.IsFrozen,
      BoxMode = AnimatableProperty.BoxMode,
      XRayMtl = AnimatableProperty.XRayMtl,
      Renderable = AnimatableProperty.Renderable
   }

   public enum AnimatableProperty : byte
   {
      IsHidden,
      IsFrozen,
      BoxMode,
      XRayMtl,
      Renderable,
      Name,
      WireColor
   }

   public static class AnimatablePropertyHelpers
   {
      public static Boolean IsBooleanProperty(AnimatableProperty property)
      {
         return Enum.IsDefined(typeof(AnimatableBooleanProperty), (byte)property);
      }

      public static AnimatableBooleanProperty ToBooleanProperty(AnimatableProperty property)
      {
         return (AnimatableBooleanProperty)Enum.ToObject(typeof(AnimatableBooleanProperty), property);
      }
   }
}
