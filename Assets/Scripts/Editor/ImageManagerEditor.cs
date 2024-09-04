using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleSpritePacker;
using System;
using UnityEditor;

[CustomEditor(typeof(ImageManager))]
//[CanEditMultipleObjects]
public class ImageManagerEditor : Editor
{
    //SerializedProperty ImageAtlases;
    //public Texture2D[] ImageAtlases;
    void OnEnable()
    {
        // Setup the SerializedProperties.
        //ImageAtlases = serializedObject.FindProperty("damage");
       
    }
	
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

     
        if (GUILayout.Button("Rebuild"))
        {
          SetupLoaders();
        }
    }

    void SetupLoaders()
    {
        ImageManager script = (ImageManager)target;
        script.SpriteLoaders.Clear();
        script.AtlasLoaders.Clear();

        foreach (Texture2D ia in script.ImageAtlases)
        {
            if (ia != null)
            {
                String path = AssetDatabase.GetAssetPath(ia);
                Debug.Log(path);
                if ( path.IndexOf("Resources") == -1)
                {
                    Debug.LogError(path + " is not in Resources Folder.");
                    continue;
                }
                if ( path.IndexOf(".png") == -1)
                {
                    Debug.LogError(path + " is not a png file.");
                    continue;
                }
                    
                path = path.Replace("Assets/Resources/", "");
                path = path.Replace(".png", "");
                Sprite[] sprites = Resources.LoadAll<Sprite>(path);

                AtlasLoader newAtlas = new AtlasLoader(path, ia.name);
                script.AtlasLoaders.Add(newAtlas);
                
                for(int i=0; i<sprites.Length; i++)
                {
                    //SpriteLoader spl = new SpriteLoader(name: sprites[i].name ,atlas: atlas, indexInAtlas: i);
                    SpriteLoader spl = new SpriteLoader(sprites[i].name, newAtlas, i);
                    script.SpriteLoaders.Add(spl);
                 
                }
               
            }
        }
        script.SpriteLoaders.Sort();
    }



}


