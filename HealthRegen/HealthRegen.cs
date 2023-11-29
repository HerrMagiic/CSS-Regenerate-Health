using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Core.Attributes;

namespace HealthRegen;

[MinimumApiVersion(78)]
public class HealthRegen : BasePlugin, IPluginConfig<HealthRegenConfig>
{
    public override string ModuleName => "HealthRegen";

    public override string ModuleDescription => "Regenerates health with a command";

    public override string ModuleVersion => "1.0.1";

    public override string ModuleAuthor => "HerrMagic";

    public HealthRegenConfig Config { get; set; } = new();

    private List<PlayerConfigModel>? PlayerConfig = new();

	public void OnConfigParsed(HealthRegenConfig config)
    {
        // Save config instance
        Config = config;
    }

    private void WriteConfigFile()
    {
        string path = Path.Join(ModuleDirectory, "PlayerConfig.json");

		if (!File.Exists(path))
        {
			File.WriteAllText(path,
				"[" +
	            "    {" +
	            "        \"SteamID\": \"STEAM_0:0:209830255\"," +
	            "        \"Health\": 100" +
	            "    }," +
	            "    {" +
	            "        \"SteamID\": \"STEAM_0:0:1098305\"," +
	            "        \"Health\": 30" +
	            "    }" +
	            "]");
		}
		var json = System.IO.File.ReadAllText(path);

        List<PlayerConfigModel>? objects = JsonSerializer.Deserialize<List<PlayerConfigModel>>(json);

	}

    public override void Load(bool hotReload)
    {
		if (!Config.IsPluginEnabled)
		{
			Logger.LogWarning($"{Config.LogPrefix} {ModuleName} is disabled");
			return;
		}
		Logger.LogInformation(
                $"HealthRegen Plugin has been loaded, and the hot reload flag was {hotReload}, path is {ModulePath}");

        WriteConfigFile();
	}

    [GameEventHandler]
    public HookResult OnPlayerSpawn(EventPlayerSpawn @event, GameEventInfo info)
    {
        if (!@event.Userid.IsValid)
            return HookResult.Continue;

        SteamID id = new SteamID(@event.Userid.SteamID);

		if (PlayerConfig.Any(p => p.SteamID == id.SteamId2 
            || Convert.ToInt32(p.SteamID) == id.SteamId32 
            || (ulong)Convert.ToInt32(p.SteamID) == id.SteamId64
            || p.SteamID == id.SteamId3))
        {
            int health = PlayerConfig.Find(p => p.SteamID == id.SteamId2
		        || Convert.ToInt32(p.SteamID) == id.SteamId32
		        || (ulong)Convert.ToInt32(p.SteamID) == id.SteamId64
		        || p.SteamID == id.SteamId3).Health;

            @event.Userid.PlayerPawn.Value.MaxHealth = health;
			@event.Userid.PlayerPawn.Value.Health = health;
			@event.Userid.MaxHealth = health;
			@event.Userid.Health = health;

			return HookResult.Continue;
        }
		@event.Userid.PlayerPawn.Value.MaxHealth = Config.SetDefaultHealth;
		@event.Userid.PlayerPawn.Value.Health = Config.SetDefaultHealth;

		return HookResult.Continue;
    }

	[ConsoleCommand("regen", "This regenerates your health")]
	[ConsoleCommand("heal", "This regenerates your health")]
	[ConsoleCommand("medic", "This regenerates your health")]
	[CommandHelper(whoCanExecute: CommandUsage.CLIENT_ONLY)]

	public void OnCommand(CCSPlayerController? player, CommandInfo command)
    {
        if (player == null) {
            Logger.LogError("Player is null");
            return;
        }
        if (player.PlayerPawn.Value.Health >= player.PlayerPawn.Value.MaxHealth) {
            player.PrintToChat("You Health is already Full");
            return;
        }


        //if (!Config.SlowlyRegenerateHealth)
        //{
		player.PlayerPawn.Value.Health = player.PlayerPawn.Value.MaxHealth;
		player.Health = player.MaxHealth;
		player.PrintToChat($"You Health has been regenerated to {Config.HealthToRegen}");
            //return;
		//}
        
    }
}
