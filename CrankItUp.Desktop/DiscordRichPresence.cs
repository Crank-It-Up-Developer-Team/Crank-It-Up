using DiscordRPC;

namespace CrankItUp.Desktop{

	public class Discord{
    	public DiscordRpcClient client;

    	//Called when your application first starts.
    	//For example, just before your main loop, on OnEnable for unity.
    	public void Initialize()

    	{

    		client = new DiscordRpcClient("1039782791602241567");			
	

    		//Subscribe to events
    		client.OnReady += (sender, e) =>
    		{
    			System.Console.WriteLine("Received Ready from user {0}", e.User.Username);
    		};
	
    		client.OnPresenceUpdate += (sender, e) =>
    		{
    			System.Console.WriteLine("Received Update! {0}", e.Presence);
    		};
	
    		//Connect to the RPC
    		client.Initialize();

    		//Set the rich presence
    		client.SetPresence(new RichPresence()
    		{
    			Details = "",
				Assets = new Assets() {
					LargeImageKey = "logo",
				}

    		});	
    	}
	}
}