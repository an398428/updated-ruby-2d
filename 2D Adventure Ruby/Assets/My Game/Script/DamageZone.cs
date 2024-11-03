using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZonePain : MonoBehaviour
{

    public void OnTriggerStay2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();


        if (controller != null)
        {
            controller.ChangeHealth(-1);
        }
    }
}