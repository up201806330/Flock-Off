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
    [SerializeField]
    float range;
    [SerializeField]
    float hitDistance;

    bool scared = true;

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

        if (hit.distance <= hitDistance) {
            navMeshAgent.enabled = false;               // Disable AI
            rb.constraints = RigidbodyConstraints.None; // Enable "ragdoll"
            Debug.Log("Ive fallen");
        }
    }
}
