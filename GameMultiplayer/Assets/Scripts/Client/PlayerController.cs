using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public PlayerManager playerManager;

    private void FixedUpdate()
    {
        SendInputToServer();
    }

    private void Update()
    {
        int spellId = -1;
        for (int i = 0; i < 2; i++)
            if (Input.GetKeyDown((KeyCode)Enum.Parse(typeof(KeyCode), (i + 49).ToString())))
                spellId = i;

        if (spellId != -1)
        {
            ClientSend.PlayerCastProjectile(spellId,transform.forward);
            playerManager.CastProjectile(spellId);
        }
    }

    /// <summary>Sends player input to the server.</summary>
    private void SendInputToServer()
    {
        bool[] _inputs = new bool[]
        {
            Input.GetKey(KeyCode.W),
            Input.GetKey(KeyCode.S),
            Input.GetKey(KeyCode.A),
            Input.GetKey(KeyCode.D),
            Input.GetKey(KeyCode.Space)
        };

        ClientSend.PlayerMovement(_inputs);
    }
}
