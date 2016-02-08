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
using System.Threading;
using System.Configuration;

namespace JeanGuyTremblayBot
{
    /// <summary>
    /// Représente un bot d'utilité et d'humour pour Discord
    /// </summary>
    class DiscordBot
    {
        /// <summary>
        /// Représente le temps donnée en millisecondes
        /// </summary>
        public enum Temps
        {
            _15_MINUTES = 900000,
            _30_MINUTES = 1800000,
            _60_MINUTES = 3600000
        }

        private DiscordClient _client;
        private Server _currentServer;
        private Channel _currentTextChannel;
        private Channel _currentVoiceChannel;

        private Timer _timer;
        AppSettingsReader _reader;

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

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="client">Client Discord utilisé</param>
        /// <param name="username">Email de connection</param>
        /// <param name="pswd">Mot de passe du compte</param>
        public DiscordBot(DiscordClient client)
        {
            _client = client;
            Connect();
        }

        /// <summary>
        /// Callback appelé lors du tick du _timer
        /// </summary>
        /// <param name="o"></param>
        private void TimerCallBack(object o)
        {
            BotWrite("Welcome to the server that does not belong to Jean-Guy Tremblay! I am BotBalthyy. Feel free to use !b help for further info!");

            GC.Collect();
        }

        /// <summary>
        /// Démarre le client et connecte le bot au serveur Discord.
        /// </summary>
        /// <param name="username">Email de connection</param>
        /// <param name="pswd">Mot de passe du compte</param>
        public void Connect()
        {
            _reader = new AppSettingsReader();

            _client.Run(async () =>
            {
                await _client.Connect(_reader.GetValue("BotUsername", typeof(string)).ToString(),
                    _reader.GetValue("BotPassword", typeof(string)).ToString());

                Initialize();

                _client.MessageReceived += _client_MessageReceived;

                Console.WriteLine("Awaiting further commands...");
                Console.ReadLine();
                Environment.Exit(0);
            });
        }

        /// <summary>
        /// Évènement trigger lorsqu'un message est reçu par le bot
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                        case "help":
                            BotWrite("Jean-Guy Tremblay Discord Bot Helper! \r\n !b help -> Help \r\n !b hi -> Say hi! \r\n Random easter eggs all around!");
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

        /// <summary>
        /// Initialise les infos nécessaires au fonctionnement du bot ainsi que le _timer
        /// </summary>
        private void Initialize()
        {
            _currentServer = _client.AllServers.First(c => c.Name == _reader.GetValue("MainServerName", typeof(string)).ToString());
            _currentTextChannel = _currentServer.TextChannels.First(t => t.Name == _reader.GetValue("MainTextChannel", typeof(string)).ToString());
            _currentVoiceChannel = _currentServer.VoiceChannels.First(t => t.Name == _reader.GetValue("BotVoiceChannel", typeof(string)).ToString());

            BotWrite("BotBalthyy initialized.");

            _timer = new Timer(TimerCallBack, null, 0, Convert.ToInt32(Temps._15_MINUTES));
        }

        /// <summary>
        /// Écrit un message dans le serveur et le TextChannel courant.
        /// </summary>
        /// <param name="message">Message à envoyer</param>
        /// <param name="tts">Text To Speech</param>
        public void BotWrite(string message, bool tts = false)
        {
            BotWrite(message, _currentTextChannel, tts);
        }

        /// <summary>
        /// Écrit un message dans le serveur courant, dans le textChannel spécifié.
        /// </summary>
        /// <param name="message">Message à envoyer</param>
        /// <param name="channel">TextChannel cible</param>
        /// <param name="tts">Text To Speech</param>
        public void BotWrite(string message, Channel channel, bool tts = false)
        {
            if (tts)
                _client.SendTTSMessage(channel, message);
            else
                _client.SendMessage(channel, message);
        }

        /// <summary>
        /// Écrit un message dans le serveur courant, dans le TextChannel spécifié par le nom.
        /// </summary>
        /// <param name="message">Message à envoyer</param>
        /// <param name="textChannelName">Nom du TextChannel cible</param>
        /// <param name="tts">Text To Speech</param>
        public void BotWrite(string message, string textChannelName, bool tts = false)
        {
            try
            {
                BotWrite(message, _currentServer.Channels.First(c => c.Name == textChannelName), tts);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Envoit un message privé à l'utilisateur ciblé.
        /// </summary>
        /// <param name="user">Utilisateur cible</param>
        /// <param name="message">Message à envoyer</param>
        public async void BotPrivateWrite(User user, string message)
        {
            await _client.SendPrivateMessage(user, message);
        }

        /// <summary>
        /// Envoit un message privé à tous les utilisateurs spécifiés.
        /// </summary>
        /// <param name="users">Utilisateurs cibles</param>
        /// <param name="message">Message à envoyer</param>
        public void BotPrivateWrite(List<User> users, string message)
        {
            foreach (User user in users)
                BotPrivateWrite(user, message);
        }

        /// <summary>
        /// Envoit un message privé à tous les utilisateurs du serveur courant.
        /// </summary>
        /// <param name="message">Message à envoyer</param>
        public void BotPrivateWrite(string message)
        {
            foreach (User user in _currentServer.Members)
                BotPrivateWrite(user, message);
        }

        /// <summary>
        /// Envoit un message privé à l'utilisateur ciblé par son nom.
        /// </summary>
        /// <param name="username">Nom de l'utilisateur cible</param>
        /// <param name="message">Message à envoyer</param>
        public void BotPrivateWrite(string username, string message)
        {
            try
            {
                BotPrivateWrite(_currentServer.Members.First(u => u.Name == username), message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
