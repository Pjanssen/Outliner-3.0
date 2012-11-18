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
/// A collection of xml serialization helper methods. XmlSerializers are cached
/// to improve performance.
/// </summary>
public static class XmlSerializationHelpers
{
   private static Dictionary<Type, XmlSerializer> pluginSerializers;

   private static XmlSerializer CreateSerializer(Type t)
   {
      XmlSerializer serializer = new XmlSerializer(t, OutlinerPlugins.GetSerializableTypes());
      if (pluginSerializers == null)
         pluginSerializers = new Dictionary<Type, XmlSerializer>();

      if (!pluginSerializers.ContainsKey(t))
         pluginSerializers.Add(t, serializer);

      return serializer;
   }

   private static XmlSerializer GetSerializerForType(Type t)
   {
      XmlSerializer serializer;
      if (pluginSerializers == null || !pluginSerializers.TryGetValue(t, out serializer))
         serializer = CreateSerializer(t);
      return serializer;
   }

   /// <summary>
   /// Clears all cached XmlSerializers.
   /// </summary>
   public static void ClearSerializerCache()
   {
      pluginSerializers = null;
   }

   /// <summary>
   /// Deserializes an XML-file containing plugin types.
   /// </summary>
   /// <typeparam name="T">The type of object to deserialize.</typeparam>
   /// <param name="filePath">The file to deserialize.</param>
   public static T Deserialize<T>(String filePath) where T : class
   {
      using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
      {
         return Deserialize<T>(stream);
      }
   }

   /// <summary>
   /// Deserializes an XML-stream containing plugin types.
   /// </summary>
   /// <typeparam name="T">The type of object to deserialize.</typeparam>
   /// <param name="stream">The stream to deserialize.</param>
   public static T Deserialize<T>(Stream stream) where T : class
   {
      XmlSerializer serializer = GetSerializerForType(typeof(T));
      return serializer.Deserialize(stream) as T;
   }


   /// <summary>
   /// Serializes an object with plugin types to an XML-file.
   /// </summary>
   /// <typeparam name="T">The type of object to serialize.</typeparam>
   /// <param name="filePath">The path of the XML-file to serialize.</param>
   /// <param name="data">The object to serialize.</param>
   public static void Serialize<T>(String filePath, T data)
   {
      using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
      {
         Serialize<T>(stream, data);
      }
   }

   /// <summary>
   /// Serializes an object with plugin types to an XML-stream.
   /// </summary>
   /// <typeparam name="T">The type of object to serialize.</typeparam>
   /// <param name="filePath">The XML-stream to serialize.</param>
   /// <param name="data">The object to serialize.</param>
   public static void Serialize<T>(Stream stream, T data)
   {
      XmlSerializer serializer = GetSerializerForType(typeof(T));
      serializer.Serialize(stream, data);
   }
}
}
