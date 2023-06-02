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

    public static void HeightMapSettings(Packet packet)
    {
        int normalizeMode = packet.ReadInt();
        float scale = packet.ReadFloat();
        int octaves = packet.ReadInt();
        float persistance = packet.ReadFloat();
        float lacunarity = packet.ReadFloat();
        int seed = packet.ReadInt();
        float offsetX = packet.ReadFloat();
        float offsetY = packet.ReadFloat();
        bool useFalloff = packet.ReadBool();
        int falloffMapSize = packet.ReadInt();
        float heightMultiplier = packet.ReadFloat();
        int numberOfTreePrefabs = packet.ReadInt();
        int numberOfTreesPerChunk = packet.ReadInt();

        int numberOfKeyframes = packet.ReadInt();
        Keyframe[] keyframes = new Keyframe[numberOfKeyframes];
        for (int i = 0; i < keyframes.Length; i++)
        {
            keyframes[i].time = packet.ReadFloat();
            keyframes[i].value = packet.ReadFloat();
            keyframes[i].inTangent = packet.ReadFloat();
            keyframes[i].outTangent = packet.ReadFloat();
        }

        GameManager.instance.heightMapSettings.noiseSettings.normalizeMode = (Noise.NormalizeMode)normalizeMode;
        GameManager.instance.heightMapSettings.noiseSettings.scale = scale;
        GameManager.instance.heightMapSettings.noiseSettings.octaves = octaves;
        GameManager.instance.heightMapSettings.noiseSettings.persistance = persistance;
        GameManager.instance.heightMapSettings.noiseSettings.lacunarity = lacunarity;
        GameManager.instance.heightMapSettings.noiseSettings.seed = seed;
        GameManager.instance.heightMapSettings.noiseSettings.offset = new Vector2(offsetX, offsetY);
        GameManager.instance.heightMapSettings.useFalloff = useFalloff;
        GameManager.instance.heightMapSettings.falloffMapSize = falloffMapSize;
        GameManager.instance.heightMapSettings.heightMultiplier = heightMultiplier;
        GameManager.instance.heightMapSettings.numberOfTreePrefabs = numberOfTreePrefabs;
        GameManager.instance.heightMapSettings.numberOfTreesPerChunk = numberOfTreesPerChunk;
        GameManager.instance.heightMapSettings.heightCurve = new AnimationCurve(keyframes);
        
        Debug.Log("A facut tot!");
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
}
