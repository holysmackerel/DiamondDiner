using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class CustomerController : MonoBehaviour
{
    private static CustomerController _instance;
    public static CustomerController Instance { get { return _instance; } }

    public GameObject coinsTextGameObject;
    public GameObject coinImageGameObject;
    public GameObject descriptionGameObject;

    public List<GameObject> listOfCustomers;

    private Animator _coinsTextAnimator;
    private Animator _coinImageAnimator;
    private Animator _descriptionAnimator;

    private int _coinsEarned;
    private string _feedback;

    public GameObject customerPrefab;

    public GameObject customerHost;

    public int CoinsEarned
    {
        get => _coinsEarned;
        set
        {
            _coinsEarned = value;
            GameManager.Instance.CoinCount = GameManager.Instance.CoinCount + CoinsEarned; 
            if (value < 0)
            {
                
                coinsTextGameObject.GetComponent<TextMeshProUGUI>().text = string.Concat("-", value.ToString());
            }
            
            else if (value > 0 )
            {
                 coinsTextGameObject.GetComponent<TextMeshProUGUI>().text = string.Concat("+", value.ToString());
            }
            else
            {
                coinsTextGameObject.GetComponent<TextMeshProUGUI>().text = value.ToString();
            }
        }
    }
    
    public string Feedback
    {
        get => _feedback;
        set
        {
            _feedback = value;
            descriptionGameObject.GetComponent<TextMeshProUGUI>().text = value;
        }
    }
    
    public List<GameObject> customerGameObjects;
    public List<Sprite> faces;
    public List<int> customerProbabilities;
    public int currentDemand;
    private static readonly int PlayAnimation = Animator.StringToHash("playAnimation");
    public bool isAnimating;
    public bool haveFirstCustomersBeenSet;
    


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }
    private void Start()
    {
        _coinsTextAnimator = coinsTextGameObject.GetComponent<Animator>();
        _coinImageAnimator = coinImageGameObject.GetComponent<Animator>();
        _descriptionAnimator = descriptionGameObject.GetComponent<Animator>();
        haveFirstCustomersBeenSet = false; 
        ToggleAnimation(false);
        
        faces = Resources.LoadAll<Sprite>("Sprites/Faces/").ToList();

        CreateCustomerGameObjects();

        AssignDemandAndTexts();
        AssignImages();
      
        GetDemand();
        
    }

    private void AssignDemandAndTexts()
    {

        for (int i = 0; i < customerGameObjects.Count; i++)
        {
            List<GameObject> tempList = listOfCustomers.Where(o => o.GetComponent<Customer>().isActive).ToList();
            
            GameObject customer = customerGameObjects[i];
            Customer customerScript = customer.GetComponent<Customer>();

            customerScript.Demand = tempList[i].GetComponent<Customer>().Demand;
            customerScript.textGameObject.GetComponent<TextMeshProUGUI>().text = customerScript.Demand.ToString();
            customerScript.isAssigned = true;
            
        }
    }

    public void AssignImages()
    {
        for (int i = 0; i < customerGameObjects.Count; i++)
        {
            List<GameObject> tempList = listOfCustomers.Where(o => o.GetComponent<Customer>().isActive).ToList();
            
            GameObject customer = customerGameObjects[i];
            Customer customerScript = customer.GetComponent<Customer>();
            Image faceImageComponent = customerScript.faceGameObject.GetComponent<Image>();

            faceImageComponent.sprite = tempList[i].GetComponent<Customer>().faceGameObject.GetComponent<Image>().sprite;
            
            Image foodImageComponent = customerScript.foodGameObject.GetComponent<Image>();
            foodImageComponent.sprite = tempList[i].GetComponent<Customer>().foodGameObject.GetComponent<Image>().sprite;

        }
    }
    

    public void RemoveCustomer()
    {
        listOfCustomers.First(o => o.GetComponent<Customer>().isActive).GetComponent<Customer>().isActive = false;

    }

    private void CreateCustomerGameObjects()
    {
        List<GameObject> tempList = new List<GameObject>();
        
        for (int i = 0; i < GameManager.Instance.CustomerCount; i++)
        {
            int rand = GameManager.Instance.GetRandomNumbers(customerProbabilities);
            GameObject go = Instantiate(customerPrefab,customerHost.transform);
            Customer customerScript = go.GetComponent<Customer>();


            //Assign faces
            Image image = customerScript.faceGameObject.GetComponent<Image>();
            image.sprite = faces[UnityEngine.Random.Range(0, faces.Count)];
            
            //Assign Foods
            Image foodImage = customerScript.foodGameObject.GetComponent<Image>();
            foodImage.sprite = GameManager.Instance.foods[UnityEngine.Random.Range(0, GameManager.Instance.foods.Count)];
            customerScript.Demand = rand;
            customerScript.isActive = true;
            tempList.Add(go);
        }
        listOfCustomers = tempList;
    }
    
    

    public IEnumerator CompareDemandWithSelected()

    {
        int selected = FoodController.Instance.Total;
        print("selected= " + selected);
        int demand = GetDemand();
        print("demand= " + demand);
        int diff = selected - demand;
        print("diff= " + diff);

       
        switch (diff)
        {
        case int n when (n < -9):
            CoinsEarned = 0;
            Feedback = "!@%#";
            break;
        case int n when (n > -8 && n <=-5):
            CoinsEarned = 0;
            Feedback = "Oh no!";
            break;
        case int n when (n > -4 && n <=-1):
            CoinsEarned = 3;
            Feedback = "Not Bad";
            break;
        case int n when (n > -1 && n <=2):
            CoinsEarned =10;
            Feedback = "Sweet";
            break;
        case int n when (n > 2 && n <=5):
            CoinsEarned = 5;
            Feedback = "A bit too much";
            break;
        case int n when (n > 6):
            CoinsEarned = 0;
            Feedback = "Ugh!";
            break;
        }
        IEnumerator coroutine = PlayResultAnimation();
        StartCoroutine(coroutine);
        GameManager.Instance.ToggleButtons();
        yield return new WaitUntil(() => !isAnimating);
        GameManager.Instance.ToggleButtons();
        RemoveCustomer();
        CheckForNoMoreCustomers();
        AssignDemandAndTexts();
        AssignImages();

        if (GameManager.Instance.CustomerCount == 1)
        {
            GameManager.Instance.EndGame();
        }
        else
        {
            GameManager.Instance.CustomerCount--;
        }
        
        
    }

    public int GetDemand()
    {
        currentDemand = customerGameObjects[0].GetComponent<Customer>().Demand;
        return currentDemand;
    }

    public IEnumerator PlayResultAnimation()
    {
        isAnimating = true;
        ToggleAnimation(true);
        yield return new WaitUntil(() => !isAnimating);
        ToggleAnimation(false);
       
    }

    public void ToggleAnimation(bool onOff)
    {
        _coinImageAnimator.SetBool(PlayAnimation,onOff);
        _coinsTextAnimator.SetBool(PlayAnimation,onOff);
        _descriptionAnimator.SetBool(PlayAnimation,onOff);
        
    }


    public void CheckForNoMoreCustomers()
    {

        List<GameObject> tempList = listOfCustomers.Where(o => o.GetComponent<Customer>().isActive).ToList();
        for (int i = 0; i < customerGameObjects.Count; i++)
        {
            
            
            if (tempList.Count - i == 0) 
            {
                customerGameObjects[i].SetActive(false);
                customerGameObjects.Remove(customerGameObjects[i]);

            }
        }
    }

    public void SubmitFoods()
    {
        if (!Instance.isAnimating)
        {
            if (GameManager.Instance.NumberSelected == 3)
            {
                GameManager.Instance.NumberSelected = 0;

                IEnumerator coroutine = Instance.CompareDemandWithSelected();
                StartCoroutine(coroutine);

                GameManager.Instance.ResetDiamonds();
                FoodController.Instance.ResetNumber();
                GameManager.Instance.areButtonsDown = false;
            }

        }
        
    }


}
