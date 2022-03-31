// Decompiled with JetBrains decompiler
// Type: BetterArchery.Player_GameCamera_UpdateCamera
// Assembly: BetterArchery, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5341B858-EB5D-47DA-A64D-602D91E9CB5C
// Assembly location: C:\Users\mugen\Desktop\BetterArchery.dll

using HarmonyLib;

namespace BetterArchery.Patches
{
  [HarmonyPatch(typeof (GameCamera), "UpdateCamera")]
  public static class Player_GameCamera_UpdateCamera
  {
    [HarmonyPriority(0)]
    public static void Prefix(GameCamera __instance)
    {
      if (!BetterArchery.configBowZoomEnabled.Value || BetterArchery.__ZoomState != BetterArchery.ZoomState.ZoomingOut && BetterArchery.__ZoomState != BetterArchery.ZoomState.ZoomingIn)
        return;
      __instance.m_fov = BetterArchery.__NewZoomFov;
    }
  }
}
