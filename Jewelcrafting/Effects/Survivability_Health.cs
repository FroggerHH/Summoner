using Jewelcrafting;

namespace Summoner.Gems.Effects;

[HarmonyPatch]
public static class Survivability_Health
{
    [PublicAPI]
    public struct Config
    {
        [MultiplicativePercentagePower] public float Power;
        public string GetLocalizedName() { return "$jc_merged_gemstone_survivabilitygem".Localize(); }
    }

    [HarmonyPatch]
    private class ApplySurvivability
    {
        private static float baseHPValue;

        [HarmonyPatch(typeof(Character), nameof(Character.GetMaxHealth))]
        [HarmonyPostfix]
        private static void ApplySpeedMod(Character __instance, ref float __result)
        {
            if (!Player.m_localPlayer ||
                __instance.IsPlayer() ||
                __instance is not Humanoid ||
                __instance.IsTamed() == false) return;
            baseHPValue = __result;

            var effect = Player.m_localPlayer.GetEffectPower<Config>(survivability);
            __result += effect.Power / 100 * baseHPValue;
        }

        [HarmonyPatch(typeof(Character), nameof(Character.GetHoverText))]
        [HarmonyPostfix]
        private static void ShowHoverSteadfastnessPower(Character __instance, ref string __result)
        {
            if (!Player.m_localPlayer || __instance.IsPlayer() || __instance.IsTamed() == false) return;

            var effect = Player.m_localPlayer.GetEffectPower<Config>(survivability);
            if (effect.Power == 0) return;
            __result +=
                $"\n{effect.GetLocalizedName()}: <color=orange>+{effect.Power}% (+{effect.Power / 100 * baseHPValue} "
                + $"{"$adds_health_survivabilitygem".Localize()})</color>";
        }
    }
}