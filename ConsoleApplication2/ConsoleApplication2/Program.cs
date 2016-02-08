using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.API;
using Discord.Commands;
using System.Configuration;

namespace JeanGuyTremblayBot
{
    class Program
    {

        #region hey
        const string _PSWD = "jgt_bot";
        const string _USERNAME = "williambalthazar@outlook.com";
        #endregion

        static void Main(string[] args)
        {
            //Instancie un bot avec un login et un client
            DiscordBot bot = new DiscordBot(new DiscordClient(new DiscordClientConfig
            {
                LogLevel = LogMessageSeverity.Debug
            }));

            Console.WriteLine("closing...");
            Console.ReadKey();
        }
    }
}
