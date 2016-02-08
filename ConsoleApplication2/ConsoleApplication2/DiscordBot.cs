using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.Commands.Permissions;
using Discord.API;
using Discord.Modules;
using Discord.Audio;

namespace JeanGuyTremblayBot
{
    class DiscordBot
    {
        private const string _SERVER_NAME = "Pas Jean-Guy Tremblay";

        private DiscordClient _client;
        private Server _currentServer;
        private Channel _currentTextChannel;
        private Channel _currentVoiceChannel;

        #region Accesseurs
        public DiscordClient Client
        {
            get
            {
                return _client;
            }
        }

        public Server CurrentServer
        {
            get
            {
                return _currentServer;
            }
        }

        public Channel CurrentTextChannel
        {
            get
            {
                return _currentTextChannel;
            }
        }

        public Channel CurrentVoiceChannel
        {
            get
            {
                return _currentVoiceChannel;
            }
        }
        #endregion

        public DiscordBot(DiscordClient client, string username, string pswd)
        {
            _client = client;
            Connect(username, pswd);
        }

        public void Connect(string username, string pswd)
        {
            _client.Run(async () =>
            {
                await _client.Connect(username, pswd);
                Initialize(username, pswd);

                _client.MessageReceived += _client_MessageReceived;
                _client.UserLeft += _client_UserLeft;
                _client.LeftServer += _client_LeftServer;

                Console.WriteLine("Awaiting further commands...");
                Console.ReadLine();
                Environment.Exit(0);
            });
        }

        private void _client_LeftServer(object sender, ServerEventArgs e)
        {
            BotWrite("Someone has left the server!", true);
        }

        private void _client_UserLeft(object sender, UserEventArgs e)
        {
            BotWrite(e.User.Name + " has left the channel!", true);
        }

        private void _client_MessageReceived(object sender, MessageEventArgs e)
        {
            if (!e.Message.IsAuthor)
            {
                string msg = e.Message.RawText;
                string[] arrayMsg = msg.Split(' ');

                if (arrayMsg[0] == "!b" && arrayMsg.Length > 1)
                {

                    switch (arrayMsg[1])
                    {
                        case "hi":
                            BotWrite("Hello!", true);
                            break;
                        default:
                            BotWrite("Unrecognised command.", true);
                            break;
                    }
                }
                else if (e.Message.RawText.Contains("unflip") || e.Message.RawText == "┬─┬﻿ ノ( ゜-゜ノ)")
                    BotWrite("http://www.quickmeme.com/img/cd/cd09c4cccf4ec99e81991aaccb559724343c39c423375ac23895fff452b69e96.jpg");
                else
                {
                    switch (e.Message.Text.ToLower())
                    {
                        case "hi":
                        case "hello":
                        case "bonjour":
                        case "bonsoir":
                        case "allo":
                        case "salut":
                            BotWrite("Hello!", true);
                            break;
                        case "ok":
                            BotWrite("ok. \r\n http://cdn.makeagif.com/media/11-19-2015/re81Us.gif");
                            break;
                    }
                }
            }
        }

        private void Initialize(string username, string pswd)
        {
            _currentServer = _client.AllServers.First(c => c.Name == _SERVER_NAME);
            _currentTextChannel = _currentServer.TextChannels.First();
            _currentVoiceChannel = _currentServer.VoiceChannels.First();

            Role role = _client.FindRoles(_currentServer, "BotBalthyy").First();
            User user = _currentServer.Members.First(u => u.Id == _client.CurrentUserId);

            List<Role> roles = user.Roles.ToList();
            if (!roles.Contains(role))
            {
                roles.Add(role);
                _client.EditUser(user, roles: roles);
            }

            BotWrite("BotBalthyy initialized.");
        }

        public void BotWrite(string message, bool tts = false)
        {
            BotWrite(message, _currentTextChannel, tts);
        }

        public void BotWrite(string message, Channel channel, bool tts = false)
        {
            if (tts)
                _client.SendTTSMessage(channel, message);
            else
                _client.SendMessage(channel, message);
        }

        public void BotWrite(string message, string textChannelName, bool tts = false)
        {
            BotWrite(message, _currentServer.Channels.First(c => c.Name == textChannelName), tts);
        }

        public async void BotPrivateWrite(User user, string message)
        {
            await _client.SendPrivateMessage(user, message);
        }

        public void BotPrivateWrite(List<User> users, string message)
        {
            foreach (User user in users)
                BotPrivateWrite(user, message);
        }

        public void BotPrivateWrite(string username, string message)
        {
            BotPrivateWrite(_currentServer.Members.First(u => u.Name == username), message);
        }
    }
}
