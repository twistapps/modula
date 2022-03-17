using System;

namespace Modula.Examples
{
    public class ModularExample : ModularBehaviour
    {
        private ExampleData _data;

        public override TypedList<IModule> AvailableModules { get; } = new TypedList<IModule>()
            .Add<ModuleFoo>()
            .Add<SecondModule>()
            .Add<ThirdModule>();

        public override Type GetDataLayerType()
        {
            return typeof(ExampleData);
        }
    }
}