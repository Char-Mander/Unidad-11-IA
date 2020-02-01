using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    #region Singleton
    public static Player _instance;


    private void Awake()
    {
        if (_instance != null) Destroy(this.gameObject);
        else _instance = this;
    }
    #endregion

    [HideInInspector]
    public float noise { get; set; }

    NavMeshAgent agent;
    AlertArea alertA;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        alertA = GetComponentInChildren<AlertArea>();
    }

    // Update is called once per frame
    void Update()
    {
        noise = agent.velocity.magnitude;
        alertA.distance = noise;
    }

    public void ApplySlowMode(int mode)
    {
        GetComponent<PointAndClickMove>().slowMode = mode;
    }
}
