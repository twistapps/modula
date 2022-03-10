using Modula.ConditionalFieldDraw;
using UnityEngine;

namespace Modula.Examples
{
    public partial class ExampleData
    {
        [Header("Second Module")] [ForModule(typeof(SecondModule))] [Range(-1, 3)]
        public float someFloat;

        [ForModule(typeof(SecondModule))] public string someAnotherString;
    }

    public class SecondModule : Module
    {
        public override TypeList RequiredOtherModules { get; } = new TypeList()
            .Add(typeof(ModuleFoo));
    }
}