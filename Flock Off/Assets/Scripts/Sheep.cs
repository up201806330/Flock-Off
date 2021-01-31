using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Sheep : MonoBehaviour 
{
    Orchestrator orchestrator;

    AudioSource audio;

    [SerializeField]
    AudioClip[] sounds;

    NavMeshAgent navMeshAgent;
    public Animator animator;
    int walkHsh = Animator.StringToHash("walking");
    int grabbedHsh = Animator.StringToHash("grabbed");
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
    bool saved = false;

    private void Awake() {
        orchestrator = GetComponentInParent<Orchestrator>();

        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();

        audio = GetComponent<AudioSource>();
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
            if (!animator.GetBool(walkHsh)) animator.SetBool(walkHsh, true);
        }
        else {
            animator.SetBool(walkHsh, false);
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Edge") {
            kill(true);
        }
        else if (other.tag == "Fence") {
            if (saved) return;
            orchestrator.markSurvived(gameObject);
            StartCoroutine(waitAndDisableNav());
            saved = true;
            audio.clip = sounds[4];
            audio.Play();
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
            float time = pushedOff ? 0.8f : 0f;
            StartCoroutine(waitAndMarkDead(time));

            int nSound = UnityEngine.Random.Range(0, 4);
            audio.clip = sounds[nSound];
            audio.Play();
        }
    }

    IEnumerator waitAndMarkDead(float time) {
        yield return new WaitForSeconds(time);
        orchestrator.markDead(gameObject);
    }

    IEnumerator waitAndDisableNav() {
        yield return new WaitForSeconds(1.5f);
        navMeshAgent.enabled = false;
    }
}
