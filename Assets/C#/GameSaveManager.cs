using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSaveManager : MonoBehaviour {
    // Game save manager for saving game information like health, progress, and so much
    // When adding a new key for usage, please add the key to the ALL_PLAYERPREF_KEYS array
    public static string CLEARED_DUNGEON_KEY_COMBINATION_KEY = "ClearedDungeonKeyCombination";
    public static string CLEARED_DUNGEON_COUNT_KEY = "ClearedDungeons";
    public static string PLAYER_HEALTH_KEY = "PlayerHealth";
    public static string PLAYER_MAX_HEALTH_KEY = "PlayerMaxHealth";
    public static string PLAYER_MAGIC_KEY = "PlayerMagic";
    public static string PLAYER_MAX_MAGIC_KEY = "PlayerMaxMagic";
    public static string ARROW_COUNT_KEY = "ArrowCount";
    public static string UPGRADE_POTION_KEY = "UpgradePotion";
    public static string UPGRADE_KIT_KEY = "UpgradeKit";

    public static string[] ALL_GAMESAVE_PLAYERPREF_KEYS = { CLEARED_DUNGEON_COUNT_KEY, PLAYER_HEALTH_KEY, PLAYER_MAX_HEALTH_KEY, PLAYER_MAGIC_KEY, PLAYER_MAX_MAGIC_KEY, ARROW_COUNT_KEY, UPGRADE_POTION_KEY, UPGRADE_KIT_KEY };
    public static void NewGame() {
        string keyCombination = PlayerPrefs.GetString(CLEARED_DUNGEON_KEY_COMBINATION_KEY);
        print("Dungeons that we are clearing: " + keyCombination);
        foreach (string key in keyCombination.Split(' ')) {
            PlayerPrefs.DeleteKey(key);
        }

        foreach (string s in ALL_GAMESAVE_PLAYERPREF_KEYS) {
            PlayerPrefs.DeleteKey(s);
        }
        SetPlayerHealth(100f);
        SetPlayerMaxHealth(100f);

        SetPlayerMagic(100f);
        SetPlayerMaxMagic(100f);

    }

    public static void SaveGame(StatsController stats, InventoryController inventory)
    {
        // Save health + max, magic + max, arrows, upgrade kits, upgrade potions
        SetPlayerHealth(stats.GetHealth());
        SetPlayerMaxHealth(stats.GetHealthMax());

        SetPlayerMagic(stats.GetMagic());
        SetPlayerMaxMagic(stats.GetMagicMax());

        SetArrowCount(stats.arrowCount);

        SetUpgradeKitCount(inventory.getUpgradeKits());
        SetUpgradePotionCount(inventory.getUpgradePotions());
    }
    public static void LoadGame(StatsController stats, InventoryController inventory)
    {
        float maxHealth = GetPlayerMaxHealth();
        float maxMagic = GetPlayerMaxMagic();

        stats.SetMaxHealth(maxHealth);
        stats.SetHealth(maxHealth);

        stats.SetMaxMagic(maxMagic);
        stats.SetMagic(maxMagic);

        stats.arrowCount = GetArrowCount();

        inventory.SetUpgradeKits(GetUpgradeKitCount());
        inventory.SetUpgradePotions(GetUpgradePotionCount());
        
    }

    public static int GetClearedDungeonCount() {
        return GetInt(CLEARED_DUNGEON_COUNT_KEY);
    }
    public static void SetClearedDungeonCount(int num) {
        Set(CLEARED_DUNGEON_COUNT_KEY, num);
    }

    public static float GetPlayerHealth() {
        return GetFloat(PLAYER_HEALTH_KEY);
    }
    public static void SetPlayerHealth(float num) {
        Set(PLAYER_HEALTH_KEY, num);
    }

    public static float GetPlayerMaxHealth() {
        return GetFloat(PLAYER_MAX_HEALTH_KEY);
    }
    public static void SetPlayerMaxHealth(float num) {
        Set(PLAYER_MAX_HEALTH_KEY, num);
    }

    public static float GetPlayerMagic() {
        return GetFloat(PLAYER_MAGIC_KEY);
    }
    public static void SetPlayerMagic(float num) {
        Set(PLAYER_MAGIC_KEY, num);
    }

    public static float GetPlayerMaxMagic() {
        return GetFloat(PLAYER_MAX_MAGIC_KEY);
    }
    public static void SetPlayerMaxMagic(float num) {
        Set(PLAYER_MAX_MAGIC_KEY, num);
    }

    public static int GetArrowCount()
    {
        return GetInt(ARROW_COUNT_KEY);
    }
    public static void SetArrowCount(int num)
    {
        Set(ARROW_COUNT_KEY, num);
    }
    public static int GetUpgradeKitCount()
    {
        return GetInt(UPGRADE_KIT_KEY);
    }
    public static void SetUpgradeKitCount(int num)
    {
        Set(UPGRADE_KIT_KEY, num);
    }
    public static int GetUpgradePotionCount()
    {
        return GetInt(UPGRADE_POTION_KEY);
    }
    public static void SetUpgradePotionCount(int num)
    {
        Set(UPGRADE_POTION_KEY, num);
    }

    /**
     * Adds the completed dungeon to the completed dungeon string, if it does not exist in there already
     */
    public static void AddComlpetedDungeon(string dungeonSeed) {
        Debug.Log("Adding a completed dungeon: " + dungeonSeed);

        string dungeonString = PlayerPrefs.GetString(CLEARED_DUNGEON_KEY_COMBINATION_KEY);
        print(dungeonString);
        if (!dungeonString.Contains(dungeonSeed)) {
            print("foo:");
            dungeonString += (dungeonSeed + " ");
        }
        PlayerPrefs.SetString(CLEARED_DUNGEON_KEY_COMBINATION_KEY, dungeonString);

        Debug.Log("New dungeon string: " + dungeonString);

    }




    /**
     * Get the integer of the given key, and return 0 if it does not exist
     */
    private static int GetInt(string key) {
        if (PlayerPrefs.HasKey(key)) {
            return PlayerPrefs.GetInt(key);
        } else {
            return 0;
        }
    }
    /**
     * Sets the given value through player prefs
     */
    private static void Set(string key, int num) {
        PlayerPrefs.SetInt(key, num);
    }

    /**
     * Get the float of the given key, and return 0 if it does not exist
     */
    private static float GetFloat(string key) {
        if (PlayerPrefs.HasKey(key)) {
            return PlayerPrefs.GetFloat(key);
        } else {
            return 0;
        }
    }
    /**
     * Sets the given value through player prefs
     */
    private static void Set(string key, float num) {
        PlayerPrefs.SetFloat(key, num);
    }

    private static string GetString(string key) {
        if (PlayerPrefs.HasKey(key)) {
            return PlayerPrefs.GetString(key);
        } else {
            return "";
        }
    }

    private static void Set(string key, string val) {
        PlayerPrefs.SetString(key, val);
    }
}
