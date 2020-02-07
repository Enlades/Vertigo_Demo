using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameOverUIController : MonoBehaviour
{
    public GameObject GameOverPanel;
    public TextMeshProUGUI GameOverTextMesh;
    public Button RestartButton;

    private void Awake(){
        GameOverPanel.SetActive(false);
    }

    public void RestartGame(){
        SceneManager.LoadScene(0);
    }
}
