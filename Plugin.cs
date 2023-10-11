using System.IO;
using System.Reflection;
using BepInEx;
using Jewelcrafting;
using LocalizationManager;
using Summoner.Gems.Effects;

namespace Summoner;

[BepInDependency("org.bepinex.plugins.jewelcrafting")]
[BepInPlugin(ModGUID, ModName, ModVersion)]
public class Plugin : BaseUnityPlugin
{
    public const string bravery = "Bravery";

    private const string
        ModName = "Summoner",
        ModVersion = "1.0.0",
        ModGUID = $"com.{ModAuthor}.{ModName}",
        ModAuthor = "Frogger";

    public static ConfigEntry<bool> enabledConfig;

    private void Awake()
    {
        var stopwatch = Stopwatch.StartNew();
        CreateMod(this, ModName, ModAuthor, ModVersion);
        enabledConfig = config("General", "Enabled", true, "");

        Localizer.Load();

        API.AddGems(type: bravery, colorName: $"BraveryGem", new Color(0.82f, 0.55f, 0.37f));
        API.AddGemEffect<Bravery.Config>(bravery, "Increases speed of your summons.",
            "Summons speed increased by $1%.");
        // StringBuilder sb = new();
        // sb.AppendLine($"{bravery}:");
        // sb.AppendLine("  slot: legs"); 
        // sb.AppendLine("  gem: yellow");
        // sb.AppendLine("  power: [100, 150, 200]");
        //
        // string yamlData = sb.ToString();
        // string YamlFilePath =
        //     Path.Combine(Paths.ConfigPath,
        //         "Jewelcrafting.Sockets_" + Assembly.GetExecutingAssembly().GetName().Name + ".yml");
        // if (!File.Exists(YamlFilePath))
        // {
        //     File.Create(YamlFilePath).Dispose();
        //     File.WriteAllText(YamlFilePath, yamlData);
        // }
        //
        // API.AddGemConfig(yamlData);
        
        stopwatch.Stop();
        Debug($"{ModName} v{ModVersion} loaded in {stopwatch.ElapsedMilliseconds}ms");
    }
}