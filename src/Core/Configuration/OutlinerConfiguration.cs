using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace PJanssen.Outliner.Configuration
{
   public class OutlinerConfiguration
   {
      //==========================================================================

      private const string CoreSectionName = "Core";
      private const string TreeViewSectionName = "TreeView";

      //==========================================================================

      private System.Configuration.Configuration config;

      //==========================================================================

      public OutlinerConfiguration(System.Configuration.Configuration config)
      {
         Throw.IfNull(config, "config");

         this.config = config;
      }
      
      //==========================================================================

      public static OutlinerConfiguration Load(string filename)
      {
         ExeConfigurationFileMap configMap = new ExeConfigurationFileMap();
         configMap.ExeConfigFilename = filename;
         System.Configuration.Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);

         return new OutlinerConfiguration(config);
      }

      //==========================================================================

      public void Save()
      {
         this.config.Save();
      }

      //==========================================================================

      public T GetSection<T>(string sectionName) where T : ConfigurationSection, new()
      {
         T section = this.config.GetSection(sectionName) as T;
         if (section == null)
         {
            section = new T();
            this.config.Sections.Add(sectionName, section);
         }

         return section;
      }

      //==========================================================================

      public CoreConfigurationSection Core
      {
         get
         {
            return this.GetSection<CoreConfigurationSection>(CoreSectionName);
         }
      }

      //==========================================================================

      public TreeViewConfigurationSection TreeView
      {
         get
         {
            return this.GetSection<TreeViewConfigurationSection>(TreeViewSectionName);
         }
      }

      //==========================================================================

   }
}
