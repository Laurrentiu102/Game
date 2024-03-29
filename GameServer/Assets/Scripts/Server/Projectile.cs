using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public static Dictionary<int, Projectile> projectiles = new Dictionary<int, Projectile>();
    public static SpellBook spellBook = null;
    
    public static int nextProjectileId = 1;

    public int id;
    public int casterId;
    public int targetId;
    public Spell spell;

    private void Awake()
    {
        if (spellBook == null)
            spellBook = GameObject.Find("SpellBook").GetComponent<SpellBook>();
    }

    private void Start()
    {
        id = nextProjectileId++;
        projectiles.Add(id, this);
        
        ServerSend.SpawnProjectile(this);
    }

    private void FixedUpdate()
    {
        if ((Server.clients.ContainsKey(casterId) && Server.clients[casterId].player==null) || (Server.clients.ContainsKey(targetId) && Server.clients[targetId].player==null))
        {
            ServerSend.ProjectileDespawn(id);
            projectiles.Remove(id);
            Destroy(gameObject);
            return;
        }
        Vector3 direction = Server.clients[targetId].player.transform.position - transform.position;
        float distance = direction.magnitude;
        Vector3 normalizedDirection = direction.normalized;
        float movementAmount = 10f * Time.deltaTime;
        if (movementAmount > distance)
        {
            // If it does, set the position directly to the target position
            transform.position = Server.clients[targetId].player.transform.position;
        }
        else
        {
            // Otherwise, move towards the target gradually
            transform.position += normalizedDirection * movementAmount;
        }
        
        ServerSend.ProjectilePosition(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Hittable") && other.gameObject.GetComponent<Player>().id==targetId)
        {
            Explode();
        }
    }

    public void Initialize(int casterId,int targetId, int spellId)
    {
        spell = spellBook.GetSpell(spellId);
        this.casterId = casterId;
        this.targetId = targetId;
    }

    private void Explode()
    {
        ServerSend.ProjectileHit(this);
        projectiles.Remove(id);
        Destroy(gameObject);
    }
}
