using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AlertStates {NORMAL, SHOCKED, ONTHEWAY, CHECKING}

public class Alert : MonoBehaviour
{
    [SerializeField]
    private float shockTime;
    [SerializeField]
    private float checkingTime;
    Enemy enemy;
    NavMeshAgent agent;
    Player player;
    public AlertStates alertState { get; set;}
    Vector3 destinationPos;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
        agent = GetComponent<NavMeshAgent>();
        player = Player._instance;
        Init();
    }

    public void Init()
    {
        alertState = AlertStates.NORMAL;
        destinationPos = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy.state == EnemyStates.ALERT)
        {
            if (alertState == AlertStates.NORMAL)
            {
                alertState = AlertStates.SHOCKED;
                StartCoroutine(Shock());
            }
            else if(alertState == AlertStates.ONTHEWAY)
            {
                if (!agent.hasPath && destinationPos != Vector3.zero)
                {
                    agent.SetDestination(destinationPos);
                }
                else if (agent.remainingDistance < agent.stoppingDistance)
                {
                    alertState = AlertStates.CHECKING;
                    StartCoroutine(Checking());
                }
                
            }
        }
    }

    public void SetAlertDestination(Vector3 pos)
    {
        destinationPos = pos;
    }

    public void SetAlertDestination()
    {
        destinationPos = player.transform.position;
    }

    IEnumerator Checking()
    {
        yield return new WaitForSeconds(checkingTime);
        if (enemy.state == EnemyStates.ALERT)
        {
            enemy.state = EnemyStates.PATROL;
            GetComponent<Patrol>().MosquedEnemy();
        }
        Init();
    }

    IEnumerator Shock()
    {
        agent.SetDestination(enemy.transform.position);
        LookAtPlayer();
        yield return new WaitForSeconds(shockTime);
        alertState = AlertStates.ONTHEWAY;
    }

    void LookAtPlayer()
    {
        Vector3 lookPos = player.transform.position - enemy.transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        enemy.transform.rotation = rotation;
    }
}
