using System;
using Modula.Common;
using Modula.Optimization;
using UnityEngine;

namespace Modula
{
    public abstract class Module : MonoBehaviour, IModule
    {
        private ModuleDefaultImplementation _defaultImplementation;
        public virtual TypeList replaces { get; } = TypeList.None;

        // ReSharper disable once MemberCanBePrivate.Global
        protected ModuleDefaultImplementation DefaultImplementation
        {
            get { return _defaultImplementation ??= new ModuleDefaultImplementation(this); }
        }

        public virtual void Update()
        {
            DefaultImplementation.Update();
        }

        public TimingConstraints UpdateInvocationConstraints => DefaultImplementation.UpdateConstraints;
        public virtual TypedList<IModule> RequiredOtherModules { get; } = new();

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

        public void ManagedUpdate()
        {
            ModuleUpdate();
        }

        public virtual bool ShouldSerialize()
        {
            return true;
        }

        protected virtual void ModuleUpdate()
        {
        }
    }
}