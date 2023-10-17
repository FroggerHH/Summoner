using Jewelcrafting;

namespace Summoner.Gems.Effects;

[HarmonyPatch]
public static class Aggression
{
    [PublicAPI]
    public struct Config
    {
        [MaxPower] public float Power;

        public string GetLocalizedName() => "$jc_merged_gemstone_aggressiongem".Localize();
    }

    [HarmonyPatch]
    private class ApplyAggression
    {
        private static ItemData itemData;
        private static float power;

        [HarmonyPatch(typeof(Attack), nameof(Attack.OnAttackTrigger))]
        private static class RegisterAttack
        {
            [HarmonyPrefix]
            private static void Prefix(Attack __instance)
            {
                if (!Player.m_localPlayer || __instance.m_character.IsPlayer() || __instance.m_character.IsTamed() ==
                    false || __instance.m_character is not Humanoid humanoid) return;

                var effect = Player.m_localPlayer.GetEffectPower<Config>(aggression);
                if (effect.Power <= 0) return;
                itemData = __instance.GetWeapon();
                power = effect.Power;
            }

            [HarmonyPostfix]
            private static void Postfix()
            {
                itemData = null;
                power = 0;
            }
        }

        [HarmonyPatch(typeof(ItemData), nameof(ItemData.GetDamage), new Type[0])]
        [HarmonyPostfix]
        private static void AddDamage(ItemData __instance, ref HitData.DamageTypes __result)
        {
            if (itemData == null || power <= 0) return;
            float multiplier = power / 100f;
            __result.m_damage += __result.m_damage * multiplier;
            __result.m_blunt += __result.m_blunt * multiplier;
            __result.m_slash += __result.m_slash * multiplier;
            __result.m_pierce += __result.m_pierce * multiplier;
            __result.m_fire += __result.m_fire * multiplier;
            __result.m_frost += __result.m_frost * multiplier;
            __result.m_lightning += __result.m_lightning * multiplier;
            __result.m_poison += __result.m_poison * multiplier;
            __result.m_spirit += __result.m_spirit * multiplier;
        }
    }

    [HarmonyPatch(typeof(Character), nameof(Character.GetHoverText))]
    private class ShowHoverSteadfastnessPowerOnTamed
    {
        [HarmonyPostfix]
        private static void Postfix(Character __instance, ref string __result)
        {
            if (!Player.m_localPlayer || __instance.IsPlayer() || __instance.IsTamed() == false) return;

            var effect = Player.m_localPlayer.GetEffectPower<Config>(aggression);
            if (effect.Power == 0) return;
            var effectSettStr = "$jc_effect_aggressiongem".Localize().Replace("$1", effect.Power.ToString());
            __result += $"\n{effect.GetLocalizedName()}: <color=orange>{effectSettStr.Localize()}</color>";
        }
    }

    // [HarmonyPatch]
    // private class Targeting
    // {
    //     [HarmonyPatch(typeof(BaseAI), nameof(BaseAI.IsEnemy), typeof(Character), typeof(Character))]
    //     [HarmonyPrefix]
    //     private static bool IsEnemyPostfix(Character a, Character b, ref bool __result)
    //     {
    //         if (a == b) return true;
    //         if (!Player.m_localPlayer || a.IsPlayer() || b.IsPlayer()) return true;
    //         var factionA = a.GetFaction();
    //         var factionB = b.GetFaction();
    //         var isTamedA = a.IsTamed();
    //         var isTamedB = b.IsTamed();
    //         if ((isTamedA && isTamedB) || factionA == Players || factionB == Players) return true;
    //         var effect = Player.m_localPlayer.GetEffectPower<Config>(aggression);
    //         if (effect.Power == 0) return true;
    //
    //         var tamedFaction = isTamedA ? factionA : factionB;
    //         var nonTamedFaction = isTamedA ? factionB : factionA;
    //
    //         if (effect.Power == 1 && nonTamedFaction == AnimalsVeg) __result = true;
    //         else if (effect.Power == 2 && nonTamedFaction != AnimalsVeg) __result = true;
    //         else if (effect.Power == 3) __result = true;
    //         else return true;
    //         return false;
    //     }
    // }
    //
    // [HarmonyPatch]
    // private static class FixTooltip
    // {
    //     [HarmonyPatch(typeof(UITooltip), nameof(UITooltip.LateUpdate))]
    //     //[HarmonyPatch(typeof(ItemData), nameof(ItemData.GetTooltip), typeof(ItemDrop.ItemData), typeof(int), typeof(bool), typeof(float))]
    //     [HarmonyPostfix]
    //     [HarmonyAfter("org.bepinex.plugins.jewelcrafting")]
    //     // private static void ItemDataTooltipPostfix(ref string __result)
    //     private static void UITooltipPostfix(UITooltip __instance)
    //     {
    //         var effectConfig = GetEffectConfig(aggression) ?? new();
    //         for (var i = 1; i <= 10; ++i)
    //             if (UITooltip.m_tooltip?.transform?.Find($"Bkg (1)/TrannyHoles/Transmute_Text_{i}")
    //                     ?.GetComponent<TMP_Text>() is { } transmute)
    //                 foreach (var effectDefs in effectConfig)
    //                 foreach (Aggression.Config config in effectDefs.Power)
    //                 {
    //                     var power = (int)config.Power;
    //                     __instance.m_text = __instance.m_text.Replace($"{aggression} {power}",
    //                         $"$jc_summoner_gems_aggression_{power}".Localize());
    //                     transmute.text = transmute.text.Replace($"{aggression} {power}",
    //                         $"$jc_summoner_gems_aggression_{power}".Localize());
    //                 }
    //     }
    //     
    //         [HarmonyPatch(typeof(ItemData), nameof(ItemData.GetTooltip), typeof(ItemDrop.ItemData), typeof(int), typeof(bool), typeof(float))]
    //     [HarmonyPostfix]
    //     [HarmonyAfter("org.bepinex.plugins.jewelcrafting")]
    //     // private static void ItemDataTooltipPostfix(ref string __result)
    //     private static void ItemDataTooltipPostfix(UITooltip __instance)
    //     {
    //         var effectConfig = GetEffectConfig(aggression) ?? new();
    //         for (var i = 1; i <= 10; ++i)
    //             if (UITooltip.m_tooltip?.transform?.Find($"Bkg (1)/TrannyHoles/Transmute_Text_{i}")
    //                     ?.GetComponent<TMP_Text>() is { } transmute)
    //                 foreach (var effectDefs in effectConfig)
    //                 foreach (Aggression.Config config in effectDefs.Power)
    //                 {
    //                     var power = (int)config.Power;
    //                     __instance.m_text = __instance.m_text.Replace($"{aggression} {power}",
    //                         $"$jc_summoner_gems_aggression_{power}".Localize());
    //                     transmute.text = transmute.text.Replace($"{aggression} {power}",
    //                         $"$jc_summoner_gems_aggression_{power}".Localize());
    //                 }
    //     }
    // }
}