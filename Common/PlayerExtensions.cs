// Decompiled with JetBrains decompiler
// Type: PlayerExtensions
// Assembly: BetterArchery, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5341B858-EB5D-47DA-A64D-602D91E9CB5C
// Assembly location: C:\Users\mugen\Desktop\BetterArchery.dll

namespace BetterArchery.Common;

public static class PlayerExtensions
{
  public static ZDO GetZDO(this Player player) => player.m_nview.GetZDO();
}