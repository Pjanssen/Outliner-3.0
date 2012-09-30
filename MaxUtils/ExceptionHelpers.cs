using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MaxUtils
{
public static class ExceptionHelpers
{
   /// <summary>
   /// Tests if an object is null, and throws an ArgumentNullException if it is.
   /// </summary>
   /// <param name="argument">The argument object to test.</param>
   /// <param name="name">The name of the argument in the called function.</param>
   public static void ThrowIfArgumentIsNull(Object argument, String name)
   {
      if (argument == null)
         throw new ArgumentNullException(name);
   }

   /// <summary>
   /// Tests if an object is null, and throws a NullReferenceException if it is.
   /// </summary>
   /// <param name="argument">The object to test.</param>
   /// <param name="message">The message for the exception if it is thrown.</param>
   public static void ThrowIfNull(Object obj, String message)
   {
      if (obj == null)
         throw new NullReferenceException(message);
   }
}
}
