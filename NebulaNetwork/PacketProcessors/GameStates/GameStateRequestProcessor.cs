﻿using NebulaAPI;
using NebulaModel.Networking;
using NebulaModel.Packets;
using NebulaModel.Packets.GameStates;
using NebulaWorld.GameStates;

namespace NebulaNetwork.PacketProcessors.GameStates
{
    [RegisterPacketProcessor]
    internal class GameStateRequestProcessor : PacketProcessor<GameStateRequest>
    {
        public override void ProcessPacket(GameStateRequest packet, NebulaConnection conn)
        {
            if (IsHost)
            {
                conn.SendPacket(new GameStateUpdate(packet.SentTimestamp, GameStatesManager.RealGameTick, GameStatesManager.RealUPS));
            }
            else
            {
                conn.SendPacket(packet);
            }
        }
    }
}
