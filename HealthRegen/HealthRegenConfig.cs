using System.Text.Json.Serialization;
using CounterStrikeSharp.API.Core;
using Microsoft.Extensions.Logging;

namespace HealthRegen;

public class HealthRegenConfig : BasePluginConfig
{
    [JsonPropertyName("IsPluginEnabled")] 
    public bool IsPluginEnabled { get; set; } = true;

    [JsonPropertyName("LogPrefix")] 
    public string LogPrefix { get; set; } = "CSSharp";

	[JsonPropertyName("SetDefaultHealth")]
	public int SetDefaultHealth { get; set; } = 100;

	[JsonPropertyName("HealthToRegen")] 
    public int HealthToRegen { get; set; } = 100;

	//[JsonPropertyName("SlowlyRegenerateHealth")]
	//public bool SlowlyRegenerateHealth { get; set; } = true;

 //   [JsonPropertyName("HealthRegenerateTime")]
	//public int HealthRegenerateTime { get; set; } = 5;
}
