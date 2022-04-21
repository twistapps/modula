using System;
using System.Linq;
using Modula.Optimization;
using UnityEngine;

namespace Modula
{
    public class ModuleDefaultImplementation
    {
        private readonly IModule _boundModule;
        //private List<IModule> attachments;

        private TimingConstraints _updateConstraints;

        public ModuleDefaultImplementation(IModule bind)
        {
            _boundModule = bind;
            Main = _boundModule.GetComponent<ModularBehaviour>();
        }

        public ModularBehaviour Main { get; private set; }

        public TimingConstraints UpdateConstraints
        {
            get { return _updateConstraints ??= new TimingConstraints(_boundModule.ManagedUpdate); }
        }

        public void OnAdd()
        {
            Main = _boundModule.GetComponent<ModularBehaviour>();
            var attachments = FindComponents<IModule>().ToList();
            var hasRequiredOtherModules = _boundModule.RequiredOtherModules?.Types != null;
            if (!hasRequiredOtherModules) return;
            foreach (var type in _boundModule.RequiredOtherModules.Types)
            {
                var isMissing = true;
                foreach (var attachedModule in attachments)
                    if (attachedModule.GetType() == type)
                        isMissing = false;

                if (isMissing) AddModule(type);
            }
        }

        public void AddModule(Type moduleType)
        {
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

        private T[] FindComponents<T>()
        {
            return _boundModule.gameObject.FindComponents<T>().ToArray();
        }
    }
}