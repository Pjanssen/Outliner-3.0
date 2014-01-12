using PJanssen.Outliner.Filters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PJanssen.Outliner.Scene;
using System;
using Moq;

namespace PJanssen.Outliner.Tests.Filters
{
/// <summary>
///This is a test class for NameFilterTest and is intended
///to contain all NameFilterTest Unit Tests
///</summary>
[TestClass()]
public class NameFilterTest
{
   private IMaxNode CreateNodeWithName(String name)
   {
      Mock<IMaxNode> node = new Mock<IMaxNode>();
      node.SetupGet(n => n.Name).Returns(name);

      return node.Object;
   }

   private void AssertActionRaisesFilterChangedEvent(Filter<IMaxNode> filter, Action action)
   {
      int filterChangedRaisedCount = 0;
      filter.FilterChanged += (sender, args) => filterChangedRaisedCount++;

      action();

      Assert.AreEqual(1, filterChangedRaisedCount);
   }

   [TestMethod()]
   public void Constructor_SetsDefaultValues() 
   {
      NameFilter filter = new NameFilter();
      Assert.AreEqual(String.Empty, filter.SearchString);
      Assert.AreEqual(false, filter.CaseSensitive);
   }

   [TestMethod]
   public void Enabled_Get_ReturnsSearchStringNotEmpty()
   {
      NameFilter filter = new NameFilter();
      Assert.IsFalse(filter.Enabled);

      filter.SearchString = "Test";
      Assert.IsTrue(filter.Enabled);
   }

   [TestMethod]
   public void SearchString_Set_RaisesFilterChangedEvent()
   {
      NameFilter filter = new NameFilter();
      AssertActionRaisesFilterChangedEvent(filter, () => filter.SearchString = "Test");
   }

   [TestMethod()]
   public void SearchString_Set_ChangesSearchString() 
   {
      NameFilter filter = new NameFilter();
      filter.SearchString = "";
      Assert.AreEqual("", filter.SearchString);

      filter.SearchString = "t";
      Assert.AreEqual("t", filter.SearchString);

      filter.SearchString = "*test";
      Assert.AreEqual("*test", filter.SearchString);

      filter.SearchString = "";
      Assert.AreEqual("", filter.SearchString);
   }

   [TestMethod]
   public void SearchString_Set_FiltersNodeByName()
   {
      NameFilter filter = new NameFilter();
      IMaxNode node = CreateNodeWithName("Test");

      filter.SearchString = "";
      Assert.IsTrue(filter.ShowNode(node));

      filter.SearchString = "T";
      Assert.IsTrue(filter.ShowNode(node));

      filter.SearchString = "Test";
      Assert.IsTrue(filter.ShowNode(node));

      filter.SearchString = "TestX";
      Assert.IsFalse(filter.ShowNode(node));

      filter.SearchString = "XTest";
      Assert.IsFalse(filter.ShowNode(node));
   }

   [TestMethod]
   public void SearchString_UsingAsterisk_WorksAsWildcard()
   {
      NameFilter filter = new NameFilter();
      IMaxNode node = CreateNodeWithName("LoremIpsum");

      filter.SearchString = "ipsum";
      Assert.IsFalse(filter.ShowNode(node));

      filter.SearchString = "*ipsum";
      Assert.IsTrue(filter.ShowNode(node));

      filter.SearchString = "Lor*sum";
      Assert.IsTrue(filter.ShowNode(node));
   }

   [TestMethod]
   public void SearchString_UsingAsterisk_WorksCombinedWithUseWildcard()
   {
      NameFilter filter = new NameFilter();
      filter.UseWildcard = true;
      IMaxNode node = CreateNodeWithName("LoremIpsum");

      filter.SearchString = "rem*sum";
      Assert.IsTrue(filter.ShowNode(node));

      filter.SearchString = "*rem*sum";
      Assert.IsTrue(filter.ShowNode(node));
   }

   [TestMethod]
   public void CaseSensitive_Set_RaisesFilterChangedEvent()
   {
      NameFilter filter = new NameFilter();
      AssertActionRaisesFilterChangedEvent(filter, () => filter.CaseSensitive = true);
   }

   [TestMethod()]
   public void CaseSensitive_SetTrue_MakesSearchCaseSensitive()
   {
      NameFilter filter = new NameFilter();
      IMaxNode node = CreateNodeWithName("Test");

      filter.SearchString = "test";
      Assert.IsTrue(filter.ShowNode(node));

      filter.CaseSensitive = true;

      Assert.IsFalse(filter.ShowNode(node));
   }

   [TestMethod]
   public void CaseSensitive_SetFalse_MakesSearchCaseInsensitive()
   {
      NameFilter filter = new NameFilter();
      filter.CaseSensitive = false;

      IMaxNode node = CreateNodeWithName("Test");

      filter.SearchString = "Test";
      Assert.IsTrue(filter.ShowNode(node));

      filter.SearchString = "test";
      Assert.IsTrue(filter.ShowNode(node));
   }

   [TestMethod]
   public void UseWildcard_Set_RaisesFilterChangedEvent()
   {
      NameFilter filter = new NameFilter();
      AssertActionRaisesFilterChangedEvent(filter, () => filter.UseWildcard = true);
   }

   [TestMethod]
   public void UseWildcard_SetTrue_ShowsNodesWithNameContainingSearchString()
   {
      NameFilter filter = new NameFilter();
      filter.UseWildcard = true;

      IMaxNode node = CreateNodeWithName("Test");

      filter.SearchString = "T";
      Assert.IsTrue(filter.ShowNode(node));

      filter.SearchString = "est";
      Assert.IsTrue(filter.ShowNode(node));

      filter.SearchString = "st";
      Assert.IsTrue(filter.ShowNode(node));

      filter.SearchString = "s";
      Assert.IsTrue(filter.ShowNode(node));
   }
}
}
