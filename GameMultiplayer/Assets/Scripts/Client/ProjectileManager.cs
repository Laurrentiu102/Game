using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public int id;
    public int casterId;
    public int targetId;
    public int damage;
    public Spell spell;
    public static SpellBook spellBook=null;

    public void Awake()
    {
        if(spellBook==null)
            spellBook = GameObject.Find("SpellBook").GetComponent<SpellBook>();
    }

    public void Initialize(int _id,int _casterId,int _targetId,int _spellId)
    {
        id = _id;
        casterId = _casterId;
        targetId = _targetId;
        spell = spellBook.GetSpell(_spellId); 
        gameObject.GetComponent<Renderer>().material.color = spell.BarColor;
    }

    public void Explode()
    {
        GameManager.projectiles.Remove(id);
        Destroy(gameObject);
    }
}
