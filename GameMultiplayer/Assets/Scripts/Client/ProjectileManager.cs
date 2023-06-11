using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public int id;
    public int casterId;
    public int targetId;
    public int damage;
    public Spell typeOfProjectile;

    public void Initialize(int _id,int _casterId,int _targetId,int _damage,int _typeOfProjectile)
    {
        id = _id;
        casterId = _casterId;
        targetId = _targetId;
        damage = _damage;
        typeOfProjectile = Spell.spells[_typeOfProjectile];
    }

    public void Explode()
    {
        GameManager.projectiles.Remove(id);
        Destroy(gameObject);
    }
}
