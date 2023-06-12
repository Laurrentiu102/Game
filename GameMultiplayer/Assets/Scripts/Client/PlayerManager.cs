using TMPro;
using UnityEngine;

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
