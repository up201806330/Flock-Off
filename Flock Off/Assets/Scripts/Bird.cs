using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bird : MonoBehaviour
{
    Orchestrator orchestrator;

    AudioSource audio;

    [SerializeField]
    AudioClip[] sounds;

    GameObject obj;
    GameObject areaObj;
    Area area;
    MeshRenderer areaMesh;

    Animator animator;
    int dangerHsh = Animator.StringToHash("danger");
    int grabbedHsh = Animator.StringToHash("grabbed");
    int attackingHsh = Animator.StringToHash("attacking");

    [Header("Circle Motion")]
    [SerializeField]
    float width;
    [SerializeField]
    float height;
    Vector3 rotationCenter;

    [Header("Area")]
    [SerializeField]
    Color idleColor;
    [SerializeField]
    Color dangerColor;
    [SerializeField]
    Color attackingColor;
    Color currColor;


    [SerializeField]
    State state = State.idle;
    float stateTimer;
    [SerializeField]
    float dangerTime;
    [SerializeField]
    float attackingTime;
    [SerializeField]
    float returningTime;

    GameObject targetSheep = null;
    Vector3 startingBirdPos = Vector3.zero;
    Quaternion startingBirdRot = Quaternion.identity;
    Quaternion reachedBirdRot = Quaternion.identity;
    Vector3 reachedBirdPos = Vector3.zero;
    Vector3 startingAreaPos = Vector3.zero;
    Vector3 reachedAreaPos = Vector3.zero;
    Vector3 startingAreaScale = Vector3.zero;
    Vector3 reachedAreaScale = Vector3.zero;

    float circleMotionCounter = 0;

    enum State {
        idle,
        danger,
        attacking,
        returningFromDanger,
        returningFromAttack,

        grabbed

    }

    void Awake() {
        orchestrator = GetComponentInParent<Orchestrator>();

        obj = transform.GetChild(0).gameObject;
        areaObj = transform.GetChild(1).gameObject;
        area = GetComponent<Area>();

        areaMesh = areaObj.GetComponent<MeshRenderer>();
        areaMesh.material.color = idleColor;

        startingAreaScale = areaObj.transform.localScale;
        startingAreaPos = areaObj.transform.position;
        rotationCenter = obj.transform.position;

        animator = GetComponentInChildren<Animator>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state) {
            case State.idle:
                Debug.DrawRay(obj.transform.position, -obj.transform.right, Color.blue);
                // Timer increments
                circleMotionCounter += Time.deltaTime;

                // New position
                Vector3 l = new Vector3(Mathf.Cos(circleMotionCounter) * width, 0, Mathf.Sin(circleMotionCounter) * height);
                obj.transform.position = rotationCenter + l;

                // Looking at new position
                Quaternion targetL = Quaternion.LookRotation(l);
                obj.transform.rotation = Quaternion.Lerp(obj.transform.rotation, targetL, Time.deltaTime * 8);;
                break;

            case State.danger:
                // Timer increments
                circleMotionCounter += Time.deltaTime * 1.4f;
                stateTimer += Time.deltaTime;

                // Update Color
                areaMesh.material.color = Color.Lerp(idleColor, dangerColor, stateTimer / dangerTime * 2f);
                currColor = areaMesh.material.color;

                // New position
                l = new Vector3(Mathf.Cos(circleMotionCounter) * width, 0, Mathf.Sin(circleMotionCounter) * height);
                obj.transform.position = rotationCenter + l;

                // Looking at new position
                targetL = Quaternion.LookRotation(l);
                obj.transform.rotation = Quaternion.Lerp(obj.transform.rotation, targetL, Time.deltaTime * 8);

                if (area.getObjectsInside().Count == 0) {
                    state = State.returningFromDanger;  // State
                    animator.SetBool(dangerHsh, false); // Animator parameter
                    stateTimer = 0;                     // Timer reset
                    reachedBirdPos = obj.transform.position;
                }

                if (stateTimer >= dangerTime) { // Played till the end, go next phase
                    state = State.attacking;              // State
                    animator.SetBool(attackingHsh, true); // Animator parameter
                    stateTimer = 0;                       // Timer reset
                    // [SFX] Attacking cackaww~~

                    audio.clip = sounds[2];
                    audio.Play();
                }
                break;

            case State.attacking:
                // Timer increments
                stateTimer += Time.deltaTime;

                // Update Color
                areaMesh.material.color = Color.Lerp(dangerColor, attackingColor, stateTimer / attackingTime);
                currColor = areaMesh.material.color;

                if (targetSheep == null && startingBirdPos == Vector3.zero) {
                    targetSheep = area.getRandomInside();
                    // Debug.Log(area.getObjectsInside());
                    if (targetSheep == null) {
                        state = State.returningFromDanger;  // State
                        animator.SetBool(dangerHsh, false); // Animator parameter
                        animator.SetBool(attackingHsh, false);
                        stateTimer = 0;                     // Timer reset
                        reachedBirdPos = obj.transform.position;
                        return;
                    }
                    startingBirdPos = obj.transform.position;
                    startingBirdRot = obj.transform.rotation;
                }

                else if (!area.isInside(targetSheep)) { // Target has left the area
                    state = State.returningFromAttack;  // State
                    animator.SetBool(dangerHsh, false); // Animator parameter
                    animator.SetBool(attackingHsh, false);
                    stateTimer = 0;                     // Timer reset
                    reachedBirdPos = obj.transform.position;
                    reachedAreaScale = areaObj.transform.localScale;
                    reachedAreaPos = areaObj.transform.position;
                }

                // Bird looking toward target and descending
                Quaternion targetAng = Quaternion.LookRotation(targetSheep.transform.position - obj.transform.position)*Quaternion.Euler(0,90,0);
                //if (targetAng.eulerAngles.z != 0) targetAng.eulerAngles = new Vector3(targetAng.eulerAngles.x, targetAng.eulerAngles.y, 0); // Lock Z axis
                obj.transform.rotation = Quaternion.RotateTowards(obj.transform.rotation, targetAng, Time.deltaTime * 500);

                if (stateTimer / attackingTime >= 0.4f) obj.transform.position = Vector3.Lerp(startingBirdPos, targetSheep.transform.position, ((stateTimer/ attackingTime) - 0.4f )* 2f );

                Debug.DrawRay(obj.transform.position, -obj.transform.right, Color.blue);
                //Debug.DrawRay(obj.transform.position, Quaternion.Inverse(targetAng).eulerAngles, Color.red);

                // Area shrinking and moving
                areaObj.transform.position = Vector3.Lerp(startingAreaPos, targetSheep.transform.position - new Vector3(0, 0.25f, 0), stateTimer / attackingTime / 1.2f);
                areaObj.transform.localScale = Vector3.Lerp(startingAreaScale, new Vector3(0, 0, 0), stateTimer / attackingTime / 1.2f);

                if (stateTimer >= attackingTime - 0.25f) { // Played till the end, go next phase
                    state = State.grabbed;              // State
                    animator.SetBool(grabbedHsh, true); // Animator parameter
                }

                break;

            case State.returningFromDanger:
                // Timer increments
                circleMotionCounter += Time.deltaTime;
                stateTimer += Time.deltaTime;

                // Update Color
                areaMesh.material.color = Color.Lerp(currColor, idleColor, stateTimer / returningTime);

                l = new Vector3(Mathf.Cos(circleMotionCounter) * width, 0, Mathf.Sin(circleMotionCounter) * height);

                obj.transform.position = Vector3.Lerp(reachedBirdPos, rotationCenter + l, stateTimer / returningTime*2);

                targetL = Quaternion.LookRotation(l);
                obj.transform.rotation = Quaternion.Lerp(obj.transform.rotation, targetL, Time.deltaTime);

                if (stateTimer >= returningTime) { // Played till the end, go next phase
                    state = State.idle;
                    stateTimer = 0;
                }
                break;

            case State.returningFromAttack:
                // Timer increments
                circleMotionCounter += Time.deltaTime;
                stateTimer += Time.deltaTime;

                // Update Color
                areaMesh.material.color = Color.Lerp(currColor, idleColor, stateTimer / returningTime);

                l = new Vector3(Mathf.Cos(circleMotionCounter) * width, 0, Mathf.Sin(circleMotionCounter) * height);

                obj.transform.position = Vector3.Lerp(reachedBirdPos, rotationCenter + l, stateTimer / returningTime);
                areaObj.transform.position = Vector3.Lerp(reachedAreaPos, startingAreaPos, stateTimer / returningTime);
                areaObj.transform.localScale = Vector3.Lerp(reachedAreaScale, startingAreaScale, stateTimer / returningTime);

                targetL = Quaternion.LookRotation(l);
                obj.transform.rotation = Quaternion.Lerp(obj.transform.rotation, targetL, Time.deltaTime);

                if (stateTimer >= returningTime) { // Played till the end, go next phase
                    state = State.idle;
                    stateTimer = 0;
                }
                break;

            case State.grabbed:
                if (isDirectChild(orchestrator.transform, targetSheep.transform) && areaObj.transform.localScale != Vector3.zero) {
                    targetSheep.transform.SetParent(obj.transform);
                    targetSheep.GetComponent<Sheep>().kill(false);
                    areaObj.transform.localScale = Vector3.zero;
                    GetComponent<Collider>().enabled = false;
                }
                l = startingBirdPos - reachedBirdPos;
                obj.transform.position += (l.normalized/7);

                targetL = Quaternion.LookRotation(l);
                obj.transform.rotation = Quaternion.Lerp(obj.transform.rotation, Quaternion.Inverse(targetL), Time.deltaTime);

                break;

            default:
                break;
        }
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Entity" && other.GetComponent<Sheep>() != null && state == State.idle) {
            area.add(other);
            state = State.danger;
            stateTimer = 0;
            animator.SetBool(dangerHsh, true);
            
            audio.clip = sounds[1];
            audio.Play();
        }
    }

    bool isDirectChild(Transform parent, Transform child) {
        foreach (Transform x in parent) if (x == child) return true;
        return false;
    }
}
