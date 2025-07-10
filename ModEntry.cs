using StardewModdingAPI;
using StardewModdingAPI.Events;
using GenericModConfigMenu;


namespace BetterBuildingUpgrades;

public class ModEntry : Mod
{
    // Config
    public static ModConfig Config;
    public static IModHelper ModHelper { get; private set; }
    public static ITranslationHelper Translation { get; private set; }


    public override void Entry(IModHelper helper)
    {
        ModHelper = helper;
        // Load translation
        Translation = helper.Translation;
        // Load mod configuration
        Config = Helper.ReadConfig<ModConfig>();
        // Register event handlers
        RegisterAllEvents(helper);
    }

    private void RegisterAllEvents(IModHelper helper)
    {
        // Config
        helper.Events.GameLoop.GameLaunched += RegisterModConfiguration;
        
        // Asset loading
        helper.Events.Content.AssetRequested += AssetManager.LoadTextures;
        helper.Events.Content.AssetRequested += AssetManager.LoadBuildings;
        helper.Events.Content.AssetRequested += AssetManager.LoadGreenhouseMap;
        
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
        configMenu.Register(
            mod: ModManifest,
            reset: () => Config = new ModConfig(),
            save: () => Helper.WriteConfig(Config)
        );

        void AddToggle(string translationKey, Func<bool> getter, Action<bool> setter)
        {
            configMenu.AddBoolOption(
                mod: ModManifest,
                name: () => Translation.Get(translationKey),
                getValue: getter,
                setValue: setter
            );
        }

        // Add all options
        AddToggle("config.EnableGreenhouseUpgrade", () => Config.EnableGreenhouseUpgrade, value => Config.EnableGreenhouseUpgrade = value);
        AddToggle("config.EnableSiloUpgrade", () => Config.EnableSiloUpgrade, value => Config.EnableSiloUpgrade = value);
        AddToggle("config.EnableWellUpgrade", () => Config.EnableWellUpgrade, value => Config.EnableWellUpgrade = value);
        AddToggle("config.EnableStableUpgrade", () => Config.EnableStableUpgrade, value => Config.EnableStableUpgrade = value);
        AddToggle("config.RetextureCompatibilityMode", () => Config.RetextureCompatibilityMode, value => Config.RetextureCompatibilityMode = value);
        AddToggle("config.FrontierFarmCompatibilityMode", () => Config.FrontierFarmCompatibilityMode, value => Config.FrontierFarmCompatibilityMode = value);
    }
}
