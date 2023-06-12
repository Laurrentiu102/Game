using System;
using UnityEngine;

[Serializable]
public class Spell
{
    [SerializeField]
    private int id;
    
    [SerializeField]
    private string name;
    
    [SerializeField]
    private int damage;

    [SerializeField]
    private float speed;

    [SerializeField] 
    private float castTime;

    public int Id => id;

    public string Name => name;

    public int Damage => damage;

    public float Speed => speed;

    public float CastTime => castTime;
}