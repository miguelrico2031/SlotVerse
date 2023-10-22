using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Clase singleton para tener un scriptable object con la información persistente entre escenas
//sobre todo lo mas importante que es la combinacion obtenida en la tragaperras
public class GameInfo : ScriptableObject
{
    public GameMode GameMode;
    public NPC NPC;
    public Setting Setting;
}

public enum GameMode { Shooter, Racing }

public enum NPC { Hedgehog, Monkey, Squirrel }

public enum Setting { Futuristic, Halloween, Beach }