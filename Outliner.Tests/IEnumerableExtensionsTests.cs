using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Outliner.Tests
{
   [TestClass]
   public class IEnumerableExtensions
   {
      #region ToIEnumerable
      
      [TestMethod]
      [ExpectedException(typeof(ArgumentNullException))]
      public void ToIEnumerable_ParamNull_ThrowsException()
      {
         object x = null;
         x.ToIEnumerable().ToList();
      }

      [TestMethod]
      public void ToIEnumerable()
      {
         int x = 42;
         IEnumerable<int> enumerable = x.ToIEnumerable();

         Assert.AreEqual(1, enumerable.Count());
         Assert.AreEqual(42, enumerable.First());
      }

      #endregion

      #region DropLast

      [TestMethod]
      [ExpectedException(typeof(ArgumentNullException))]
      public void DropLast_ParamNull_ThrowsException()
      {
         IEnumerable<int> x = null;
         x.DropLast(0).ToList();
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentOutOfRangeException))]
      public void DropLast_LessThanZero_ThrowsException()
      {
         IEnumerable<int> ints = Enumerable.Range(0, 10);
         ints.DropLast(-1).ToList();
      }

      [TestMethod]
      public void DropLast_DropOne_ReturnsSubSet()
      {
         IEnumerable<int> ints = Enumerable.Range(0, 10);
         List<int> result = ints.DropLast(1).ToList();

         Assert.AreEqual(9, result.Count);
         CollectionAssert.AreEqual(result, Enumerable.Range(0, 9).ToList());
      }

      [TestMethod]
      public void DropLast_MoreThanCount_ReturnsEmptySet()
      {
         IEnumerable<int> ints = Enumerable.Range(0, 10);
         List<int> result = ints.DropLast(11).ToList();

         Assert.AreEqual(0, result.Count);
      }

      #endregion

      #region ForEach

      [TestMethod]
      [ExpectedException(typeof(ArgumentNullException))]
      public void ForEach_Param1Null_ThrowsException()
      {
         IEnumerable<int> x = null;
         x.ForEach(y => { });
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentNullException))]
      public void ForEach_Param2Null_ThrowsException()
      {
         IEnumerable<int> ints = Enumerable.Range(0, 10);
         ints.ForEach(null);
      }

      [TestMethod]
      public void ForEach_SimpleAction_ExecutesActionOnEachElement()
      {
         IEnumerable<int> ints = Enumerable.Range(0, 10);
         List<int> result = new List<int>();
         ints.ForEach(y => result.Add(y));

         CollectionAssert.AreEqual(result, ints.ToList());
      }

      #endregion

      #region Map

      [TestMethod]
      [ExpectedException(typeof(ArgumentNullException))]
      public void Map_Param1Null_ThrowsException()
      {
         IEnumerable<int> ints = null;
         ints.Map(y => { }).ToList();
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentNullException))]
      public void Map_Param2Null_ThrowsException()
      {
         IEnumerable<int> ints = Enumerable.Range(0, 10);
         ints.Map(null).ToList();
      }

      [TestMethod]
      public void Map_SimpleAction_ExecutesActionOnEachElement()
      {
         IEnumerable<int> ints = Enumerable.Range(0, 10);
         List<int> resultA = new List<int>();
         List<int> resultB = ints.Map(y => resultA.Add(y)).ToList();

         CollectionAssert.AreEqual(resultA, ints.ToList());
         CollectionAssert.AreEqual(resultB, ints.ToList());
      }

      #endregion

      #region IsEmpty

      [TestMethod]
      [ExpectedException(typeof(ArgumentNullException))]
      public void IsEmpty_ParamNull_ThrowsException()
      {
         IEnumerable<int> x = null;
         x.IsEmpty();
      }

      [TestMethod]
      public void IsEmpty()
      {
         IEnumerable<int> emptyEnumerable = Enumerable.Empty<int>();
         Assert.IsTrue(emptyEnumerable.IsEmpty());

         IEnumerable<int> nonEmptyEnumerable = new List<int>() { 1 };
         Assert.IsFalse(nonEmptyEnumerable.IsEmpty());
      }

      #endregion
   }
}
