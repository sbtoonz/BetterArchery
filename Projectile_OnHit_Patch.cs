// Decompiled with JetBrains decompiler
// Type: BetterArchery.Projectile_OnHit_Patch
// Assembly: BetterArchery, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5341B858-EB5D-47DA-A64D-602D91E9CB5C
// Assembly location: C:\Users\mugen\Desktop\BetterArchery.dll

using HarmonyLib;
using UnityEngine;

namespace BetterArchery
{
  [HarmonyPatch(typeof (Projectile), "OnHit")]
  public static class Projectile_OnHit_Patch
  {
    public static void Prefix(
      Projectile __instance,
      Collider collider,
      Vector3 hitPoint,
      bool water)
    {
      if (BetterArchery.ArrowRetrieveOldVersion.Value || !(bool) (Object) __instance.gameObject.GetComponent<Pickable>())
        return;
      GameObject gameObject = (bool) (Object) collider ? Projectile.FindHitObject(collider) : (GameObject) null;
      bool hitCharacter = false;
      IDestructible destr = (bool) (Object) gameObject ? gameObject.GetComponent<IDestructible>() : (IDestructible) null;
      if (destr != null && !__instance.IsValidTarget(destr, ref hitCharacter))
        return;
      BoxCollider boxCollider = !(__instance.name == "bow_projectile_needle(Clone)") ? (!(__instance.name == "bow_projectile_frost(Clone)") ? __instance.transform.GetChild(0).gameObject.AddComponent<BoxCollider>() : __instance.transform.GetChild(3).gameObject.AddComponent<BoxCollider>()) : __instance.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
      if (!((Object) boxCollider != (Object) null))
        return;
      boxCollider.enabled = true;
    }
  }
}
