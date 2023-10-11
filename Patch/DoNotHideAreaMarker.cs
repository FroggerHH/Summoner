namespace Summoner.Patch;

[HarmonyPatch]
public static class DoNotHideAreaMarker
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(PrivateArea), nameof(PrivateArea.HideMarker))]
    private static bool PlayerFVXLevelUpPatch() => !enabledConfig.Value;
}