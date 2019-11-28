using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatDisplayAnim : MonoBehaviour
{
    public PlayerCharacteristics characteristics;

    //public GameObject statIndicator;
    public Transform animationParent;
    public GameObject statIncreaseGraphics;

    public Sprite slugImage;
    public Sprite hotfullImage;
    public Sprite diplomatiskImage;

    int savedSlug;
    int savedDiplo;
    int savedHotfull;

    private void Start()
    {
        int currentSlug = characteristics.GetSlug();
        int currentDiplo = characteristics.GetDiplomatisk();
        int currentHotfull = characteristics.GetHotfull();

        savedSlug = currentSlug;
        savedDiplo = currentDiplo;
        savedHotfull = currentHotfull;
    }

    private void Update()
    {
        int currentSlug = characteristics.GetSlug();
        int currentDiplo = characteristics.GetDiplomatisk();
        int currentHotfull = characteristics.GetHotfull();

        int numberOfSlugToSpawn = currentSlug - savedSlug;
        int numberOfDiploToSpawn = currentDiplo - savedDiplo;
        int numberOfHotfullToSpawn = currentHotfull - savedHotfull;

        savedSlug = currentSlug;
        savedDiplo = currentDiplo;
        savedHotfull = currentHotfull;

        if (numberOfSlugToSpawn + numberOfDiploToSpawn + numberOfHotfullToSpawn > 0)
        {
            if (numberOfSlugToSpawn > 0)
            {
                leveledUpCharacteristics.Add(Characteristics.Slug);
            }

            if (numberOfDiploToSpawn > 0)
            {
                leveledUpCharacteristics.Add(Characteristics.Diplomatisk);
            }

            if (numberOfHotfullToSpawn > 0)
            {
                leveledUpCharacteristics.Add(Characteristics.Hotfull);
            }

            if (!coroutineIsOn)
            {
                StartCoroutine(PlayAnimation());
            }

            //for (int i = 0; i < numberOfSlugToSpawn; i++)
            //{
            //    LevelUpStat(Characteristics.Slug);
            //}
            //for (int i = 0; i < numberOfDiploToSpawn; i++)
            //{
            //    LevelUpStat(Characteristics.Diplomatisk);
            //}
            //for (int i = 0; i < numberOfHotfullToSpawn; i++)
            //{
            //    LevelUpStat(Characteristics.Hotfull);
            //}

        }
    }

    //public void LevelUpStat(Characteristics type)
    //{
    //    switch (type)
    //    {
    //        default:
    //        case Characteristics.None:
    //            break;
    //        case Characteristics.Diplomatisk:
    //            statIndicator.GetComponent<Image>().sprite = diplomatiskImage;
    //            break;
    //        case Characteristics.Hotfull:
    //            statIndicator.GetComponent<Image>().sprite = hotfullImage;
    //            break;
    //        case Characteristics.Slug:
    //            statIndicator.GetComponent<Image>().sprite = slugImage;
    //            break;
    //    }
    //    statIndicator.GetComponent<Animator>().SetTrigger("Show");
    //}

    List<Characteristics> leveledUpCharacteristics = new List<Characteristics>();
    bool coroutineIsOn = false;
    float timeBetweenDisplays = 0.25f;

    IEnumerator PlayAnimation()
    {
        GetComponent<Animator>().SetTrigger("Show");
        coroutineIsOn = true;

        while (leveledUpCharacteristics.Count > 0)
        {
            GameObject spawnedIndicator = Instantiate(statIncreaseGraphics, animationParent);
            switch (leveledUpCharacteristics[0])
            {
                default:
                case Characteristics.None:
                    break;
                case Characteristics.Diplomatisk:
                    spawnedIndicator.GetComponent<Image>().sprite = diplomatiskImage;
                    break;
                case Characteristics.Hotfull:
                    spawnedIndicator.GetComponent<Image>().sprite = hotfullImage;
                    break;
                case Characteristics.Slug:
                    spawnedIndicator.GetComponent<Image>().sprite = slugImage;
                    break;
            }
            spawnedIndicator.GetComponent<Animator>().SetTrigger("Show");
            leveledUpCharacteristics.RemoveAt(0);
            Destroy(spawnedIndicator, 2);
            yield return new WaitForSeconds(timeBetweenDisplays);
        }
        coroutineIsOn = false;
    }
}
