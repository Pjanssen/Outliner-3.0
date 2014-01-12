using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PJanssen.Outliner.Configuration
{
/// <summary>
/// Defines helper functions for managing configuration files.
/// </summary>
public static class Configurations
{
   internal const String ConfigurationFileExtension = ".xml";

   /// <summary>
   /// Gets the paths of all configuration files in the given directory.
   /// </summary>
   /// <param name="directory">The directory to get files from.</param>
   public static IEnumerable<String> GetConfigurationFiles(String directory)
   {
      return Directory.EnumerateFiles( directory
                                     , "*" + ConfigurationFileExtension
                                     , SearchOption.TopDirectoryOnly);
   }

   /// <summary>
   /// Gets a dictionary of all files and Configurations from the given directory.
   /// </summary>
   /// <typeparam name="T">The type of ConfigurationFile class to load.</typeparam>
   /// <param name="directory">The directory to get files from.</param>
   public static IDictionary<String, T> GetConfigurationFiles<T>(String directory) where T : class
   {
      Dictionary<String, T> items = new Dictionary<String, T>();
      IEnumerable<String> files = GetConfigurationFiles(directory);
      
      foreach (String file in files)
      {
         items.Add(file, XmlSerialization.Deserialize<T>(file));
      }

      return items;
   }

   /// <summary>
   /// Gets all Configurations from the given directory.
   /// </summary>
   /// <typeparam name="T">The type of ConfigurationFile class to load.</typeparam>
   /// <param name="directory">The directory to get files from.</param>
   public static IEnumerable<T> GetConfigurations<T>(String directory) where T : class
   {
      IEnumerable<String> files = Directory.EnumerateFiles(directory
                                                          , "*" + ConfigurationFileExtension
                                                          , SearchOption.TopDirectoryOnly);
      return files.Select(f => XmlSerialization.Deserialize<T>(f));
   }

   /// <summary>
   /// Writes a new, default Configuration file of the given type.
   /// </summary>
   /// <typeparam name="T">The type of ConfigurationFile class to create.</typeparam>
   /// <param name="directory">The directory to write the file to.</param>
   /// <param name="basename">The base name of the configuration file. Numbers may be added to avoid overwriting existing files.</param>
   /// <returns>A Tuple with the file name and instance of the created Configuration.</returns>
   public static Tuple<String, T> NewConfigurationFile<T>(String directory, String basename) where T : class, new()
   {
      Int32 fileIndex = 1;
      String filename = null;
      while (filename == null || !IsValidNewFileName(filename))
      {
         filename = Path.Combine(directory, basename + fileIndex.ToString("D3") + ConfigurationFileExtension);
         fileIndex++;
      }

      T newConfig = new T();
      XmlSerialization.Serialize<T>(filename, newConfig);
      return new Tuple<String, T>(filename, newConfig);
   }

   /// <summary>
   /// Tests whether the given filename is valid for a new configuration file.
   /// </summary>
   /// <param name="filename">The filename to test.</param>
   public static Boolean IsValidNewFileName(String filename)
   {
      return !String.IsNullOrEmpty(filename) &&
             !String.IsNullOrEmpty(Path.GetFileNameWithoutExtension(filename)) &&
             Path.IsPathRooted(filename) &&
             !File.Exists(filename);
   }

   /// <summary>
   /// Deletes the given configuration file.
   /// </summary>
   /// <param name="file">The file to delete.</param>
   public static void DeleteConfigurationFile(String file)
   {
      File.Delete(file);
   }

   /// <summary>
   /// Renames the given configuration file.
   /// </summary>
   /// <param name="oldFilename">The old filename.</param>
   /// <param name="newFilename">The new filename.</param>
   public static void RenameConfigurationFile(String oldFilename, String newFilename)
   {
      if (!File.Exists(oldFilename))
         throw new InvalidOperationException("oldFilename does not exist");
      if (!IsValidNewFileName(newFilename))
         throw new InvalidOperationException("newFilename is not valid.");

      File.Move(oldFilename, newFilename);
   }
}
}
