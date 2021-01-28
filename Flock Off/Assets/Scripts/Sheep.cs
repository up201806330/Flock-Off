using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Sheep : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    Animator animator;
    int wlkHash = Animator.StringToHash("walking");
    Rigidbody rb;

    [SerializeField]
    GameObject player;
    [Header("Stats")]
    [SerializeField]
    float range;
    [SerializeField]
    float hitDistance;

    public int nNeighbors;

    bool scared = false;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Calculating neighbor sheep
        nNeighbors = 0;
        Vector3 sumDestination = Vector3.zero;
        foreach (Collider x in Physics.OverlapSphere(transform.position, range * 0.8f)) {
            if (x.GetComponent<Sheep>() != null) {
                nNeighbors++;
                sumDestination += x.GetComponent<NavMeshAgent>().destination;
            }
        }
        if (nNeighbors > 1) {
            sumDestination /= nNeighbors;
            navMeshAgent.destination = sumDestination;
        }

        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance <= range) {
            Vector3 dirToPlayer = transform.position - player.transform.position;
            Vector3 newPos = transform.position + dirToPlayer;
            navMeshAgent.SetDestination(newPos / 1.5f);
            if (!animator.GetBool(wlkHash)) animator.SetBool(wlkHash, true);
        }
        else {
            animator.SetBool(wlkHash, false);
        }

        // Seeing if it will fall off edge
        NavMeshHit hit;
        NavMesh.FindClosestEdge(transform.position, out hit, NavMesh.AllAreas);

        if (hit.distance <= hitDistance && scared) {
            navMeshAgent.enabled = false;               // Disable AI
            rb.constraints = RigidbodyConstraints.None; // Enable "ragdoll"
            Debug.Log("Ive fallen");
        }
    }
}
