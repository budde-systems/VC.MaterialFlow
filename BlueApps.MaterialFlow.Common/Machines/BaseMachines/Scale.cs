using BlueApps.MaterialFlow.Common.Models.Machines;

namespace BlueApps.MaterialFlow.Common.Machines.BaseMachines
{
    public abstract class Scale : IMachine
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string BasePosition { get; set; }
        public string SubPosition { get; set; }


        public override string ToString() => $"{Name} {Id}";
    }
}
