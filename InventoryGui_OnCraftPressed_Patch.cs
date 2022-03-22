// Decompiled with JetBrains decompiler
// Type: BetterArchery.InventoryGui_OnCraftPressed_Patch
// Assembly: BetterArchery, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5341B858-EB5D-47DA-A64D-602D91E9CB5C
// Assembly location: C:\Users\mugen\Desktop\BetterArchery.dll

using HarmonyLib;
using UnityEngine;

namespace BetterArchery
{
  [HarmonyPatch(typeof (InventoryGui), "OnCraftPressed")]
  public static class InventoryGui_OnCraftPressed_Patch
  {
    public static void Prefix(InventoryGui __instance)
    {
      if (!(bool) (Object) __instance.m_selectedRecipe.Key)
        return;
      __instance.m_craftRecipe = __instance.m_selectedRecipe.Key;
      if ((Object) __instance.m_craftRecipe != (Object) null && __instance.m_craftRecipe.name == "Recipe_ArrowWoodAnywhere")
        __instance.m_craftDuration = 7f;
      else
        __instance.m_craftDuration = 2f;
    }
  }
}
