using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outliner.Configuration;
using Outliner.Plugins;
using System.Reflection;

namespace Outliner.Controls.Options
{
public partial class ConfigurationFileEditor : OutlinerUserControl
{
   public ConfigurationFileEditor()
   {
      InitializeComponent();
   }

   private ConfigurationFile config;
   public ConfigurationFile Configuration
   {
      get { return this.config; }
      set
      {
         this.config = value;
         this.UpdateUI();
      }
   }

   private void UpdateUI()
   {
      if (this.config == null)
         return;

      this.configurationFileBindingSource.Clear();

      this.resTypeComboBox.DataSource = OutlinerPlugins.PluginAssemblies
                                                       .SelectMany(a => a.GetTypes())
                                                       .Where(t => t.GetProperty("ResourceManager", BindingFlags.Static | BindingFlags.NonPublic) != null)
                                                       .ToArray();
      this.resTypeComboBox.DisplayMember = "FullName";
      this.resTypeComboBox.ValueMember = "FullName";

      Type resType = this.config.ResourceType;
      if (resType != null)
      {
         this.textComboBox.DataSource = GetPropNames(resType, typeof(String));
         this.img16ComboBox.DataSource = GetPropNames(resType, typeof(Image));
         this.img24ComboBox.DataSource = GetPropNames(resType, typeof(Image));
      }

      this.configurationFileBindingSource.DataSource = this.config;
   }

   private IList<String> GetPropNames(Type resType, Type propType)
   {
      return resType.GetProperties(BindingFlags.Static | BindingFlags.NonPublic)
                    .Where(p => p.PropertyType.Equals(propType) || p.PropertyType.IsSubclassOf(propType))
                    .Select(p => p.Name)
                    .ToList();
   }

}
}
