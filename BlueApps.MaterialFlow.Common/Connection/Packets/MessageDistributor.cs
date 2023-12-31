﻿using BlueApps.MaterialFlow.Common.Connection.PacketHelper;
using BlueApps.MaterialFlow.Common.Connection.Packets.Events;
using BlueApps.MaterialFlow.Common.Models.EventArgs;

namespace BlueApps.MaterialFlow.Common.Connection.Packets;

public abstract class MessageDistributor
{
    public abstract event EventHandler<BarcodeScanEventArgs> BarcodeScanned;
    public abstract event EventHandler<WeightScanEventArgs> WeightScanned;
    public abstract event EventHandler<UnsubscribedPacketEventArgs> UnsubscribedPacket;
    public abstract event EventHandler<ErrorcodeEventArgs> ErrorCodeTriggered;

    protected readonly List<MessagePacketHelper> _packetHelpers;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="packetHelpers"></param>
    /// <exception cref="ArgumentException"></exception>
    protected MessageDistributor(List<MessagePacketHelper> packetHelpers)
    {
        _packetHelpers = packetHelpers;

        if (packetHelpers == null || !packetHelpers.Any())
            throw new ArgumentException("PacketHelpers cannot be empty or null!");
    }

    public abstract void DistributeIncomingMessages(object sender, MessagePacketEventArgs messageEvent);
}