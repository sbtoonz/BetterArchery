// Decompiled with JetBrains decompiler
// Type: BetterArchery.Player_SetControls_Patch
// Assembly: BetterArchery, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5341B858-EB5D-47DA-A64D-602D91E9CB5C
// Assembly location: C:\Users\mugen\Desktop\BetterArchery.dll

using HarmonyLib;
using UnityEngine;

namespace BetterArchery.Patches
{
  [HarmonyPatch(typeof (Player), "SetControls")]
  public static class Player_SetControls_Patch
  {
    private static void Prefix(
      Player __instance,
      ref Vector3 movedir,
      ref bool attack,
      ref bool attackHold,
      ref bool secondaryAttack,
      ref bool block,
      ref bool blockHold,
      ref bool jump,
      ref bool crouch,
      ref bool run,
      ref bool autoRun)
    {
      if (attackHold)
      {
        if (Input.GetKey(BetterArchery.BowDrawCancelKey.Value.MainKey))
          blockHold = true;
        else
          blockHold = false;
      }
      else
      {
        ItemDrop.ItemData leftItem = __instance.GetLeftItem();
        if (leftItem != null && leftItem.m_dropPrefab.name.Contains("Bow") && BetterArchery.__ZoomState == BetterArchery.ZoomState.ZoomingIn)
          blockHold = false;
      }
    }
  }
}
