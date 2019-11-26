using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsDisplayer : MonoBehaviour
{
    public PlayerCharacteristics characteristics;

    public Transform slugStatHolder = null;
    public Transform diplomaticStatHolder = null;
    public Transform hotfullStatHolder = null;

    public Color slugColor;
    public Color diplomaticColor;
    public Color hotfullColor;

    public GameObject statImage;

    private void Start()
    {
        UpdateStats();
    }

    private void Update()
    {
        UpdateStats();
    }

    public void UpdateStats()
    {
        int numberOfSlugToSpawn = characteristics.GetSlug() - slugStatHolder.childCount;
        int numberOfDiploToSpawn = characteristics.GetDiplomatisk() - diplomaticStatHolder.childCount;
        int numberOfHotfullToSpawn = characteristics.GetHotfull() - hotfullStatHolder.childCount;

        if (numberOfSlugToSpawn + numberOfDiploToSpawn + numberOfHotfullToSpawn > 0)
        {
            for (int i = 0; i < numberOfSlugToSpawn; i++)
            {
                SpawnStatImage(Characteristics.Slug);
            }
            for (int i = 0; i < numberOfDiploToSpawn; i++)
            {
                SpawnStatImage(Characteristics.Diplomatisk);
            }
            for (int i = 0; i < numberOfHotfullToSpawn; i++)
            {
                SpawnStatImage(Characteristics.Hotfull);
            }
        }
    }

    private void SpawnStatImage(Characteristics type)
    {
        switch (type)
        {
            case Characteristics.Diplomatisk:
                {
                    GameObject spawn = Instantiate(statImage, diplomaticStatHolder);
                    spawn.GetComponent<Image>().color = diplomaticColor;
                    break;
                }
            case Characteristics.Hotfull:
                {
                    GameObject spawn = Instantiate(statImage, hotfullStatHolder);
                    spawn.GetComponent<Image>().color = hotfullColor;
                    break;
                }
            case Characteristics.Slug:
                {
                    GameObject spawn = Instantiate(statImage, slugStatHolder);
                    spawn.GetComponent<Image>().color = slugColor;
                    break;
                }
            default:
            case Characteristics.None:
                Debug.LogWarning("Tried to spawn a nonexisting stat");
                break;
        }
    }
}
