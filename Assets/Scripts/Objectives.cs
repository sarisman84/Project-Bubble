using TMPro;
using UnityEngine;

public class Objectives : MonoBehaviour
{
    [SerializeField] TextMeshPro objectiveTextField;
    [SerializeField] string[] objectives;

    void Start()
    {
        objectiveTextField = GetComponent<TextMeshPro>();
    }

    public void PrintCurrentObjective(string currentObjectiveToPrint)
    {
        objectiveTextField.text = currentObjectiveToPrint;
    }

}
