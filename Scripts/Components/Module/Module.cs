using System;
using Modula.Optimization;
using UnityEngine;

namespace Modula
{
    public abstract class Module : MonoBehaviour, IModule
    {
        public virtual TypeList replaces { get; } = TypeList.None;

        public TimingConstraints UpdateInvocationConstraints => DefaultImplementation.UpdateConstraints;
        public virtual TypedList<IModule> RequiredOtherModules { get; } = new TypedList<IModule>();
        
        private ModuleDefaultImplementation _defaultImplementation;
        // ReSharper disable once MemberCanBePrivate.Global
        protected ModuleDefaultImplementation DefaultImplementation
        {
            get { return _defaultImplementation ??= new ModuleDefaultImplementation(this); }
        }

        public ModularBehaviour Main => DefaultImplementation.Main;

        public void OnAdd()
        {
            DefaultImplementation.OnAdd();
        }

        public void AddModule(Type moduleType)
        {
            DefaultImplementation.AddModule(moduleType);
        }

        public string GetName()
        {
            return DefaultImplementation.GetName();
        }

        public DataLayer GetData()
        {
            return DefaultImplementation.GetData();
        }

        // public virtual void ManagedUpdate()
        // {
        // }
        public virtual void ModuleUpdate()
        {
        }

        public virtual void Update()
        {
            DefaultImplementation.Update();
        }
    }
}