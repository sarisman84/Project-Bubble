using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiladAnimations : MonoBehaviour
{
    [SerializeField] Guard guard = null;
    [SerializeField] Animator animator = null;

    private void Update()
    {
        if (guard.playerdUncovered)
        {
            animator.SetBool("talking", true);
        }
        else
        {
            animator.SetBool("talking", false);
        }
    }
}
