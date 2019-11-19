using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InnerMonologue : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI monologueField;
    [SerializeField] string[] Lines;
    [SerializeField] public static int currentIndex = 0;

    private void Start()
    {
        PrintCurrentObjective();
        StartCoroutine("TimeBetweenLines");
    }

    public void Update()
    {
        monologueField.text = Lines[currentIndex].ToString();
    }

    void PrintCurrentObjective()
    {
        monologueField.text = Lines[currentIndex].ToString();
    }

    public void IncreaseCurrentIndex(int index)
    {
        currentIndex = index;
    }
    
    IEnumerator TimeBetweenLines()
    {
        yield return new WaitForSeconds(4);
        currentIndex = 1;
        monologueField.text = Lines[currentIndex].ToString();
        yield return new WaitForSeconds(2);
        currentIndex = 2;
        monologueField.text = Lines[currentIndex].ToString();
        yield return new WaitForSeconds(2);
        currentIndex = 3;
        monologueField.text = Lines[currentIndex].ToString();
        yield return new WaitForSeconds(2);
        currentIndex = 4;
        monologueField.text = Lines[currentIndex].ToString();
        monologueField.text = Lines[currentIndex].ToString();
    }
           
        

        
    
}

