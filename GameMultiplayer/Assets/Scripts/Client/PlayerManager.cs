using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;
    public int targetId;
    public TextMeshProUGUI tagName;
    private void Start()
    {
        tagName.text = username;
    }
}
