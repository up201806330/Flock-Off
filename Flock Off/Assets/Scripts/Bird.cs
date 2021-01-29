using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        returningFromAttack

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
                circleMotionCounter += Time.deltaTime;
                Vector3 l = new Vector3(Mathf.Cos(circleMotionCounter) * width, 0, Mathf.Sin(circleMotionCounter) * height);
                obj.transform.position = rotationCenter + l;

                Quaternion targetL = Quaternion.LookRotation(l, Vector3.up);
                obj.transform.rotation = Quaternion.Lerp(obj.transform.rotation, targetL, Time.deltaTime * 8);
                break;

            case State.danger:
                circleMotionCounter += Time.deltaTime;
                l = new Vector3(Mathf.Cos(circleMotionCounter) * width, 0, Mathf.Sin(circleMotionCounter) * height);
                obj.transform.position = rotationCenter + l;

                targetL = Quaternion.LookRotation(l, Vector3.up);
                obj.transform.rotation = Quaternion.Lerp(obj.transform.rotation, targetL, Time.deltaTime * 8);

                if (area.getObjectsInside().Count == 0) {
                    stateTimer = 0;
                    state = State.returningFromDanger;
                    reachedBirdPos = obj.transform.position;
                }

                areaMesh.material.color = Color.Lerp(idleColor, dangerColor, stateTimer / dangerTime * 2f);
                stateTimer += Time.deltaTime;
                if (stateTimer >= dangerTime) {
                    state = State.attacking;
                    stateTimer = 0;
                    // Attacking cackaww~~
                }
                break;

            case State.attacking:
                areaMesh.material.color = Color.Lerp(dangerColor, attackingColor, stateTimer / attackingTime);
                stateTimer += Time.deltaTime;

                if (targetSheep == null && startingBirdPos == Vector3.zero) {
                    targetSheep = area.getRandomInside();
                    if (targetSheep == null) {
                        state = State.returningFromDanger;
                        stateTimer = 0;
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

                Quaternion targetAng = Quaternion.LookRotation(targetSheep.transform.position - startingBirdPos, Vector3.up);
                obj.transform.rotation = Quaternion.Lerp(obj.transform.rotation, targetAng, Time.deltaTime * 5);
                obj.transform.position = Vector3.Lerp(startingBirdPos, targetSheep.transform.position, stateTimer / attackingTime);


                areaObj.transform.position = Vector3.Lerp(startingAreaPos, targetSheep.transform.position - new Vector3(0, 0.25f, 0), stateTimer / attackingTime / 1.2f);
                areaObj.transform.localScale = Vector3.Lerp(startingAreaScale, new Vector3(0, 0, 0), stateTimer / attackingTime / 1.2f);

                break;

            case State.returningFromDanger:
                circleMotionCounter += Time.deltaTime;
                areaMesh.material.color = Color.Lerp(dangerColor, idleColor, stateTimer / returningTime);

                l = new Vector3(Mathf.Cos(circleMotionCounter) * width, 0, Mathf.Sin(circleMotionCounter) * height);

                stateTimer += Time.deltaTime;
                obj.transform.position = Vector3.Lerp(reachedBirdPos, rotationCenter + l, stateTimer / returningTime*2);

                targetL = Quaternion.LookRotation(l, Vector3.up);
                obj.transform.rotation = Quaternion.Lerp(obj.transform.rotation, targetL, Time.deltaTime * 8);

                if (stateTimer >= returningTime) {
                    state = State.idle;
                    stateTimer = 0;
                }
                break;

            case State.returningFromAttack:
                circleMotionCounter += Time.deltaTime;
                areaMesh.material.color = Color.Lerp(attackingColor, idleColor, stateTimer / returningTime);

                l = new Vector3(Mathf.Cos(circleMotionCounter) * width, 0, Mathf.Sin(circleMotionCounter) * height);

                stateTimer += Time.deltaTime;
                obj.transform.position = Vector3.Lerp(reachedBirdPos, rotationCenter + l, stateTimer / returningTime);
                areaObj.transform.position = Vector3.Lerp(reachedAreaPos, startingAreaPos, stateTimer / returningTime);
                areaObj.transform.localScale = Vector3.Lerp(reachedAreaScale, startingAreaScale, stateTimer / returningTime);

                targetL = Quaternion.LookRotation(l, Vector3.up);
                obj.transform.rotation = Quaternion.Lerp(obj.transform.rotation, targetL, Time.deltaTime * 8);

                if (stateTimer >= returningTime) {
                    state = State.idle;
                    stateTimer = 0;
                }
                break;

            default:
                break;
        }
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Entity" && other.GetComponent<Sheep>() != null && state == State.idle) {
            state = State.danger;
            stateTimer = 0;
            // First cackawww~~~ 
        }
    }
}
