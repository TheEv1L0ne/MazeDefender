﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : Singleton<SpriteManager>
{
    public Sprite Clif;
    public Sprite Dirt;
    public Sprite Passage;

    public Sprite Base;

    [Header("ON OFF")]
    public Sprite on;
    public Sprite off;
}
