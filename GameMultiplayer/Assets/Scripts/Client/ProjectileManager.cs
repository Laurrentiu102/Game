using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public int id;
    public int casterId;
    public int targetId;
    public int damage;

    public void Initialize(int _id,int _casterId,int _targetId,int _damage)
    {
        id = _id;
        casterId = _casterId;
        targetId = _targetId;
        damage = _damage;
    }

    public void Explode()
    {
        GameManager.projectiles.Remove(id);
        Destroy(gameObject);
    }
}
