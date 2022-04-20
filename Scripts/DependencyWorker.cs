using System;
using System.Collections.Generic;
using System.Linq;
using Modula.Common;
using UnityEditor;
using UnityEngine;

namespace Modula
{
    public static class DependencyWorker
    {
        private static IModule[] FindDependent(IModule module)
        {
            var attachments = module.Main.attachments;
            var dependent = attachments.Where(m =>
            {
                var required = m.RequiredOtherModules;
                return required != null && required.Contains(module.GetType());
            });
            
            return dependent.ToArray();
        }

        private static IModule[] FindDependencies(IModule module)
        {
            var attachments = module.Main.attachments;
            return attachments.Where(m =>
                module.RequiredOtherModules.Contains(m.GetType())).ToArray();
        }

        private static HashSet<IModule> FindDependenciesRecursive(IModule module, HashSet<IModule> dependencies = null)
        {
            dependencies ??= new HashSet<IModule> { module };
            dependencies.Add(module);
            
            var found = FindDependencies(module).Where(d => !dependencies.Contains(d)).ToArray();
            
            if (found.Length == 0)
            {
                dependencies.Add(module);
                return dependencies;
            }
            
            foreach (var m in found)
            {
                dependencies.UnionWith(FindDependenciesRecursive(m, dependencies));
            }
            
            return dependencies;
        }

        public static string[] FindDependentModuleNames(IModule module)
        {
            var dependent = FindDependent(module);
            return dependent.Select(m => m.GetName()).ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="module"></param>
        /// <param name="forceDelete">Remove dependencies even if other modules depend on them</param>
        /// <param name="reason"></param>
        /// <param name="ignore"></param>
        public static void RemoveModuleWithDependencies(IModule module, bool forceDelete=false,
            ModuleRemoveReason reason = ModuleRemoveReason.NotSpecified)
        {
            var attachments = module.Main.attachments;
            var dependencies = FindDependenciesRecursive(module);//.Reverse().ToArray();
            foreach (var dependency in dependencies)
            {
                bool whitelist = false;
                if (!forceDelete)
                {
                    foreach (var other in attachments.Where(m => !dependencies.Contains(m)) )
                    {
                        if (FindDependencies(other).Contains(dependency))
                        {
                            whitelist = true;
                        }
                    }
                }
                if (!whitelist) module.Main.RemoveModule(dependency);
            }
        }

        // public static void RemoveWithDependencies(IModule module, ModularBehaviour main)
        // {
        //     
        // }
        //
        // public static void RemoveWithDependencies(IModule[] modules, ModularBehaviour main)
        // {
        //     
        // }

        /// <summary>
        ///     Check if a module has dependencies that are not present in this GameObject.
        ///     Adds these 'unresolved' dependencies to the GameObject.
        /// </summary>
        /// <param name="module">The module to check dependencies of.</param>
        /// <returns>true, if any module(s) have been added to this behaviour during the method run.</returns>
        public static bool ResolveDependencies(IModule module)
        {
            var attachments = module.Main.attachments;
            var modulesModified = false;
            foreach (var requirement in module.RequiredOtherModules)
                if (attachments.Find(m => m.GetType() == requirement) == null)
                {
                    module.Main.AddModule(requirement);
                    modulesModified = true;
                }

            return modulesModified;
        }

        public static bool ResolveDependencies(IEnumerable<IModule> modules, ModularBehaviour main)
        {
            var attachments = main.attachments.Select(module => module.GetType());
            var missing = new List<Type>();
            foreach (var module in modules)
            {
                missing.AddRange(module.RequiredOtherModules.Where(
                    requirement => !attachments.Contains(requirement))
                );
            }
            
            main.AddModules(missing.ToArray());
            return missing.Count > 0;
        }


        public static void Clear(ModularBehaviour main, bool printReason=false)
        {
            var reason = printReason ? ModuleRemoveReason.ClearingBehaviour : ModuleRemoveReason.__EMPTY;
            if (main.attachments == null) return;
            main.RemoveModules(main.attachments.ToArray(), reason);
        }
    }
}