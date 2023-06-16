using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;

    public GameObject playerPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        Server.Start(Constants.MAX_PLAYERS, 26951);
    }

    private void OnApplicationQuit()
    {
        Server.Stop();
    }
    
    public Player InstantiatePlayer()
    {
        Player player = Instantiate(playerPrefab, new Vector3(0f, 20f, 0f), Quaternion.identity).GetComponent<Player>();
        return player;
    }

    public void FixedUpdate()
    {
        ServerSend.ServerDayNightTime();
    }
    
    public Projectile InstantiateProjectile(Transform shootOrigin,int spellId)
    {
        return Instantiate(SpellBook.instance.GetSpell(spellId).projectilePrefab, shootOrigin.position+shootOrigin.forward*0.7f, Quaternion.identity).GetComponent<Projectile>();
    }
}
