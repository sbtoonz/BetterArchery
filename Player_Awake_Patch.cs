// Decompiled with JetBrains decompiler
// Type: BetterArchery.Player_Awake_Patch
// Assembly: BetterArchery, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5341B858-EB5D-47DA-A64D-602D91E9CB5C
// Assembly location: C:\Users\mugen\Desktop\BetterArchery.dll

using HarmonyLib;
using UnityEngine;

namespace BetterArchery
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
