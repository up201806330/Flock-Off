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
    [SerializeField]
    float pushForce;
    [SerializeField]
    float maxVelocity;

    public int nNeighbors;

    bool dead = false;

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
        if (dead) return;
        if (rb.velocity.magnitude > maxVelocity) rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);

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
            Vector3 newPos = transform.position + dirToPlayer * 0.75f;

            if (Vector3.Distance(newPos, transform.position) > 5) newPos = transform.position;

            navMeshAgent.SetDestination(newPos);
            if (!animator.GetBool(wlkHash)) animator.SetBool(wlkHash, true);
        }
        else {
            animator.SetBool(wlkHash, false);
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Edge") {
            kill(true);
        }
    }

    public void kill(bool pushedOff) {
        if (!dead) {
            if (pushedOff) {
                rb.constraints = RigidbodyConstraints.None;  // Enable "ragdoll"
                rb.velocity = transform.forward * pushForce; // Apply push
            }
            else Destroy(rb);
            navMeshAgent.enabled = false;                    // Disable AI
            dead = true;
            // [SFX] Bahh here (random from list of 2 or 3 sounds)
        }
    }
}
