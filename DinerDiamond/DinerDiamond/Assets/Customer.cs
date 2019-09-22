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

    public GameObject textGameObject;
    public GameObject imageGameObject;

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

            else
            {
                imageGameObject = child.gameObject;
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
