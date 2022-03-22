// Decompiled with JetBrains decompiler
// Type: Common.RecipeConfig
// Assembly: BetterArchery, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5341B858-EB5D-47DA-A64D-602D91E9CB5C
// Assembly location: C:\Users\mugen\Desktop\BetterArchery.dll

using System;
using System.Collections.Generic;
using Common;

namespace BetterArchery.Common
{
  [Serializable]
  public class RecipeConfig
  {
    public string name = "";
    public string item = "";
    public int amount = 1;
    public string craftingStation = "";
    public int minStationLevel = 1;
    public bool enabled = true;
    public string repairStation = "";
    public List<RecipeRequirementConfig> resources = new List<RecipeRequirementConfig>();
  }
}
