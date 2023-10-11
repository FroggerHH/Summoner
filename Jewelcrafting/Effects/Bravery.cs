using JetBrains.Annotations;
using Jewelcrafting;

namespace Summoner.Gems.Effects;

[HarmonyPatch]
public static class Bravery
{
    private static readonly int hash = bravery.GetStableHashCode();

    [PublicAPI]
    public struct Config
    {
        [MultiplicativePercentagePower] public float Power;
    }

    [HarmonyPatch(typeof(SEMan), nameof(SEMan.ApplyStatusEffectSpeedMods))]
    private class ApplySpeedMod
    {
        [HarmonyPostfix]
        private static void Postfix(SEMan __instance, ref float speed)
        {
            if (!Player.m_localPlayer || !__instance.m_character || __instance.m_character.IsPlayer()
                || __instance.m_character is not Humanoid humanoid) return;
            if (humanoid.IsTamed() == false) return;
            float baseSpeed = speed;


            var BraveryEffect = Player.m_localPlayer.GetEffectPower<Config>(bravery);
            speed += BraveryEffect.Power / 100 * baseSpeed;
        }
    }

    [HarmonyPatch(typeof(Character), nameof(Character.GetHoverText))]
    private class ShowHoverBraveryPower
    {
        [HarmonyPostfix]
        private static void Postfix(Character __instance, ref string __result)
        {
            if (!Player.m_localPlayer || __instance.IsPlayer() || __instance.IsTamed() == false) return;


            var BraveryEffect = Player.m_localPlayer.GetEffectPower<Config>(bravery);
            __result += $"{bravery} Power: {BraveryEffect.Power}";
        }
    }
}