using HarmonyLib;

namespace BetterArchery
{
  [HarmonyPatch(typeof (Attack), "FireProjectileBurst")]
  public static class Attack_FireProjectileBurst_Patch
  {
    public static bool Prefix(ref Attack __instance)
    {
      if (!__instance.m_character.IsPlayer() || !BetterArchery.configArrowImprovementsEnabled.Value)
        return true;
      ItemDrop.ItemData ammoItem = Player.m_localPlayer.m_ammoItem;
      ItemDrop.ItemData leftItem = Player.m_localPlayer.GetLeftItem();
      if (leftItem == null || leftItem.m_shared.m_itemType != ItemDrop.ItemData.ItemType.Bow || ammoItem == null)
        return true;
      __instance.m_projectileVel = BetterArchery.ArrowVelocity.Value;
      if ((double) BetterArchery.ArrowAccuracy.Value >= 0.0)
        __instance.m_projectileAccuracy = BetterArchery.ArrowAccuracy.Value;
      return true;
    }
  }
}
