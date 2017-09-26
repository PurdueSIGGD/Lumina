using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

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
    public static string WEAPON_LIST_KEY = "WeaponList";
    public static string WEAPON_CONDITION_LIST_KEY = "WeaponConditionList";
    public static string MAGIC_LIST_KEY = "MagicList";
    public static string ARMOR_HELMET_KEY = "ArmorHelmet";
    public static string ARMOR_HELMET_CONDITION_KEY = "ArmorHelmetCondition";
    public static string ARMOR_CHESTPLATE_KEY = "ArmorChestplate";
    public static string ARMOR_CHESTPLATE_CONDITION_KEY = "ArmorChestplateCondition";
    public static string SCENE_NAME_KEY = "SceneName";
    public static string SCENE_LOCATION_X = "SceneLocationX";
    public static string SCENE_LOCATION_Y = "SceneLocationY";
    public static string SCENE_LOCATION_Z = "SceneLocationZ";
    public static string SCENE_ROTATION_X = "SceneRotationX";
    public static string SCENE_ROTATION_Y = "SceneRotationY";
    public static string SCENE_ROTATION_Z = "SceneRotationZ";

    public static string[] ALL_GAMESAVE_PLAYERPREF_KEYS = { SCENE_ROTATION_Z, SCENE_ROTATION_Y, SCENE_ROTATION_X, SCENE_LOCATION_Z, SCENE_LOCATION_Y, SCENE_LOCATION_X, SCENE_NAME_KEY, ARMOR_CHESTPLATE_CONDITION_KEY, ARMOR_CHESTPLATE_KEY, ARMOR_HELMET_CONDITION_KEY, ARMOR_HELMET_KEY, MAGIC_LIST_KEY, WEAPON_CONDITION_LIST_KEY, WEAPON_LIST_KEY, CLEARED_DUNGEON_COUNT_KEY, PLAYER_HEALTH_KEY, PLAYER_MAX_HEALTH_KEY, PLAYER_MAGIC_KEY, PLAYER_MAX_MAGIC_KEY, ARROW_COUNT_KEY, UPGRADE_POTION_KEY, UPGRADE_KIT_KEY };

    public static bool HasSave() {
        return PlayerPrefs.HasKey(ALL_GAMESAVE_PLAYERPREF_KEYS[0]);
    }
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

        // Clear inventory
        // for armor, -1 means no armor at all
        SetWeaponList(new int[] { 0 }); // add fists, and nothing else
        SetWeaponConditionList(new float[] { 100 });

        SetMagicList(new int[] { });

        SetHelmet(-1, 100);
        SetChestplate(-1, 100);

        SetPlayerScene("Island1");
        

    }
    
    public static void SaveGame(StatsController stats, InventoryController inventory, WeaponController rightHand, WeaponController leftHand)
    {
        if (SceneManager.GetActiveScene().name == "Dungeon") {
            //TODO popup message
            Debug.LogError("CANNOT SAVE INSIDE OF A DUNGEON, GO OUTSIDE YOU BLOCKHEAD");
            return;
        }
        // Save location, and current island
        SetPlayerLocation(stats.transform.position);
        SetPlayerRotation(stats.transform.localEulerAngles);
        SetPlayerScene(SceneManager.GetActiveScene().name);

        // Save health + max, magic + max, arrows, upgrade kits, upgrade potions
        SetPlayerHealth(stats.GetHealth());
        SetPlayerMaxHealth(stats.GetHealthMax());

        SetPlayerMagic(stats.GetMagic());
        SetPlayerMaxMagic(stats.GetMagicMax());

        SetArrowCount(stats.arrowCount);

        SetUpgradeKitCount(inventory.getUpgradeKits());
        SetUpgradePotionCount(inventory.getUpgradePotions());

        Weapon[] weaponList = rightHand.weapons;
        int[] weaponTypes = new int[weaponList.Length];
        float[] weaponConditions = new float[weaponList.Length];
        for (int i = 0; i < weaponList.Length; i++) {
            if (weaponList[i] != null) {
                //print(i + " " + weaponList[i]);
                weaponTypes[i] = GetMatchingNameIndex(weaponList[i].gameObject, rightHand.weaponTypes);
                weaponConditions[i] = weaponList[i].condition;
            } else {
                weaponTypes[i] = -1; //-1 for empty
                weaponConditions[i] = -1f; //-1 for empty
            }
        }

        SetWeaponList(weaponTypes);
        SetWeaponConditionList(weaponConditions);

        Weapon[] magicList = leftHand.weapons;
        int[] magicTypes = new int[magicList.Length];
        for (int i = 0; i < magicList.Length; i++) {
            if (magicList[i] != null) {
                //print(i + " " + magicList[i]);
                magicTypes[i] = GetMatchingNameIndex(magicList[i].gameObject, leftHand.weaponTypes);
            } else {
                magicTypes[i] = -1; //-1 for empty
            }
        }

        SetMagicList(magicTypes);

        if (inventory.helmet) {
            int helmetType = GetMatchingNameIndex(inventory.helmet.gameObject, inventory.helmetTypes);
            SetHelmet(helmetType, inventory.helmet.condition);
        }
        if (inventory.chestPlate) {
            int chestplateType = GetMatchingNameIndex(inventory.chestPlate.gameObject, inventory.chestplateTypes);
            SetChestplate(chestplateType, inventory.chestPlate.condition);
        }


    }
    // For when we want to find the index of an object in the possible options, like weapons or magic or armor
    private static int GetMatchingNameIndex(GameObject searchObject, GameObject[] objectArray) {
        GameObject idx;
        for (int i = 0; i < objectArray.Length; i++) {
            idx = objectArray[i];
            // Fists(Clone) contains Fists
            if (searchObject.name.Contains(idx.name)) {
                return i;
            }
        }
        Debug.LogError("SOMETHING SAVED AS -1 THIS AINT GOOD: " + searchObject.name);
        return -1; //fuck

    }
    public static void LoadGameScene() {


        // Load the game based off of the last save, and load the level
        print(GetPlayerScene());
        SceneManager.LoadScene(GetPlayerScene());
    }

    /* to be called upon only by the player when entering a scene */
    public static void LoadGameStats(StatsController stats, InventoryController inventory, WeaponController rightHand, WeaponController leftHand)
    {
        float maxHealth = GetPlayerMaxHealth();
        float maxMagic = GetPlayerMaxMagic();

        stats.SetMaxHealth(maxHealth);
        stats.SetHealth(GetPlayerHealth());

        stats.SetMaxMagic(maxMagic);
        stats.SetMagic(GetPlayerMagic());

        stats.SetArrows(GetArrowCount());

        inventory.SetUpgradeKits(GetUpgradeKitCount());
        inventory.SetUpgradePotions(GetUpgradePotionCount());

        int helmetType = GetHelmetType();
        int chestplateType = GetChestplateType();

        if (helmetType >= 0) inventory.SetArmor(0, helmetType, GetHelmetCondition());
        if (chestplateType >= 0) inventory.SetArmor(1, chestplateType, GetChestplateCondition());

        int[] weaponTypes = GetWeaponList();
        float[] weaponConditions = GetWeaponConditionList();
        EquipWeapons(rightHand, weaponTypes, weaponConditions);

        int[] magicTypes = GetMagicList();
        EquipWeapons(leftHand, magicTypes, null);

    }
    private static void EquipWeapons(WeaponController weaponController, int[] weaponTypes, float[] weaponConditions) {
        for (int i = 0; i < weaponTypes.Length; i++) {
            //print(i + " " + weaponController.weaponTypes.Length + " " +  weaponTypes.Length + " " + weaponTypes[i]);
            if (weaponTypes[i] != -1) {
                // Don't do it if it is -1... I think there's a glitch in playerprefsx that repeats a null value (oh shit it does, cause null is zero in this case)
                Weapon newWeapon = GameObject.Instantiate(weaponController.weaponTypes[weaponTypes[i]]).GetComponent<Weapon>();
                newWeapon.storedAmmo = 0; // Don't want to get duplicate ammo each time we start
                newWeapon.gameObject.SetActive(false); // Set active as false to hide it, in case two handed
                DontDestroyOnLoad(newWeapon.gameObject); // And we do this just in case the player does not equip the weapons before entering a dungeon
                newWeapon.transform.position = weaponController.transform.position;
                if (weaponConditions != null) newWeapon.condition = weaponConditions[i];
                if (i == 0) {
                    // Show animations, get states started
                    weaponController.EquipWeapon(newWeapon);
                } else {
                    weaponController.EquipWeaponInstantly(newWeapon, i);
                }
            }
            
        }
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
    public static int[] GetWeaponList() {
        return GetIntArray(WEAPON_LIST_KEY);
    }
    public static void SetWeaponList(int[] nums) {
        Set(WEAPON_LIST_KEY, nums);
    }
    public static int[] GetMagicList() {
        return GetIntArray(MAGIC_LIST_KEY);
    }
    public static void SetMagicList(int[] nums) {
        Set(MAGIC_LIST_KEY, nums);
    }
    public static float[] GetWeaponConditionList() {
        return GetFloatArray(WEAPON_CONDITION_LIST_KEY);
    }
    public static void SetWeaponConditionList(float[] nums) {
        Set(WEAPON_CONDITION_LIST_KEY, nums);
    }
    public static void SetHelmet(int hemletType, float condition) {
        Set(ARMOR_HELMET_KEY, hemletType);
        Set(ARMOR_HELMET_CONDITION_KEY, condition);
    }
    public static int GetHelmetType() {
        return GetInt(ARMOR_HELMET_KEY);
    }
    public static float GetHelmetCondition() {
        return GetFloat(ARMOR_HELMET_CONDITION_KEY);
    }
    public static void SetChestplate(int hemletType, float condition) {
        Set(ARMOR_CHESTPLATE_KEY, hemletType);
        Set(ARMOR_CHESTPLATE_CONDITION_KEY, condition);
    }
    public static int GetChestplateType() {
        return GetInt(ARMOR_CHESTPLATE_KEY);
    }
    public static float GetChestplateCondition() {
        return GetFloat(ARMOR_CHESTPLATE_CONDITION_KEY);
    }
    public static void SetPlayerLocation(Vector3 location) {
        Set(SCENE_LOCATION_X, location.x);
        Set(SCENE_LOCATION_Y, location.y);
        Set(SCENE_LOCATION_Z, location.z);
    }
    public static Vector3 GetPlayerLocation() {
        return new Vector3(GetFloat(SCENE_LOCATION_X), GetFloat(SCENE_LOCATION_Y), GetFloat(SCENE_LOCATION_Z));
    }
    public static void SetPlayerRotation(Vector3 location) {
        Set(SCENE_ROTATION_X, location.x);
        Set(SCENE_ROTATION_Y, location.y);
        Set(SCENE_ROTATION_Z, location.z);
    }
    public static Vector3 GetPlayerRotation() {
        return new Vector3(GetFloat(SCENE_ROTATION_X), GetFloat(SCENE_ROTATION_Y), GetFloat(SCENE_ROTATION_Z));
    }
    public static void SetPlayerScene(string name) {
        print("setting: " + name);
        Set(SCENE_NAME_KEY, name);
    }
    public static string GetPlayerScene() {
       // print("getting: " + GetString(SCENE_NAME_KEY));
        return GetString(SCENE_NAME_KEY);
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

    private static void Set(string key, float[] nums) {
        PlayerPrefsX.SetFloatArray(key, nums);
    }
    private static float[] GetFloatArray(string key) { 
        if (PlayerPrefs.HasKey(key)) {
            return PlayerPrefsX.GetFloatArray(key);
        } else {
            return new float[] { };
        }
    }

    private static void Set(string key, int[] nums) {
        PlayerPrefsX.SetIntArray(key, nums);
    }
    private static int[] GetIntArray(string key) {
        if (PlayerPrefs.HasKey(key)) {
            return PlayerPrefsX.GetIntArray(key);
        } else {
            return new int[] { };
        }
    }
}
