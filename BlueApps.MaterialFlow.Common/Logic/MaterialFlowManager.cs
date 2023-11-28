using BlueApps.MaterialFlow.Common.Connection.Packets;
using BlueApps.MaterialFlow.Common.Sectors;

namespace BlueApps.MaterialFlow.Common.Logic
{
    public abstract class MaterialFlowManager
    {
        public ICollection<Sector> Sectors {get; protected set;}
        protected MessageDistributor _messageDistributor;


        protected abstract ICollection<Sector> CreateSectors();
    }
}
