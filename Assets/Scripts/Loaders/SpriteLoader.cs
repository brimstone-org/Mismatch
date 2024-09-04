using UnityEngine;
using System.Collections;
using System;
using System.Diagnostics;

[System.Serializable]
public class SpriteLoader : IComparable<SpriteLoader>
{
    public String name;
    public AtlasLoader atlas;
    public int indexInAtlas;
    
    public SpriteLoader(String name, AtlasLoader atlas, int indexInAtlas)
    {
        this.name = name;
        this.atlas = atlas;
        this.indexInAtlas = indexInAtlas;
    }
   public Sprite Load()
   {
       //Stopwatch watch = new Stopwatch();
       //watch.Start();
         Sprite sprite = atlas.Load()[indexInAtlas];
       // Sprite sprite = atlas.getSprites()[indexInAtlas];
		//UnityEngine.Debug.Log("Sprite " + name + " loaded in " + watch.ElapsedMilliseconds);
		//watch.Stop();
		//UnityEngine.Debug.LogWarning("STOPWATCHMESSAGE2:  SPRITELOADER LOAD  TOOK " + watch.Elapsed);
       return sprite;

   }
   public int CompareTo(SpriteLoader other)
   {
	    return this.name.CompareTo(other.name);
   }
  
}
