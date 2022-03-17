namespace Modula.Examples
{
    public class FirstPersonCamera : Module
    {
        public override TypeList replaces { get; } = new TypeList()
            .Add(typeof(ThirdPersonCamera));
    }
}