using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagingTheScene : MonoBehaviour
{
    public void LoadingGameOverScene() {
        SceneManager.LoadScene("GameOver");
    }
}
