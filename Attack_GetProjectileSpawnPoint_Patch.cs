using HarmonyLib;
using UnityEngine;

namespace BetterArchery
{
  [HarmonyPatch(typeof (Attack), "GetProjectileSpawnPoint")]
  public static class Attack_GetProjectileSpawnPoint_Patch
  {
    public static void Postfix(ref Attack __instance, ref Vector3 spawnPoint, ref Vector3 aimDir)
    {
      if (!__instance.m_character.IsPlayer() || !BetterArchery.configArrowImprovementsEnabled.Value)
        return;
      ItemDrop.ItemData ammoItem = Player.m_localPlayer.m_ammoItem;
      if (ammoItem == null || !(bool) (Object) ammoItem.m_shared.m_attack.m_attackProjectile)
        return;
      aimDir += BetterArchery.ArrowAimDir.Value;
    }
  }
}
