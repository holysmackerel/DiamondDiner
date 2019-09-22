using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }
    
    [SerializeField]
    
    public List<int> foodProbabilities;
    
    public List<int> diamondNumbers;
    [SerializeField] private List<GameObject> diamondGameObjects;
    private List<GameObject> _textGameObjects;
    public List<GameObject> selectedDiamonds;

    public GameObject coinTextGameObject;
    public GameObject customerTextGameObject;
    public int minCustomers;
    public int maxCustomers;

    public GameObject startScreenGameObject;
    public GameObject endScreenGameObject;
    public GameObject earnedScoreGameObject;
    public GameObject highScoreEndScreenGameObject;
    public GameObject highScoreInLevelGameObject;
    public GameObject highScoreHUD;
    
    public List<Sprite> foods;
    
    public bool isMouseDown;
    private int _numberSelected;
         
         public int NumberSelected
         {
             get => _numberSelected;
             set
             {
                 _numberSelected = value;
             }
         }
         
         private int _coinCount;
         
         public int CoinCount
         {
             get => _coinCount;
             set
             {
                 _coinCount = value;
                 coinTextGameObject.GetComponent<TextMeshProUGUI>().text = value.ToString();
                 earnedScoreGameObject.GetComponent<TextMeshProUGUI>().text = CoinCount.ToString();
             }
         }
         
         private int _customerCount;
         
         public int CustomerCount
         {
             get => _customerCount;
             set
             {
                 _customerCount = value;
                 customerTextGameObject.GetComponent<TextMeshProUGUI>().text = value.ToString();
             }
         }
         
         
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
    }
    
          
    // Start is called before the first frame update
    void Start()
    {
        ToggleButtons();
        ToggleButtons();
        foods = Resources.LoadAll<Sprite>("Sprites/Foods/").ToList();

        for (int i = 0; i < diamondNumbers.Count; i++)
        {
            diamondNumbers[i] = GetRandomNumbers(foodProbabilities);
        }

        for (int i = 0; i < diamondNumbers.Count; i++)
        {
            int num = diamondNumbers[i];
            GameObject diamond = diamondGameObjects[i];
            DiamondManager diamondScript = diamond.GetComponent<DiamondManager>();
                ChangeDiamondText(diamondScript.textGameObject,num);
                diamondScript.number = num;
                diamond.transform.GetChild(1).GetComponent<Image>().sprite = ChooseRandomSprite(foods);
        }

        CustomerCount = Random.Range(minCustomers, maxCustomers);
        CoinCount = 0;

        if (HighScoreHolder.Instance.firstTimePlaying)
        {
            HighScoreHolder.Instance.firstTimePlaying = false;
            highScoreHUD.SetActive(false);
        }

        else
        {
            HighScoreHolder.Instance.HighScore = HighScoreHolder.Instance.HighScore;
        }
        
    }


    public void StartGame()
    {
        startScreenGameObject.SetActive(false);
        
    }

    public void ResetDiamonds()
    {
        foreach (var diamonds in diamondGameObjects)
        {
            

            if (diamonds != null) diamonds.GetComponent<DiamondManager>().ResetColor();
        }
        
    }
    public int GetRandomNumbers(List<int> probs)
    {
        int diceRoll = Random.Range(0, probs.Sum());
        
        
        Dictionary<int, int> tempDict = SetRandomRanges(probs);
        int selectedElement = 0;
        int cumulative = 0;

        foreach (KeyValuePair<int,int> item in tempDict)
        {
            cumulative += item.Value;
            if (diceRoll < cumulative)
            {
                selectedElement = item.Key +1;
                break;
            }
        }
        return selectedElement;
    }

    public Dictionary<int, int> SetRandomRanges(List<int> probs)
    {
        Dictionary<int, int> tempDict = new Dictionary<int, int>();
      
        for (int i = 0; i < probs.Count; i++)
        {
            int currentProb = probs[i];
            tempDict.Add(i,currentProb);
            
        }

        return tempDict;
    }


    public void ChangeDiamondText(TextMeshProUGUI go,int text)
    {
        print(go);
        go.text = text.ToString();
    }

    public Sprite ChooseRandomSprite(List<Sprite> sprite)
    {
        int rand = Random.Range(0, sprite.Count);
        return sprite[rand];

    }

    public void ToggleButtons()
    {
        foreach (var buttons in diamondGameObjects)
        {
            if (!buttons.GetComponent<Button>().interactable)
            {
                buttons.GetComponent<Button>().interactable = true;
            }
            
            else if (buttons.GetComponent<Button>().interactable)
            {
                buttons.GetComponent<Button>().interactable = false;
            }
            
        }
        
        
    }

    public void EndGame()
    {
        endScreenGameObject.SetActive(true);

        if (CoinCount > HighScoreHolder.Instance.HighScore)
        {
            HighScoreHolder.Instance.HighScore = CoinCount;
        }

        else
        {
            HighScoreHolder.Instance.HighScore = HighScoreHolder.Instance.HighScore;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);

    }




}
    
    
    
  

