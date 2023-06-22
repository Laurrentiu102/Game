using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public PlayerManager playerManager;
    
    public GameObject spellCastCanvas;
    public Image spellCastBar;
    public TextMeshProUGUI spellCastTime;
    public TextMeshProUGUI spellName;
    public Image spellIcon;
    public Spell currentSpell;
    [NonSerialized]
    public Vector3 startCastPosition;
    private bool isCasting = false;
    private Coroutine showSpellCastCanvasCoroutine;

    private void FixedUpdate()
    {
        SendInputToServer();
    }

    private void Awake()
    {
        spellCastCanvas = InterfaceElements.instance.spellCastCanvas;
        spellCastBar = InterfaceElements.instance.spellCastBar;
        spellCastTime = InterfaceElements.instance.spellCastTime;
        spellName = InterfaceElements.instance.spellName;
        spellIcon = InterfaceElements.instance.spellIcon;
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
            CastProjectile(spellId);
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isCasting)
            {
                StopCoroutine(showSpellCastCanvasCoroutine);
                spellCastCanvas.SetActive(false);
                isCasting = false;
                ClientSend.PlayerCastProjectileCancel();
            }
            else
            {
                Highlight.instance.EscapePressed();
            }
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
    
    public void CastProjectile(int spellId)
    {
        if (!isCasting && playerManager.targetId!=-1 && playerManager.targetId!=0)
        {
            Spell spell = SpellBook.instance.GetSpell(spellId);
            currentSpell = spell;
            spellName.text = spell.Name;
            spellIcon.sprite = spell.Icon;
            spellCastBar.color = spell.BarColor;
            spellCastTime.text = spell.CastTime.ToString(CultureInfo.InvariantCulture);
            spellCastBar.fillAmount = 0;
            startCastPosition = transform.position;
            showSpellCastCanvasCoroutine=StartCoroutine(ShowSpellCastCanvas());
        }
    }
    
    private IEnumerator ShowSpellCastCanvas()
    {
        isCasting = true;
        spellCastCanvas.SetActive(true);
        while(spellCastBar.fillAmount<1)
        {
            if(startCastPosition!=transform.position)
                break;
            yield return null;
        }
        spellCastCanvas.SetActive(false);
        isCasting = false;
    }
}
