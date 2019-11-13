using TMPro;
using UnityEngine;

// Erik Neuhofer
public class Objectives : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI objectiveTextField;
    [SerializeField] string[] objectives;

    int currentIndex = 0;

    private void Start()
    {
        PrintCurrentObjective();
    }

    void PrintCurrentObjective()
    {
        objectiveTextField.text = objectives[currentIndex].ToString();
    }

    public void IncreaseCurrentIndex(int index)
    {
        currentIndex = index;
    }
}