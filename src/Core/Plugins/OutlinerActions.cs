using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using PJanssen.Outliner.Scene;
using PJanssen.Outliner.Controls.Tree;

namespace PJanssen.Outliner.Plugins
{
public delegate void OutlinerAction(TreeNode contextTn, IEnumerable<IMaxNode> contextNodes);
public delegate Boolean OutlinerPredicate(TreeNode contextTn, IEnumerable<IMaxNode> contextNodes);

/// <summary>
/// An interface used to manage OutlinerActions and OutlinerPredicates.
/// </summary>
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
            if (method.HasAttribute<OutlinerActionAttribute>())
               actions.Add(method.Name, Delegate.CreateDelegate(typeof(OutlinerAction), method) as OutlinerAction);
            else if (method.HasAttribute<OutlinerPredicateAttribute>())
               predicates.Add(method.Name, Delegate.CreateDelegate(typeof(OutlinerPredicate), method) as OutlinerPredicate);
         }
      }
   }

   /// <summary>
   /// Gets all OutlinerActions.
   /// </summary>
   public static IEnumerable<OutlinerAction> Actions
   {
      get
      {
         if (actions == null)
            BuildActionsSet();

         return actions.Values;
      }
   }

   /// <summary>
   /// Gets the names of all OutlinerActions.
   /// </summary>
   public static IEnumerable<String> ActionNames
   {
      get
      {
         if (actions == null)
            BuildActionsSet();

         return actions.Keys;
      }
   }

   /// <summary>
   /// Gets all OutlinerPredicates.
   /// </summary>
   public static IEnumerable<OutlinerPredicate> Predicates
   {
      get
      {
         if (predicates == null)
            BuildActionsSet();

         return predicates.Values;
      }
   }

   /// <summary>
   /// Gets the names of all OutlinerPredicates.
   /// </summary>
   public static IEnumerable<String> PredicateNames
   {
      get
      {
         if (actions == null)
            BuildActionsSet();

         return predicates.Keys;
      }
   }

   /// <summary>
   /// Gets the OutlinerAction with the supplied name.
   /// </summary>
   /// <param name="name">The name of the action to retrieve.</param>
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

   /// <summary>
   /// Gets the OutlinerPredicate with the supplied name.
   /// </summary>
   /// <param name="name">The name of the action to retrieve.</param>
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

/// <summary>
/// Marks a method as an OutlinerAction.
/// </summary>
/// <remarks>A method marked with this attribute must have a unique name 
/// and have the following signature:<br />
/// void OutlinerAction(TreeNode, IEnumerable&lt;IMaxNode&gt;)</remarks>
[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class OutlinerActionAttribute : Attribute
{
   /// <summary>
   /// Initializes a new instance of the OutlinerActionAttribute class.
   /// </summary>
   public OutlinerActionAttribute() { }
}

/// <summary>
/// Marks a method as an OutlinerPredicate.
/// </summary>
/// <remarks>A method marked with this attribute must have a unique name 
/// and have the following signature:<br />
/// Boolean OutlinerPredicate(TreeNode, IEnumerable&lt;IMaxNode&gt;)</remarks>
[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class OutlinerPredicateAttribute : Attribute
{
   /// <summary>
   /// Initializes a new instance of the OutlinerPredicateAttribute class.
   /// </summary>
   public OutlinerPredicateAttribute() { }
}
}
