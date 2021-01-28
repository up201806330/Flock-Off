using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations;

public class Player : MonoBehaviour
{
    PlayerControls controls;
    Vector2 move;

    Animator animator;
    int walkingHsh = Animator.StringToHash("walking");
    int shoutHsh = Animator.StringToHash("shout");

    [Header("Shout")]
    [SerializeField]
    float cooldown = 0f;
    [SerializeField]
    float cooldownAmount;
    [SerializeField]
    float factor;
    [SerializeField]
    float shakeTime; 
    [SerializeField]
    float shakeAmount;
    [SerializeField]
    float shoutRadius;

    [Header("Movement")]
    [SerializeField]
    float moveSpeed;
    [SerializeField]
    float rotSpeed = 1f;

    private void Awake() {
        controls = new PlayerControls();
        animator = GetComponentInChildren<Animator>();

        controls.Gameplay.Shout.performed += ctx => Shout();
        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;
    }

    private void Update() {
        if (cooldown > 0) cooldown -= Time.deltaTime;
        else cooldown = 0;

        if (move == Vector2.zero) {
            animator.SetBool(walkingHsh, false);
            return;
        }
        else animator.SetBool(walkingHsh, true);
        Vector3 m = new Vector3(-move.x, 0, -move.y) * Time.deltaTime * moveSpeed;
        transform.Translate(m, Space.World);

        Vector3 l = new Vector3(-move.x, 0, -move.y);
        Quaternion targetL = Quaternion.LookRotation(l, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetL, Time.deltaTime * rotSpeed);
    }

    void Shout() {
        if (cooldown == 0) {
            animator.SetBool(shoutHsh, true);
            CameraShake.Shake(shakeTime, shakeAmount);
            //foreach(Collider x in Physics.OverlapSphere(transform.position, shoutRadius)) {
            //    if (x.gameObject.tag == "Entity") x.GetComponent<Rigidbody>().AddForce(x.transform.position - transform.position, ForceMode.Impulse);
            //}
            cooldown = cooldownAmount;
        }
    }

    public void resetShout() {
        animator.SetBool(shoutHsh, false);
    }

    private void OnEnable() {
        controls.Gameplay.Enable();
    }

    private void OnDisable() {
        controls.Gameplay.Disable();
    }
}
