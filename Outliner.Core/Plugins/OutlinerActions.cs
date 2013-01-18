using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;
using System.Reflection;
using Outliner.Controls.Tree;

namespace Outliner.Plugins
{
   public delegate void OutlinerAction(TreeNode contextTn, IEnumerable<MaxNodeWrapper> contextNodes);
   public delegate Boolean OutlinerPredicate(TreeNode contextTn, IEnumerable<MaxNodeWrapper> contextNodes);

   public static class OutlinerActions
   {
      private static Dictionary<String, OutlinerAction> actions;
      private static Dictionary<String, OutlinerPredicate> predicates;

      private static void BuildActionsSet()
      {
         actions = new Dictionary<String, OutlinerAction>();
         predicates = new Dictionary<String, OutlinerPredicate>();

         IEnumerable<OutlinerPluginData> plugins = OutlinerPlugins.GetPlugins(OutlinerPluginType.ActionProvider);
         foreach (OutlinerPluginData plugin in plugins)
         {
            MethodInfo[] methods = plugin.Type.GetMethods(BindingFlags.Static | BindingFlags.Public);
            foreach (MethodInfo method in methods)
            {
               if (TypeHelpers.HasAttribute<OutlinerActionAttribute>(method))
                  actions.Add(method.Name, Delegate.CreateDelegate(typeof(OutlinerAction), method) as OutlinerAction);
               else if (TypeHelpers.HasAttribute<OutlinerPredicateAttribute>(method))
                  predicates.Add(method.Name, Delegate.CreateDelegate(typeof(OutlinerPredicate), method) as OutlinerPredicate);
            }
         }
      }

      public static IEnumerable<OutlinerAction> Actions
      {
         get
         {
            if (actions == null)
               BuildActionsSet();

            return actions.Values;
         }
      }

      public static IEnumerable<String> ActionNames
      {
         get
         {
            if (actions == null)
               BuildActionsSet();

            return actions.Keys;
         }
      }

      public static IEnumerable<OutlinerPredicate> Predicates
      {
         get
         {
            if (predicates == null)
               BuildActionsSet();

            return predicates.Values;
         }
      }

      public static IEnumerable<String> PredicateNames
      {
         get
         {
            if (actions == null)
               BuildActionsSet();

            return predicates.Keys;
         }
      }

      public static OutlinerAction GetAction(String name)
      {
         if (actions == null)
            BuildActionsSet();

         OutlinerAction action = null;
         if (actions.TryGetValue(name, out action))
            return action;
         else
            return null;
      }

      public static OutlinerPredicate GetPredicate(String name)
      {
         if (predicates == null)
            BuildActionsSet();

         OutlinerPredicate predicate = null;
         if (predicates.TryGetValue(name, out predicate))
            return predicate;
         else
            return null;
      }
   }

   [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
   public sealed class OutlinerActionAttribute : Attribute
   {
      public OutlinerActionAttribute() { }
   }

   [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
   public sealed class OutlinerPredicateAttribute : Attribute
   {
      public OutlinerPredicateAttribute() { }
   }
}
