using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Net.NetworkInformation;
using System.Text;

public class InterfaceElements : MonoBehaviour
{
    public static InterfaceElements instance;
    public GameObject spellCastCanvas;
    public Image spellCastBar;
    public TextMeshProUGUI spellCastTime;
    public TextMeshProUGUI spellName;
    public Image spellIcon;

    public void Awake()
    {
        if (instance == null)
            instance = this;
    }
}
