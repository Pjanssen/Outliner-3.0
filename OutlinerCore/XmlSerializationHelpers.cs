using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Outliner.Plugins;
using System.Xml.Serialization;

namespace Outliner
{
/// <summary>
/// A collection of xml serialization helper methods, to avoid some boilerplate code.
/// </summary>
public static class XmlSerializationHelpers
{
   public static T FromXml<T>(String filePath) where T : class
   {
      using (FileStream stream = new FileStream(filePath, FileMode.Open))
      {
         return FromXml<T>(stream);
      }
   }

   public static T FromXml<T>(Stream stream) where T : class
   {
      Type[] extraTypes = OutlinerPlugins.GetSerializableTypes();
      XmlSerializer serializer = new XmlSerializer(typeof(T), extraTypes);
      return serializer.Deserialize(stream) as T;
   }

   public static void ToXml<T>(String FilePath, T data)
   {
      using (FileStream stream = new FileStream(FilePath, FileMode.Create))
      {
         XmlSerializationHelpers.ToXml<T>(stream, data);
      }
   }

   public static void ToXml<T>(Stream stream, T data)
   {
      Type[] extraTypes = OutlinerPlugins.GetSerializableTypes();
      XmlSerializer serializer = new XmlSerializer(typeof(T), extraTypes);
      serializer.Serialize(stream, data);
   }
}
}
