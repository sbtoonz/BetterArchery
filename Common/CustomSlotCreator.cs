// Decompiled with JetBrains decompiler
// Type: Common.CustomSlotCreator
// Assembly: BetterArchery, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5341B858-EB5D-47DA-A64D-602D91E9CB5C
// Assembly location: C:\Users\mugen\Desktop\BetterArchery.dll

using System.Collections.Generic;
using UnityEngine;

namespace BetterArchery.Common
{
  public static class CustomSlotCreator
  {
    public static readonly Dictionary<Humanoid, Dictionary<string, ItemDrop.ItemData>> customSlotItemData = new Dictionary<Humanoid, Dictionary<string, ItemDrop.ItemData>>();

    public static string GetCustomSlotName(ItemDrop.ItemData item) => item.m_dropPrefab.GetComponent<CustomSlotItem>().m_slotName;

    public static bool IsCustomSlotItem(ItemDrop.ItemData item) => item != null && (bool) (Object) item.m_dropPrefab && (bool) (Object) item.m_dropPrefab.GetComponent<CustomSlotItem>();

    public static ItemDrop.ItemData GetPrefabItemData(Humanoid humanoid, string slotName) => !CustomSlotCreator.DoesSlotExist(humanoid, slotName) ? (ItemDrop.ItemData) null : CustomSlotCreator.customSlotItemData[humanoid][slotName].m_dropPrefab.GetComponent<ItemDrop>().m_itemData;

    public static ItemDrop.ItemData GetSlotItem(Humanoid humanoid, string slotName) => CustomSlotCreator.DoesSlotExist(humanoid, slotName) ? CustomSlotCreator.customSlotItemData[humanoid][slotName] : (ItemDrop.ItemData) null;

    public static void SetSlotItem(Humanoid humanoid, string slotName, ItemDrop.ItemData item) => CustomSlotCreator.customSlotItemData[humanoid][slotName] = item;

    public static bool DoesSlotExist(Humanoid humanoid, string slotName) => CustomSlotCreator.customSlotItemData[humanoid] != null && CustomSlotCreator.customSlotItemData[humanoid].ContainsKey(slotName);

    public static bool IsSlotOccupied(Humanoid humanoid, string slotName) => CustomSlotCreator.customSlotItemData[humanoid] != null && CustomSlotCreator.customSlotItemData[humanoid].ContainsKey(slotName) && CustomSlotCreator.customSlotItemData[humanoid][slotName] != null;

    public static void ApplyCustomSlotItem(GameObject prefab, string slotName)
    {
      if (!(bool) (Object) prefab.GetComponent<CustomSlotItem>())
        prefab.AddComponent<CustomSlotItem>();
      prefab.GetComponent<CustomSlotItem>().m_slotName = slotName;
      prefab.GetComponent<ItemDrop>().m_itemData.m_shared.m_itemType = ItemDrop.ItemData.ItemType.None;
      Debug.Log((object) ("[CustomSlotCreator] Created " + slotName + " slot for " + prefab.name + "."));
    }
  }
}
