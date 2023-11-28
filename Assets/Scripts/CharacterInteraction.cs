using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInteraction : MonoBehaviour
{
    public Transform restartpoint;
    public Transform checkpoint1;
    public bool isDead = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "coin")
        {
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "dead")
        {
            isDead = true;
        }
        else if (other.gameObject.tag == "checkpoint")
        {
            restartpoint = checkpoint1;
        }
    }

}
