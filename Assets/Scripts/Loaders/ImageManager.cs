using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleSpritePacker;
using System;
using System.Linq;
using System.Diagnostics;
 #if UNITY_EDITOR
using UnityEditor;

#endif

public class ImageManager : MonoBehaviour
{
    #if UNITY_EDITOR
    public List<Texture2D> ImageAtlases = new List<Texture2D>();
    #endif

    public List<Texture2D> KeepLoaded = new List<Texture2D>();
    public List<SpriteLoader> SpriteLoaders = new List<SpriteLoader>();
    public List<AtlasLoader> AtlasLoaders = new List<AtlasLoader>();

    public ResourceRequest AtlasPreload;
    public Texture2D preloadedTexture;
    public bool preload;

    #region UnityFunctions
    // Use this for initialization
	void Start () {
        ClearReferences();
    }

    // Update is called once per frame
    void Update()
    {
      /*  if (preload)
        {
            if (AtlasPreload.isDone)
            {
             
                preloadedTexture = (Texture2D)AtlasPreload.asset;
                preload = false;
               // Debug.Log("preloaded");
            }
        }*/
    }
    #endregion

    public void ImageAtlasPreload(string name)
    {
     /*   AtlasLoader al = AtlasLoaders.Where(x => x.name == name).FirstOrDefault();
        if (al == null) return; 
        AtlasPreload = al.PreLoad();
        //Debug.Log("preloading");
        preload = true;*/
    }
    void ClearReferences()
    {
      #if UNITY_EDITOR
        ImageAtlases = null;
      #endif
    }

    public Sprite GetSpriteByName(String name)
    {
        //Stopwatch sw = new Stopwatch();
        //sw.Start();
        foreach (SpriteLoader sl in SpriteLoaders )
        {
            //if (sl == null) continue;

            if (sl.name == name)
            {
				//sw.Stop(); 
				//UnityEngine.Debug.LogWarning("STOPWATCHMESSAGE2: IMAGEMANAGER LOOP TOOK " + sw.Elapsed);
                return sl.Load();
            }
        }
        return null;
    }
    public Sprite GetRandomSprite()
    {
        if (SpriteLoaders.Count == 0) return null;

        int num = UnityEngine.Random.Range(0, SpriteLoaders.Count - 1);
        return SpriteLoaders[num].Load();
    }
    public void TestAtlas()
    {

        for (int i = 0; i < UnityEngine.Random.Range(1, 10); i++)
        {
            var go = new GameObject();
            go.AddComponent<SpriteRenderer>().sprite = GetRandomSprite();
            go.transform.position = new Vector3(UnityEngine.Random.Range(1, 10), UnityEngine.Random.Range(1, 10), 0);
        }
        
    }


}


