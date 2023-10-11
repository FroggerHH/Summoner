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
    public const string bravery = "BraveryGem";

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


        API.AddGems(type: "Bravery", colorName: bravery, new Color(0.82f, 0.55f, 0.37f));
        API.AddGemEffect<Bravery.Config>("BraveryGem");
        
        StringBuilder sb = new();
        sb.AppendLine($"{bravery}:");
        sb.AppendLine($"  slot: legs"); 
        sb.AppendLine($"  gem: {bravery}");
        sb.AppendLine($"  power: [50, 120, 200]");
        
        string yamlData = sb.ToString();
        string YamlFilePath =
            Path.Combine(Paths.ConfigPath,
                "Jewelcrafting.Sockets_" + Assembly.GetExecutingAssembly().GetName().Name + ".yml");
        if (!File.Exists(YamlFilePath))
        {
            File.Create(YamlFilePath).Dispose();
            File.WriteAllText(YamlFilePath, yamlData);
        }
        
        API.AddGemConfig(yamlData);
        
        Localizer.Load();
        stopwatch.Stop();
        Debug($"{ModName} v{ModVersion} loaded in {stopwatch.ElapsedMilliseconds}ms");
    }
}