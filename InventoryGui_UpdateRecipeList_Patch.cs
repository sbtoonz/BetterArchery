// Decompiled with JetBrains decompiler
// Type: BetterArchery.InventoryGui_UpdateRecipeList_Patch
// Assembly: BetterArchery, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5341B858-EB5D-47DA-A64D-602D91E9CB5C
// Assembly location: C:\Users\mugen\Desktop\BetterArchery.dll

using HarmonyLib;
using System;
using System.Collections.Generic;

namespace BetterArchery
{
  [HarmonyPatch(typeof (InventoryGui), "UpdateRecipeList")]
  public static class InventoryGui_UpdateRecipeList_Patch
  {
    public static void Prefix(InventoryGui __instance, List<Recipe> recipes)
    {
      if (!(bool) (UnityEngine.Object) Player.m_localPlayer.GetCurrentCraftingStation())
        return;
      Recipe recipe = recipes.Find((Predicate<Recipe>) (e => e.name == "Recipe_ArrowWoodAnywhere"));
      if ((UnityEngine.Object) recipe != (UnityEngine.Object) null)
        recipes.Remove(recipe);
    }
  }
}
