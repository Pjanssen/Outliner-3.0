using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using SettingsCategory = System.Collections.Generic.Dictionary<string, object>;

namespace Outliner.Configuration
{
/// <summary>
/// The SettingsCollection is a generic class which holds typed values that can be 
/// retrieved using a category and a key string. The collection implements IXmlSerializable, 
/// to save the collection in an xml file.
/// </summary>
public class SettingsCollection : IXmlSerializable
{
   private Dictionary<String, SettingsCategory> categories;

   /// <summary>
   /// Initializes a new instance of the SettingsCollection class.
   /// </summary>
   public SettingsCollection()
   {
      this.categories = new Dictionary<string, SettingsCategory>();
   }

   private SettingsCategory GetCategory(String category)
   {
      SettingsCategory settings = null;
      if (this.categories.TryGetValue(category, out settings))
         return settings;
      else
         return null;
   }


   /// <summary>
   /// Retrieves all categories in this SettingsCollection.
   /// </summary>
   public IEnumerable<String> GetCategories()
   {
      return this.categories.Keys;
   }

   /// <summary>
   /// Retrieves all keys in the given category in this SettingsCollection.
   /// </summary>
   public IEnumerable<String> GetKeys(String category)
   {
      Throw.IfArgumentIsNull(category, "category");

      SettingsCategory settings = null;
      if (this.categories.TryGetValue(category, out settings))
         return settings.Keys;
      else
         return new List<String>();
   }


   /// <summary>
   /// Tests whether this SettingsCollection contains a value in the given category and key.
   /// </summary>
   public Boolean ContainsValue(String category, String key)
   {
      SettingsCategory settings = this.GetCategory(category);
      if (settings != null)
         return settings.ContainsKey(key);
      else
         return false;
   }


   /// <summary>
   /// Retrieves the value of a setting.
   /// </summary>
   /// <typeparam name="T">The type of value to retrieve.</typeparam>
   /// <param name="category">The category of the setting.</param>
   /// <param name="key">The name of the setting.</param>
   /// <exception cref="ArgumentNullException"></exception>
   /// <exception cref="InvalidOperationException"></exception>
   public T GetValue<T>(String category, String key)
   {
      return GetValue<T>(category, key, default(T));
   }

   /// <summary>
   /// Retrieves the value of a setting.
   /// </summary>
   /// <typeparam name="T">The type of value to retrieve.</typeparam>
   /// <param name="category">The category of the setting.</param>
   /// <param name="key">The name of the setting.</param>
   /// <param name="defaultValue">The default value to return when the key could not be found.</param>
   /// <exception cref="ArgumentNullException"></exception>
   /// <exception cref="InvalidOperationException"></exception>
   public T GetValue<T>(String category, String key, T defaultValue)
   {
      Throw.IfArgumentIsNull(category, "category");
      Throw.IfArgumentIsNull(key, "key");

      if (!this.ContainsValue(category, key))
         return defaultValue;
      else
      {
         Object value = this.categories[category][key];

         if (value is T)
            return (T)value;

         if (value is String)
         {
            String stringValue = this.categories[category][key] as String;
            TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(T));
            if (typeConverter != null && typeConverter.CanConvertFrom(typeof(String)))
               return (T)typeConverter.ConvertFromString(stringValue);
         }
      }

      throw new InvalidOperationException("Cannot convert value into requested type");
   }


   /// <summary>
   /// Attempts to retrieve the value of a setting.
   /// </summary>
   /// <typeparam name="T">The type of the setting value.</typeparam>
   /// <param name="category">The category of the setting.</param>
   /// <param name="key">The setting name.</param>
   /// <param name="value">The object in which to store the setting value.</param>
   public Boolean TryGetValue<T>(String category, String key, out T value) 
   {
      Throw.IfArgumentIsNull(category, "category");
      Throw.IfArgumentIsNull(key, "key");

      SettingsCategory settings = this.GetCategory(category);
      if (settings != null)
      {
         Object retrievedValue = null;
         if (settings.TryGetValue(key, out retrievedValue))
         {
            if (retrievedValue is T)
            {
               value = (T)retrievedValue;
               return true;
            }

            if (retrievedValue is String)
            {
               String stringValue = this.categories[category][key] as String;
               TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(T));
               if (typeConverter != null && typeConverter.CanConvertFrom(typeof(String)))
               {
                  value = (T)typeConverter.ConvertFromString(stringValue);
                  return true;
               }
            }
         }
      }

      value = default(T);
      return false;
   }


   /// <summary>
   /// Sets the value of a setting.
   /// </summary>
   /// <typeparam name="T">The type of the setting value.</typeparam>
   /// <param name="category">The category of the setting.</param>
   /// <param name="key">The name of the setting.</param>
   /// <param name="value">The (new) setting value.</param>
   public void SetValue<T>(String category, String key, T value)
   {
      Throw.IfArgumentIsNull(category, "category");
      Throw.IfArgumentIsNull(key, "key");
      Throw.IfArgumentIsNull(value, "value");

      SettingsCategory settings = this.GetCategory(category);
      if (settings == null)
      {
         settings = new SettingsCategory();
         this.categories.Add(category, settings);
      }
      settings.Remove(key);
      settings.Add(key, value);
   }


   /// <summary>
   /// Removes a setting from this SettingsCollection.
   /// </summary>
   public void RemoveKey(String category, String key)
   {
      Throw.IfArgumentIsNull(category, "category");
      Throw.IfArgumentIsNull(key, "key");

      SettingsCategory settings = this.GetCategory(category);
      if (settings != null)
      {
         settings.Remove(key);
         if (settings.Count == 0)
            this.categories.Remove(category);
      }
   }



   #region Xml Serialization
      
   public System.Xml.Schema.XmlSchema GetSchema()
   {
      return null;
   }

   public void ReadXml(System.Xml.XmlReader reader)
   {
      bool wasEmpty = reader.IsEmptyElement;
      reader.Read();

      if (wasEmpty)
         return;

      while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
      {
         if (reader.Name == "category")
         {
            String categoryName = reader.GetAttribute("name");
            reader.ReadStartElement();

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
               if (reader.Name == "setting")
               {
                  String settingName = reader.GetAttribute("name");

                  reader.ReadStartElement();
                  String settingValue = reader.ReadContentAsString();

                  this.SetValue<String>(categoryName, settingName, settingValue);

                  reader.ReadEndElement();
               }
            }

            reader.ReadEndElement();

            reader.MoveToContent();
         }
      }

      reader.ReadEndElement();
   }

   public void WriteXml(System.Xml.XmlWriter writer)
   {
      foreach (KeyValuePair<String, SettingsCategory> category in this.categories)
      {
         if (category.Value.Count > 0)
         {
            writer.WriteStartElement("category");
            writer.WriteAttributeString("name", category.Key);

            foreach (KeyValuePair<String, Object> setting in category.Value)
            {
               writer.WriteStartElement("setting");
               writer.WriteAttributeString("name", setting.Key);

               TypeConverter typeConverter = TypeDescriptor.GetConverter(setting.Value.GetType());
               if (typeConverter != null && typeConverter.CanConvertTo(typeof(String)))
                  writer.WriteValue(typeConverter.ConvertToString(setting.Value));
               
               writer.WriteEndElement();
            }

            writer.WriteEndElement();
         }
      }
   }

   #endregion
}
}
