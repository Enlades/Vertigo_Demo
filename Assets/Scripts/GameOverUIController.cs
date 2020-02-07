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

    private Color[] _availableColors;

    private void Awake(){
        GameOverPanel.SetActive(false);
    }

    public void Init(Color[] p_colors){
        _availableColors = p_colors;

        string gameOverText = "Game Over";
        string rtfGameOverText = "";


        for(int i = 0; i < gameOverText.Length; i++){
            rtfGameOverText += "<color=#" 
            + RGBToHex(_availableColors[Random.Range(0, _availableColors.Length)]) 
            + ">"+ gameOverText[i] + "</color>";
        }
        
        GameOverTextMesh.text = rtfGameOverText;
    }

    public void RestartGame(){
        SceneManager.LoadScene(0);
    }

    public void ShowUI(){
        GameOverPanel.SetActive(true);
    }

    private string RGBToHex(Color p_color){
        return ColorUtility.ToHtmlStringRGB(p_color);
    }
}
