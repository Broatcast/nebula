﻿using NebulaAPI;
using NebulaModel.Attributes;
using NebulaModel.DataStructures;
using NebulaModel.Utils;
using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

namespace NebulaModel
{
    [System.Serializable]
    public class MultiplayerOptions : ICloneable
    {
        [DisplayName("Nickname")]
        public string Nickname { get; set; } = string.Empty;

        [DisplayName("Server Password"), Category("Network")]
        [Description("If provided, this will set a password for your hosted server.")]
        [UIContentType(InputField.ContentType.Password)]
        public string ServerPassword { get; set; } = string.Empty;

        public string LastClientPassword { get; set; } = string.Empty;

        [DisplayName("Host Port"), Category("Network")]
        [UIRange(1, ushort.MaxValue)]
        public ushort HostPort { get; set; } = 8469;

        [DisplayName("Enable Experimental Ngrok support"), Category("Network")]
        [Description("If enabled, when hosting a server this will automatically download and install the Ngrok client and set up an Ngrok tunnel that provides an address at which the server can be joined")]
        public bool EnableNgrok { get; set; } = false;

        private const string _ngrokAuthtokenDisplayname = "Ngrok Authtoken";
        [DisplayName(_ngrokAuthtokenDisplayname), Category("Network")]
        [Description("This is required for Ngrok support and can be obtained by creating a free account at https://ngrok.com/")]
        [UICharacterLimit(49)]
        public string NgrokAuthtoken { get; set; } = string.Empty;

        [DisplayName("Ngrok Region"), Category("Network")]
        [Description("Available Regions: us, eu, au, ap, sa, jp, in")]
        public string NgrokRegion { get; set; } = string.Empty;

        [DisplayName("Remember Last IP"), Category("Network")]
        public bool RememberLastIP { get; set; } = true;

        [DisplayName("Remember Last Client Password"), Category("Network")]
        public bool RememberLastClientPassword { get; set; } = true;

        [DisplayName("Enable Discord RPC (requires restart)"), Category("Network")]
        public bool EnableDiscordRPC { get; set; } = true;

        [DisplayName("Auto accept Discord join requests"), Category("Network")]
        public bool AutoAcceptDiscordJoinRequests { get; set; } = false;

        [DisplayName("IP Configuration"), Category("Network")]
        [Description("Configure which type of IP should be used by Discord RPC")]
        public IPUtils.IPConfiguration IPConfiguration { get; set; }

        [DisplayName("Show Lobby Hints")]
        public bool ShowLobbyHints { get; set; } = true;

        public string LastIP { get; set; } = string.Empty;

        [DisplayName("Sync Ups")]
        [Description("If enabled the UPS of each player is synced. This ensures a similar amount of GameTick() calls.")]
        public bool SyncUps { get; set; } = true;

        [DisplayName("Sync Soil")]
        [Description("If enabled the soil count of each players is added together and used as one big pool for everyone. Note that this is a server side setting applied to all clients.")]
        public bool SyncSoil { get; set; } = false;

        private bool _streamerMode = false;
        [DisplayName("Streamer mode")]
        [Description("If enabled specific personal information like your IP address is hidden from the ingame chat.")]
        public bool StreamerMode { 
            get => _streamerMode; 
            set { 
                _streamerMode = value;

                InputField ngrokAuthTokenInput = GameObject.Find("list/scroll-view/viewport/content/Network/NgrokAuthtoken")?.GetComponentInChildren<InputField>();
                UpdateNgrokAuthtokenInputFieldContentType(ref ngrokAuthTokenInput);
            }
        }

        [DisplayName("Auto Open Chat"), Category("Chat")]
        [Description("Auto open chat window when receiving message from other players")]
        public bool AutoOpenChat { get; set; } = true;

        [DisplayName("Show system warn message"), Category("Chat")]
        public bool EnableWarnMessage { get; set; } = true;

        [DisplayName("Show system info message"), Category("Chat")]
        public bool EnableInfoMessage { get; set; } = true;

        [DisplayName("Default chat position"), Category("Chat")]
        public ChatPosition DefaultChatPosition { get; set; } = ChatPosition.LeftMiddle;
        
        [DisplayName("Default chat size"), Category("Chat")]
        public ChatSize DefaultChatSize { get; set; } = ChatSize.Medium;

        [DisplayName("Cleanup inactive sessions"), Category("Network")]
        [Description("If disabled the underlying networking library will not cleanup inactive connections. This might solve issues with clients randomly disconnecting and hosts having a 'System.ObjectDisposedException'.")]
        public bool CleanupInactiveSessions { get; set; } = true;

        public string MechaColors { get; set; } = "209 151 76 255;184 90 72 255;94 92 92 255;123 234 255 255;229 155 94 255;255 243 235 255;255 248 245 255;255 255 255 255;";

        public Float4[] GetMechaColors()
        {
            string[] colors = MechaColors.Split(';');
            Float4[] mechaColors = new Float4[colors.Length - 1];
            for (int i = 0; i < colors.Length - 1; i++)
            {
                string[] color = colors[i].Split(' ');
                if (!float.TryParse(color[0], out mechaColors[i].x) ||
                    !float.TryParse(color[1], out mechaColors[i].y) ||
                    !float.TryParse(color[2], out mechaColors[i].z) ||
                    !float.TryParse(color[3], out mechaColors[i].w))
                {
                    Logger.Log.Error($"Color {i} is invalid.");
                }
            }
            return mechaColors;
        }

        public void SetMechaColors()
        {
            /*UnityEngine.Color32[] mainColors = GameMain.mainPlayer.mecha.mainColors;
            string mechaColors = string.Empty;
            for (int i = 0; i < mainColors.Length; i++)
            {
                mechaColors += $"{(int)mainColors[i].r} {(int)mainColors[i].g} {(int)mainColors[i].b} {(int)mainColors[i].a};";
            }
            MechaColors = mechaColors;
            Config.SaveOptions();*/
        }

        // Detail function group buttons
        public bool PowerGridEnabled { get; set; } = false;
        public bool VeinDistributionEnabled { get; set; } = false;
        public bool SpaceNavigationEnabled { get; set; } = true;
        public bool BuildingWarningEnabled { get; set; } = true;
        public bool BuildingIconEnabled { get; set; } = true;
        public bool GuidingLightEnabled { get; set; } = true;

        public bool EnableRemoteSaveAccess { get; set; } = false;
        public bool AutoPauseEnabled { get; set; } = true;


        public object Clone()
        {
            return MemberwiseClone();
        }

        private void UpdateNgrokAuthtokenInputFieldContentType(ref InputField ngrokAuthTokenInput)
        {
            if (ngrokAuthTokenInput != null)
            {
                if (StreamerMode)
                {
                    ngrokAuthTokenInput.contentType = InputField.ContentType.Password;
                }
                else
                {
                    ngrokAuthTokenInput.contentType = InputField.ContentType.Standard;
                }
                ngrokAuthTokenInput.UpdateLabel();
            }
        }

        public void ModifyInputFieldAtCreation(string displayName, ref InputField inputField)
        {
            switch (displayName)
            {
                case _ngrokAuthtokenDisplayname:
                    {
                        UpdateNgrokAuthtokenInputFieldContentType(ref inputField);
                        break;
                    }
            }

        }

    }
}
