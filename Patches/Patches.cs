// Decompiled with JetBrains decompiler
// Type: BetterArchery.Patches
// Assembly: BetterArchery, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5341B858-EB5D-47DA-A64D-602D91E9CB5C
// Assembly location: C:\Users\mugen\Desktop\BetterArchery.dll

using System;
using System.Collections.Generic;
using System.Linq;
using BetterArchery.Common;
using HarmonyLib;
using Object = UnityEngine.Object;

namespace BetterArchery.Patches
{
  [HarmonyPatch]
  public class Patches
  {
    [HarmonyPatch(typeof (ItemDrop.ItemData), "IsEquipable")]
    [HarmonyPostfix]
    private static void IsEquipablePostfix(ref bool __result, ref ItemDrop.ItemData __instance) => __result = __result || CustomSlotCreator.IsCustomSlotItem(__instance);

    [HarmonyPatch(typeof (Humanoid), "Awake")]
    [HarmonyPostfix]
    private static void HumanoidEntryPostfix(ref Humanoid __instance)
    {
      CustomSlotCreator.customSlotItemData[__instance] = new Dictionary<string, ItemDrop.ItemData>();
    }

    [HarmonyPatch(typeof (Player), "Load")]
    [HarmonyPostfix]
    private static void InventoryLoadPostfix(ref Player __instance)
    {
      foreach (ItemDrop.ItemData equipedtem in __instance.m_inventory.GetEquipedtems())
      {
        if (CustomSlotCreator.IsCustomSlotItem(equipedtem))
        {
          string customSlotName = CustomSlotCreator.GetCustomSlotName(equipedtem);
          CustomSlotCreator.SetSlotItem((Humanoid) __instance, customSlotName, equipedtem);
        }
      }
    }

    [HarmonyPatch(typeof (Humanoid), "EquipItem")]
    [HarmonyPostfix]
    private static void EquipItemPostfix(
      ref bool __result,
      ref Humanoid __instance,
      ItemDrop.ItemData item,
      bool triggerEquipEffects = true)
    {
      if(__instance != Player.m_localPlayer)return;
      if (!CustomSlotCreator.IsCustomSlotItem(item))
        return;
      string customSlotName = CustomSlotCreator.GetCustomSlotName(item);
      if (CustomSlotCreator.IsSlotOccupied(__instance, customSlotName))
        __instance.UnequipItem(CustomSlotCreator.GetSlotItem(__instance, customSlotName), triggerEquipEffects);
      CustomSlotCreator.SetSlotItem(__instance, customSlotName, item);
      if (__instance.IsItemEquiped(item))
        item.m_equiped = true;
      __instance.SetupEquipment();
      if (triggerEquipEffects)
        __instance.TriggerEquipEffect(item);
      __result = true;
    }

    [HarmonyPatch(typeof (Humanoid), "UnequipItem")]
    [HarmonyPostfix]
    private static void UnequipItemPostfix(
      ref Humanoid __instance,
      ItemDrop.ItemData item,
      bool triggerEquipEffects = true)
    {
      if(__instance != Player.m_localPlayer)return;
      if (!CustomSlotCreator.IsCustomSlotItem(item))
        return;
      string customSlotName = CustomSlotCreator.GetCustomSlotName(item);
      if (item == CustomSlotCreator.GetSlotItem(__instance, customSlotName))
        CustomSlotCreator.SetSlotItem(__instance, customSlotName, (ItemDrop.ItemData) null);
      __instance.UpdateEquipmentStatusEffects();
    }

    [HarmonyPatch(typeof (Humanoid), "IsItemEquiped")]
    [HarmonyPostfix]
    private static void IsItemEquipedPostfix(
      ref bool __result,
      ref Humanoid __instance,
      ItemDrop.ItemData item)
    {
      if(__instance != Player.m_localPlayer)return;
      if (!CustomSlotCreator.IsCustomSlotItem(item))
        return;
      string customSlotName = CustomSlotCreator.GetCustomSlotName(item);
      bool flag = CustomSlotCreator.DoesSlotExist(__instance, customSlotName) && CustomSlotCreator.GetSlotItem(__instance, customSlotName) == item;
      __result |= flag;
    }

    [HarmonyPatch(typeof (Humanoid), "GetEquipmentWeight")]
    [HarmonyPostfix]
    private static void GetEquipmentWeightPostfix(ref float __result, ref Humanoid __instance)
    {
      if(__instance != Player.m_localPlayer)return;
      if(CustomSlotCreator.customSlotItemData == null) return;
      foreach (string key in CustomSlotCreator.customSlotItemData[__instance].Keys)
      {
        if (CustomSlotCreator.IsSlotOccupied(__instance, key))
          __result += CustomSlotCreator.GetSlotItem(__instance, key).m_shared.m_weight;
      }
    }

    [HarmonyPatch(typeof (Humanoid), "UnequipAllItems")]
    [HarmonyPostfix]
    private static void UnequipAllItemsPostfix(ref Humanoid __instance)
    {
      if(__instance != Player.m_localPlayer)return;
      if(CustomSlotCreator.customSlotItemData == null) return;
      foreach (string slotName in CustomSlotCreator.customSlotItemData[__instance].Keys.ToList<string>())
      {
        if (CustomSlotCreator.IsSlotOccupied(__instance, slotName))
          __instance.UnequipItem(CustomSlotCreator.GetSlotItem(Player.m_localPlayer, slotName), false);
      }
    }

    [HarmonyPatch(typeof (Humanoid), "GetSetCount")]
    [HarmonyPostfix]
    private static void GetSetCountPostfix(
      ref int __result,
      ref Humanoid __instance,
      string setName)
    {
      if(__instance != Player.m_localPlayer)return;
      foreach (string slotName in CustomSlotCreator.customSlotItemData[__instance].Keys.ToList<string>())
      {
        if (CustomSlotCreator.IsSlotOccupied(__instance, slotName) && CustomSlotCreator.GetSlotItem(__instance, slotName).m_shared.m_setName == setName)
          ++__result;
      }
    }
[HarmonyPatch(typeof(TombStone), "Interact")]
		public static class TombStone_Interact_Patch
		{
			public static bool interactingTombstone;

			public static void Prefix()
			{
				interactingTombstone = true;
			}

			public static void Postfix()
			{
				interactingTombstone = false;
			}
		}
		
		[HarmonyPatch(typeof(Player), "Load")]
		public static class Player_Load_Patch
		{
			public static bool loading;

			public static void Prefix()
			{
				loading = true;
			}

			public static void Postfix(Player __instance)
			{
				if (!BetterArcheryState.QuiverEnabled)
				{
					return;
				}
				loading = false;
				if (__instance != Player.m_localPlayer)
				{
					return;
				}
				Inventory inventory = __instance.m_inventory;
				BetterArchery.Log("Searching player inventory for lost items.");
				for (int num = inventory.m_inventory.Count - 1; num >= 0; num--)
				{
					ItemDrop.ItemData itemData = inventory.m_inventory[num];
					Vector2i gridPos = itemData.m_gridPos;
					if (gridPos.y >= BetterArcheryState.RowStartIndex && gridPos.y <= BetterArcheryState.RowEndIndex && (!global::BetterArchery.BetterArchery.IsQuiverSlot(gridPos) || itemData.m_shared.m_itemType != ItemDrop.ItemData.ItemType.Ammo))
					{
						BetterArchery.Log($"Found {itemData.m_shared.m_name} x {itemData.m_stack} in Better Archery slots; attempting to drop.");
						__instance.DropItem(inventory, itemData, itemData.m_stack);
					}
				}
			}
		}
		
		[HarmonyPatch(typeof(Player), "Awake")]
		public static class Player_Awake_Patch
		{
			public static void Postfix()
			{
				BetterArcheryState.UpdateRowIndex();
			}
		}
		
		[HarmonyPatch(typeof(InventoryGui), "OnTakeAll")]
		public static class InventoryGui_OnTakeAll_Patch
		{
			public static bool takingAllTombstone;

			public static void Prefix(InventoryGui __instance)
			{
				Container currentContainer = __instance.m_currentContainer;
				if (currentContainer != null && (bool)currentContainer.GetComponent<TombStone>())
				{
					takingAllTombstone = true;
				}
			}

			public static void Postfix()
			{
				takingAllTombstone = false;
			}
		}
		
		[HarmonyPatch(typeof(Inventory), "GetEmptySlots")]
		public static class Inventory_GetEmptySlots_Patch
		{
			public static void Postfix(Inventory __instance, ref int __result)
			{
				if (BetterArcheryState.QuiverEnabled && TombStone_Interact_Patch.interactingTombstone && __instance.GetName() == "Inventory")
				{
					int num = __instance.m_width * 2;
					__result = Math.Max(0, __result - num);
					BetterArchery.Log($"GetEmptySlots: subtracted {num} BetterArchery slots (now {__result})");
				}
			}
		}
		
		[HarmonyPatch(typeof(Inventory), "AddItem", new Type[]
		{
			typeof(ItemDrop.ItemData),
			typeof(int),
			typeof(int),
			typeof(int)
		})]
		public static class Inventory_AddItem_Patch
		{
			public static bool Prefix(ItemDrop.ItemData item, int amount, int x, int y, ref bool __result)
			{
				if (!BetterArcheryState.QuiverEnabled)
				{
					return true;
				}
				if ((InventoryGui_OnTakeAll_Patch.takingAllTombstone || TombStone_Interact_Patch.interactingTombstone) && y >= BetterArcheryState.RowStartIndex && y <= BetterArcheryState.RowEndIndex)
				{
					BetterArchery.Log($"Blocking Inventory.Additem {item.m_shared.m_name} x {amount} into Better Archery slot {x},{y}.");
					__result = false;
					return false;
				}
				return true;
			}
		}
		
    public static HashSet<StatusEffect> GetStatusEffectsFromCustomSlotItems(
      Humanoid __instance)
    {
      HashSet<StatusEffect> fromCustomSlotItems = new HashSet<StatusEffect>();
      foreach (string key in CustomSlotCreator.customSlotItemData[__instance].Keys)
      {
        if (CustomSlotCreator.IsSlotOccupied(__instance, key))
        {
          if ((bool) (Object) CustomSlotCreator.GetSlotItem(__instance, key).m_shared.m_equipStatusEffect)
          {
            StatusEffect equipStatusEffect = CustomSlotCreator.GetSlotItem(__instance, key).m_shared.m_equipStatusEffect;
            fromCustomSlotItems.Add(equipStatusEffect);
          }
          if (__instance.HaveSetEffect(CustomSlotCreator.GetSlotItem(__instance, key)))
          {
            StatusEffect setStatusEffect = CustomSlotCreator.GetSlotItem(__instance, key).m_shared.m_setStatusEffect;
            fromCustomSlotItems.Add(setStatusEffect);
          }
        }
      }
      return fromCustomSlotItems;
    }
  }
}
