using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Outliner
{
internal static class ExceptionHelper
{
   /// <summary>
   /// Tests if an object is null, and throws an ArgumentNullException if it is.
   /// </summary>
   /// <param name="argument">The argument object to test.</param>
   /// <param name="name">The name of the argument in the called function.</param>
   internal static void ThrowIfArgumentIsNull(Object argument, String name)
   {
      if (argument == null)
         throw new ArgumentNullException(name);
   }
}
}
