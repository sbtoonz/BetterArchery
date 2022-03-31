// Decompiled with JetBrains decompiler
// Type: BetterArchery.ModifyOnSpawned
// Assembly: BetterArchery, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5341B858-EB5D-47DA-A64D-602D91E9CB5C
// Assembly location: C:\Users\mugen\Desktop\BetterArchery.dll

using System;
using HarmonyLib;
using UnityEngine;

namespace BetterArchery.Common
{
  [HarmonyPatch(typeof (Player), "OnSpawned")]
  public static class ModifyOnSpawned
  {
    private static void Prefix(Player __instance)
    {
      Tutorial.TutorialText tutorialText = new Tutorial.TutorialText()
      {
        m_label = "BetterArchery",
        m_name = "betterarchery",
        m_text = "Thank you for using this mod! Don't forget to change configurations to get your own taste!",
        m_topic = "Welcome to BetterArchery"
      };
      if (!Tutorial.instance.m_texts.Contains(tutorialText))
        Tutorial.instance.m_texts.Add(tutorialText);
      Player.m_localPlayer.ShowTutorial("betterarchery");
      if (BetterArchery.configQuiverEnabled.Value)
      {
        foreach (ItemDrop.ItemData equipedtem in __instance.m_inventory.GetEquipedtems())
        {
          if (CustomSlotCreator.IsCustomSlotItem(equipedtem))
            equipedtem.m_equiped = true;
          equipedtem.m_equiped = true;
        }
      }
      if (!BetterArchery.BowZoomSFXEnabled.Value)
        return;
      try
      {
        BetterArchery.playerAudioSource = Player.m_localPlayer.transform.gameObject.AddComponent<AudioSource>();
        Player.m_localPlayer.transform.gameObject.AddComponent<ZSFX>();
      }
      catch (Exception ex)
      {
        BetterArchery.Log(ex.ToString(), 0);
      }
    }
  }
}
