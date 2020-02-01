using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlowArea : MonoBehaviour
{
    [SerializeField]
    private int slowModificator = 4;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<NavMeshAgent>().speed /= slowModificator;
        }
        else if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().ApplySlowMode(slowModificator);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<NavMeshAgent>().speed *= slowModificator;
        }
        else if(other.CompareTag("Player"))
        {
            other.GetComponent<Player>().ApplySlowMode(1);
        }
    }
}
