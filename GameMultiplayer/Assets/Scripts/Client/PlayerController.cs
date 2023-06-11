using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public TextMeshProUGUI spellCastTime;
    public TextMeshProUGUI spellName;
    public Image spellIcon;
    private void FixedUpdate()
    {
        SendInputToServer();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            spellName.text = Spell.spells[0].spellName;
            spellCastTime.text = Spell.spells[0].castTime.ToString();
            spellIcon.sprite = Spell.spells[0].spellImage;
            ClientSend.PlayerCastProjectile(0,transform.forward);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            spellName.text = Spell.spells[1].spellName;
            spellCastTime.text = Spell.spells[1].castTime.ToString();
            spellIcon.sprite = Spell.spells[1].spellImage;
            ClientSend.PlayerCastProjectile(1,transform.forward);
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
