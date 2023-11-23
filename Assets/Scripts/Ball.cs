using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Point"))
        {
            if (GameManager.throwing)
            {
                AudioManager.instance.PlaySFX("JinitaimeiSFX");
                GameManager.getPoint = true;
            }
        }
    }
}
