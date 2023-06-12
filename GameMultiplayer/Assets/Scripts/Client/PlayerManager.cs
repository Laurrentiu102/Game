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
    public SpellBook spellBook;
    public Spell currentSpell;
    [NonSerialized]
    public Vector3 startCastPosition;

    public TextMeshProUGUI ping;
    private void Start()
    {
        tagName.text = username;
        StartCoroutine(PingServer());
    }

    private IEnumerator PingServer()
    {
        string ip = GameObject.Find("ClientManager").GetComponent<Client>().ip;
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
}
