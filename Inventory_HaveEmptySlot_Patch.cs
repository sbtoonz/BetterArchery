// Decompiled with JetBrains decompiler
// Type: BetterArchery.Inventory_HaveEmptySlot_Patch
// Assembly: BetterArchery, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5341B858-EB5D-47DA-A64D-602D91E9CB5C
// Assembly location: C:\Users\mugen\Desktop\BetterArchery.dll

using HarmonyLib;

namespace BetterArchery
{
  [HarmonyPatch(typeof (Inventory), "HaveEmptySlot")]
  public static class Inventory_HaveEmptySlot_Patch
  {
    [HarmonyPriority(800)]
    public static bool Prefix(Inventory __instance, ref bool __result)
    {
      if (__instance.GetName() != "PlayerGrid" || !BetterArchery.configQuiverEnabled.Value)
        return true;
      int num = __instance.m_width * __instance.m_height - 3 - 5;
      if (__instance.GetHeight() > 5)
        num -= 8;
      int count = __instance.m_inventory.Count;
      for (int index = 0; index < 3; ++index)
      {
        Vector2i quiverSlotPosition = BetterArchery.GetQuiverSlotPosition(index);
        ItemDrop.ItemData itemAt = __instance.GetItemAt(quiverSlotPosition.x, quiverSlotPosition.y);
        count -= itemAt != null ? 1 : 0;
      }
      __result = count < num;
      return false;
    }
  }
}
