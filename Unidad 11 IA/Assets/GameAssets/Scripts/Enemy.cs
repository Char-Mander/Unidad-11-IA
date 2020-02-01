using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStates { PATROL, ALERT, KILLER }

public class Enemy : MonoBehaviour
{
    public EnemyStates state { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        state = EnemyStates.PATROL;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
