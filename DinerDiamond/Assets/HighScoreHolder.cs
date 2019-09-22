using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScoreHolder : MonoBehaviour
{
    private static HighScoreHolder _instance;

    public static HighScoreHolder Instance { get { return _instance; } }
    public bool firstTimePlaying;
    
    
    private int _highScore;
    public int HighScore
    {
        get => _highScore;
        set
        {
            _highScore = value;
            GameManager.Instance.highScoreEndScreenGameObject.GetComponent<TextMeshProUGUI>().text = value.ToString();
            GameManager.Instance.highScoreInLevelGameObject.GetComponent<TextMeshProUGUI>().text = value.ToString();
        }
    }

    public GameObject highScoreText;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        } 
        
        else {
            _instance = this;
        }
    }

}
