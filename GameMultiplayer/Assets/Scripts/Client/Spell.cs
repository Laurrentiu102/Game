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
    private Sprite icon;
    
    [SerializeField]
    private float speed;

    [SerializeField] 
    private float castTime;
    
    [SerializeField]
    private Color barColor;

    public string Name
    {
        get => name;
        set => name = value;
    }

    public int Damage
    {
        get => damage;
        set => damage = value;
    }

    public Sprite Icon
    {
        get => icon;
        set => icon = value;
    }

    public float Speed
    {
        get => speed;
        set => speed = value;
    }

    public float CastTime
    {
        get => castTime;
        set => castTime = value;
    }

    public Color BarColor
    {
        get => barColor;
        set => barColor = value;
    }
}