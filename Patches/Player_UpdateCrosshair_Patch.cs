// Decompiled with JetBrains decompiler
// Type: BetterArchery.Player_UpdateCrosshair_Patch
// Assembly: BetterArchery, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5341B858-EB5D-47DA-A64D-602D91E9CB5C
// Assembly location: C:\Users\mugen\Desktop\BetterArchery.dll

using HarmonyLib;
using UnityEngine;

namespace BetterArchery.Patches
{
  [HarmonyPatch(typeof (Hud), "UpdateCrosshair")]
  [HarmonyPriority(0)]
  public static class Player_UpdateCrosshair_Patch
  {
    private static void Postfix(Hud __instance, Player player, float bowDrawPercentage)
    {
      if (__instance.m_crosshair.enabled != BetterArchery.IsCrosshairVisible.Value)
        __instance.m_crosshair.enabled = BetterArchery.IsCrosshairVisible.Value;
      if (__instance.m_crosshairBow.enabled != BetterArchery.IsBowCrosshairVisible.Value)
        __instance.m_crosshairBow.enabled = BetterArchery.IsBowCrosshairVisible.Value;
      if (BetterArchery.BowDrawMovementSpeedReductionEnabled.Value)
      {
        if ((double) bowDrawPercentage > 0.100000001490116)
        {
          player.SetWalk(true);
          BetterArchery.speedReduction = true;
        }
        else if (player.GetWalk() && BetterArchery.speedReduction)
        {
          BetterArchery.speedReduction = false;
          player.SetWalk(false);
        }
      }
      if (!BetterArchery.configBowZoomEnabled.Value || (double) BetterArchery.__BaseFov == 0.0)
        return;
      ItemDrop.ItemData currentWeapon = player.GetCurrentWeapon();
      if (currentWeapon == null || currentWeapon.m_shared.m_itemType != ItemDrop.ItemData.ItemType.Bow)
      {
        Player_UpdateCrosshair_Patch.ZoomOut();
      }
      else
      {
        float skillFactor = player.GetSkillFactor(currentWeapon.m_shared.m_skillType);
        float b = currentWeapon.m_shared.m_holdDurationMin * (1f - skillFactor);
        bool key = Input.GetKey(BetterArchery.BowZoomKey.Value.MainKey);
        if (BetterArchery.__ZoomState == BetterArchery.ZoomState.ZoomingIn && !BetterArchery.zoomSFX)
        {
          BetterArchery.PlayCustomSFX("inhale", false);
          BetterArchery.zoomSFX = true;
        }
        else if (BetterArchery.__ZoomState == BetterArchery.ZoomState.ZoomingOut && BetterArchery.zoomSFX)
        {
          BetterArchery.PlayCustomSFX("exhale", false);
          BetterArchery.zoomSFX = false;
        }
        if (BetterArchery.AutomaticBowZoom.Value)
        {
          if ((double) bowDrawPercentage > 0.00999999977648258)
          {
            BetterArchery.ZoomInTimer += Time.deltaTime;
            BetterArchery.__ZoomState = BetterArchery.ZoomState.ZoomingIn;
            float t = (double) BetterArchery.BowZoomConstantTime.Value <= 0.0 ? Mathf.InverseLerp(0.05f, b, BetterArchery.ZoomInTimer) : Mathf.InverseLerp(0.05f, BetterArchery.BowZoomConstantTime.Value, BetterArchery.ZoomInTimer);
            GameCamera.instance.m_fov =BetterArchery.__LastZoomFov = Mathf.Lerp(BetterArchery.__BaseFov, BetterArchery.__BaseFov / BetterArchery.BowZoomFactor.Value, t);
            BetterArchery.__NewZoomFov = BetterArchery.__LastZoomFov = Mathf.Lerp(BetterArchery.__BaseFov, BetterArchery.__BaseFov / BetterArchery.BowZoomFactor.Value, t);
            return;
          }
          Player_UpdateCrosshair_Patch.ZoomOut();
        }
        if (key)
        {
          if ((double) bowDrawPercentage <= 0.00999999977648258)
          {
            BetterArchery.ZoomOutDelayTimer += Time.deltaTime;
            if ((double) BetterArchery.ZoomOutDelayTimer <= (double) BetterArchery.StayInZoomTime.Value)
              return;
            Player_UpdateCrosshair_Patch.ZoomOut();
          }
          else
          {
            BetterArchery.ZoomInTimer += Time.deltaTime;
            BetterArchery.ZoomOutDelayTimer = 0.0f;
            BetterArchery.__ZoomState = BetterArchery.ZoomState.ZoomingIn;
            float t = (double) BetterArchery.BowZoomConstantTime.Value <= 0.0 ? Mathf.InverseLerp(0.05f, b, BetterArchery.ZoomInTimer) : Mathf.InverseLerp(0.05f, BetterArchery.BowZoomConstantTime.Value, BetterArchery.ZoomInTimer);
            GameCamera.instance.m_fov = BetterArchery.__LastZoomFov = Mathf.Lerp(BetterArchery.__BaseFov, BetterArchery.__BaseFov / BetterArchery.BowZoomFactor.Value, t);
            BetterArchery.__NewZoomFov = BetterArchery.__LastZoomFov = Mathf.Lerp(BetterArchery.__BaseFov, BetterArchery.__BaseFov / BetterArchery.BowZoomFactor.Value, t);
          }
        }
        else
        {
          BetterArchery.ZoomOutDelayTimer = 1f;
          Player_UpdateCrosshair_Patch.ZoomOut();
        }
      }
    }

    public static void ZoomOut()
    {
      if ((uint) BetterArchery.__ZoomState > 0U)
      {
        if (BetterArchery.__ZoomState == BetterArchery.ZoomState.ZoomingIn)
        {
          BetterArchery.__ZoomState = BetterArchery.ZoomState.ZoomingOut;
          BetterArchery.ZoomOutTimer = 0.0f;
          BetterArchery.ZoomInTimer = 0.0f;
        }
        else
        {
          BetterArchery.ZoomOutTimer += Time.deltaTime;
          if ((double) BetterArchery.ZoomOutTimer > 1.0)
          {
            GameCamera.instance.m_fov = BetterArchery.__BaseFov;
            BetterArchery.__ZoomState = BetterArchery.ZoomState.Fixed;
            BetterArchery.ZoomOutDelayTimer = 0.0f;
            return;
          }
        }
        float t = Mathf.InverseLerp(0.0f, 0.3f, BetterArchery.ZoomOutTimer);
        GameCamera.instance.m_fov = Mathf.Lerp(BetterArchery.__LastZoomFov, BetterArchery.__BaseFov, t);
        BetterArchery.__NewZoomFov = Mathf.Lerp(BetterArchery.__LastZoomFov, BetterArchery.__BaseFov, t);
      }
      else
      {
        if ((double) GameCamera.instance.m_fov == (double) BetterArchery.__BaseFov)
          return;
        GameCamera.instance.m_fov = BetterArchery.__BaseFov;
      }
    }
  }
}
