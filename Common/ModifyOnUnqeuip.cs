// Decompiled with JetBrains decompiler
// Type: BetterArchery.ModifyOnUnqeuip
// Assembly: BetterArchery, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5341B858-EB5D-47DA-A64D-602D91E9CB5C
// Assembly location: C:\Users\mugen\Desktop\BetterArchery.dll

using HarmonyLib;

namespace BetterArchery.Common
{
  [HarmonyPatch(typeof (Player), "QueueUnequipItem")]
  public static class ModifyOnUnqeuip
  {
    public static bool Prefix(ItemDrop.ItemData item, Player __instance)
    {
      if (!BetterArchery.configQuiverEnabled.Value || !item.m_dropPrefab.name.Contains("Quiver"))
        return true;
      foreach (ItemDrop.ItemData itemData in Player.m_localPlayer.m_inventory.m_inventory)
      {
        if (BetterArchery.IsQuiverSlot(itemData.m_gridPos) && (double) itemData.GetWeight() > 0.0)
        {
          MessageHud.instance.ShowMessage(MessageHud.MessageType.Center, "Empty the quiver first.");
          return false;
        }
      }
      return true;
    }
  }
}
