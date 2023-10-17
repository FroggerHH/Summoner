using Jewelcrafting;

namespace Summoner.Gems.Effects;

[HarmonyPatch]
public static class Steadfastness_Armor
{
    [PublicAPI]
    public struct Config
    {
        [MultiplicativePercentagePower] public float Power;
        public string GetLocalizedName() => "$jc_merged_gemstone_steadfastnessgem".Localize();
    }

    [HarmonyPatch]
    private class ApplySteadfastness
    {
        private static float baseArmorValue;

        [HarmonyPatch(typeof(Character), nameof(Character.GetBodyArmor))]
        [HarmonyPostfix]
        private static void ApplySpeed(Character __instance, ref float __result)
        {
            if (!Player.m_localPlayer || __instance.IsPlayer()
                                      || __instance is not Humanoid
                                      || __instance.IsTamed() == false)
                return;
            baseArmorValue = __result;

            var effect = Player.m_localPlayer.GetEffectPower<Config>(steadfastness);
            __result += effect.Power / 100 * (baseArmorValue == 0 ? 1 : baseArmorValue);
        }

        [HarmonyPatch(typeof(Character), nameof(Character.GetHoverText))]
        [HarmonyPostfix]
        private static void ShowHover(Character __instance, ref string __result)
        {
            if (!Player.m_localPlayer || __instance.IsPlayer() || __instance.IsTamed() == false) return;

            var effect = Player.m_localPlayer.GetEffectPower<Config>(steadfastness);
            if (effect.Power == 0) return;
            __result +=
                $"\n{effect.GetLocalizedName()}: <color=orange>+{effect.Power}% (+{effect.Power / 100 * (baseArmorValue == 0 ? 1 : baseArmorValue)} "
                + $"{"$adds_armor_steadfastnessgem".Localize()})</color>";
        }
    }
}