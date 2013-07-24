using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Outliner.Filters;

namespace Outliner.Tests.Filters
{
   [TestClass]
   public class FilterCollectionTests
   {
      [TestMethod]
      public void Constructor_NoParams_InitializesEmptyCollection()
      {
         FilterCollection<int> filters = new FilterCollection<int>();
         Assert.IsTrue(filters.IsEmpty());
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentNullException))]
      public void Constructor_CollectionNull_ThrowsException()
      {
         new FilterCollection<int>(null);
      }

      [TestMethod]
      public void Constructor_Collection_AddsFilters()
      {
         FilterCollection<int> filters = new FilterCollection<int>();
         Filter<int> filter = new Mock<Filter<int>>().Object;
         filters.Add(filter);

         FilterCollection<int> filtersCopy = new FilterCollection<int>(filters);
         Assert.AreEqual(1, filtersCopy.Count);
         Assert.IsTrue(filtersCopy.Contains(filter));
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentNullException))]
      public void Add_Null_ThrowsException()
      {
         FilterCollection<int> filters = new FilterCollection<int>();

         filters.Add(null);
      }

      [TestMethod]
      public void Add_Filter_RaisesFilterAddedEvent()
      {
         int filterAddedEventRaised = 0;
         FilterCollection<int> filters = new FilterCollection<int>();
         filters.FilterAdded += (sender, args) => filterAddedEventRaised++;

         filters.Add(new Mock<Filter<int>>().Object);

         Assert.AreEqual(1, filterAddedEventRaised);
      }

      [TestMethod]
      public void Add_FilterTwice_OnlyAddsOnce()
      {
         int filterAddedEventRaised = 0;
         FilterCollection<int> filters = new FilterCollection<int>();
         filters.FilterAdded += (sender, args) => filterAddedEventRaised++;
         Filter<int> filter = new Mock<Filter<int>>().Object;

         filters.Add(filter);
         filters.Add(filter);

         Assert.AreEqual(1, filters.Count);
         Assert.IsTrue(filters.Contains(filter));
         Assert.AreEqual(1, filterAddedEventRaised);
      }

      [TestMethod]
      public void Clear_ClearsCollection()
      {
         FilterCollection<int> filters = new FilterCollection<int>();
         Filter<int> filter = new Mock<Filter<int>>().Object;

         filters.Clear();

         Assert.IsTrue(filters.IsEmpty());
         Assert.IsFalse(filters.Contains(filter));
      }

      [TestMethod]
      public void Clear_RaisesFiltersClearedEvent()
      {
         int filtersClearedEventRaised = 0;
         FilterCollection<int> filters = new FilterCollection<int>();
         filters.FiltersCleared += (sender, args) => filtersClearedEventRaised++;

         filters.Clear();

         Assert.AreEqual(1, filtersClearedEventRaised);
      }

      [TestMethod]
      public void Contains_Type_ReturnsFiltersOfType()
      {
         FilterCollection<int> filters = new FilterCollection<int>();
         Filter<int> filterInt = new Mock<Filter<int>>().Object;
         Filter<double> filterDouble = new Mock<Filter<double>>().Object;
         filters.Add(filterInt);

         Assert.IsTrue(filters.Contains(filterInt.GetType()));
         Assert.IsFalse(filters.Contains(filterDouble.GetType()));
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentNullException))]
      public void Remove_Null_ThrowsException()
      {
         FilterCollection<int> filters = new FilterCollection<int>();

         Filter<int> filter = null;
         filters.Remove(filter);
      }

      [TestMethod]
      public void Remove_Filter_RaisesFilterRemovedEvent()
      {
         int filtersRemovedRaised = 0;
         FilterCollection<int> filters = new FilterCollection<int>();
         filters.FilterRemoved += (sender, args) => filtersRemovedRaised++;
         Filter<int> filter = new Mock<Filter<int>>().Object;
         filters.Add(filter);

         filters.Remove(filter);

         Assert.AreEqual(1, filtersRemovedRaised);
      }

      [TestMethod]
      public void Remove_Filter()
      {
         FilterCollection<int> filters = new FilterCollection<int>();
         Filter<int> filter = new Mock<Filter<int>>().Object;
         filters.Add(filter);

         filters.Remove(filter);
         Assert.IsFalse(filters.Contains(filter));
      }

      [TestMethod]
      public void Remove_ByType()
      {
         FilterCollection<int> filters = new FilterCollection<int>();
         Filter<int> filter = new Mock<Filter<int>>().Object;
         filters.Add(filter);

         filters.Remove(filter.GetType());

         Assert.IsFalse(filters.Contains(filter));
      }

      [TestMethod]
      public void Get_ByType()
      {
         FilterCollection<int> filters = new FilterCollection<int>();
         Filter<int> filter = new Mock<Filter<int>>().Object;
         filters.Add(filter);

         Assert.AreEqual(filter, filters.Get(filter.GetType()));
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentOutOfRangeException))]
      public void IndexOperator_GetIndexLargerThanSize_ThrowsException()
      {
         FilterCollection<int> filters = new FilterCollection<int>();
         Filter<int> filter = filters[0];
      }

      [TestMethod]
      public void IndexOperator_Get_ReturnsFilterAtIndex()
      {
         FilterCollection<int> filters = new FilterCollection<int>();
         Filter<int> filterA = new Mock<Filter<int>>().Object;
         Filter<int> filterB = new Mock<Filter<int>>().Object;
         filters.Add(filterA);
         filters.Add(filterB);

         Assert.AreEqual(filterA, filters[0]);
         Assert.AreEqual(filterB, filters[1]);
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentOutOfRangeException))]
      public void IndexOperator_SetIndexLargerThanSize_ThrowsException()
      {
         FilterCollection<int> filters = new FilterCollection<int>();
         filters[0] = new Mock<Filter<int>>().Object;
      }

      [TestMethod]
      public void IndexOperator_Set_ReplacesFilter()
      {
         FilterCollection<int> filters = new FilterCollection<int>();
         Filter<int> filterA = new Mock<Filter<int>>().Object;
         Filter<int> filterB = new Mock<Filter<int>>().Object;
         filters.Add(filterA);

         filters[0] = filterB;

         Assert.IsFalse(filters.Contains(filterA));
         Assert.IsTrue(filters.Contains(filterB));
         Assert.AreEqual(filterB, filters[0]);
      }

      [TestMethod]
      public void IndexOperator_Set_RaisesFilterRemovedAndFilterAddedEvent()
      {
         int filterRemovedRaised = 0;
         int filterAddedRaised = 0;
         FilterCollection<int> filters = new FilterCollection<int>();
         Filter<int> filterA = new Mock<Filter<int>>().Object;
         Filter<int> filterB = new Mock<Filter<int>>().Object;
         filters.Add(filterA);
         filters.FilterRemoved += (sender, args) => filterRemovedRaised++;
         filters.FilterAdded += (sender, args) => filterAddedRaised++;

         filters[0] = filterB;

         Assert.AreEqual(1, filterRemovedRaised);
         Assert.AreEqual(1, filterAddedRaised);
      }
   }
}
