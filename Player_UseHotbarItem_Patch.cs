// Decompiled with JetBrains decompiler
// Type: BetterArchery.Player_UseHotbarItem_Patch
// Assembly: BetterArchery, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5341B858-EB5D-47DA-A64D-602D91E9CB5C
// Assembly location: C:\Users\mugen\Desktop\BetterArchery.dll

using HarmonyLib;
using UnityEngine;

namespace BetterArchery
{
  [HarmonyPatch(typeof (Player), "UseHotbarItem")]
  public static class Player_UseHotbarItem_Patch
  {
    public static bool Prefix(Player __instance) => !BetterArchery.configQuiverEnabled.Value || !(BetterArchery.HoldingKeyCode.Value.MainKey.ToString() != "") || !Input.GetKey(BetterArchery.HoldingKeyCode.Value.MainKey);
  }
}
