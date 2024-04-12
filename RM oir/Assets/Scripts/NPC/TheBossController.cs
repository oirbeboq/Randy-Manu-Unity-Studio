//using System.Collections;
//using UnityEngine;
//using UnityEngine.AI;

//public class TheBossController : MonoBehaviour
//{
//    public NavMeshAgent agent;
//    public Transform[] waypoints;
//    int waypointIndex;
//    Vector3 target;
//    Animator animator;
//    public void Awake()
//    {
//        agent = GetComponent<NavMeshAgent>();
//        animator = GetComponent<Animator>();
//        UpdateDestination();
//    }
//    private void Update()
//    {
//        if (Vector3.Distance(transform.position, target) < 1)
//        {
//            IterateWaypointIndex();
//            UpdateDestination();
//        }
//        animator.SetBool("isWalking", agent.velocity.magnitude > 0.1f);
//        Debug.Log(agent.velocity.magnitude);
//    }

//    void UpdateDestination()
//    {
//        target = waypoints[waypointIndex].position;
//        agent.SetDestination(target);
//    }

//    void IterateWaypointIndex()
//    {
//        waypointIndex++;
//        if(waypointIndex >= waypoints.Length)
//        {
//            waypointIndex = 0;
//        }
//    }
//}
using UnityEngine;
using UnityEngine.AI;
using System.Collections;


public class Patrol : MonoBehaviour
{

    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;

    Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();


        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;
        GotoNextPoint();
    }


    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
        {
            return;
        }
        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
    }


    void Update()
    {
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!agent.pathPending && agent.remainingDistance < 0.25f)
        {
            GotoNextPoint();
        }

        animator.SetBool("isWalking", agent.velocity.magnitude > 0.01f);
        Debug.Log(agent.velocity.magnitude);



    }
}