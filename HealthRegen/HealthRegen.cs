using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using Microsoft.Extensions.Logging;

namespace HealthRegen;

public class HealthRegen : BasePlugin, IPluginConfig<HealthRegenConfig>
{
    public override string ModuleName => "HealthRegen";

    public override string ModuleDescription => "Regenerates health with a command";

    public override string ModuleVersion => "1.0.0";

    public override string ModuleAuthor => "HerrMagic";

    public HealthRegenConfig Config { get; set; } = new();

    public void OnConfigParsed(HealthRegenConfig config)
    {
        // Save config instance
        Config = config;
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
        if (player.PlayerPawn.Value.Health >= Config.HealthToRegen) {
            player.PrintToChat("You Health is already Full");
            return;
        }

        player.PlayerPawn.Value.Health = Config.HealthToRegen;
        player.PrintToChat($"You Health has been regenerated to {Config.HealthToRegen}");
    }
}
