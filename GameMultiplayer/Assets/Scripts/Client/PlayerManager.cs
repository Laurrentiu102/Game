using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Net.NetworkInformation;
using System.Text;
using Ping = System.Net.NetworkInformation.Ping;


public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;
    public int targetId;
    public TextMeshProUGUI tagName;
    
    public GameObject spellCastCanvas;
    public Image spellCastBar;
    public TextMeshProUGUI spellCastTime;
    public TextMeshProUGUI spellName;
    public Image spellIcon;
    public Spell currentSpell;
    [NonSerialized]
    public Vector3 startCastPosition;
    private GameObject clientManager;
    private bool isCasting = false;
    private Coroutine showSpellCastCanvasCoroutine;

    public TextMeshProUGUI ping;

    private void Awake()
    {
        clientManager = GameObject.Find("ClientManager");
        spellCastCanvas = GameObject.Find("CastingBar");
        spellCastBar = GameObject.Find("Fill").GetComponent<Image>();
        spellCastTime = GameObject.Find("CastTime").GetComponent<TextMeshProUGUI>();
        spellName = GameObject.Find("SpellName").GetComponent<TextMeshProUGUI>();
        spellIcon = GameObject.Find("SpellIcon").GetComponent<Image>();
        
        spellCastCanvas.SetActive(false);
        
        ping = GameObject.Find("Ping").GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        tagName.text = username;
        //StartCoroutine(PingServer());
    }

    private IEnumerator PingServer()
    {
        string ip = clientManager.GetComponent<Client>().ip;
        while (true)
        {
            Ping pingSender = new Ping ();
            PingOptions options = new PingOptions ();

            // Use the default Ttl value which is 128,
            // but change the fragmentation behavior.
            options.DontFragment = true;

            // Create a buffer of 32 bytes of data to be transmitted.
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes (data);
            int timeout = 120;
            PingReply reply = pingSender.Send (ip, timeout, buffer, options);
            if (reply.Status == IPStatus.Success)
            {
                ping.text= "Ping: "+reply.RoundtripTime+"ms ";
            }
            yield return new WaitForSeconds(2f);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isCasting)
            {
                StopCoroutine(showSpellCastCanvasCoroutine);
                spellCastCanvas.SetActive(false);
                isCasting = false;
            }
            else
            {
                Highlight.instance.EscapePressed();
            }
        }
    }

    public void CastProjectile(int spellId)
    {
        if (!isCasting && targetId!=-1 && targetId!=0)
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
