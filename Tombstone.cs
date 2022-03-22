// Decompiled with JetBrains decompiler
// Type: BetterArchery.Tombstone
// Assembly: BetterArchery, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5341B858-EB5D-47DA-A64D-602D91E9CB5C
// Assembly location: C:\Users\mugen\Desktop\BetterArchery.dll

using HarmonyLib;

namespace BetterArchery
{
  internal class Tombstone
  {
    [HarmonyPatch(typeof (TombStone), "EasyFitInInventory")]
    public static class Tombstone_EasyFitInInventory_Patch
    {
      public static bool Prefix(TombStone __instance, Player player)
      {
        int num1 = player.GetInventory().GetEmptySlots() - player.GetInventory().m_width * (BetterArchery.QuiverRowIndex - 3);
        int num2 = num1 < 0 ? -num1 : num1;
        BetterArchery.Log(string.Format("GetEmptySlots: subtracted {0} BetterArchery slots (now {1})", (object) (player.GetInventory().m_width * (BetterArchery.QuiverRowIndex - 3)), (object) num2));
        return __instance.m_container.GetInventory().NrOfItems() <= num2 && (double) player.GetInventory().GetTotalWeight() + (double) __instance.m_container.GetInventory().GetTotalWeight() <= (double) player.GetMaxCarryWeight();
      }
    }
  }
}
