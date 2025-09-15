using StardewModdingAPI;
using StardewModdingAPI.Events;
using GenericModConfigMenu;


namespace BetterBuildingUpgrades;

public class ModEntry : Mod
{
    public static ModConfig Config { get; private set; } = null!;
    public static IModHelper ModHelper { get; private set; } = null!;
    public static ITranslationHelper Translation { get; private set; } = null!;

    public override void Entry(IModHelper helper)
    {
        ModHelper = helper; // Mod helper
        Translation = helper.Translation;   // Translation
        Config = Helper.ReadConfig<ModConfig>(); // Mod configuration
        RegisterGameEvents(helper); // Register game events
    }

    private void RegisterGameEvents(IModHelper helper)
    {
        // Config
        helper.Events.GameLoop.GameLaunched += RegisterModConfiguration;
        
        // Asset loading
        helper.Events.Content.AssetRequested += ContentManager.LoadTextures;
        helper.Events.Content.AssetRequested += ContentManager.LoadBuildings;
        helper.Events.Content.AssetRequested += ContentManager.LoadMaps;
        
        // Game events
        helper.Events.GameLoop.DayStarted += FeatureManager.OnDayStarted;
        helper.Events.GameLoop.SaveLoaded += FeatureManager.OnSaveLoaded;
        helper.Events.GameLoop.UpdateTicked += FeatureManager.OnUpdateTicked;
        helper.Events.Player.Warped += FeatureManager.OnPlayerWarped;
    }


    // Register the mod configuration with Generic Mod Config Menu
    private void RegisterModConfiguration(object? sender, GameLaunchedEventArgs e)
    {
        // get Generic Mod Config Menu's API (if it's installed)
        var configMenu = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
        if (configMenu is null) return;

        // register mod
        configMenu.Register(ModManifest, () => Config = new ModConfig(), () => Helper.WriteConfig(Config));

        // method to add a toggle option to the menu
        void AddToggle(string translationKey, Func<bool> getter, Action<bool> setter) =>
            configMenu.AddBoolOption(ModManifest, getter, setter, () => Translation.Get(translationKey));
        
        // Add all config menu options
        AddToggle("config.EnableGreenhouseUpgrade", () => Config.EnableGreenhouseUpgrade, value => Config.EnableGreenhouseUpgrade = value);
        AddToggle("config.EnableSiloUpgrade", () => Config.EnableSiloUpgrade, value => Config.EnableSiloUpgrade = value);
        AddToggle("config.EnableWellUpgrade", () => Config.EnableWellUpgrade, value => Config.EnableWellUpgrade = value);
        AddToggle("config.EnableStableUpgrade", () => Config.EnableStableUpgrade, value => Config.EnableStableUpgrade = value);
        AddToggle("config.RetextureCompatibilityMode", () => Config.RetextureCompatibilityMode, value => Config.RetextureCompatibilityMode = value);
        AddToggle("config.FrontierFarmCompatibilityMode", () => Config.FrontierFarmCompatibilityMode, value => Config.FrontierFarmCompatibilityMode = value);
    }
}
