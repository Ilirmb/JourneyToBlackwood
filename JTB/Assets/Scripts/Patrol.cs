using UnityEngine;
using System.Collections;
using UnityEngine.AI;


public class Patrol : MonoBehaviour
{
    public bool attacking = false;
    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;
    Vector3 destination;
    private GameObject target;
    void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;
        target = GameObject.FindWithTag("Player");
        GotoNextPoint();
    }
    public void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

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
        if (attacking)
        {
            if (Vector3.Distance(destination, target.gameObject.transform.position) > 1.0f)
            {
                destination = target.gameObject.transform.position;
                agent.destination = destination;
            }
        }

        if(!attacking)
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GotoNextPoint();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            attacking = true;
            agent.autoBraking = true;
            agent.stoppingDistance = 2;
        }
    }

}