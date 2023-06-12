using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public TextMeshProUGUI spellCastTime;
    public TextMeshProUGUI spellName;
    public Image spellIcon;
    public SpellBook spellBook;

    private void Awake()
    {
        spellBook = GameObject.Find("SpellBook").GetComponent<SpellBook>();
    }

    private void FixedUpdate()
    {
        SendInputToServer();
    }

    private void Update()
    {
        int id = -1;
        for (int i = 0; i < 2; i++)
            if (Input.GetKeyDown((KeyCode)Enum.Parse(typeof(KeyCode), (i + 49).ToString())))
                id = i;

        if (id != -1)
        {
            ClientSend.PlayerCastProjectile(id,transform.forward);
            Spell spell = spellBook.GetSpell(id);
            spellName.text = spell.Name;
            spellIcon.sprite = spell.Icon;
            spellCastTime.text = spell.CastTime.ToString(CultureInfo.InvariantCulture);
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
