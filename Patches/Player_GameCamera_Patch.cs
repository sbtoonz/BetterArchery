// Decompiled with JetBrains decompiler
// Type: BetterArchery.Player_GameCamera_Patch
// Assembly: BetterArchery, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5341B858-EB5D-47DA-A64D-602D91E9CB5C
// Assembly location: C:\Users\mugen\Desktop\BetterArchery.dll

using HarmonyLib;
using UnityEngine;

namespace BetterArchery.Patches
{
  [HarmonyPatch(typeof (GameCamera), "GetCameraPosition")]
  public static class Player_GameCamera_Patch
  {
    [HarmonyPriority(100)]
    private static void Postfix(GameCamera __instance, float dt, Vector3 pos, Quaternion rot)
    {
      if ((Object) __instance == (Object) null || !BetterArchery.configBowZoomEnabled.Value || (double) BetterArchery.__BaseFov != 0.0)
        return;
      BetterArchery.__BaseFov = __instance.m_fov;
    }
  }
}
