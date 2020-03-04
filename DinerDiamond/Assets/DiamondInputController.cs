using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class DiamondInputController : MonoBehaviour,IPointerUpHandler
{
    // Start is called before the first frame update
    public bool isSwitchedOn;
   [SerializeField] private int _strength; 
   [SerializeField] private int _inventory;
   [SerializeField] public Sprite foodSprite;
   public TextMeshProUGUI strengthTextGameObject;
    public TextMeshProUGUI inventoryTextGameObject;
    public GameObject foodImage;
    public GameObject sliderGameObject;
    
    public int Strength
    {
        get => _strength;
        set
        {
            _strength = value; 
            strengthTextGameObject.GetComponent<TextMeshProUGUI>().text = _strength.ToString();
            
            
           
        }
    }
    
    public int Inventory
    {
        get => _inventory;
        set
        {
            _inventory = value; 
            inventoryTextGameObject.GetComponent<TextMeshProUGUI>().text = _inventory.ToString();
            sliderGameObject.GetComponent<Slider>().value = Inventory;



        }
    }
    
    

    private void Awake()
    {

        foreach (Transform child in transform)
        {
            if (child.gameObject.name == "Strength")
            {
                strengthTextGameObject = child.gameObject.GetComponent<TextMeshProUGUI>();
                
            }

            else if (child.gameObject.name == "Inventory")
            {
                inventoryTextGameObject = child.gameObject.GetComponent<TextMeshProUGUI>();
                
            }
            
            else if (child.gameObject.name == "Food")
            {
                foodImage = child.gameObject;
                foodSprite = foodImage.GetComponent<Sprite>();

            }
            
            else if (child.gameObject.name == "Slider")
            {
                sliderGameObject = child.gameObject;
            }
        }
        inventoryTextGameObject.text = Inventory.ToString();
        strengthTextGameObject.text = Strength.ToString();
        
        ChangeColorOfButton(Color.grey);
        ResetColorAndState();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!CustomerController.Instance.isAnimating)
        {
            if (!isSwitchedOn && Inventory > 0)
            {
                isSwitchedOn = true;
                Inventory--;
                ChangeColorOfButton(Color.cyan);
                FoodController.Instance.ChangeNumber(Strength);
                GameManager.Instance.NumberSelected++;
                GameManager.Instance.selectedDiamonds.Add(gameObject);
            }
            else if (isSwitchedOn)
            {
                isSwitchedOn = false;
                Inventory++;
                ChangeColorOfButton(Color.grey);
                FoodController.Instance.ChangeNumber(-Strength);
                GameManager.Instance.NumberSelected--;
                GameManager.Instance.selectedDiamonds.Remove(gameObject);
            }
        }

        if (GameManager.Instance.NumberSelected >= 1)
        {
            FoodController.Instance.submitFoodButton.GetComponent<Button>().interactable = true;
        }
        else if (GameManager.Instance.NumberSelected < 1)
        {
            FoodController.Instance.submitFoodButton.GetComponent<Button>().interactable = false;
        }
        
        
    }

    private void ChangeColorOfButton(Color color)
    {
        
        gameObject.GetComponentInChildren<Image>().color = color;
    }


    public void ResetColorAndState()
    {
        gameObject.GetComponentInChildren<Image>().color = Color.grey;
        isSwitchedOn = false;

    }
   
    
    
}
