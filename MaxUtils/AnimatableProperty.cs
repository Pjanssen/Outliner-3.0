using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MaxUtils
{
   /// <summary>
   /// Animatable property enumeration.
   /// </summary>
   /// <remarks>New values should only be added at the end to preserve
   /// compatibility with previously stored vakues.</remarks>
   public enum AnimatableProperty : byte
   {
      IsHidden,
      IsFrozen,
      BoxMode,
      XRayMtl,
      Renderable
   }
}
