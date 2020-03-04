using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinAndPraiseController : MonoBehaviour
{
    

    public List<int> listoOfScore;
    public List<string> listOfPraises;
    public int bonusScore;
    private static CoinAndPraiseController _instance;
    public static CoinAndPraiseController Instance { get { return _instance; } }
    
    
    
   
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
       
    }

    public int GetCoinResults(int value, bool bonus)
    {
        return listoOfScore[value] + IsThereABonus(bonus);

    }

    public string GetPraise(int value)
    {
        return listOfPraises[value];
        
    }

    public int IsThereABonus(bool bonus)
    {
        if (bonus)
        {
            return bonusScore;
        }

        return 0;

    }
    
}
