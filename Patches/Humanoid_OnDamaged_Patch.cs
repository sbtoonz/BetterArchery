// Decompiled with JetBrains decompiler
// Type: BetterArchery.Humanoid_OnDamaged_Patch
// Assembly: BetterArchery, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5341B858-EB5D-47DA-A64D-602D91E9CB5C
// Assembly location: C:\Users\mugen\Desktop\BetterArchery.dll

using HarmonyLib;

namespace BetterArchery.Patches
{
  [HarmonyPatch(typeof (Humanoid), "OnDamaged")]
  public static class Humanoid_OnDamaged_Patch
  {
    public static void Postfix(ref Humanoid __instance, HitData hit)
    {
      if (!hit.HaveAttacker() || !hit.GetAttacker().IsPlayer() || !BetterArchery.ShowSneakDamage.Value || __instance.InAttack() || __instance.m_baseAI.IsAlerted())
        return;
      hit.GetAttacker().Message(MessageHud.MessageType.TopLeft, string.Format("<size=25>Sneak attack for {0}X ({1}) damage!</size>", hit.m_backstabBonus, (object) hit.GetTotalDamage().ToString("0.0")));
    }
  }
}
