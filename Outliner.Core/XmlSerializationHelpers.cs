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
public static class XmlSerializationHelpers<T> where T : class
{
   public static T FromXml(String filePath)
   {
      return FromXml(filePath, new Type[0]);
   }

   public static T FromXml(String filePath, Type[] extraTypes)
   {
      using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
      {
         return FromXml(stream, extraTypes);
      }
   }

   public static T FromXml(Stream stream, Type[] extraTypes)
   {
      XmlSerializer serializer = new XmlSerializer(typeof(T), extraTypes);
      return serializer.Deserialize(stream) as T;
   }


   public static void ToXml(String filePath, T data)
   {
      ToXml(filePath, new Type[0], data);
   }

   public static void ToXml(String filePath, Type[] extraTypes, T data)
   {
      using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
      {
         ToXml(stream, extraTypes, data);
      }
   }

   public static void ToXml(Stream stream, Type[] extraTypes, T data)
   {
      XmlSerializer serializer = new XmlSerializer(typeof(T), extraTypes);
      serializer.Serialize(stream, data);
   }
}
}
