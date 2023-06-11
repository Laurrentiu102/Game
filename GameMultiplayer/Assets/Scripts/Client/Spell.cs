using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spell
{
    public int id;
    public string spellName;
    public float castTime;
    public Sprite spellImage;
    public static Dictionary<int,Spell> spells = new Dictionary<int, Spell>();
    
    public Spell(int _id, string _spellName, float _castTime)
    {
        id = _id;
        spellName = _spellName;
        castTime = _castTime;
        spellImage = GameManager.instance.spellIcons[id];
    }

    public static void InitializeSpells()
    {
        int spellId = 0;
        Spell fireball = new Spell(spellId++,"Fireball",1f);
        Spell shadowball = new Spell(spellId++,"Shadowball",1f);
        
        spells.Add(fireball.id,fireball);
        spells.Add(shadowball.id,shadowball);
    }
    
}