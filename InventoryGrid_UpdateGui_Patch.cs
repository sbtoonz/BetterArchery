// Decompiled with JetBrains decompiler
// Type: BetterArchery.InventoryGrid_UpdateGui_Patch
// Assembly: BetterArchery, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5341B858-EB5D-47DA-A64D-602D91E9CB5C
// Assembly location: C:\Users\mugen\Desktop\BetterArchery.dll

using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BetterArchery
{
  [HarmonyPatch(typeof (InventoryGrid), "UpdateGui", typeof (Player), typeof (ItemDrop.ItemData))]
  public static class InventoryGrid_UpdateGui_Patch
  {
    internal static GameObject AugaQuiverPanel;
    private static void Postfix(
      InventoryGrid __instance,
      Player player,
      ItemDrop.ItemData dragItem,
      List<InventoryGrid.Element> ___m_elements)
    {
      if (__instance.name != "PlayerGrid" || !BetterArchery.configQuiverEnabled.Value)
        return;
      if (__instance.GetInventory().GetHeight() > 5)
      {
        if (GetElement(___m_elements, 7, __instance.GetInventory().GetHeight() - 1).m_go.activeSelf)
        {
          for (int y = __instance.GetInventory().GetHeight() - 2; y < __instance.GetInventory().GetHeight(); ++y)
          {
            for (int x = 0; x < 8; ++x)
              GetElement(___m_elements, x, y).m_go.SetActive(false);
          }
        }
      }
      else if (GetElement(___m_elements, 7, __instance.GetInventory().GetHeight() - 1).m_go.activeSelf)
      {
        for (int y = __instance.GetInventory().GetHeight() - 1; y < __instance.GetInventory().GetHeight(); ++y)
        {
          for (int x = 0; x < 8; ++x)
            GetElement(___m_elements, x, y).m_go.SetActive(false);
        }
      }
      if (BetterArchery.IsQuiverEquipped())
        CreateQuiverSlots(__instance, ___m_elements);
      else
        RemoveQuiverSlots(__instance, ___m_elements);
    }

    public static void CreateQuiverSlots(
      InventoryGrid __instance,
      List<InventoryGrid.Element> ___m_elements)
    {
      if (Auga.API.IsLoaded())
      {
        List<GameObject> templist = new List<GameObject>();
        int inventoryRowIndex = BetterArchery.GetBonusInventoryRowIndex();
        for (int index = 0; index < 3; ++index)
        {
          int x = index;
          InventoryGrid.Element element = GetElement(___m_elements, x, inventoryRowIndex);
          element.m_go.SetActive(true);
          Text component = element.m_go.transform.Find("binding").GetComponent<Text>();
          component.enabled = true;
          component.horizontalOverflow = HorizontalWrapMode.Overflow;
          component.fontSize = 12;
          KeyCode mainKey = BetterArchery.HoldingKeyCode.Value.MainKey;
          if (mainKey.ToString() == "")
          {
            component.text = BetterArchery.GetBindingLabel(index);
          }
          else
          {
            Text text = component;
            mainKey = BetterArchery.HoldingKeyCode.Value.MainKey;
            string str = mainKey.ToString() + " + " + BetterArchery.GetBindingLabel(index);
            text.text = str;
          }
          component.rectTransform.anchoredPosition = new Vector2(28f, -7f);
          Vector2 vector2_1 = BetterArchery.InventoryQuiverSlotLocation.Value;
          Vector2 vector2_2 = new Vector2((float) x * 70, (float) (4.0 * -(double) __instance.m_elementSpace));
          //(element.m_go.transform as RectTransform).anchoredPosition = vector2_1 + vector2_2;
          element.m_go.transform.SetParent(__instance.transform.parent.Find("QuiverSlotBkg"));
          templist.Add(element.m_go);
          
        }
        templist[0].transform.position = new Vector3(529.5165f, 472.9594f, 0);
        templist[0].transform.localPosition = new Vector3(-102.1694f, 31.9757f, 0);
        templist[1].transform.position = new Vector3(582.2625f, 472.9594f, 0);
        templist[1].transform.localPosition = new Vector3(-32.3585f, 31.9757f, 0);
        templist[2].transform.position = new Vector3(635.2397f, 472.9594f, 0);
        templist[2].transform.localPosition = new Vector3(37.7584f, 31.9757f, 0);
        templist.Clear();
        //el1 pos 529.5165 472.9594 0
        //el1 localpos -102.1694 31.9757 0
        //el2 pos 582.2625f, 472.9594f, 0
        //el2 localpos -32.3585f, 31.9757f, 0
        //el3 pos 635.2397f, 472.9594f, 0
        //ell localpos 37.7584f, 31.9757f, 0
        
        if ((bool) (Object) __instance.transform.parent.Find("QuiverSlotBkg"))
          return;
        RectTransform background = GetOrCreateBackground(__instance, "QuiverSlotBkg");
        Vector2 vector2 = BetterArchery.InventoryQuiverSlotLocation.Value;
        float num1 = 3 - vector2.x;
        float num2 = -25f - vector2.y;
        background.anchoredPosition = new Vector2((float) (-(double) num1 - 176.0), (float) (-(double) num2 - 200.0));
        background.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 235f);
        background.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 90f);
        background.localScale = new Vector3(1f, 1f, 1f);
      }
      else
      {
        if ((bool) (Object) __instance.transform.parent.Find("QuiverSlotBkg"))
          return;
        int inventoryRowIndex = BetterArchery.GetBonusInventoryRowIndex();
        for (int index = 0; index < 3; ++index)
        {
          int x = index;
          InventoryGrid.Element element = GetElement(___m_elements, x, inventoryRowIndex);
          element.m_go.SetActive(true);
          Text component = element.m_go.transform.Find("binding").GetComponent<Text>();
          component.enabled = true;
          component.horizontalOverflow = HorizontalWrapMode.Overflow;
          component.fontSize = 12;
          KeyCode mainKey = BetterArchery.HoldingKeyCode.Value.MainKey;
          if (mainKey.ToString() == "")
          {
            component.text = BetterArchery.GetBindingLabel(index);
          }
          else
          {
            Text text = component;
            mainKey = BetterArchery.HoldingKeyCode.Value.MainKey;
            string str = mainKey.ToString() + " + " + BetterArchery.GetBindingLabel(index);
            text.text = str;
          }
          component.rectTransform.anchoredPosition = new Vector2(28f, -7f);
          Vector2 vector2_1 = BetterArchery.InventoryQuiverSlotLocation.Value;
          Vector2 vector2_2 = new Vector2((float) x * __instance.m_elementSpace, (float) (4.0 * -(double) __instance.m_elementSpace));
          (element.m_go.transform as RectTransform).anchoredPosition = vector2_1 + vector2_2;
        }
        
        
        RectTransform background = GetOrCreateBackground(__instance, "QuiverSlotBkg");
        Vector2 vector2 = BetterArchery.InventoryQuiverSlotLocation.Value;
        float num1 = 3f - vector2.x;
        float num2 = -25f - vector2.y;
        background.anchoredPosition = new Vector2((float) (-(double) num1 - 176.0), (float) (-(double) num2 - 200.0));
        background.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 235f);
        background.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 90f);
        background.localScale = new Vector3(1f, 1f, 1f);
      }
      
    }

    public static void RemoveQuiverSlots(
      InventoryGrid __instance,
      List<InventoryGrid.Element> ___m_elements)
    {
      if (Auga.API.IsLoaded())
      {
        int inventoryRowIndex = BetterArchery.GetBonusInventoryRowIndex();
        for (int index = 0; index < 3; ++index)
        {
          int x = index;
          GetElement(___m_elements, x, inventoryRowIndex).m_go.SetActive(false);
        }
        Transform transform = __instance.transform.parent.Find("QuiverSlotBkg");
        if (!((Object) transform != (Object) null))
          return;
        Object.Destroy((Object) transform.gameObject);
      }
      else
      {
        int inventoryRowIndex = BetterArchery.GetBonusInventoryRowIndex();
        for (int index = 0; index < 3; ++index)
        {
          int x = index;
          GetElement(___m_elements, x, inventoryRowIndex).m_go.SetActive(false);
        }
        Transform transform = __instance.transform.parent.Find("QuiverSlotBkg");
        if (!((Object) transform != (Object) null))
          return;
        Object.Destroy((Object) transform.gameObject);
      }
    }

    private static RectTransform GetOrCreateBackground(
      InventoryGrid __instance,
      string name)
    {
      if (Auga.API.IsLoaded())
      {
        Transform transform = __instance.transform.parent.Find(name);
        GameObject gameObject1 = __instance.transform.parent.Find("Bkg").gameObject;
        var panel = Auga.API.Panel_Create(gameObject1.transform.parent, new Vector2(10, 10), name, true);
        panel.name = name;
        AugaQuiverPanel = panel;
        var temp = __instance.transform.parent.Find("PlayerGrid/Main").gameObject;
        temp.GetComponent<RectMask2D>().enabled = false;
        panel.transform.SetSiblingIndex(temp.transform.GetSiblingIndex() + 1);
        transform = panel.transform;
        return  transform as RectTransform;
      }
      else if (!Auga.API.IsLoaded())
      {
        Transform transform = __instance.transform.parent.Find(name);
        if ((Object) transform == (Object) null)
        {
          GameObject gameObject1 = __instance.transform.parent.Find("Bkg").gameObject;
          GameObject gameObject2 = Object.Instantiate<GameObject>(gameObject1, gameObject1.transform.parent);
          gameObject2.name = name;
          gameObject2.transform.SetSiblingIndex(gameObject1.transform.GetSiblingIndex() + 1);
          transform = gameObject2.transform;
        }
        return transform as RectTransform;
      }

      return null;
    }

    private static InventoryGrid.Element GetElement(
      List<InventoryGrid.Element> elements,
      int x,
      int y)
    {
      int width = Player.m_localPlayer.GetInventory().GetWidth();
      return elements[y * width + x];
    }
  }
}
