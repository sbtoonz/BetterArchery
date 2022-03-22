// Decompiled with JetBrains decompiler
// Type: BetterArchery.Attack_GetProjectileSpawnPoint_Patch
// Assembly: BetterArchery, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5341B858-EB5D-47DA-A64D-602D91E9CB5C
// Assembly location: C:\Users\mugen\Desktop\BetterArchery.dll

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
