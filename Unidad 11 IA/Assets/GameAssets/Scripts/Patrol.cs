using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour
{
    [SerializeField]
    public float waitingTime;
    [SerializeField]
    public float enemySpeed = 3.5f;
    [SerializeField]
    private List<Transform> wayPoints = new List<Transform>();
    NavMeshAgent agent;
    Enemy enemy;
    bool isMoving = false;
    bool isMosqued = false;
    Coroutine wpStop;
    float mosquedWaitingTime;
    float mosquedEnemySpeed;

    int wpIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemy = GetComponent<Enemy>();
        Init();
    }

    public void Init()
    {
        isMoving = false;
        wpStop = StartCoroutine(WaitOnWP());
        agent.speed = enemySpeed;
        mosquedWaitingTime = waitingTime;

    }
    // Update is called once per frame
    void Update()
    {
        if (enemy.state == EnemyStates.PATROL)
        {
            Patrolling();
        }
        else
        {
            if (wpStop != null) StopCoroutine(wpStop);
        }
    }

    public void MosquedEnemy()
    {
        Init();
        isMosqued = true;
        mosquedWaitingTime = waitingTime / 2;
        mosquedEnemySpeed = enemySpeed + enemySpeed / 2;
        agent.speed = mosquedEnemySpeed;
    }

    void Patrolling()
    {
        if (wpIndex < wayPoints.Count)
        {
            if (isMoving && agent.remainingDistance <= agent.stoppingDistance)
            {
                wpStop = StartCoroutine(WaitOnWP());
                isMoving = false;
            }
        }
        else
        {
            wpIndex = 0;
        }
    } 

    IEnumerator WaitOnWP()
    {
        yield return new WaitForSeconds(mosquedWaitingTime);
        agent.SetDestination(wayPoints[wpIndex].position);
        if (isMosqued)
        {
            int randomNum = Random.Range(0, wayPoints.Count);
            while (wpIndex == randomNum)
            {
                randomNum = Random.Range(0, wayPoints.Count);
            }
            wpIndex = randomNum;
        }
        else wpIndex++;
        isMoving = true;
    }
}
