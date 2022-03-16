using System;
using System.Collections.Generic;
using System.Linq;
using Modula.Optimization;
using UnityEngine;

namespace Modula
{
    public partial class ModuleDefaultImplementation
    {
        private readonly IModule _boundModule;
        private List<IModule> _modules;

        private TimingConstraints _updateConstraints;

        public ModuleDefaultImplementation(IModule bind)
        {
            _boundModule = bind;
        }

        public ModularBehaviour Main { get; private set; }

        public TimingConstraints UpdateConstraints
        {
            get { return _updateConstraints ??= new TimingConstraints(_boundModule.ManagedUpdate); }
        }

        public void OnAdd()
        {
            Main = _boundModule.GetComponent<ModularBehaviour>();
            _modules = FindComponents<IModule>().ToList();
            var hasRequiredOtherModules = _boundModule.RequiredOtherModules?.Types != null;
            if (hasRequiredOtherModules)
                foreach (var type in _boundModule.RequiredOtherModules.Types)
                {
                    var isMissing = true;
                    foreach (var attachedModule in _modules)
                        if (attachedModule.GetType() == type)
                            isMissing = false;
                    
                    if (isMissing) AddModule(type);
                }
        }

        public void AddModule(Type moduleType)
        {
            //var module = _bind.gameObject.AddComponent(moduleType) as IModule;
            //modules.Add(module);
            Main.AddModule(moduleType);
        }

        public string GetName()
        {
            return _boundModule.GetType().Name;
        }

        public DataLayer GetData()
        {
            return Main.GetData();
        }

        public void Update()
        {
            UpdateConstraints.Update(Time.deltaTime);
        }
    }
}