// Decompiled with JetBrains decompiler
// Type: Common.Utils
// Assembly: BetterArchery, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5341B858-EB5D-47DA-A64D-602D91E9CB5C
// Assembly location: C:\Users\mugen\Desktop\BetterArchery.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BepInEx;
using UnityEngine;

namespace BetterArchery.Common
{
  public static class Utils
  {
    public static void PrintObject(object o)
    {
      if (o == null)
        Debug.Log((object) "null");
      else
        Debug.Log((object) (o?.ToString() + ":\n" + Utils.GetObjectString(o, "  ")));
    }

    public static string GetObjectString(object obj, string indent)
    {
      string objectString = "";
      foreach (FieldInfo fieldInfo in ((IEnumerable<FieldInfo>) obj.GetType().GetFields()).Where<FieldInfo>((Func<FieldInfo, bool>) (f => f.IsPublic)))
      {
        object obj1 = fieldInfo.GetValue(obj);
        string str = obj1 == null ? "null" : obj1.ToString();
        objectString = objectString + "\n" + indent + fieldInfo.Name + ": " + str;
      }
      return objectString;
    }

    public static Sprite LoadSpriteFromFile(string spritePath)
    {
      spritePath = Path.Combine(Paths.PluginPath, spritePath);
      if (File.Exists(spritePath))
      {
        byte[] data = File.ReadAllBytes(spritePath);
        Texture2D texture2D = new Texture2D(20, 20);
        if (texture2D.LoadImage(data))
          return Sprite.Create(texture2D, new Rect(0.0f, 0.0f, (float) texture2D.width, (float) texture2D.height), new Vector2(), 100f);
      }
      return (Sprite) null;
    }

    public static Sprite LoadSpriteFromFile(string modFolder, string iconName) => Utils.LoadSpriteFromFile(Path.Combine(modFolder, iconName));
  }
}
