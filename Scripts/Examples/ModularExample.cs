using System;

namespace Modula.Examples
{
    public class ModularExample : ModularBehaviour
    {
        private ExampleData _data;

        public override TypeList AvailableModules { get; } = new TypeList()
            .Add(typeof(ModuleFoo))
            .Add(typeof(SecondModule))
            .Add(typeof(ThirdModule));

        public override Type GetDataLayerType()
        {
            return typeof(ExampleData);
        }
    }
}