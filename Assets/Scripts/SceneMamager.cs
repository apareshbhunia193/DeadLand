using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMamager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickPlayButton() {
        SceneManager.LoadScene("GameLevel");
    }

    public void OnClickExitButton() {
        Application.Quit();
    }

    public void OnClickMainMenuButton() {
        SceneManager.LoadScene("Main Menu");
    }
}
