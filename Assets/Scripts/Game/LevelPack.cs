using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class LevelPack {

    /// <summary>
    /// The name of the level pack
    /// </summary>
    public String name;

    public int id;
    public String imagepack;

    public String soomlaID;

    /// <summary>
    /// The number of levels in the level pack
    /// </summary>
    public int no_levels;

    public int no_levelsUnlocked = 1;

    /// <summary>
    /// The difficulty of the level pack 1-3;
    /// </summary>
    public Difficulty difficulty;

    public List<Level> levels = new List<Level>();

}
