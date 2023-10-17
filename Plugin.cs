using System.IO;
using System.Reflection;
using BepInEx;
using Jewelcrafting;
using Jewelcrafting.GemEffects;
using LocalizationManager;
using Summoner.Gems.Effects;

namespace Summoner;

[BepInDependency("org.bepinex.plugins.jewelcrafting")]
[BepInPlugin(ModGUID, ModName, ModVersion)]
public class Plugin : BaseUnityPlugin
{
    public const string bravery = "BraveryGem";
    public const string steadfastness = "SteadfastnessGem";
    public const string survivability = "SurvivabilityGem";
    public const string aggression = "AggressionGem";

    private const string
        ModName = "Summoner",
        ModVersion = "1.0.0",
        ModGUID = $"com.{ModAuthor}.{ModName}",
        ModAuthor = "Frogger";

    private void Awake()
    {
        var stopwatch = Stopwatch.StartNew();
        CreateMod(this, ModName, ModAuthor, ModVersion, ModGUID);


        API.AddGems("Bravery", bravery, new Color(0.82f, 0.64f, 0.21f));
        API.AddGems("Steadfastness", steadfastness, new Color(0.73f, 0.82f, 0.61f));
        API.AddGems("Survivability", survivability, new Color(0.16f, 0.82f, 0.43f));
        API.AddGems("Aggression", aggression, new Color(0.65f, 0.3f, 0.23f));
        API.AddGemEffect<Bravery_Speed.Config>(bravery);
        API.AddGemEffect<Steadfastness_Armor.Config>(steadfastness);
        API.AddGemEffect<Survivability_Health.Config>(survivability);
        API.AddGemEffect<Aggression.Config>(aggression);
        StringBuilder sb = new();
        sb.AppendLine();
        sb.AppendLine($"{bravery}:");
        sb.AppendLine("  slot: legs");
        sb.AppendLine($"  gem: {bravery}");
        sb.AppendLine("  power: [80, 120, 130]");
        sb.AppendLine($"{bravery}:");
        sb.AppendLine("  slot: cloak");
        sb.AppendLine($"  gem: {bravery}");
        sb.AppendLine("  power: [20, 100, 200]");
        sb.AppendLine();
        sb.AppendLine($"{steadfastness}:");
        sb.AppendLine($"  gem: {steadfastness}");
        sb.AppendLine("  slot: [shield]");
        sb.AppendLine("  power: [25, 50, 120]");
        sb.AppendLine($"{steadfastness}:");
        sb.AppendLine($"  gem: {steadfastness}");
        sb.AppendLine("  slot: [chest]");
        sb.AppendLine("  power: [25, 50, 100]");
        sb.AppendLine($"{steadfastness}:");
        sb.AppendLine($"  gem: {steadfastness}");
        sb.AppendLine("  slot: [head, legs]");
        sb.AppendLine("  power: [20, 30, 50]");
        sb.AppendLine();
        sb.AppendLine($"{survivability}:");
        sb.AppendLine("  slot: [chest, head, legs]");
        sb.AppendLine($"  gem: {survivability}");
        sb.AppendLine("  power: [30, 80, 150]");
        sb.AppendLine();
        sb.AppendLine($"{aggression}:");
        sb.AppendLine("  slot: [weapon]");
        sb.AppendLine($"  gem: {aggression}");
        sb.AppendLine("  power: [25, 80, 150]");
        sb.AppendLine("  unique: Gem");
        sb.AppendLine();

        var yamlData = sb.ToString();
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

    internal static List<EffectDef> GetEffectConfig(string effect)
    {
        Jewelcrafting.Jewelcrafting.SocketEffects.TryGetValue((Effect)effect.GetStableHashCode(), out var result);
        return result;
    }
}