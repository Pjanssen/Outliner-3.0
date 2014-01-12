using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PJanssen.Outliner.Filters;
using PJanssen.Outliner.MaxUtils;
using Moq;

namespace PJanssen.Outliner.Tests.Filters
{
[TestClass]
public class FilterCombinatorTest
{
   [TestMethod]
   public void Constructor_SetsDefaultValues()
   {
      FilterCombinator<Boolean> combinator = new FilterCombinator<Boolean>();
      Assert.IsNotNull(combinator.Filters, "Initial Filters value");
      Assert.AreEqual(0, combinator.Filters.Count, "Initial Filters count");
      Assert.AreEqual(Functor.Or, combinator.Predicate, "Initial Predicate.");
   }

   [TestMethod]
   public void Predicate_SetNewValue_RaisesFilterChangedEvent()
   {
      Boolean filterChangedRaised = false;
      FilterCombinator<Boolean> combinator = new FilterCombinator<Boolean>();
      combinator.FilterChanged += (sender, args) => filterChangedRaised = true;

      combinator.Predicate = Functor.And;

      Assert.IsTrue(filterChangedRaised);
   }

   [TestMethod]
   public void Predicate_SetNewValue_ChangesPredicateValue()
   {
      FilterCombinator<Boolean> combinator = new FilterCombinator<Boolean>();

      combinator.Predicate = Functor.And;

      Assert.AreEqual(Functor.And, combinator.Predicate);

      combinator.Predicate = Functor.Or;

      Assert.AreEqual(Functor.Or, combinator.Predicate);
   }

   [TestMethod]
   public void AddFilter_RaisesFilterChangedEvent()
   {
      Boolean filterChangedEventRaised = false;
      FilterCombinator<Boolean> combinator = new FilterCombinator<bool>();
      combinator.FilterChanged += (sender, args) => filterChangedEventRaised = true;

      combinator.Filters.Add(new Mock<Filter<Boolean>>().Object);

      Assert.IsTrue(filterChangedEventRaised);
   }

   [TestMethod]
   public void RemoveFilter_RaisesFilterChangedEvent()
   {
      Boolean filterChangedEventRaised = false;
      FilterCombinator<Boolean> combinator = new FilterCombinator<bool>();
      Mock<Filter<Boolean>> filter = new Mock<Filter<Boolean>>();
      combinator.Filters.Add(filter.Object);
      combinator.FilterChanged += (sender, args) => filterChangedEventRaised = true;
      
      combinator.Filters.Remove(filter.Object.GetType());

      Assert.IsTrue(filterChangedEventRaised);
   }

   [TestMethod]
   public void RaiseChildFilterChanged_RaisesFilterChangedEvent()
   {
      Boolean filterChangedEventRaised = false;
      FilterCombinator<Boolean> combinator = new FilterCombinator<bool>();
      Mock<Filter<Boolean>> filter = new Mock<Filter<Boolean>>();
      combinator.Filters.Add(filter.Object);
      combinator.FilterChanged += (sender, args) => filterChangedEventRaised = true;

      filter.Raise(f => f.FilterChanged += null, EventArgs.Empty);

      Assert.IsTrue(filterChangedEventRaised);
   }

   [TestMethod]
   public void ShowNode_EmptyFilterSet_AlwaysReturnsTrue()
   {
      FilterCombinator<Boolean> combinator = new FilterCombinator<Boolean>();
      Assert.AreEqual(0, combinator.Filters.Count, "Initial Filters count");

      Assert.IsTrue(combinator.ShowNode(true), "Empty combinator should return true for ShowNode(true).");
      Assert.IsTrue(combinator.ShowNode(false), "Empty combinator should return true for ShowNode(false).");
   }

   [TestMethod]
   public void ShowNode_OrPredicate_ORsFilterResults()
   {
      FilterCombinator<Boolean> combinator = new FilterCombinator<Boolean>();
      combinator.Predicate = Functor.Or;

      Mock<Filter<Boolean>> filterA = new Mock<Filter<Boolean>>();
      filterA.Setup(f => f.ShowNode(It.IsAny<Boolean>())).Returns(true);
      combinator.Filters.Add(filterA.Object);

      Mock<Filter<Boolean>> filterB = new Mock<Filter<Boolean>>();
      filterB.Setup(f => f.ShowNode(It.IsAny<Boolean>())).Returns(false);
      combinator.Filters.Add(filterB.Object);

      Assert.IsTrue(combinator.ShowNode(true));
      Assert.IsTrue(combinator.ShowNode(false));
      filterA.Verify(f => f.ShowNode(It.IsAny<Boolean>()), Times.AtLeastOnce);
      filterB.Verify(f => f.ShowNode(It.IsAny<Boolean>()), Times.AtLeastOnce);
   }

   [TestMethod]
   public void ShowNode_AndPredicate_ANDsFilterResults()
   {
      FilterCombinator<Boolean> combinator = new FilterCombinator<Boolean>();
      combinator.Predicate = Functor.And;

      Mock<Filter<Boolean>> filterA = new Mock<Filter<Boolean>>();
      filterA.Setup(f => f.ShowNode(It.IsAny<Boolean>())).Returns(true);
      combinator.Filters.Add(filterA.Object);

      Mock<Filter<Boolean>> filterB = new Mock<Filter<Boolean>>();
      filterB.Setup(f => f.ShowNode(It.IsAny<Boolean>())).Returns(false);
      combinator.Filters.Add(filterB.Object);

      Assert.IsFalse(combinator.ShowNode(true));
      Assert.IsFalse(combinator.ShowNode(false));
      filterA.Verify(f => f.ShowNode(It.IsAny<Boolean>()), Times.AtLeastOnce);
      filterB.Verify(f => f.ShowNode(It.IsAny<Boolean>()), Times.AtLeastOnce);
   }
}
}
