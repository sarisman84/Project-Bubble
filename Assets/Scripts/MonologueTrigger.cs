using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//Eliott
public class MonologueTrigger : MonoBehaviour
{
   [SerializeField] TextMeshProUGUI monologueField; //Dra in dialogfältet här
   [SerializeField] GameObject monologueStorer; //och dra in monolog grejen här
   [SerializeField] List<float> delay; //Hur länge linen ska vara aktiv
   [SerializeField] List<string> lines; //Vilken line som ska visas

    public void Start()
    {
       InnerMonologue innermMonologue = monologueStorer.GetComponent<InnerMonologue>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log(lines[0]);
        InnerMonologue.instance.PrintThis(lines,delay);
            Destroy(this.gameObject);
        }
    }
   
}
