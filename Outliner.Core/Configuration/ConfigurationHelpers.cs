using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Outliner.Configuration
{
public static class ConfigurationHelpers
{
   internal const String ConfigurationFileExtension = ".xml";

   public static IDictionary<String, T> GetConfigurationFiles<T>(String directory) where T : class
   {
      Dictionary<String, T> items = new Dictionary<String, T>();
      IEnumerable<String> files = Directory.EnumerateFiles( directory
                                                          ,  "*" + ConfigurationFileExtension
                                                          , SearchOption.TopDirectoryOnly);
      
      foreach (String file in files)
      {
         items.Add(file, XmlSerializationHelpers.Deserialize<T>(file));
      }

      return items;
   }

   public static IEnumerable<T> GetConfigurations<T>(String directory) where T : class
   {
      IEnumerable<String> files = Directory.EnumerateFiles( directory
                                                          ,  "*" + ConfigurationFileExtension
                                                          , SearchOption.TopDirectoryOnly);
      return files.Select(f => XmlSerializationHelpers.Deserialize<T>(f));
   }

   public static Tuple<String, T> NewConfigurationFile<T>(String directory, String basename) where T : class, new()
   {
      Int32 fileIndex = 1;
      String filename = null;
      while (filename == null || !IsValidFileName(filename))
      {
         filename = Path.Combine(directory, basename + fileIndex.ToString("D3") + ConfigurationFileExtension);
         fileIndex++;
      }

      T newConfig = new T();
      XmlSerializationHelpers.Serialize<T>(filename, newConfig);
      return new Tuple<String, T>(filename, newConfig);
   }

   public static Boolean IsValidFileName(String filename)
   {
      return !String.IsNullOrEmpty(filename) &&
             !String.IsNullOrEmpty(Path.GetFileNameWithoutExtension(filename)) &&
             Path.IsPathRooted(filename) &&
             !File.Exists(filename);
   }

   public static void DeleteConfigurationFile(String file)
   {
      File.Delete(file);
   }

   public static void RenameConfigurationFile(String oldFilename, String newFilename)
   {
      if (!File.Exists(oldFilename))
         throw new InvalidOperationException("oldFilename does not exist");
      if (!IsValidFileName(newFilename))
         throw new InvalidOperationException("newFilename is not valid.");

      File.Move(oldFilename, newFilename);
   }
}
}
