// Decompiled with JetBrains decompiler
// Type: BetterArchery.RetrieveArrows
// Assembly: BetterArchery, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5341B858-EB5D-47DA-A64D-602D91E9CB5C
// Assembly location: C:\Users\mugen\Desktop\BetterArchery.dll

using System;
using HarmonyLib;
using UnityEngine;

namespace BetterArchery.Common
{
  [HarmonyPatch(typeof (Projectile), "Setup")]
  public static class RetrieveArrows
  {
    public static void Prefix(
      ref Projectile __instance,
      Character owner,
      Vector3 velocity,
      float hitNoise,
      HitData hitData,
      ItemDrop.ItemData item)
    {
      if (!owner.IsPlayer() || item == null || item.m_shared.m_itemType != ItemDrop.ItemData.ItemType.Bow || !__instance.name.Contains("bow_projectile"))
        return;
      if (BetterArchery.configArrowImprovementsEnabled.Value)
        __instance.m_gravity = BetterArchery.ArrowGravity.Value;
      if (!BetterArchery.configRetrievableArrowsEnabled.Value)
        return;
      Player localPlayer = Player.m_localPlayer;
      if ((UnityEngine.Object) localPlayer == (UnityEngine.Object) null)
        return;
      string arrowType = localPlayer.GetAmmoItem().m_dropPrefab.name;
      if (string.IsNullOrEmpty(arrowType))
        return;
      Arrow arrow = BetterArchery.ArrowRetrieves.Find((Predicate<Arrow>) (e => e.Name == arrowType));
      if (arrow == null)
        return;
      float num = UnityEngine.Random.Range(0.0f, 1f);
      BetterArchery.Log(string.Format("m_dropPrefab: {0}, x.Name: {1}, x.SpawnChance: {2}, x.SpawnArrow: {3}", (object) arrowType, (object) arrow.Name, (object) arrow.SpawnChance, (object) arrow.SpawnArrow));
      BetterArchery.Log(string.Format("spawnChance: {0}, currentArrow.SpawnChance: {1}", (object) num, (object) arrow.SpawnChance));
      if (BetterArchery.ArrowRetrieveOldVersion.Value)
      {
        if ((double) num < (double) arrow.SpawnChance)
        {
          GameObject itemPrefab = ObjectDB.instance.GetItemPrefab(arrow.SpawnArrow);
          __instance.m_spawnOffset = new Vector3(0.5f, 0.05f, -0.5f);
          __instance.m_spawnOnHitChance = 1f;
          __instance.m_spawnOnHit = itemPrefab;
          __instance.m_hideOnHit = __instance.gameObject;
        }
        else
        {
          __instance.m_spawnOnHitChance = 0.0f;
          __instance.m_ttl = 60f;
        }
      }
      else
      {
        if ((double) num < (double) arrow.SpawnChance)
        {
          GameObject itemPrefab = ObjectDB.instance.GetItemPrefab(arrow.SpawnArrow);
          Pickable pickable = __instance.gameObject.AddComponent<Pickable>();
          pickable.m_hideWhenPicked = __instance.gameObject;
          pickable.m_amount = 1;
          pickable.m_itemPrefab = itemPrefab;
        }
        else if (BetterArchery.ArrowDisappearOnHit.Value)
          __instance.m_hideOnHit = __instance.gameObject;
        else if ((bool) (UnityEngine.Object) __instance.GetComponentInChildren<MeshRenderer>())
          __instance.GetComponentInChildren<MeshRenderer>().material.color = Color.black;
        __instance.m_ttl = BetterArchery.ArrowDisappearTime.Value;
      }
    }
  }
}
