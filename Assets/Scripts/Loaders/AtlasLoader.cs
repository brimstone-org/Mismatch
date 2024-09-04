using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.U2D;

[System.Serializable]
public class AtlasLoader
{
    public String path;
    public String name;

    bool cached = false;

    public Sprite[] sprites;

    public AtlasLoader(String Path, String Name)
    {
        this.path = Path;
        this.name = Name;
        //AsyncPreload();
    }


  /*  public Sprite[] getSprites(){
        return sprites;
    }*/

    //HACK: PERFORMANCE HARDCODED HACK
    public Sprite[] Load()
    {
        if(name.Equals("ImgBasePack"))
        {
            if(!cached)
            {
                AsyncPreloaderHackfix.instance.grabSprites(sprites);

                if(sprites==null)
                    return Resources.LoadAll<Sprite>(path);
            }
            return sprites;
        }
            
        return Resources.LoadAll<Sprite>(path);
    }

   /* public void AsyncPreload(){
        // sprites = Resources.LoadAll<Sprite>(path);
        sprites = new (Resources.LoadAsync<SpriteAtlas>(path).asset as SpriteAtlas).GetSprites();
    }*/

/*   public ResourceRequest PreLoad()
    {
       /* SpriteAtlas test = ;
        test.*/
    //}*/

}