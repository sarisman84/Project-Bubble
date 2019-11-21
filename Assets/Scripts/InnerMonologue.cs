using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InnerMonologue : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI monologueField;
    [SerializeField] public static int currentIndex = 0;

    public static InnerMonologue instance;
    public void Awake()
    {
        if (instance == null)
        {
        instance = this;

        }
        else
        {
            Destroy(this.gameObject);
        }
    }
   
    public void IncreaseCurrentIndex(int index)
    {
        currentIndex = index;
    }
    

    public void PrintThis(List<string>Lines, List<float> delay)
    {
        StartCoroutine(TimeBetweenLines(Lines,delay));
    }
    IEnumerator TimeBetweenLines(List<string> Lines,List<float>delay)
    {
        for (int i = 0; i < Lines.Count; i++)
        {
            Debug.Log(Lines[i]);
            monologueField.text = Lines[i];
            yield return new WaitForSeconds(delay[i]);

             
        }
        monologueField.text = "";

    }
}

