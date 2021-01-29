using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bird : MonoBehaviour
{
    GameObject obj;
    GameObject areaObj;
    Area area;
    MeshRenderer areaMesh;

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

    void Awake()
    {
        obj = transform.GetChild(0).gameObject;
        areaObj = transform.GetChild(1).gameObject;
        area = GetComponent<Area>();

        areaMesh = areaObj.GetComponent<MeshRenderer>();
        areaMesh.material.color = idleColor;

        startingAreaScale = areaObj.transform.localScale;
        startingAreaPos = areaObj.transform.position;
        rotationCenter = obj.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state) {
            case State.idle:
                // Timer increments
                circleMotionCounter += Time.deltaTime;

                // New position
                Vector3 l = new Vector3(Mathf.Cos(circleMotionCounter) * width, 0, Mathf.Sin(circleMotionCounter) * height);
                obj.transform.position = rotationCenter + l;

                // Looking at new position
                Quaternion targetL = Quaternion.LookRotation(l, Vector3.up);
                obj.transform.rotation = Quaternion.Lerp(obj.transform.rotation, targetL, Time.deltaTime * 8);
                break;

            case State.danger:
                // Timer increments
                circleMotionCounter += Time.deltaTime;
                stateTimer += Time.deltaTime;

                // Update Color
                areaMesh.material.color = Color.Lerp(idleColor, dangerColor, stateTimer / dangerTime * 2f);
                currColor = areaMesh.material.color;

                // New position
                l = new Vector3(Mathf.Cos(circleMotionCounter) * width, 0, Mathf.Sin(circleMotionCounter) * height);
                obj.transform.position = rotationCenter + l;

                // Looking at new position
                targetL = Quaternion.LookRotation(l, Vector3.up);
                obj.transform.rotation = Quaternion.Lerp(obj.transform.rotation, targetL, Time.deltaTime * 8);

                if (area.getObjectsInside().Count == 0) {
                    stateTimer = 0;
                    state = State.returningFromDanger;
                    reachedBirdPos = obj.transform.position;
                }

                if (stateTimer >= dangerTime) { // Played till the end, go next phase
                    state = State.attacking;
                    stateTimer = 0;
                    // [SFX] Attacking cackaww~~
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
                        stateTimer = 0;
                        state = State.returningFromDanger;
                        reachedBirdPos = obj.transform.position;
                        return;
                    }
                    startingBirdPos = obj.transform.position;
                }

                else if (!area.isInside(targetSheep)) { // Target has left the area
                    state = State.returningFromAttack;
                    stateTimer = 0;
                    reachedBirdPos = obj.transform.position;
                    reachedAreaScale = areaObj.transform.localScale;
                    reachedAreaPos = areaObj.transform.position;
                }

                Quaternion targetAng = Quaternion.LookRotation(targetSheep.transform.position - obj.transform.position);
                obj.transform.rotation = Quaternion.Lerp(obj.transform.rotation, Quaternion.Inverse(targetAng), Time.deltaTime * 2);
                obj.transform.position = Vector3.Lerp(startingBirdPos, targetSheep.transform.position, stateTimer / attackingTime);

                //Debug.DrawRay(obj.transform.position, obj.transform.rotation.eulerAngles, Color.blue);
                //Debug.DrawRay(obj.transform.position, Quaternion.Inverse(targetAng).eulerAngles, Color.red);

                areaObj.transform.position = Vector3.Lerp(startingAreaPos, targetSheep.transform.position - new Vector3(0, 0.25f, 0), stateTimer / attackingTime / 1.2f);
                areaObj.transform.localScale = Vector3.Lerp(startingAreaScale, new Vector3(0, 0, 0), stateTimer / attackingTime / 1.2f);

                if (stateTimer >= attackingTime) { // Played till the end, go next phase
                    state = State.grabbed;
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

                targetL = Quaternion.LookRotation(l, Vector3.up);
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

                targetL = Quaternion.LookRotation(l, Vector3.up);
                obj.transform.rotation = Quaternion.Lerp(obj.transform.rotation, targetL, Time.deltaTime);

                if (stateTimer >= returningTime) { // Played till the end, go next phase
                    state = State.idle;
                    stateTimer = 0;
                }
                break;

            case State.grabbed:
                // [SFX] Bahh here (random from list of 2 or 3 sounds)
                if (targetSheep.transform.parent == null && areaObj.transform.localScale != Vector3.zero) {
                    targetSheep.transform.SetParent(obj.transform);
                    targetSheep.GetComponent<Sheep>().kill(false);
                    areaObj.transform.localScale = Vector3.zero;
                    GetComponent<Collider>().enabled = false;
                }
                l = startingBirdPos - reachedBirdPos;
                obj.transform.position += (l.normalized/7);

                targetL = Quaternion.LookRotation(l, Vector3.up);
                obj.transform.rotation = Quaternion.Lerp(obj.transform.rotation, targetL, Time.deltaTime);
                break;

            default:
                break;
        }
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Entity" && other.GetComponent<Sheep>() != null && state == State.idle) {
            state = State.danger;
            stateTimer = 0;
            // [SFX] First cackawww~~~ 
        }
    }
}
