using HarmonyLib;

namespace BetterArchery.Patches
{
  [HarmonyPatch(typeof (Player), "Awake")]
  public static class Player_Awake_Patch
  {
    [HarmonyPriority(0)]
    private static void Prefix(Player __instance)
    {
      if (!BetterArchery.configQuiverEnabled.Value)
        return;
      __instance.m_inventory.m_height += 2;
      BetterArchery.Log(string.Format("Inventory h: {0}", (object) __instance.m_inventory.m_height));
      BetterArchery.QuiverRowIndex = __instance.m_inventory.m_height - 1;
    }
  }
}
