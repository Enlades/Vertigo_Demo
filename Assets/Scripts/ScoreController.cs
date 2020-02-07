using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    public TextMeshProUGUI UIText;

    public int Score
    {
        get{
            return _score;
        }
        private set{
            UIText.text = "Score : " + value;
            _score = value;
        }
    }
    private int _score;

    public void Init(){
        Score = 0;
    }

    public void AddSore(int p_score){
        Score += p_score;
    }
}
