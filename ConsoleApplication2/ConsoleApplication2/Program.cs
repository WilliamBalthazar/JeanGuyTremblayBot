using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.API;
using Discord.Commands;

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
            DiscordBot bot = new DiscordBot(new DiscordClient(new DiscordClientConfig
            {
                LogLevel = LogMessageSeverity.Debug
            }), _USERNAME, _PSWD);
            
            Console.WriteLine("closing...");
            Console.ReadKey();
        }
    }
}
