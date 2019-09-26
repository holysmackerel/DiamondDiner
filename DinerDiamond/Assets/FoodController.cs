using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FoodController : MonoBehaviour
{
    [SerializeField]
    private int total;
    public GameObject giveGameObject;
    public GameObject submitFoodButton;
    
    private static FoodController _instance;
    public static FoodController Instance { get { return _instance; } }

    public int Total
    {
        get => total;
        set
        {
            total = value;
            giveGameObject.GetComponent<TextMeshProUGUI>().text = value.ToString();
            if (value == 0)
            {
                giveGameObject.SetActive(false);
            }

            else
            {
                giveGameObject.SetActive(true);
            }
            
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
        
        Instance.submitFoodButton.GetComponent<Button>().interactable = false;
    }
    
    public void ChangeNumber(int newNumber)
    {
        Total += newNumber;
    }

    public void ResetNumber()
    {
        Total = 0;
        

    }


    
}
