// Decompiled with JetBrains decompiler
// Type: Common.PrefabCreator
// Assembly: BetterArchery, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5341B858-EB5D-47DA-A64D-602D91E9CB5C
// Assembly location: C:\Users\mugen\Desktop\BetterArchery.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BetterArchery.Common
{
  public static class PrefabCreator
  {
    public static Dictionary<string, CraftingStation> CraftingStations;

    public static T RequireComponent<T>(GameObject go) where T : Component
    {
      T obj = go.GetComponent<T>();
      if ((UnityEngine.Object) obj == (UnityEngine.Object) null)
        obj = go.AddComponent<T>();
      return obj;
    }

    public static void Reset() => PrefabCreator.CraftingStations = (Dictionary<string, CraftingStation>) null;

    private static void InitCraftingStations()
    {
      if (PrefabCreator.CraftingStations != null)
        return;
      PrefabCreator.CraftingStations = new Dictionary<string, CraftingStation>();
      foreach (Recipe recipe in ObjectDB.instance.m_recipes)
      {
        if ((UnityEngine.Object) recipe.m_craftingStation != (UnityEngine.Object) null && !PrefabCreator.CraftingStations.ContainsKey(recipe.m_craftingStation.name))
          PrefabCreator.CraftingStations.Add(recipe.m_craftingStation.name, recipe.m_craftingStation);
      }
    }

    public static Recipe CreateRecipe(string name, string itemId, RecipeConfig recipeConfig)
    {
      PrefabCreator.InitCraftingStations();
      GameObject itemPrefab1 = ObjectDB.instance.GetItemPrefab(itemId);
      if ((UnityEngine.Object) itemPrefab1 == (UnityEngine.Object) null)
      {
        Debug.LogWarning((object) ("[PrefabCreator] Could not find item prefab (" + itemId + ")"));
        return (Recipe) null;
      }
      Recipe instance = ScriptableObject.CreateInstance<Recipe>();
      instance.name = name;
      instance.m_amount = recipeConfig.amount;
      instance.m_minStationLevel = recipeConfig.minStationLevel;
      instance.m_item = itemPrefab1.GetComponent<ItemDrop>();
      instance.m_enabled = recipeConfig.enabled;
      if (!string.IsNullOrEmpty(recipeConfig.craftingStation))
      {
        if (!PrefabCreator.CraftingStations.ContainsKey(recipeConfig.craftingStation))
        {
          Debug.LogWarning((object) ("[PrefabCreator] Could not find crafting station (" + itemId + "): " + recipeConfig.craftingStation));
          Debug.Log((object) ("[PrefabCreator] Available Stations: " + string.Join(", ", (IEnumerable<string>) PrefabCreator.CraftingStations.Keys)));
        }
        else
          instance.m_craftingStation = PrefabCreator.CraftingStations[recipeConfig.craftingStation];
      }
      if (!string.IsNullOrEmpty(recipeConfig.repairStation))
      {
        if (!PrefabCreator.CraftingStations.ContainsKey(recipeConfig.repairStation))
        {
          Debug.LogWarning((object) ("[PrefabCreator] Could not find repair station (" + itemId + "): " + recipeConfig.repairStation));
          Debug.Log((object) ("[PrefabCreator] Available Stations: " + string.Join(", ", (IEnumerable<string>) PrefabCreator.CraftingStations.Keys)));
        }
        else
          instance.m_repairStation = PrefabCreator.CraftingStations[recipeConfig.repairStation];
      }
      List<Piece.Requirement> requirementList = new List<Piece.Requirement>();
      foreach (RecipeRequirementConfig resource in recipeConfig.resources)
      {
        GameObject itemPrefab2 = ObjectDB.instance.GetItemPrefab(resource.item);
        if ((UnityEngine.Object) itemPrefab2 == (UnityEngine.Object) null)
          Debug.LogError((object) ("[PrefabCreator] Could not find requirement item (" + itemId + "): " + resource.item));
        else
          requirementList.Add(new Piece.Requirement()
          {
            m_amount = resource.amount,
            m_resItem = itemPrefab2.GetComponent<ItemDrop>()
          });
      }
      instance.m_resources = requirementList.ToArray();
      return instance;
    }

    public static Recipe AddNewRecipe(string name, string itemId, RecipeConfig recipeConfig)
    {
      Recipe recipe = PrefabCreator.CreateRecipe(name, itemId, recipeConfig);
      if (!((UnityEngine.Object) recipe == (UnityEngine.Object) null))
        return PrefabCreator.AddNewRecipe(recipe);
      Debug.LogError((object) ("[PrefabCreator] Failed to create recipe (" + name + ")"));
      return (Recipe) null;
    }

    public static Recipe AddNewRecipe(Recipe recipe)
    {
      int num = ObjectDB.instance.m_recipes.RemoveAll((Predicate<Recipe>) (x => x.name == recipe.name));
      if (num > 0)
        Debug.Log((object) string.Format("[PrefabCreator] Removed recipe ({0}): {1}", (object) recipe.name, (object) num));
      ObjectDB.instance.m_recipes.Add(recipe);
      Debug.Log((object) ("[PrefabCreator] Added recipe: " + recipe.name));
      return recipe;
    }
  }
}
