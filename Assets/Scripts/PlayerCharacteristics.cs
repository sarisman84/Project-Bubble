using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Characteristics { None, Diplomatisk, Hotfull, Slug }


//Simon Voss
[CreateAssetMenu(menuName = "Character/Player")]
public class PlayerCharacteristics : ScriptableObject
{
    [SerializeField] int diplomatisk;
    [SerializeField] int hotfull;
    [SerializeField] int slug;
    public List<ScriptableQuest> activeQuests = new List<ScriptableQuest>();
    public List<ScriptableQuest> completedQuests = new List<ScriptableQuest>();
    public List<ScriptableQuest> failedQuests = new List<ScriptableQuest>();

    public void IncreaseStat(Characteristics typeOfStat, int increase)
    {
        bool statIncreased = false;
        switch (typeOfStat)
        {
            case Characteristics.None:
                Debug.Log("No increase in player stat");
                break;
            case Characteristics.Diplomatisk:
                diplomatisk+= increase;
                statIncreased = true;
                break;
            case Characteristics.Hotfull:
                hotfull+= increase;
                statIncreased = true;
                break;
            case Characteristics.Slug:
                slug+= increase;
                statIncreased = true;
                break;
        }
        if (statIncreased)
        {
            Debug.Log("Stats are now at: \n " +
                "Diplomatisk: " + diplomatisk +
                " Slug: " + slug +
                " Hotfull: " + hotfull);
        }
    }

    public int GetCharacteristic(Characteristics charactersitsticsToCheck)
    {
        switch (charactersitsticsToCheck)
        {
            case Characteristics.Diplomatisk:
                return diplomatisk;
            case Characteristics.Hotfull:
                return hotfull;
            case Characteristics.Slug:
                return slug;
            default:
            case Characteristics.None:
                Debug.LogWarning("Checking for non-existing skill");
                return -1;
        }
    }

    public int GetDiplomatisk()
    {
        return diplomatisk;
    }

    public int GetHotfull()
    {
        return hotfull;
    }
    public int GetSlug()
    {
        return slug;
    }
}
