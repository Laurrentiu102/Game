using System;
using UnityEngine;

public class SpellBook :MonoBehaviour
{
    [SerializeField]
    private Spell[] spells;
    
    public static SpellBook instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public Spell GetSpell(int index)
    {
        return spells[index];
    }
}
