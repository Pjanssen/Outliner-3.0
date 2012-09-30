using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using Outliner.Plugins;
using System.Xml;

namespace Outliner
{
public static class DataContractHelpers
{
   public const String DefaultNamespace = "Outliner";

   public static T FromXml<T>(String filePath) where T : class
   {
      using (FileStream stream = new FileStream(filePath, FileMode.Open))
      {
         return DataContractHelpers.FromXml<T>(stream);
      }
   }

   public static T FromXml<T>(Stream stream) where T : class
   {
      IEnumerable<Type> knownTypes = OutlinerPlugins.GetSerializableTypes();
      DataContractSerializer serializer = new DataContractSerializer(typeof(T), knownTypes);
      return serializer.ReadObject(stream) as T;
   }

   public static void ToXml<T>(String path, T data)
   {
      XmlWriterSettings settings = new XmlWriterSettings();
      settings.Indent = true;

      using (XmlWriter writer = XmlWriter.Create(path, settings))
      {
         DataContractHelpers.ToXml<T>(writer, data);
      }
   }

   public static void ToXml<T>(XmlWriter writer, T data)
   {
      IEnumerable<Type> knownTypes = OutlinerPlugins.GetSerializableTypes();
      DataContractSerializer serializer = new DataContractSerializer(typeof(T), knownTypes);
      serializer.WriteObject(writer, data);
   }
}
}
