using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Characteristics { None, Charming, Intimidation, Logical }


//Simon Voss
public class PlayerCharacteristics : MonoBehaviour
{
    #region Singleton
    public static PlayerCharacteristics instance;
    private void Awake()
    {
        if (PlayerCharacteristics.instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Another instance of: " + this + " , was tried to be instantiated, but was destroyed! This instance was tried to be instantiated on: " + this.gameObject);
            Destroy(this);
        }
    }
    #endregion


    [SerializeField] int charm;
    [SerializeField] int intimidation;
    [SerializeField] int intelligence;

    public void IncreaseStat(Characteristics typeOfStat)
    {
        bool statIncreased = false;
        switch (typeOfStat)
        {
            case Characteristics.None:
                Debug.Log("No increase in player stat");
                break;
            case Characteristics.Charming:
                charm++;
                statIncreased = true;
                break;
            case Characteristics.Intimidation:
                intimidation++;
                statIncreased = true;
                break;
            case Characteristics.Logical:
                intelligence++;
                statIncreased = true;
                break;
        }
        if (statIncreased)
        {
            Debug.Log("Stats are now at: \n " +
                "Charm: " + charm +
                " Intelligence: " + intelligence +
                " Intimidation: " + intimidation);
        }
    }
}
