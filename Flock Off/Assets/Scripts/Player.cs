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
    GameObject shout;
    Renderer rnd;
    [SerializeField]
    float cooldown = 0f;
    [SerializeField]
    float cooldownAmount;
    [SerializeField]
    float factor;

    [Header("Movement")]
    [SerializeField]
    float moveSpeed = 1f;
    [SerializeField]
    float rotSpeed = 1f;

    private void Awake() {
        rnd = shout.GetComponentInChildren<Renderer>();

        controls = new PlayerControls();
        animator = GetComponentInChildren<Animator>();

        controls.Gameplay.Shout.performed += ctx => Shout();
        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;
    }

    private void Update() {
        if (cooldown > 0) cooldown -= Time.deltaTime;
        else cooldown = 0;

        if (rnd.material.color.a >= 0) {
            Color newC = new Color(rnd.material.color.r, rnd.material.color.g, rnd.material.color.b, rnd.material.color.a - Time.deltaTime * factor);
            rnd.material.color = newC;
        }

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
            rnd.material.color = new Color(rnd.material.color.r, rnd.material.color.g, rnd.material.color.b, 0.5f);
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
