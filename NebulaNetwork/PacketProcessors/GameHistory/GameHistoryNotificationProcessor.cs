﻿using NebulaAPI;
using NebulaModel.Networking;
using NebulaModel.Packets;
using NebulaModel.Packets.GameHistory;
using NebulaWorld;

namespace NebulaNetwork.PacketProcessors.GameHistory
{
    [RegisterPacketProcessor]
    internal class GameHistoryNotificationProcessor : PacketProcessor<GameHistoryNotificationPacket>
    {
        public override void ProcessPacket(GameHistoryNotificationPacket packet, NebulaConnection conn)
        {
            if (IsHost && packet.Event != GameHistoryEvent.OneKeyUnlock)
            {
                Multiplayer.Session.Network.SendPacketExclude(packet, conn);
            }

            using (Multiplayer.Session.History.IsIncomingRequest.On())
            {
                switch (packet.Event)
                {
                    case GameHistoryEvent.ResumeQueue:
                        GameMain.history.ResumeTechQueue();
                        break;
                    case GameHistoryEvent.PauseQueue:
                        GameMain.history.PauseTechQueue();
                        break;
                    case GameHistoryEvent.OneKeyUnlock:
                        UIRoot.instance.uiGame.techTree.Do1KeyUnlock();
                        break;
                }
            }
        }
    }
}