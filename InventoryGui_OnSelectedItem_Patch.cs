// Decompiled with JetBrains decompiler
// Type: BetterArchery.InventoryGui_OnSelectedItem_Patch
// Assembly: BetterArchery, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5341B858-EB5D-47DA-A64D-602D91E9CB5C
// Assembly location: C:\Users\mugen\Desktop\BetterArchery.dll

using HarmonyLib;
using UnityEngine;

namespace BetterArchery
{
  [HarmonyPatch(typeof (InventoryGui), "OnSelectedItem", new System.Type[] {typeof (InventoryGrid), typeof (ItemDrop.ItemData), typeof (Vector2i), typeof (InventoryGrid.Modifier)})]
  public static class InventoryGui_OnSelectedItem_Patch
  {
    public static bool Prefix(
      InventoryGui __instance,
      InventoryGrid grid,
      ItemDrop.ItemData item,
      Vector2i pos,
      InventoryGrid.Modifier mod,
      GameObject ___m_dragGo,
      ItemDrop.ItemData ___m_dragItem)
    {
      if (!BetterArchery.configQuiverEnabled.Value)
        return true;
      if (grid.m_inventory.m_name.Equals("PlayerGrid") && BetterArchery.IsQuiverSlot(pos))
      {
        if (___m_dragItem != null && ___m_dragItem.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Ammo)
          return true;
        if (___m_dragItem != null && ___m_dragItem.m_shared.m_itemType != ItemDrop.ItemData.ItemType.Ammo)
        {
          MessageHud.instance.ShowMessage(MessageHud.MessageType.Center, "You can not put this inside of the quiver.");
          return false;
        }
      }
      if ((bool) (UnityEngine.Object) __instance.m_dragGo)
      {
        if (item != null && item.m_equiped && __instance.m_dragInventory != grid.GetInventory())
        {
          MessageHud.instance.ShowMessage(MessageHud.MessageType.Center, "Unequip the item.");
          return false;
        }
        if (item != null && ___m_dragItem.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Ammo && BetterArchery.IsQuiverSlot(___m_dragItem.m_gridPos))
        {
          BetterArchery.Log("Can't swap this item.");
          return false;
        }
      }
      if (___m_dragItem == null || !___m_dragItem.m_equiped || __instance.m_dragInventory == grid.GetInventory())
        return true;
      MessageHud.instance.ShowMessage(MessageHud.MessageType.Center, "Unequip the item before moving it.");
      return false;
    }
  }
}
