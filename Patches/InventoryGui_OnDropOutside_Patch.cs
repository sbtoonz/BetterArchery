// Decompiled with JetBrains decompiler
// Type: BetterArchery.InventoryGui_OnDropOutside_Patch
// Assembly: BetterArchery, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5341B858-EB5D-47DA-A64D-602D91E9CB5C
// Assembly location: C:\Users\mugen\Desktop\BetterArchery.dll

using HarmonyLib;
using UnityEngine;

namespace BetterArchery.Patches
{
  [HarmonyPatch(typeof (InventoryGui), "OnDropOutside")]
  public static class InventoryGui_OnDropOutside_Patch
  {
    public static bool Prefix(GameObject ___m_dragGo, ItemDrop.ItemData ___m_dragItem)
    {
      if (___m_dragItem is not { m_equiped: true } || !BetterArchery.configQuiverEnabled.Value)
        return true;
      MessageHud.instance.ShowMessage(MessageHud.MessageType.Center, "Unequip the item first.");
      return false;
    }
  }
}
