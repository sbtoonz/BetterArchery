// Decompiled with JetBrains decompiler
// Type: BetterArchery.InventoryGrid_OnLeftClick_Patch
// Assembly: BetterArchery, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5341B858-EB5D-47DA-A64D-602D91E9CB5C
// Assembly location: C:\Users\mugen\Desktop\BetterArchery.dll

using HarmonyLib;
using UnityEngine;

namespace BetterArchery
{
  [HarmonyPatch(typeof (InventoryGrid), "OnLeftClick")]
  public static class InventoryGrid_OnLeftClick_Patch
  {
    public static bool Prefix(InventoryGrid __instance, UIInputHandler clickHandler)
    {
      if (!BetterArchery.configQuiverEnabled.Value)
        return true;
      GameObject gameObject = clickHandler.gameObject;
      Vector2i buttonPos = __instance.GetButtonPos(gameObject);
      ItemDrop.ItemData itemAt = __instance.m_inventory.GetItemAt(buttonPos.x, buttonPos.y);
      if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl) || itemAt == null || !itemAt.m_equiped || !itemAt.m_dropPrefab.name.Contains("Quiver"))
        return true;
      MessageHud.instance.ShowMessage(MessageHud.MessageType.Center, "You can't drop the quiver while equipped.", 0, (Sprite) null);
      return false;
    }
  }
}
