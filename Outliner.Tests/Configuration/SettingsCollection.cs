﻿using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Outliner.Configuration;

namespace Outliner.Tests.Configuration
{
   [TestClass]
   public class SettingsCollectionTests
   {
      [TestMethod]
      [ExpectedException(typeof(ArgumentNullException))]
      public void GetKeysCategoryNullTest()
      {
         SettingsCollection settings = new SettingsCollection();
         settings.GetKeys(null);
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentNullException))]
      public void GetValueCategoryNullTest()
      {
         SettingsCollection settings = new SettingsCollection();
         settings.GetValue<Boolean>(null, "");
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentNullException))]
      public void GetValueKeyNullTest()
      {
         SettingsCollection settings = new SettingsCollection();
         settings.GetValue<Boolean>("", null);
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentNullException))]
      public void SetValueCategoryNullTest()
      {
         SettingsCollection settings = new SettingsCollection();
         settings.SetValue<Boolean>(null, "", false);
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentNullException))]
      public void SetValueKeyNullTest()
      {
         SettingsCollection settings = new SettingsCollection();
         settings.SetValue<Boolean>("", null, false);
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentNullException))]
      public void SetValueValueNullTest()
      {
         SettingsCollection settings = new SettingsCollection();
         settings.SetValue<String>("", "", null);
      }

      [TestMethod]
      public void GetSetValueTest()
      {
         SettingsCollection settings = new SettingsCollection();

         String category = "default";
         String key = "a";

         Assert.IsFalse(settings.GetCategories().Contains(category));

         settings.SetValue<Boolean>(category, key, false);

         Assert.IsTrue(settings.GetCategories().Contains(category));
         Assert.IsTrue(settings.GetKeys(category).Contains(key));
         Assert.AreEqual(false, settings.GetValue<Boolean>(category, key));

         settings.SetValue<Boolean>(category, key, true);
         Assert.AreEqual(true, settings.GetValue<Boolean>(category, key));
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentNullException))]
      public void TryGetValueNullCategoryTest()
      {
         SettingsCollection settings = new SettingsCollection();
         Boolean outValue;
         settings.TryGetValue<Boolean>(null, "", out outValue);
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentNullException))]
      public void TryGetValueNullKeyTest()
      {
         SettingsCollection settings = new SettingsCollection();
         Boolean outValue;
         settings.TryGetValue<Boolean>("", null, out outValue);
      }

      [TestMethod]
      public void TryGetValueTest()
      {
         SettingsCollection settings = new SettingsCollection();

         String category = "default";
         String key = "a";
         Boolean outValue;

         settings.SetValue<Boolean>(category, key, false);

         Assert.AreEqual(false, settings.GetValue<Boolean>(category, key));
         Assert.IsTrue(settings.TryGetValue<Boolean>(category, key, out outValue));
         Assert.AreEqual(false, outValue);

         Assert.IsFalse(settings.TryGetValue<Boolean>("", "", out outValue));

         String outString;
         Assert.IsFalse(settings.TryGetValue<String>(category, key, out outString));
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentNullException))]
      public void RemoveKeyCategoryNullTest()
      {
         SettingsCollection settings = new SettingsCollection();
         settings.RemoveKey(null, "");
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentNullException))]
      public void RemoveKeyKeyNullTest()
      {
         SettingsCollection settings = new SettingsCollection();
         settings.RemoveKey("", null);
      }
      
      [TestMethod]
      public void RemoveKeyTest()
      {
         SettingsCollection settings = new SettingsCollection();

         String category = "default";
         String key = "a";

         settings.SetValue<Boolean>(category, key, true);

         Assert.IsTrue(settings.GetCategories().Contains(category));
         Assert.AreEqual(true, settings.GetValue<Boolean>(category, key));

         settings.RemoveKey(category, key);

         Boolean outValue;
         Assert.IsFalse(settings.TryGetValue<Boolean>(category, key, out outValue));
         Assert.IsFalse(settings.GetCategories().Contains(category));
      }

      [TestMethod]
      public void SerializeTest()
      {
         SettingsCollection settings = new SettingsCollection();
         settings.SetValue<Boolean>("categoryA", "keyA", true);
         settings.SetValue<String>("categoryB", "keyA", "valueA");
         settings.SetValue<String>("categoryB", "keyB", "valueB");
         settings.SetValue<UriComponents>("categoryC", "keyA", UriComponents.Path | UriComponents.Port);
         
         String serializedSettings;
         SettingsCollection deserializedSettings;

         XmlSerializer serializer = new XmlSerializer(typeof(SettingsCollection));
         using (MemoryStream stream = new MemoryStream())
         {
            serializer.Serialize(stream, settings);
            serializedSettings = Encoding.UTF8.GetString(stream.GetBuffer());
         }

         Assert.IsNotNull(serializedSettings);

         using (StringReader reader = new StringReader(serializedSettings))
         {
            deserializedSettings = serializer.Deserialize(reader) as SettingsCollection;
         }

         Assert.IsNotNull(deserializedSettings);

         Assert.AreEqual(true, deserializedSettings.GetValue<Boolean>("categoryA", "keyA"));
         Assert.AreEqual("valueA", deserializedSettings.GetValue<String>("categoryB", "keyA"));
         Assert.AreEqual("valueB", deserializedSettings.GetValue<String>("categoryB", "keyB"));
         Assert.AreEqual(UriComponents.Path | UriComponents.Port, deserializedSettings.GetValue<UriComponents>("categoryC", "keyA"));
      }
   }
}