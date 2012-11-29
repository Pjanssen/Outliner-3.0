using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Outliner.Plugins;

namespace Outliner.Presets
{
public static class OutlinerPresets
{
   private class PresetComparer : IComparer<OutlinerPreset>
   {
      public int Compare(OutlinerPreset x, OutlinerPreset y)
      {
         return x.Text.CompareTo(y.Text);
      }
   }

   private static SortedDictionary<OutlinerPreset, String> presets;
   private static Dictionary<OutlinerPreset, OutlinerPluginData> defaultPresets;

   public static IEnumerable<OutlinerPreset> Presets
   {
      get
      {
         if (OutlinerPresets.presets == null)
            return new List<OutlinerPreset>();
         else
            return OutlinerPresets.presets.Keys;
      }
   }

   private static OutlinerPreset CreatePreset(Type presetType)
   {
      return Activator.CreateInstance(presetType) as OutlinerPreset;
   }

   public static void LoadPresets()
   {
      OutlinerPresets.presets = new SortedDictionary<OutlinerPreset, String>(new PresetComparer());
      OutlinerPresets.defaultPresets = new Dictionary<OutlinerPreset, OutlinerPluginData>();

      String presetsDir = OutlinerPaths.PresetsDir;
      if (Directory.Exists(presetsDir))
      {
         String[] presetFiles = Directory.GetFiles(presetsDir, "*.xml");
         foreach (String presetFile in presetFiles)
         {
            OutlinerPreset preset = XmlSerializationHelpers.Deserialize<OutlinerPreset>(presetFile);
            presets.Add(preset, presetFile);
         }
      }

      IEnumerable<OutlinerPluginData> plugins = OutlinerPlugins.GetPlugins(OutlinerPluginType.DefaultPreset);
      foreach (OutlinerPluginData plugin in plugins)
      {
         OutlinerPreset preset = CreatePreset(plugin.Type);
         OutlinerPreset loadedPreset = presets.Keys.FirstOrDefault(p => p.Text.Equals(preset.Text));
         if (loadedPreset == null)
         {
            presets.Add(preset, null);
            loadedPreset = preset;
         }
         loadedPreset.IsDefaultPreset = true;
         OutlinerPresets.defaultPresets.Add(preset, plugin);
      }
   }


   /// <summary>
   /// Creates a new preset and preset file.
   /// </summary>
   public static OutlinerPreset NewPreset()
   {
      OutlinerPreset preset = new OutlinerPreset();
      String nameBase = "Preset";
      Int32 nameCount = 0;
      String name = null;
      String file = null;
      while(file == null || File.Exists(file))
      {
         nameCount++;
         name = nameBase + nameCount.ToString("D3");
         file = Path.Combine(OutlinerPaths.PresetsDir, name + ".xml");
      }
      preset.TextRes = name;

      XmlSerializationHelpers.Serialize<OutlinerPreset>(file, preset);
      OutlinerPresets.presets.Add(preset, file);
      
      return preset;
   }


   /// <summary>
   /// Removes the preset from the loaded presets, and deletes the preset file.
   /// </summary>
   public static void DeletePreset(OutlinerPreset preset)
   {
      String fileName = null;
      if (OutlinerPresets.presets.TryGetValue(preset, out fileName))
      {
         if (fileName != null)
            File.Delete(fileName);

         OutlinerPresets.presets.Remove(preset);

         if (preset.IsDefaultPreset)
         {
            OutlinerPluginData plugin = null;
            if (OutlinerPresets.defaultPresets.TryGetValue(preset, out plugin))
            {
               OutlinerPreset newPreset = OutlinerPresets.CreatePreset(plugin.Type);
               OutlinerPresets.presets.Add(newPreset, null);
               OutlinerPresets.defaultPresets.Remove(preset);
               OutlinerPresets.defaultPresets.Add(newPreset, plugin);
            }
         }
      }
   }
}
}
