// Decompiled with JetBrains decompiler
// Type: BetterArchery.HotkeyBar_UpdateIcons_Patch
// Assembly: BetterArchery, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5341B858-EB5D-47DA-A64D-602D91E9CB5C
// Assembly location: C:\Users\mugen\Desktop\BetterArchery.dll

using HarmonyLib;
using System.Collections.Generic;

namespace BetterArchery
{
  [HarmonyPatch(typeof (HotkeyBar), "UpdateIcons", new System.Type[] {typeof (Player)})]
  public static class HotkeyBar_UpdateIcons_Patch
  {
    public static bool Prefix(
      HotkeyBar __instance,
      Player player,
      List<HotkeyBar.ElementData> ___m_elements,
      List<ItemDrop.ItemData> ___m_items,
      int ___m_selected)
    {
      return BetterArchery.configQuiverEnabled.Value && BetterArchery.QuiverHudEnabled.Value || true;
    }

    private static HotkeyBar.ElementData GetElementForItem(
      List<HotkeyBar.ElementData> elements,
      ItemDrop.ItemData item)
    {
      return item.m_gridPos.y == 0 ? elements[item.m_gridPos.x] : elements[Player.m_localPlayer.GetInventory().m_width + item.m_gridPos.x];
    }
  }
}
