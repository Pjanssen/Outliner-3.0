using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Controls;
using System.IO;

namespace Outliner.UserFiles
{
public static class UserFiles
{
   private const String UserFilesSearchPattern = "*.xml";

   public static IEnumerable<T> GetUserFiles<T>(String directory) where T : class
   {
      IEnumerable<String> files = Directory.EnumerateFiles( directory
                                                          , UserFilesSearchPattern
                                                          , SearchOption.AllDirectories);
      return files.Select(f => XmlSerializationHelpers.Deserialize<T>(f));       
   }
}
}
