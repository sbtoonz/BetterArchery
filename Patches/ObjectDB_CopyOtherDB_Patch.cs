// Decompiled with JetBrains decompiler
// Type: BetterArchery.ObjectDB_CopyOtherDB_Patch
// Assembly: BetterArchery, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5341B858-EB5D-47DA-A64D-602D91E9CB5C
// Assembly location: C:\Users\mugen\Desktop\BetterArchery.dll

using HarmonyLib;

namespace BetterArchery.Patches
{
  [HarmonyPatch(typeof (ObjectDB), "CopyOtherDB")]
  public static class ObjectDB_CopyOtherDB_Patch
  {
    public static void Postfix()
    {
      BetterArchery.TryRegisterItems();
      BetterArchery.TryRegisterRecipes();
    }
  }
}
