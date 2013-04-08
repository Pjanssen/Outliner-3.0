using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Outliner
{
/// <summary>
/// Provides helper methods for exception throwing.
/// </summary>
public static class Throw
{
   /// <summary>
   /// Tests if an object argument is null.
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
   /// Tests if a string argument is null or empty.
   /// </summary>
   /// <param name="str">The string argument to test.</param>
   /// <param name="name">The name of the argument</param>
   /// <exception cref="ArgumentNullException"></exception>
   public static void IfArgumentIsNullOrEmpty(String str, String name)
   {
      if (String.IsNullOrEmpty(str))
         throw new ArgumentNullException(name);
   }

   /// <summary>
   /// Tests if a string argument is null or empty.
   /// </summary>
   /// <param name="argument">The collection argument to test.</param>
   /// <param name="name">The name of the argument</param>
   /// <exception cref="ArgumentNullException"></exception>
   public static void IfArgumentIsNullOrEmpty([ValidatedNotNull] IEnumerable argument, String name)
   {
      Throw.IfArgumentIsNull(argument, name);
      
      if (!argument.GetEnumerator().MoveNext())
         throw new ArgumentNullException(name);
   }


   /// <summary>
   /// Tests if the given argument is smaller than a minimum value.
   /// </summary>
   /// <param name="argument">The argument to test.</param>
   /// <param name="minValue">The minimum valid value of the argument.</param>
   /// <param name="argumentName">The name of the argument being tested.</param>
   /// <exception cref="ArgumentOutOfRangeException" />
   public static void IfArgumentIsSmallerThan(IComparable argument, IComparable minValue, string argumentName)
   {
      Throw.IfNull(argument, "argument");
      Throw.IfNull(minValue, "minValue");

      if (argument.CompareTo(minValue) < 0)
         throw new ArgumentOutOfRangeException( argumentName
                                              , string.Format("Argument {0} can not be smaller than {1}", argumentName, minValue));
   }

   /// <summary>
   /// Tests if the given argument is larger than a minimum value.
   /// </summary>
   /// <param name="argument">The argument to test.</param>
   /// <param name="maxValue">The maximum valid value of the argument.</param>
   /// <param name="argumentName">The name of the argument being tested.</param>
   /// <exception cref="ArgumentOutOfRangeException" />
   public static void IfArgumentIsLargerThan(IComparable argument, IComparable maxValue, string argumentName)
   {
      Throw.IfNull(argument, "argument");
      Throw.IfNull(maxValue, "maxValue");

      if (argument.CompareTo(maxValue) > 0)
         throw new ArgumentOutOfRangeException( argumentName
                                              , string.Format("Argument {0} can not be larger than {1}", argumentName, maxValue));
   }


   /// <summary>
   /// Tests if an object is null.
   /// </summary>
   /// <param name="obj">The object to test.</param>
   /// <param name="message">The message for the exception if it is thrown.</param>
   /// <exception cref="InvalidOperationException"></exception>
   public static void IfNull(Object obj, String message)
   {
      if (obj == null)
         throw new InvalidOperationException(message);
   }

   /// <summary>
   /// Tests if a string is null or empty.
   /// </summary>
   /// <param name="str">The string to test.</param>
   /// <param name="varName">The name of the string variable.</param>
   /// <exception cref="InvalidOperationException"/>
   public static void IfNullOrEmpty(String str, String varName)
   {
      if (String.IsNullOrEmpty(str))
         throw new InvalidOperationException(varName + " cannot be null or empty");
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
internal sealed class ValidatedNotNullAttribute : Attribute { }
}
