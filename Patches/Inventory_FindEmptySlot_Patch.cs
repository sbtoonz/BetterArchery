// Decompiled with JetBrains decompiler
// Type: BetterArchery.Inventory_FindEmptySlot_Patch
// Assembly: BetterArchery, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5341B858-EB5D-47DA-A64D-602D91E9CB5C
// Assembly location: C:\Users\mugen\Desktop\BetterArchery.dll
// Compiler-generated code is shown

using HarmonyLib;

namespace BetterArchery.Patches
{
    [HarmonyPatch(typeof (Inventory), "FindEmptySlot")]
    public static class Inventory_FindEmptySlot_Patch
    {
        [HarmonyPriority(800)]
        public static bool Prefix(
            Inventory __instance,
            ref Vector2i __result,
            bool topFirst,
            int ___m_height,
            int ___m_width)
        {
            if (__instance.GetName() != "Inventory" || !BetterArchery.configQuiverEnabled.Value)
                return true;
            if (topFirst)
            {
                int num = __instance.GetHeight() - 1;
                if (__instance.GetHeight() > 5)
                    --num;
                for (int index1 = 0; index1 < num; ++index1)
                {
                    for (int index2 = 0; index2 < ___m_width; ++index2)
                    {
                        if (__instance.GetItemAt(index2, index1) == null)
                        {
                            __result = new Vector2i(index2, index1);
                            return false;
                        }
                    }
                }
            }
            else
            {
                int num = __instance.GetHeight() - 1;
                if (__instance.GetHeight() > 5)
                    --num;
                for (int index3 = 1; index3 < num; ++index3)
                {
                    for (int index4 = 0; index4 < ___m_width; ++index4)
                    {
                        if (__instance.GetItemAt(index4, index3) == null)
                        {
                            __result = new Vector2i(index4, index3);
                            return false;
                        }
                    }
                }
            }
            __result = new Vector2i(-1, -1);
            return false;
        }
    }
}