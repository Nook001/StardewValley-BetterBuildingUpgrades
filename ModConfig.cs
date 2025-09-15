namespace BetterBuildingUpgrades;


public sealed class ModConfig
{
   // Features
	public bool EnableGreenhouseUpgrade { get; set; } = true;
	public bool EnableSiloUpgrade { get; set; } = true;
	public bool EnableWellUpgrade { get; set; } = true;
	public bool EnableStableUpgrade { get; set; } = true;

	// Compatibility
	public bool RetextureCompatibilityMode { get; set; } = false;
	public bool FrontierFarmCompatibilityMode { get; set; } = false;
}