using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg}");
        Client.instance.myId = _myId;
        ClientSend.WelcomeReceived();

        // Now that we have the client's id, connect UDP
        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.instance.SpawnPlayer(_id, _username, _position, _rotation);
    }

    public static void PlayerPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        GameManager.players[_id].transform.position = _position;
    }

    public static void PlayerRotation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.players[_id].transform.rotation = _rotation;
    }

    public static void ServerDayNightTime(Packet packet)
    {
        DayNightCycle.time = packet.ReadFloat();
    }
    
    public static void PlayerDisconnected(Packet packet)
    {
        int _id = packet.ReadInt();
        Destroy(GameManager.players[_id].gameObject);
        GameManager.players.Remove(_id);
    }

    public static void SpawnProjectile(Packet _packet)
    {
        int id = _packet.ReadInt();
        int casterId = _packet.ReadInt();
        int targetId = _packet.ReadInt();
        Vector3 positon = _packet.ReadVector3();
        int spellId = _packet.ReadInt();
        
        GameManager.instance.SpawnProjectile(id,casterId,targetId,positon,spellId);
    }
    
    public static void ProjectilePosition(Packet _packet)
    {
        int id = _packet.ReadInt();
        Vector3 positon = _packet.ReadVector3();
        
        if(GameManager.projectiles.TryGetValue(id, out var projectile))
            projectile.transform.position = positon;
    }

    public static void ProjectileHit(Packet _packet)
    {
        int id = _packet.ReadInt();
        
        GameManager.projectiles[id].Explode();
    }

    public static void ProjectileDespawn(Packet _packet)
    {
        int id = _packet.ReadInt();

        GameObject aux = GameManager.projectiles[id].gameObject;
        GameManager.projectiles.Remove(id);
        Destroy(aux);
    }

    public static void CastBarProgress(Packet _packet)
    {
        int id = _packet.ReadInt();
        float progress = _packet.ReadFloat();
        
        GameManager.players[id].spellCastBar.fillAmount = Mathf.Lerp(0,1,progress);
        
    }
}
