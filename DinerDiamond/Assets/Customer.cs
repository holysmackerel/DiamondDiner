using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    [SerializeField]
    private int demand;

    public int foodNumber;
    public Sprite foodSprite;

    public GameObject textGameObject;
    public GameObject faceGameObject;
    public GameObject foodGameObject;

    public bool isAssigned;
    public bool isActive; 
    
    public int Demand
    {
        get => demand;
        set
        {
            demand = value;
            
        }
    }
    
    
    
    
    // Start is called before the first frame update
    void OnEnable()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.name == "Text")
            {
                textGameObject = child.gameObject;
            }

            else if (child.gameObject.name == "Face")
            {
                faceGameObject = child.gameObject;
            }
            else
            {
                foodGameObject = child.gameObject;
                foodSprite = foodGameObject.GetComponent<Sprite>();
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
