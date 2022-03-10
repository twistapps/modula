#if MIRROR
using Modula.ConditionalFieldDraw;
using UnityEngine;

namespace Modula.Examples
{
    public class ExampleData
    {
        [Header("Network Module")] [ForModule(typeof(NetworkModuleExample))] [Range(-1, 3)]
        public float someAnotherFloat;

        [ForModule(typeof(NetworkModuleExample))]
        public string thisIsAString;
    }

    public class NetworkModuleExample : NetworkModule
    {
        public override TypeList RequiredOtherModules { get; } = new TypeList()
            .Add(
                typeof(ModuleFoo)
            );
    }
}
#endif