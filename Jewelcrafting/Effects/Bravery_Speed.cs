using Jewelcrafting;

namespace Summoner.Gems.Effects;

[HarmonyPatch]
public static class Bravery_Speed
{
    [PublicAPI]
    public struct Config
    {
        [MultiplicativePercentagePower] public float Power;
        public string GetLocalizedName() { return "$jc_merged_gemstone_braverygem".Localize(); }
    }

    [HarmonyPatch]
    private class ApplyBravery
    {
        private static float baseSpeedValue;

        [HarmonyPatch(typeof(SEMan), nameof(SEMan.ApplyStatusEffectSpeedMods))]
        [HarmonyPostfix]
        private static void ApplySpeed(SEMan __instance, ref float speed)
        {
            if (!Player.m_localPlayer || !__instance.m_character || __instance.m_character.IsPlayer()
                || __instance.m_character is not Humanoid humanoid) return;
            if (humanoid.IsTamed() == false) return;
            baseSpeedValue = speed;

            var effect = Player.m_localPlayer.GetEffectPower<Config>(bravery);
            speed += effect.Power / 100 * baseSpeedValue;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Character), nameof(Character.GetHoverText))]
        private static void ShowHover(Character __instance, ref string __result)
        {
            if (!Player.m_localPlayer || __instance.IsPlayer() || __instance.IsTamed() == false) return;

            var effect = Player.m_localPlayer.GetEffectPower<Config>(bravery);
            if (effect.Power == 0) return;
            __result +=
                $"\n{effect.GetLocalizedName()}: <color=orange>+{effect.Power}% (+{effect.Power / 100 * baseSpeedValue} "
                + $"{"$adds_speed_braverygem".Localize()})</color>";
        }
    }
}