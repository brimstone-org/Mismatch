using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleSpritePacker;

public class LoadFromAtlas : MonoBehaviour {

    public SPInstance[] spinstance;
    public List<SPSpriteInfo> sprites;
	// Use this for initialization
	void Start () {
        spinstance = Resources.LoadAll<SPInstance>("Images/");

        sprites = spinstance[0].copyOfSprites;
        foreach (SPSpriteInfo s in sprites)
        {
            s.source = null;
        }
        
        
        
        //Sprite sp = Sprite.Create
        //(spinstance[0].texture, new Rect(0, 0, spinstance[0].texture.width, spinstance[0].texture.height), new Vector2(0.5f, 0.5f));
        this.GetComponent<SpriteRenderer>().sprite = sprites[0].targetSprite;

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
