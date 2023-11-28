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

    [JsonPropertyName("HealthToRegen")] 
    public int HealthToRegen { get; set; } = 100;
}
