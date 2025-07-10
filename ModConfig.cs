namespace BetterBuildingUpgrades;


public sealed class ModConfig
{
   public bool EnableGreenhouseUpgrade { get; set; }
   public bool EnableSiloUpgrade { get; set; }
   public bool EnableWellUpgrade { get; set; }
   public bool EnableStableUpgrade { get; set; }

   public bool RetextureCompatibilityMode { get; set; }
   public bool FrontierFarmCompatibilityMode { get; set; }

   public ModConfig()
   {
      EnableGreenhouseUpgrade = true;
      EnableSiloUpgrade = true;
      EnableWellUpgrade = true;
      EnableStableUpgrade = true;

      RetextureCompatibilityMode = false;
      FrontierFarmCompatibilityMode = false;
   }
}