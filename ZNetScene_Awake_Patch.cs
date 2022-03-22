// Decompiled with JetBrains decompiler
// Type: BetterArchery.ZNetScene_Awake_Patch
// Assembly: BetterArchery, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5341B858-EB5D-47DA-A64D-602D91E9CB5C
// Assembly location: C:\Users\mugen\Desktop\BetterArchery.dll

using HarmonyLib;

namespace BetterArchery
{
  [HarmonyPatch(typeof (ZNetScene), "Awake")]
  public static class ZNetScene_Awake_Patch
  {
    public static void Prefix(ZNetScene __instance)
    {
      BetterArchery.TryRegisterPrefabs(__instance);
    }

    public static void Postfix(ZNetScene __instance)
    {
      BetterArchery.TryCreateCustomSlot(__instance);
      BetterArchery.TryCreateCustomSFX();
    }
  }
}
