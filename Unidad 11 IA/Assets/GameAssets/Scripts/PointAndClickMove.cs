using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PointAndClickMove : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private LayerMask lm;
    [SerializeField]
    private float crouchSpeed;
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    int _slowMode = 1;
    public int slowMode
    {
        get { return _slowMode; }
        set
        {
            _slowMode = value;
            if (_slowMode == 0)
                _slowMode = 1;
        }
    }

    float speed;
    private NavMeshAgent agent;
    

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        speed = walkSpeed;
    }

    private void Update()
    {
        speed = walkSpeed;
        if (Input.GetMouseButtonDown(1))
        {
            MoveToTarget();
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            speed = crouchSpeed;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = runSpeed;
        }

        agent.speed = speed / slowMode;
    }

    void MoveToTarget()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, lm))
        {
            target.position = hit.point;
            agent.SetDestination(hit.point);
        }
    }
}
