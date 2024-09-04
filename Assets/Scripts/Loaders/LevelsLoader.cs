using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System.Linq;
using System.IO;

public class LevelsLoader : MonoBehaviour
{
    public const string loadPathDebug = @"/Levels/";// "/Debug/";
    public bool doDebugLoading = false;

    public List<LevelPack> LevelPacks = new List<LevelPack>();
    // Use this for initialization
    void Start()
    {
        LoadLevels();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LoadLevels()
    {
        LevelPacks.Clear();

        //Debug mode
		if (doDebugLoading)
        {
            LoadLevelsDebug();
            if (LevelPacks.Count == 0)
                LoadLevelFiles();
        }
        //Normal Mode
		if (!doDebugLoading)
        {
            LoadLevelFiles(); 
        }

        LevelPacks = LevelPacks.OrderBy(x => x.id).ToList();
        //We're done. let Gamecontrol know;
        GameControl.me.OnLevelsLoaded();
    }
    void LoadLevelFiles()
    {
        var LevelJSONs = Resources.LoadAll<TextAsset>("Levels/");
        foreach (TextAsset ta in LevelJSONs)
        {
            LoadLevelPackFromJson(ta.text);
        }
    }
    void LoadLevelsDebug()
    {
        string path = Application.persistentDataPath + loadPathDebug;
        if (!System.IO.Directory.Exists(path))
           System.IO.Directory.CreateDirectory(path);
        var info = new System.IO.DirectoryInfo(path);
        Debug.Log("Loading levelpacks from: " + path);
        var fileInfo = info.GetFiles();
        if (fileInfo.Length == 0)   Debug.Log("No files found. " + path);
        foreach (var file in fileInfo)
        {
               if ( file.Extension.ToLowerInvariant() == ".json")
               {
                   //Debug.Log("Loaded levelpack from: " + file.FullName);
                   string sLevelPack = File.ReadAllText(file.FullName, System.Text.Encoding.UTF8);
                   LoadLevelPackFromJson(sLevelPack);
               }
        }
    }

    void LoadLevelPackFromJson(string jsonLevelPack)
    {
        var N = JSON.Parse(jsonLevelPack);

        LevelPack levelPack = new LevelPack();
        levelPack.name = N["name"].Value;
        levelPack.imagepack = N["imagepack"].Value;
        levelPack.soomlaID = N["soomlaID"].Value;
        levelPack.id = N["id"].AsInt;
        levelPack.no_levels = N["no_levels"].AsInt;
        levelPack.difficulty = (Difficulty) ( N["difficulty"].AsInt );

        LevelPacks.Add(levelPack);
        
        // var name = N["data"]["sampleArray"][2]["name"];// name will be a string containing "sub object"


        for (int i = 0; i < N["levels"].Count; i++ )
        {
            JSONNode L = N["levels"][i];
            Level level = new Level();

            //Add the level to the level pack. 
            levelPack.levels.Add(level);

            //Continue loading the level
            level.id = L["id"].AsInt;
            level.label = L["label"].Value;
            level.rows = L["columns"].AsInt; // they call em columns I call em rows.
            level.columns = L["rows"].AsInt; // they call em rows I call em columns.
            level.depends = L["depends"].AsInt;
            level.difficulty = (Difficulty)levelPack.difficulty; //the level should know its own difficulty
            //Load the headers and the columns
            for (int x = 1; x < level.columns+1; x++ )
            {
                JSONNode jcol = L["row" + x]; // they call em rows I call em columns.

                //Add the header, its always the last in the list
                level.headers.Add(jcol[jcol.Count - 1].Value);

                //Add a solved column for this level
                Column solvedColumn = new Column();
                for (int y = 0; y < jcol.Count-1;y++)
                {
                    solvedColumn.list.Add(jcol[y].Value);
                }
                level.solutionColumns.Add(solvedColumn);
            }

            
        }
   
    }

}
