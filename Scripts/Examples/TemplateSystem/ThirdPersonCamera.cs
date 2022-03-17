namespace Modula.Examples
{
    public class ThirdPersonCamera : Module
    {
        public override TypeList replaces { get; } = new TypeList()
            .Add(typeof(FirstPersonCamera));
    }
}