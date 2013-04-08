﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Outliner
{
   /// <summary>
   /// Provides extension methods for IEnumerable objects.
   /// </summary>
   public static class IEnumerableExtensions
   {
      /// <summary>
      /// Returns an IEnumerable with a single element.
      /// </summary>
      public static IEnumerable<T> ToIEnumerable<T>(this T item)
      {
         Throw.IfArgumentIsNull(item, "item");

         yield return item;
      }

      /// <summary>
      /// Iterates over all elements in the collection with the supplied function.
      /// </summary>
      public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
      {
         Throw.IfArgumentIsNull(items, "items");
         Throw.IfArgumentIsNull(action, "action");

         foreach (T item in items)
            action(item);
      }

      /// <summary>
      /// Applies a function to all elements in the collection and returns the collection.
      /// NOTE: Only works when calling ToList() or similar after the operation!
      /// </summary>
      public static IEnumerable<T> Map<T>(this IEnumerable<T> items, Action<T> action)
      {
         Throw.IfArgumentIsNull(items, "items");
         Throw.IfArgumentIsNull(action, "action");

         foreach (T item in items)
         {
            action(item);
            yield return item;
         }
      }

      /// <summary>
      /// Drops the last n number of elements from the IEnumerable.
      /// </summary>
      public static IEnumerable<T> DropLast<T>(this IEnumerable<T> source, int n)
      {
         Throw.IfArgumentIsNull(source, "source");
         Throw.IfArgumentIsSmallerThan(n, 0, "n");

         Queue<T> buffer = new Queue<T>(n + 1);

         foreach (T x in source)
         {
            buffer.Enqueue(x);

            if (buffer.Count == n + 1)
               yield return buffer.Dequeue();
         }
      }

      /// <summary>
      /// Tests if the collection is empty.
      /// </summary>
      public static Boolean IsEmpty<T>(this IEnumerable<T> enumerable)
      {
         return !enumerable.Any();
      }
   }
}
