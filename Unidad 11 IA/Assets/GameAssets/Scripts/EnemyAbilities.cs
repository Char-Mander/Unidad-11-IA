using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAbilities : MonoBehaviour
{
    [SerializeField]
    private float viewDistance;
    [SerializeField]
    private float viewAngle;
    [SerializeField]
    LayerMask lm;

    Player player;
    Enemy enemy;
    VisionCone visionC;

    private void Start()
    {
        player = Player._instance;
        enemy = GetComponent<Enemy>();
        visionC = GetComponentInChildren<VisionCone>();
    }

    private void Update()
    {
        Hearing();
        Looking();
    }

    void UpdateVisionCone()
    {
        visionC.angle = viewAngle;
        visionC.GetComponent<Projector>().orthographicSize = viewDistance;
        switch (enemy.state)
        {
            case EnemyStates.PATROL:
                visionC.color = Color.green;
                break;
            case EnemyStates.ALERT:
                visionC.color = Color.yellow;
                break;
            case EnemyStates.KILLER:
                visionC.color = Color.red;
                break;
        }
    }

    public void Looking()
    {
        UpdateVisionCone();
        Vector3 direToPlayer = player.transform.position - enemy.transform.position;
        float angle = Vector3.Angle(enemy.transform.forward, direToPlayer);
        if (angle < viewAngle && direToPlayer.magnitude < viewDistance)
        {
            RaycastHit hit;
            if(Physics.Raycast(enemy.transform.position, direToPlayer.normalized, out hit, viewDistance, lm))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    if(enemy.state == EnemyStates.PATROL)
                    {
                        enemy.state = EnemyStates.ALERT;
                        GetComponent<Alert>().SetAlertDestination();
                    }
                    else if(enemy.state == EnemyStates.ALERT && enemy.GetComponent<Alert>().alertState != AlertStates.SHOCKED)
                    {
                        enemy.state = EnemyStates.KILLER;
                    }
                }
            }
        }
    }

    public void Hearing()
    {
        float distPlayerEnemy = Vector3.Distance(this.transform.position, player.transform.position);
        if (distPlayerEnemy < player.noise)
        {
            enemy.state = EnemyStates.ALERT;
            if(GetComponent<Alert>().alertState == AlertStates.SHOCKED) GetComponent<Alert>().SetAlertDestination();
            
        }
    }
}
