// Decompiled with JetBrains decompiler
// Type: 
// Assembly: BetterArchery, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5341B858-EB5D-47DA-A64D-602D91E9CB5C
// Assembly location: C:\Users\mugen\Desktop\BetterArchery.dll

using BepInEx;
using BepInEx.Configuration;
using Common;
using HarmonyLib;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BetterArchery.Common;
using UnityEngine;
using UnityEngine.Networking;

namespace BetterArchery
{
  [BepInPlugin("ishid4.mods.betterarchery", "Better Archery", "1.7.0")]
  public class BetterArchery : BaseUnityPlugin
  {
    public static bool zoomSFX;
    public static Dictionary<string, Dictionary<string, AudioClip>> customSFXDict = new Dictionary<string, Dictionary<string, AudioClip>>();
    public static bool speedReduction;
    public static float ZoomInTimer = 0.0f;
    public static float ZoomOutTimer;
    public static float ZoomOutDelayTimer = 0.0f;
    public static float __BaseFov;
    public static float __LastZoomFov;
    public static float __NewZoomFov = 0.0f;
    public static ZoomState __ZoomState;
    public const int QuiverUseSlotCount = 3;
    public static int QuiverRowIndex = 0;
    public static ConfigEntry<KeyboardShortcut> HoldingKeyCode;
    public static ConfigEntry<KeyboardShortcut>[] KeyCodes = new ConfigEntry<KeyboardShortcut>[3];
    public static ConfigEntry<float>[] ArrowRetrieveChance = new ConfigEntry<float>[10];
    public static ConfigEntry<string>[] ArrowRetrieveSpawnType = new ConfigEntry<string>[10];
    public static List<Arrow> ArrowRetrieves = new List<Arrow>();
    public static ConfigEntry<float> ArrowDisappearTime;
    public static ConfigEntry<bool> ArrowDisappearOnHit;
    public static ConfigEntry<bool> ArrowRetrieveOldVersion;
    public static ConfigEntry<Vector2> InventoryQuiverSlotLocation;
    public static ConfigEntry<bool> QuiverHudEnabled;
    public static ConfigEntry<float> ArrowVelocity;
    public static ConfigEntry<float> ArrowGravity;
    public static ConfigEntry<float> ArrowAccuracy;
    public static ConfigEntry<Vector3> ArrowAimDir;
    public static ConfigEntry<bool> configQuiverEnabled;
    public static ConfigEntry<bool> configArrowImprovementsEnabled;
    public static ConfigEntry<bool> configRetrievableArrowsEnabled;
    public static ConfigEntry<bool> configBowZoomEnabled;
    public static ConfigEntry<KeyboardShortcut> BowDrawCancelKey;
    public static ConfigEntry<float> BowZoomFactor;
    public static ConfigEntry<bool> AutomaticBowZoom;
    public static ConfigEntry<KeyboardShortcut> BowZoomKey;
    public static ConfigEntry<float> BowZoomConstantTime;
    public static ConfigEntry<bool> BowZoomSFXEnabled;
    public static ConfigEntry<float> StayInZoomTime;
    public static ConfigEntry<bool> IsCrosshairVisible;
    public static ConfigEntry<bool> IsBowCrosshairVisible;
    public static ConfigEntry<bool> ShowSneakDamage;
    public static ConfigEntry<bool> BowDrawMovementSpeedReductionEnabled;
    public static ConfigEntry<DebugLevel> _DebugLevel;
    public static RecipesConfig Recipes;
    public static readonly Dictionary<string, GameObject> Prefabs = new Dictionary<string, GameObject>();
    public static ConfigEntry<int> nexusID;
    public static AudioSource playerAudioSource;
    private Harmony _harmony;
    public static BetterArchery _instance;

    public static void Log(string str = "", int warningType = 2)
    {
      if (_DebugLevel.Value == DebugLevel.LevelNone)
        return;
      if ((_DebugLevel.Value == DebugLevel.Level0 || _DebugLevel.Value == DebugLevel.Level1 || _DebugLevel.Value == DebugLevel.Level2) && warningType == 0)
        Debug.LogError((object) ("[" + typeof (BetterArchery).Namespace + "]: " + str));
      if ((_DebugLevel.Value == DebugLevel.Level1 || _DebugLevel.Value == DebugLevel.Level2) && warningType == 1)
        Debug.LogWarning((object) ("[" + typeof (BetterArchery).Namespace + "]: " + str));
      if (_DebugLevel.Value != DebugLevel.Level2 || warningType != 2)
        return;
      Debug.Log((object) ("[" + typeof (BetterArchery).Namespace + "]: " + str));
    }

    private void Awake()
    {
       _instance = this;
      configQuiverEnabled = this.Config.Bind<bool>("Quiver", "Enable Quiver", true, "(Charlie) Enable the quiver. Don't change this value while in the game. If you disable this while arrows are in the quiver, you will LOSE ALL OF THEM!");
      InventoryQuiverSlotLocation = this.Config.Bind<Vector2>("Quiver", "Change location of inventory quiver slot", new Vector2(3f, -25f), "If you lower the y point, the quiver slot will go down. For More Slots you can use this location, 'x:3.0, y:-167.0'.");
      QuiverHudEnabled = this.Config.Bind<bool>("Quiver", "Enable Quiver Hud", false, "(Currently, there is no hud.) Enable the quiver's squares on the screen/hud. This doesn't affect inventory.");
      HoldingKeyCode = this.Config.Bind<KeyboardShortcut>("Quiver", "Quiver slot hotkey holding key", new KeyboardShortcut(KeyCode.LeftAlt, Array.Empty<KeyCode>()), "Change holding key. For the inputs: https://docs.unity3d.com/ScriptReference/KeyCode.html");
      KeyCodes[0] = this.Config.Bind<KeyboardShortcut>("Quiver", "Quiver slot hotkey 1", new KeyboardShortcut(KeyCode.Alpha1, Array.Empty<KeyCode>()), "Hotkey for Quiver Slot 1. For the inputs: https://docs.unity3d.com/ScriptReference/KeyCode.html");
      KeyCodes[1] = this.Config.Bind<KeyboardShortcut>("Quiver", "Quiver slot hotkey 2", new KeyboardShortcut(KeyCode.Alpha2, Array.Empty<KeyCode>()), "Hotkey for Quiver Slot 2. For the inputs: https://docs.unity3d.com/ScriptReference/KeyCode.html");
      KeyCodes[2] = this.Config.Bind<KeyboardShortcut>("Quiver", "Quiver slot hotkey 3", new KeyboardShortcut(KeyCode.Alpha3, Array.Empty<KeyCode>()), "Hotkey for Quiver Slot 3. For the inputs: https://docs.unity3d.com/ScriptReference/KeyCode.html");
      configArrowImprovementsEnabled = this.Config.Bind<bool>("Arrow Improvements", "Enable Arrow Improvements", true, "Arrow improvements including aim problems fixes, gravity changes.");
      ArrowVelocity = this.Config.Bind<float>("Arrow Improvements", "Set Arrow Velocity", 70f, "Change the arrow's velocity. Vanilla is '60'.");
      ArrowGravity = this.Config.Bind<float>("Arrow Improvements", "Set Arrow Gravity", 15f, "Change the arrow's gravity. Vanilla is '5'.");
      ArrowAccuracy = this.Config.Bind<float>("Arrow Improvements", "Set Arrow Accuracy", 0.0f, "Change the arrow's accuracy. Vanilla is '-1'.");
      ArrowAimDir = this.Config.Bind<Vector3>("Arrow Improvements", "Set Aim Direction", new Vector3(0.0f, 0.06f, 0.0f), "Change the aim direction. Vanilla is 'x:0.0, y:0.0, z: 0.0'.");
      configBowZoomEnabled = this.Config.Bind<bool>("Bow Zoom", "Enable Bow Zoom", true, "Enable the zooming with bow.");
      AutomaticBowZoom = this.Config.Bind<bool>("Bow Zoom", "Automatic Bow Zoom", false, "Zoom while drawing bow automatically.");
      BowZoomSFXEnabled = this.Config.Bind<bool>("Bow Zoom", "Enable Bow Zoom SFX", true, "Sound effects for zoom-in and zoom-out.");
      BowZoomFactor = this.Config.Bind<float>("Bow Zoom", "Zoom Factor", 2f, "Max zoom.");
      BowZoomConstantTime = this.Config.Bind<float>("Bow Zoom", "Bow Zoom Constant Time", -1f, "Change this value to '-1' if you don't want constant time while zooming. '1' is recommended.");
      BowZoomKey = this.Config.Bind<KeyboardShortcut>("Bow Zoom", "Bow Zoom Hotkey", new KeyboardShortcut(KeyCode.Mouse1, Array.Empty<KeyCode>()), "Mouse0: Left Click, Mouse1: Right Click, Mouse2: Middle Click. For the other inputs: https://docs.unity3d.com/ScriptReference/KeyCode.html");
      BowDrawCancelKey = this.Config.Bind<KeyboardShortcut>("Bow Zoom", "Bow Draw Cancel Hotkey", new KeyboardShortcut(KeyCode.E, Array.Empty<KeyCode>()), "Mouse0: Left Click, Mouse1: Right Click, Mouse2: Middle Click. For the other inputs: https://docs.unity3d.com/ScriptReference/KeyCode.html");
      StayInZoomTime = this.Config.Bind<float>("Bow Zoom", "Stay In-Zoom Time", 2f, "Set the max time of staying on zoom while holding RMB after releasing an arrow.");
      configRetrievableArrowsEnabled = this.Config.Bind<bool>("Retrievable Arrows", "Enable Retrievable Arrows", true, "Enable the retrievable arrows.");
      ArrowDisappearTime = this.Config.Bind<float>("Retrievable Arrows", "Arrow Disappear Time", 60f, "Set arrow's disappear countdown time.");
      ArrowDisappearOnHit = this.Config.Bind<bool>("Retrievable Arrows", "Arrow Disappear On Hit", false, "Make arrows disappear when they are not retrievable.");
      ArrowRetrieveOldVersion = this.Config.Bind<bool>("Retrievable Arrows", "Enable Old Retrievable Version", false, "Instantly arrow drops to the ground on hit if retrievable. Enabling this will ignore 'Arrow Disappear On Hit' and 'Arrow Disappear Time' settings.");
      ArrowRetrieveChance[0] = this.Config.Bind<float>("Retrievable Arrows", "ArrowRetrieveChance for Wooden Arrow", 0.2f, "Example: 0.9 for 90% chance to retrieve.");
      ArrowRetrieveChance[1] = this.Config.Bind<float>("Retrievable Arrows", "ArrowRetrieveChance for Flint Arrow", 0.3f, "Example: 0.9 for 90% chance to retrieve.");
      ArrowRetrieveChance[2] = this.Config.Bind<float>("Retrievable Arrows", "ArrowRetrieveChance for Bronze Arrow", 0.5f, "Example: 0.9 for 90% chance to retrieve.");
      ArrowRetrieveChance[3] = this.Config.Bind<float>("Retrievable Arrows", "ArrowRetrieveChance for Iron Arrow", 0.7f, "Example: 0.9 for 90% chance to retrieve.");
      ArrowRetrieveChance[4] = this.Config.Bind<float>("Retrievable Arrows", "ArrowRetrieveChance for Obsidian Arrow", 0.7f, "Example: 0.9 for 90% chance to retrieve.");
      ArrowRetrieveChance[5] = this.Config.Bind<float>("Retrievable Arrows", "ArrowRetrieveChance for Needle Arrow", 0.1f, "Example: 0.9 for 90% chance to retrieve.");
      ArrowRetrieveChance[6] = this.Config.Bind<float>("Retrievable Arrows", "ArrowRetrieveChance for Fire Arrow", 0.0f, "Example: 0.9 for 90% chance to retrieve.");
      ArrowRetrieveChance[7] = this.Config.Bind<float>("Retrievable Arrows", "ArrowRetrieveChance for Poison Arrow", 0.7f, "Example: 0.9 for 90% chance to retrieve.");
      ArrowRetrieveChance[8] = this.Config.Bind<float>("Retrievable Arrows", "ArrowRetrieveChance for Silver Arrow", 0.5f, "Example: 0.9 for 90% chance to retrieve.");
      ArrowRetrieveChance[9] = this.Config.Bind<float>("Retrievable Arrows", "ArrowRetrieveChance for Frost Arrow", 0.7f, "Example: 0.9 for 90% chance to retrieve.");
      ArrowRetrieveSpawnType[0] = this.Config.Bind<string>("Retrievable Arrows", "ArrowRetrieveSpawnType for Wooden Arrow", "ArrowWood", "https://valheim.fandom.com/wiki/Category:Arrows use internal ids.");
      ArrowRetrieveSpawnType[1] = this.Config.Bind<string>("Retrievable Arrows", "ArrowRetrieveSpawnType for Flint Arrow", "ArrowFlint", "https://valheim.fandom.com/wiki/Category:Arrows use internal ids.");
      ArrowRetrieveSpawnType[2] = this.Config.Bind<string>("Retrievable Arrows", "ArrowRetrieveSpawnType for Bronze Arrow", "ArrowBronze", "https://valheim.fandom.com/wiki/Category:Arrows use internal ids.");
      ArrowRetrieveSpawnType[3] = this.Config.Bind<string>("Retrievable Arrows", "ArrowRetrieveSpawnType for Iron Arrow", "ArrowIron", "https://valheim.fandom.com/wiki/Category:Arrows use internal ids.");
      ArrowRetrieveSpawnType[4] = this.Config.Bind<string>("Retrievable Arrows", "ArrowRetrieveSpawnType for Obsidian Arrow", "ArrowObsidian", "https://valheim.fandom.com/wiki/Category:Arrows use internal ids.");
      ArrowRetrieveSpawnType[5] = this.Config.Bind<string>("Retrievable Arrows", "ArrowRetrieveSpawnType for Needle Arrow", "ArrowNeedle", "https://valheim.fandom.com/wiki/Category:Arrows use internal ids.");
      ArrowRetrieveSpawnType[6] = this.Config.Bind<string>("Retrievable Arrows", "ArrowRetrieveSpawnType for Fire Arrow", "ArrowFire", "https://valheim.fandom.com/wiki/Category:Arrows use internal ids.");
      ArrowRetrieveSpawnType[7] = this.Config.Bind<string>("Retrievable Arrows", "ArrowRetrieveSpawnType for Poison Arrow", "ArrowObsidian", "https://valheim.fandom.com/wiki/Category:Arrows use internal ids.");
      ArrowRetrieveSpawnType[8] = this.Config.Bind<string>("Retrievable Arrows", "ArrowRetrieveSpawnType for Silver Arrow", "ArrowSilver", "https://valheim.fandom.com/wiki/Category:Arrows use internal ids.");
      ArrowRetrieveSpawnType[9] = this.Config.Bind<string>("Retrievable Arrows", "ArrowRetrieveSpawnType for Frost Arrow", "ArrowObsidian", "https://valheim.fandom.com/wiki/Category:Arrows use internal ids.");
      nexusID = this.Config.Bind<int>("Other", "NexusID", 348, "Nexus mod ID for updates.");
      IsCrosshairVisible = this.Config.Bind<bool>("Other", "Enable Crosshair", true, "You can hide your crosshair.");
      IsBowCrosshairVisible = this.Config.Bind<bool>("Other", "Enable Bow Crosshair", true, "You can hide your bow crosshair, circle one.");
      ShowSneakDamage = this.Config.Bind<bool>("Other", "Enable Sneak Damage Showing", true, "Show sneak damage at top left.");
      BowDrawMovementSpeedReductionEnabled = this.Config.Bind<bool>("Other", "Enable Bow Draw Movement Speed Reduction", true, "Set walk speed while drawing bow.");
      _DebugLevel = this.Config.Bind<DebugLevel>("Other", "Set Debugging Level", DebugLevel.Level1, "Level0: Only Errors, Level1: Errors and Warnings, Level2: Everything, LevelNone: Nothing");
      ArrowRetrieves.Add(new Arrow()
      {
        Name = "ArrowWood",
        SpawnChance = ArrowRetrieveChance[0].Value,
        SpawnArrow = ArrowRetrieveSpawnType[0].Value
      });
      ArrowRetrieves.Add(new Arrow()
      {
        Name = "ArrowFlint",
        SpawnChance = ArrowRetrieveChance[1].Value,
        SpawnArrow = ArrowRetrieveSpawnType[1].Value
      });
      ArrowRetrieves.Add(new Arrow()
      {
        Name = "ArrowBronze",
        SpawnChance = ArrowRetrieveChance[2].Value,
        SpawnArrow = ArrowRetrieveSpawnType[2].Value
      });
      ArrowRetrieves.Add(new Arrow()
      {
        Name = "ArrowIron",
        SpawnChance = ArrowRetrieveChance[3].Value,
        SpawnArrow = ArrowRetrieveSpawnType[3].Value
      });
      ArrowRetrieves.Add(new Arrow()
      {
        Name = "ArrowObsidian",
        SpawnChance = ArrowRetrieveChance[4].Value,
        SpawnArrow = ArrowRetrieveSpawnType[4].Value
      });
      ArrowRetrieves.Add(new Arrow()
      {
        Name = "ArrowNeedle",
        SpawnChance = ArrowRetrieveChance[5].Value,
        SpawnArrow = ArrowRetrieveSpawnType[5].Value
      });
      ArrowRetrieves.Add(new Arrow()
      {
        Name = "ArrowFire",
        SpawnChance = ArrowRetrieveChance[6].Value,
        SpawnArrow = ArrowRetrieveSpawnType[6].Value
      });
      ArrowRetrieves.Add(new Arrow()
      {
        Name = "ArrowPoison",
        SpawnChance = ArrowRetrieveChance[7].Value,
        SpawnArrow = ArrowRetrieveSpawnType[7].Value
      });
      ArrowRetrieves.Add(new Arrow()
      {
        Name = "ArrowSilver",
        SpawnChance = ArrowRetrieveChance[8].Value,
        SpawnArrow = ArrowRetrieveSpawnType[8].Value
      });
      ArrowRetrieves.Add(new Arrow()
      {
        Name = "ArrowFrost",
        SpawnChance = ArrowRetrieveChance[9].Value,
        SpawnArrow = ArrowRetrieveSpawnType[9].Value
      });
      Recipes = LoadJsonFile<RecipesConfig>("BetterArcheryRecipes.json");
      if (configQuiverEnabled.Value)
      {
        AssetBundle assetBundle = LoadAssetBundle("quiverassets");
        if (Recipes != null && (UnityEngine.Object) assetBundle != (UnityEngine.Object) null)
        {
          foreach (RecipeConfig recipe in Recipes.recipes)
          {
            if (assetBundle.Contains(recipe.item))
            {
              GameObject gameObject = assetBundle.LoadAsset<GameObject>(recipe.item);
              Prefabs.Add(recipe.item, gameObject);
            }
          }
        }
        assetBundle?.Unload(false);
      }
      _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
    }

    private void OnDestroy()
    {
      this._harmony?.UnpatchSelf();
      foreach (UnityEngine.Object @object in Prefabs.Values)
        UnityEngine.Object.Destroy(@object);
      Prefabs.Clear();
    }

    private void Update()
    {
      if (!configQuiverEnabled.Value)
        return;
      Player localPlayer = Player.m_localPlayer;
      if ((UnityEngine.Object) localPlayer != (UnityEngine.Object) null && localPlayer.TakeInput())
      {
        for (int index = 0; index < 3; ++index)
          CheckQuiverUseInput(localPlayer, index);
      }
    }

    public static string GetBindingLabel(int index)
    {
      index = Mathf.Clamp(index, 0, 2);
      KeyCode bindingKeycode = GetBindingKeycode(index);
      return bindingKeycode.ToString().Contains("Alpha") ? bindingKeycode.ToString().Replace("Alpha", "") : bindingKeycode.ToString().ToUpperInvariant();
    }

    public static KeyCode GetBindingKeycode(int index)
    {
      index = Mathf.Clamp(index, 0, 2);
      return KeyCodes[index].Value.MainKey;
    }

    public static int GetBonusInventoryRowIndex() => (UnityEngine.Object) Player.m_localPlayer != (UnityEngine.Object) null && (uint) QuiverRowIndex > 0U ? QuiverRowIndex : 0;

    public static void CheckQuiverUseInput(Player player, int index)
    {
      KeyCode bindingKeycode = GetBindingKeycode(index);
      if (!(!(HoldingKeyCode.Value.MainKey.ToString() != "") ? Input.GetKeyDown(bindingKeycode) : Input.GetKey(HoldingKeyCode.Value.MainKey) && Input.GetKeyDown(bindingKeycode)))
        return;
      int inventoryRowIndex = GetBonusInventoryRowIndex();
      ItemDrop.ItemData itemAt = player.GetInventory().GetItemAt(index, inventoryRowIndex);
      if (itemAt != null)
        player.UseItem((Inventory) null, itemAt, false);
    }

    public static bool IsQuiverSlot(Vector2i pos) => IsQuiverSlot(pos.x, pos.y);

    public static bool IsQuiverSlot(int x, int y)
    {
      int inventoryRowIndex = GetBonusInventoryRowIndex();
      return y == inventoryRowIndex && x >= 0 && x < 3;
    }

    public static Vector2i GetQuiverSlotPosition(int index)
    {
      int inventoryRowIndex = GetBonusInventoryRowIndex();
      return new Vector2i(index, inventoryRowIndex);
    }

    public static bool IsQuiverEquipped() => !((UnityEngine.Object) Player.m_localPlayer == (UnityEngine.Object) null) && CustomSlotCreator.IsSlotOccupied((Humanoid) Player.m_localPlayer, "quiver");

    private static T LoadJsonFile<T>(string filename) where T : class
    {
      string assetPath = GetAssetPath(filename);
      return !string.IsNullOrEmpty(assetPath) ? JsonMapper.ToObject<T>(File.ReadAllText(assetPath)) : default (T);
    }

    private static AssetBundle LoadAssetBundle(string filename)
    {
      string assetPath = GetAssetPath(filename);
      return !string.IsNullOrEmpty(assetPath) ? AssetBundle.LoadFromFile(assetPath) : (AssetBundle) null;
    }

    public static string GetAssetPath(string assetName, bool isDirectory = false)
    {
      string path = Path.Combine(Paths.PluginPath, nameof (BetterArchery), assetName);
      if (isDirectory)
      {
        if (!Directory.Exists(path))
        {
          path = Path.Combine(Path.GetDirectoryName(typeof (BetterArchery).Assembly.Location), assetName);
          if (!Directory.Exists(path))
          {
            Log("Could not find directory (" + assetName + ").", 1);
            return (string) null;
          }
        }
        return path;
      }
      if (!File.Exists(path))
      {
        path = Path.Combine(Path.GetDirectoryName(typeof (BetterArchery).Assembly.Location), assetName);
        if (!File.Exists(path))
        {
          Log("Could not find asset (" + assetName + ").", 1);
          return (string) null;
        }
      }
      return path;
    }

    public static void TryCreateCustomSlot(ZNetScene zNetScene)
    {
      if ((UnityEngine.Object) zNetScene == (UnityEngine.Object) null || !configQuiverEnabled.Value)
        return;
      CustomSlotCreator.ApplyCustomSlotItem(zNetScene.GetPrefab("LeatherQuiver"), "quiver");
    }

    public static void TryCreateCustomSFX()
    {
      if (!configBowZoomEnabled.Value || !BowZoomSFXEnabled.Value)
        return;
      string assetPath = GetAssetPath("SFX", true);
      if (string.IsNullOrEmpty(assetPath))
        return;
      Log("path: " + assetPath);
      foreach (string directory in Directory.GetDirectories(assetPath))
      {
        Log("Checking folder " + Path.GetFileName(directory));
        customSFXDict[Path.GetFileName(directory)] = new Dictionary<string, AudioClip>();
        foreach (string file in Directory.GetFiles(directory))
        {
          if (Path.GetExtension(file).ToLower().Equals(".wav"))
          {
            Log("Checking file " + Path.GetFileName(file));
            _instance.StartCoroutine(LoadSFXCoroutine(file, customSFXDict[Path.GetFileName(directory)]));
          }
        }
      }
    }

    public static void PlayCustomSFX(string name, bool checkIfPlaying = true)
    {
      if ((UnityEngine.Object) playerAudioSource == (UnityEngine.Object) null || !BowZoomSFXEnabled.Value)
        return;
      ZSFX component = playerAudioSource.GetComponent<ZSFX>();
      if (component.IsPlaying() & checkIfPlaying)
        return;
      if (customSFXDict.ContainsKey(name))
      {
        component.m_audioClips = customSFXDict[name].Values.ToArray<AudioClip>();
        component.Play();
      }
      else
        Log("SFX not found.", 0);
    }

    public static IEnumerator LoadSFXCoroutine(
      string path,
      Dictionary<string, AudioClip> dict)
    {
      path = "file:///" + path;
      bool flag;
      using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.WAV))
      {
        yield return (object) www.SendWebRequest();
        if (www == null)
        {
          Log("ww error.", 0);
          flag = false;
        }
        else
        {
          DownloadHandlerAudioClip dh = (DownloadHandlerAudioClip) www.downloadHandler;
          if (dh != null)
          {
            AudioClip ac = dh.audioClip;
            if ((UnityEngine.Object) ac != (UnityEngine.Object) null)
            {
              ac.name = Path.GetFileNameWithoutExtension(path);
              if (!dict.ContainsKey(ac.name))
                dict[ac.name] = ac;
              Log("Added " + ac.name + " SFX.");
              flag = false;
              goto label_11;
            }
            else
              ac = (AudioClip) null;
          }
          Log("Error while adding custom SFX.", 0);
          flag = false;
        }
label_11:;
      }
      yield return flag;
    }

    public static void TryRegisterPrefabs(ZNetScene zNetScene)
    {
      if ((UnityEngine.Object) zNetScene == (UnityEngine.Object) null)
        return;
      foreach (GameObject gameObject in Prefabs.Values)
      {
        if (!configQuiverEnabled.Value)
        {
          Log(gameObject.name);
          if (gameObject.name.Contains("Quiver"))
          {
            Debug.Log((object) ("[PrefabCreator] " + gameObject.name + " prefab skipped."));
            continue;
          }
        }
        if (!zNetScene.m_prefabs.Contains(gameObject))
          zNetScene.m_prefabs.Add(gameObject);
      }
    }

    public static bool IsObjectDBReady() => (UnityEngine.Object) ObjectDB.instance != (UnityEngine.Object) null && ObjectDB.instance.m_items.Count != 0 && (UnityEngine.Object) ObjectDB.instance.GetItemPrefab("Amber") != (UnityEngine.Object) null;

    public static void TryRegisterItems()
    {
      if (!IsObjectDBReady())
        return;
      foreach (GameObject gameObject in Prefabs.Values)
      {
        ItemDrop component = gameObject.GetComponent<ItemDrop>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        {
          if (!configQuiverEnabled.Value && component.m_itemData.m_dropPrefab.name.Contains("Quiver"))
            Debug.Log((object) ("[PrefabCreator] Skipped prefab: " + component.m_itemData.m_dropPrefab.name));
          else if ((UnityEngine.Object) ObjectDB.instance.GetItemPrefab(gameObject.name.GetStableHashCode()) == (UnityEngine.Object) null)
            ObjectDB.instance.m_items.Add(gameObject);
        }
      }
    }

    public static void TryRegisterRecipes()
    {
      if (!IsObjectDBReady())
        return;
      PrefabCreator.Reset();
      foreach (RecipeConfig recipe in Recipes.recipes)
      {
        if (!configQuiverEnabled.Value && recipe.item.Contains("Quiver"))
          Debug.Log((object) ("[PrefabCreator] Skipped recipe: " + recipe.item));
        else
          PrefabCreator.AddNewRecipe(recipe.name, recipe.item, recipe);
      }
    }

    public enum ZoomState
    {
      Fixed,
      ZoomingIn,
      ZoomingOut,
    }

    public enum DebugLevel
    {
       Level0,
       Level1,
       Level2,
       LevelNone,
    }
  }
}
