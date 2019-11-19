using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//Eliott
public class MonologueTrigger : MonoBehaviour
{
   [SerializeField] TextMeshProUGUI monologueField; //Dra in dialogfältet här
   [SerializeField] GameObject monologueStorer; //och dra in monolog grejen här
   [SerializeField] int length; //Hur länge linen ska vara aktiv
   [SerializeField] int LineNumber; //Vilken line som ska visas

    public void Start()
    {
       InnerMonologue innermMonologue = monologueStorer.GetComponent<InnerMonologue>();
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine("TimeBetweenLines");     
    }
    IEnumerator TimeBetweenLines()
    {
        InnerMonologue.currentIndex = LineNumber;
        yield return new WaitForSeconds(length);
        InnerMonologue.currentIndex = 9;
    }
}
