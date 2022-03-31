// Decompiled with JetBrains decompiler
// Type: BetterArchery.InputText_Patch
// Assembly: BetterArchery, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5341B858-EB5D-47DA-A64D-602D91E9CB5C
// Assembly location: C:\Users\mugen\Desktop\BetterArchery.dll

using HarmonyLib;

namespace BetterArchery.Patches
{
  [HarmonyPatch(typeof (Terminal), "InputText")]
  public static class InputText_Patch
  {
    public static bool Prefix(Terminal __instance)
    {
      string lower = __instance.m_input.text.ToLower();
      if (lower.Equals("ba reload"))
      {
        BetterArchery._instance.Config.Reload();
        __instance.AddString("Better Archery Reloaded.");
        return false;
      }
      if (lower.Equals("ba drop"))
      {
        Inventory inventory = Player.m_localPlayer.m_inventory;
        for (int index = inventory.m_inventory.Count - 1; index >= 0; --index)
        {
          ItemDrop.ItemData itemData = inventory.m_inventory[index];
          if (itemData.m_gridPos.y >= BetterArchery.QuiverRowIndex - 1 && itemData.m_gridPos.y <= BetterArchery.QuiverRowIndex + 1 && !BetterArchery.IsQuiverSlot(itemData.m_gridPos))
          {
            BetterArchery.Log(string.Format("Found {0} x {1} in invisible slots; attempting to drop.", (object) itemData.m_dropPrefab.name, (object) itemData.m_stack));
            Player.m_localPlayer.DropItem(inventory, itemData, itemData.m_stack);
          }
        }
        return false;
      }
      if (lower.Equals("ba clear"))
      {
        Player.m_localPlayer.GetInventory().RemoveAll();
        return false;
      }
      if (lower.Equals("ba sfx"))
      {
        BetterArchery.PlayCustomSFX("exhale");
        return false;
      }
      if (lower.Equals("ba kill"))
      {
        Player.m_localPlayer.SetHealth(0.0f);
        return false;
      }
      return !lower.Equals("ba fill");
    }
  }
}
