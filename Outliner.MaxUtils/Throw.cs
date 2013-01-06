using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Outliner
{
public static class Throw
{
   /// <summary>
   /// Tests if an object is null, and throws an ArgumentNullException if it is.
   /// </summary>
   /// <param name="argument">The argument object to test.</param>
   /// <param name="name">The name of the argument in the called function.</param>
   /// <exception cref="ArgumentNullException"></exception>
   public static void IfArgumentIsNull([ValidatedNotNull] Object argument, String name)
   {
      if (argument == null)
      {
         throw new ArgumentNullException(name);
      }
   }

   /// <summary>
   /// Tests if an object is null, and throws a NullReferenceException if it is.
   /// </summary>
   /// <param name="argument">The object to test.</param>
   /// <param name="message">The message for the exception if it is thrown.</param>
   /// <exception cref="NullReferenceException"></exception>
   public static void IfNull(Object obj, String message)
   {
      if (obj == null)
         throw new NullReferenceException(message);
   }

   /// <summary>
   /// Tests if an enumerable is empty, and throws an InvalidOperationException if it is.
   /// </summary>
   /// <param name="source">The IEnumerable to test.</param>
   /// <param name="message">The message for the exception if it is thrown.</param>
   /// <exception cref="InvalidOperationException"></exception>
   public static void IfEmpty<T>(IEnumerable<T> source, String message)
   {
      if (source.Count() == 0)
         throw new InvalidOperationException(message);
   }
}

[AttributeUsage(AttributeTargets.Parameter)]
public sealed class ValidatedNotNullAttribute : Attribute { }
}
