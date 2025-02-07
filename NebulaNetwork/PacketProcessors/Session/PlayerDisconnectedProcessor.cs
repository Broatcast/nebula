﻿using NebulaAPI;
using NebulaModel.Networking;
using NebulaModel.Packets;
using NebulaModel.Packets.Session;
using NebulaWorld;
using NebulaWorld.SocialIntegration;

namespace NebulaNetwork.PacketProcessors.Session
{
    [RegisterPacketProcessor]
    public class PlayerDisconnectedProcessor : PacketProcessor<PlayerDisconnected>
    {
        public override void ProcessPacket(PlayerDisconnected packet, NebulaConnection conn)
        {
            Multiplayer.Session.NumPlayers = packet.NumPlayers;
            DiscordManager.UpdateRichPresence();
            Multiplayer.Session.World.DestroyRemotePlayerModel(packet.PlayerId);
        }
    }
}
