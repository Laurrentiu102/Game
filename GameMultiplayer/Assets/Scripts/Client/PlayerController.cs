using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public PlayerManager playerManager;

    private void Awake()
    {
        playerManager.spellBook = GameObject.Find("SpellBook").GetComponent<SpellBook>();
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
            Spell spell = playerManager.spellBook.GetSpell(id);
            playerManager.currentSpell = spell;
            playerManager.spellName.text = spell.Name;
            playerManager.spellIcon.sprite = spell.Icon;
            playerManager.spellCastBar.color = spell.BarColor;
            playerManager.spellCastTime.text = spell.CastTime.ToString(CultureInfo.InvariantCulture);
            playerManager.spellCastBar.fillAmount = 0;
            playerManager.startCastPosition = transform.position;
            StartCoroutine(ShowSpellCastCanvas());
        }
    }
    
    private IEnumerator ShowSpellCastCanvas()
    {
        playerManager.spellCastCanvas.SetActive(true);
        while(playerManager.spellCastBar.fillAmount<1)
        {
            if(playerManager.startCastPosition!=transform.position)
                break;
            yield return null;
        }
        playerManager.spellCastCanvas.SetActive(false);
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
