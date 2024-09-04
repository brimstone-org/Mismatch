using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System.IO;
using System.Linq;


public class PlayerData : MonoBehaviour {

    public JSONNode data;
    public const string savePath = @"/";// "/Debug/";
    public const string saveFile = @"player.dat";

    #if UNITY_EDITOR
    public bool load = false;
    public bool save = false;
    #endif

    public bool LoadData()
    {
        bool one = ReadPlayerData();
        bool two = LoadPlayerData();
        return one && two;
    }
    public bool SaveData()
    {
        bool one = UpdatePlayerData();
        //Debug.Log("one - " + one);

        bool two = WritePlayerData();
        //Debug.Log("two - " + two);

        return one && two;
    }

    #if UNITY_EDITOR
	void Update () 
    {
        if (load) { load = false; LoadData(); }
        if (save) { save = false; SaveData(); }
	}
    #endif

    bool ReadPlayerData()
    {
        //Debug.Log("PLAYER DATA - reading player data");
        //Get the file
        if (!System.IO.Directory.Exists(Application.persistentDataPath + savePath))
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + savePath);

        string curFile = Application.persistentDataPath + savePath + saveFile;

        JSONNode N = null;
        if (File.Exists(curFile))
            N = JSONNode.LoadFromCompressedFile(Application.persistentDataPath + savePath + saveFile);
        else
            N = getNewPlayerData();
        data = N;

        return true;
    }
    bool WritePlayerData()
    {
        if (data == null) return false;
        //Get the file
        if (!System.IO.Directory.Exists(Application.persistentDataPath + savePath))
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + savePath);

        string curFile = Application.persistentDataPath + savePath + saveFile;
        data.SaveToCompressedFile(curFile);
        #if UNITY_EDITOR
        SavePlayerDataDebug();
        #endif
        return true;
    }
    bool LoadPlayerData()
    {
        if (GameControl.me == null) return false;
        if (data == null) return false;
        if (!GameControl.me.levelsLoaded) return false;
        List<LevelPack> gameLevelPacks = GameControl.me.levelsLoader.LevelPacks;

        JSONNode N = data;
 
        //GameControl.me.HintsRemaining = N["playerdata"]["hintsRemaining"].AsInt;

        var levelpacks = N["playerdata"]["levelpacks"].AsArray;
        foreach( JSONNode lp in levelpacks)
        {
            LevelPack levelpack = gameLevelPacks.Where(x => x.id == lp["id"].AsInt).FirstOrDefault();
            if (levelpack == null) continue;
            levelpack.no_levelsUnlocked = lp["no_levelsUnlocked"].AsInt ;
        }
        return true;
    }
    bool UpdatePlayerData()
    {
        if (GameControl.me == null)
        {
            Debug.Log("Prima");
            return false;
        }
        if (data == null)
        {
            Debug.Log("A Doua");

            return false;
        }
        if (!GameControl.me.levelsLoaded)
        {
            Debug.Log("A treia");

            return false;
        }

        JSONNode N = data;
        //N["playerdata"]["hintsRemaining"].AsInt = StoreInventory.GetItemBalance("hints_currency_ID"); //GameControl.me.HintsRemaining;

        //Levelpack info array;
        JSONArray array = new JSONArray();
        array.Value = "levelpacks";
        N["playerdata"]["levelpacks"] = array;

        List<LevelPack> gameLevelPacks = GameControl.me.levelsLoader.LevelPacks;

        
        foreach (var glp in gameLevelPacks)
        {
            JSONClass node = new JSONClass();
            node["no_levelsUnlocked"].AsInt = glp.no_levelsUnlocked;
            node["name"] = glp.name;
            node["id"].AsInt = glp.id;
            array.Add(node);
        }
        return true;
    }
    JSONNode getNewPlayerData()
    {
        //no fancy cheating. only 10 hints.
        //int balance = StoreInventory.GetItemBalance("hints_currency_ID");
        //StoreInventory.TakeItem("hints_currency_ID", balance);
        //give the player 10 hints if first time
        //StoreInventory.GiveItem("hints_currency_ID", 10);

        JSONNode N = JSON.Parse("{}");
        //N["playerdata"]["hintsRemaining"].AsInt = balance;// GameControl.me.HintsRemaining;
        N["playerdata"]["reviewGiven"].AsBool = false;
        N["playerdata"]["levelsplayed"].AsInt = 0;
        N["playerdata"]["LastShareDate"] = "";
        return N;
    }
    #if UNITY_EDITOR
    void SavePlayerDataDebug()
    {
        JSONNode N = data;
        SaveDataToDisk("debug.json", N.ToJSON());
    }
    void SaveDataToDisk(string name, string data)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(data);
        //var decoded = System.Text.Encoding.UTF8.GetString(bytes);
        //byte[] bytes = new byte[4];
        if (!System.IO.Directory.Exists(Application.persistentDataPath + "/Debug/"))
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/Debug/");

        string file = Application.persistentDataPath + "/Debug/" + name ;

        System.IO.File.WriteAllBytes(file, bytes);
        Debug.Log("Saved as " + file);
    }
    #endif

    public void SetPlayerDataString(string s, string newString)
    {
        JSONNode N = data;
        N["playerdata"][s] = newString;
        WritePlayerData();
    }
    public string GetPlayerDataString(string s)
    {
        JSONNode N = data;
        return N["playerdata"][s];
    }
}
