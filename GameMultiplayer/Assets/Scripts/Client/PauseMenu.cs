using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    private bool isActive = false;
    private GameObject clientManager;

    public static PauseMenu instance;
    // Start is called before the first frame update
    void Awake()
    {
        clientManager = GameObject.Find("ClientManager");
        if(instance == null)
            instance = this;
    }
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
        }
    }

    public void EscapePressed()
    {
        if(isActive)
        {
            pauseMenu.SetActive(false);
            isActive = false;
        }
        else
        {
            pauseMenu.SetActive(true);
            isActive = true;
        }
    }

    public void Disconnect()
    {
        clientManager.GetComponent<Client>().Disconnect(); 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameManager.players.Remove(clientManager.GetComponent<Client>().myId);
    }
}
