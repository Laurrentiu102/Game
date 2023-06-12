using System.Net;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject startMenu;
    public InputField usernameField;
    public InputField ipField;
    private string ip;

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

    /// <summary>Attempts to connect to the server.</summary>
    public void ConnectToServer()
    {
        bool containsOnlyDigitsAndDot = Regex.IsMatch(ipField.text, @"^[0-9.]+$");
        if (containsOnlyDigitsAndDot)
        {
            ip = ipField.text;
        }
        else
        {
            IPAddress[] addresses = Dns.GetHostAddresses(ipField.text);
            foreach (IPAddress address in addresses)
                if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    ip = address.ToString();
        }
        if (ipField.text.Equals(""))
            ip = "127.0.0.1";
        
        startMenu.SetActive(false);
        usernameField.interactable = false;
        Client.instance.ConnectToServer(ip);
    }
}
