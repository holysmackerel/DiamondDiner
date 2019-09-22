using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class DiamondManager : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler,IPointerUpHandler
{
    // Start is called before the first frame update
    public bool isSwitchedOn;
    public int number; 
    public TextMeshProUGUI textGameObject;

    private void Awake()
    {
        textGameObject = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        ResetColor();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!CustomerController.Instance.isAnimating)
        {
            FoodController.Instance.ChangeNumber(number);
            GameManager.Instance.isMouseDown = true;
            GameManager.Instance.NumberSelected++;
            isSwitchedOn = true;
            ChangeColorOfButton();
            GameManager.Instance.selectedDiamonds.Add(gameObject);
            
        }
        
       
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        if (!CustomerController.Instance.isAnimating)
        {

            if (GameManager.Instance.isMouseDown && GameManager.Instance.NumberSelected < 3)
            {
                FoodController.Instance.ChangeNumber(number);
                GameManager.Instance.NumberSelected++;
                isSwitchedOn = true;
                ChangeColorOfButton();
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!CustomerController.Instance.isAnimating)
        {
            if (GameManager.Instance.NumberSelected == 3)
            {
                GameManager.Instance.NumberSelected = 0;

                if (GameManager.Instance.selectedDiamonds.Contains(gameObject))
                {
                    IEnumerator coroutine = CustomerController.Instance.CompareDemandWithSelected();
                    StartCoroutine(coroutine);
                }

                GameManager.Instance.ResetDiamonds();
                FoodController.Instance.ResetNumber();
                GameManager.Instance.isMouseDown = false;
            }

        }

    }
    
    

    public void OnPointerExit(PointerEventData eventData)
    {
        if (GameManager.Instance.isMouseDown && gameObject == GameManager.Instance.selectedDiamonds[0])
        {
            if (isSwitchedOn)
            {
                ResetColor();
            }
            
        }
    }

    private void ChangeColorOfButton()
    {
        
        gameObject.GetComponentInChildren<Image>().color = Color.cyan;
    }


    public void ResetColor()
    {
        gameObject.GetComponentInChildren<Image>().color = Color.grey;
        
    }
   
    
    
}
