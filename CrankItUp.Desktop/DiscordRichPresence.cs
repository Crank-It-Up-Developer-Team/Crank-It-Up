using DiscordRPC;

namespace CrankItUp.Desktop
{
    public class Discord
    {
        public DiscordRpcClient Client;

        //Called when your application first starts.
        //For example, just before your main loop, on OnEnable for unity.
        public void Initialize()

        {
            Client = new DiscordRpcClient("1039782791602241567");

            //Subscribe to events
            Client.OnReady += (sender, e) => { System.Console.WriteLine("Received Ready from user {0}", e.User.Username); };

            Client.OnPresenceUpdate += (sender, e) => { System.Console.WriteLine("Received Update! {0}", e.Presence); };

            //Connect to the RPC
            Client.Initialize();

            //Set the rich presence
            Client.SetPresence(new RichPresence
            {
                Details = "",
                Assets = new Assets
                {
                    LargeImageKey = "logo",
                }
            });
        }
    }
}
