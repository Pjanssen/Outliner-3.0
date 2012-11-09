using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Outliner.Filters;
using Outliner.MaxUtils;

namespace Outliner.Tests.Filters
{
[TestClass]
public class FilterCombinatorTest
{
   private Boolean filterChangedEventFired;

   [TestInitialize]
   public void TestInit()
   {
      this.filterChangedEventFired = false;
   }

   void combinator_FilterChanged(object sender, EventArgs e)
   {
      this.filterChangedEventFired = true;
   }

   [TestMethod]
   public void ConstructorTest()
   {
      FilterCombinator<Boolean> combinator = new FilterCombinator<bool>();
      Assert.IsNotNull(combinator.Filters, "Initial Filters value");
      Assert.AreEqual(0, combinator.Filters.Count, "Initial Filters count");
      Assert.AreEqual(Functor.Or, combinator.Predicate, "Initial Predicate.");
   }

   [TestMethod]
   public void SetPredicateTest()
   {
      FilterCombinator<Boolean> combinator = new FilterCombinator<bool>();
      combinator.FilterChanged += new EventHandler(combinator_FilterChanged);

      combinator.Predicate = Functor.And;
      Assert.AreEqual(Functor.And, combinator.Predicate, "Setting Predicate to And should work as expected.");
      Assert.IsTrue(this.filterChangedEventFired, "Setting Predicate should fire FilterChanged event. 1");
      this.filterChangedEventFired = false;

      combinator.Predicate = Functor.Or;
      Assert.AreEqual(Functor.Or, combinator.Predicate, "Setting Predicate to Or should work as expected.");
      Assert.IsTrue(this.filterChangedEventFired, "Setting Predicate should fire FilterChanged event. 2");
   }

   [TestMethod]
   public void EmptyTest()
   {
      FilterCombinator<Boolean> combinator = new FilterCombinator<bool>();
      Assert.AreEqual(0, combinator.Filters.Count, "Initial Filters count");
      Assert.IsTrue(combinator.ShowNode(true), "Empty combinator should return true for ShowNode(true).");
      Assert.IsTrue(combinator.ShowNode(false), "Empty combinator should return true for ShowNode(false).");
   }

   [TestMethod]
   public void PredicateTest()
   {
      FilterCombinator<bool> combinator = new FilterCombinator<bool>();

      combinator.Filters.Add(new MockFilterTrue());
      combinator.Filters.Add(new MockFilterFalse());

      combinator.Predicate = Functor.Or;
      Assert.IsTrue(combinator.ShowNode(true), "Or predicate should return true for ShowNode(true)");
      Assert.IsTrue(combinator.ShowNode(false), "Or predicate should return true for ShowNode(false)");

      combinator.Predicate = Functor.And;
      Assert.IsFalse(combinator.ShowNode(true), "And predicate should return false for ShowNode(true)");
      Assert.IsFalse(combinator.ShowNode(false), "And predicate should return false for ShowNode(false)");

      FilterCombinator<Boolean> f = new FilterCombinator<bool>(Functor.Or);
      f.Filters.Add(new MockFilterTrue());
      f.Filters.Add(new MockFilterFalse());

      combinator.Filters.Clear();
      combinator.Filters.Add(f);
      combinator.Filters.Add(new MockFilterTrue());
      combinator.Predicate = Functor.And;

      Assert.IsTrue(combinator.ShowNode(true));
      Assert.IsFalse(combinator.ShowNode(false));
   }
}
}
