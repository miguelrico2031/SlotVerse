using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfo : ScriptableObject
{
    public GameMode GameMode;
    public NPC NPC;
    public Setting Setting;
}

public enum GameMode { Shooter, Racing }

public enum NPC { Hedgehog, Monkey, Squirrel }

public enum Setting { Futuristic, Halloween, Beach }