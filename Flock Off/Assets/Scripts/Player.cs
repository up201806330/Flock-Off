using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations;

public class Player : MonoBehaviour
{
    Orchestrator orchestrator;

    AudioSource audio;

    [SerializeField]
    AudioClip[] sounds;

    PlayerControls controls;
    float horizontal;
    float vertical;

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

    float shoutDuration;
    float counter = 0;
    public float shoutRange;

    bool restarting = false;
    float restartCounter = 0;

    private void Awake() {
        orchestrator = GetComponentInParent<Orchestrator>();

        controls = new PlayerControls();
        animator = GetComponentInChildren<Animator>();

        controls.Gameplay.Shout.performed += ctx => Shout();
        controls.Gameplay.Move.performed += ctx => { Vector2 move = ctx.ReadValue<Vector2>(); horizontal = move.x; vertical = move.y; };
        controls.Gameplay.Move.canceled += ctx => { horizontal = 0; vertical = 0; };

        controls.Gameplay.MoveN.performed += ctx => vertical = 1;
        controls.Gameplay.MoveN.canceled += ctx => vertical = 0;
        controls.Gameplay.MoveS.performed += ctx => vertical = -1;
        controls.Gameplay.MoveS.canceled += ctx => vertical = 0;
        controls.Gameplay.MoveW.performed += ctx => horizontal = 1;
        controls.Gameplay.MoveW.canceled += ctx => horizontal = 0;
        controls.Gameplay.MoveE.performed += ctx => horizontal = -1;
        controls.Gameplay.MoveE.canceled += ctx => horizontal = 0;

        controls.Gameplay.Restart.performed += ctx => restarting = true;
        controls.Gameplay.Restart.canceled += ctx => restarting = false;

        audio = GetComponent<AudioSource>();
    }

    private void Update() {
        if (restarting) {
            if (restartCounter < 1f) restartCounter += Time.deltaTime;
            else {
                restartCounter = 0;
                orchestrator.levelLoader.reload();
            }
        }
        else if (restartCounter != 0) restartCounter = 0;

        if (counter >= 0 && shoutRange != 0) counter -= Time.deltaTime;
        else if (shoutRange != 0) shoutRange = 0;

        if (transform.position.y < -5f) {
            orchestrator.levelLoader.reload();
        }

        if (cooldown > 0) cooldown -= Time.deltaTime;
        else cooldown = 0;

        if (horizontal == 0 && vertical == 0) {
            animator.SetBool(walkingHsh, false);
            return;
        }
        else animator.SetBool(walkingHsh, true);
        Vector3 l = new Vector3(-horizontal, 0, -vertical);
        transform.Translate(l * Time.deltaTime * moveSpeed, Space.World);

        Quaternion targetL = Quaternion.LookRotation(l, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetL, Time.deltaTime * rotSpeed);
    }

    void Shout() {
        if (cooldown == 0) {
            animator.SetBool(shoutHsh, true);
            CameraShake.Shake(shakeTime, shakeAmount);
            orchestrator.rumbler.RumblePulse(0.5f, 0.5f, 0.25f, 0.5f);
            shoutDuration = 0.2f;
            shoutRange = shoutRadius;
            cooldown = cooldownAmount;

            int nSound = Random.Range(0, 4);
            audio.clip = sounds[nSound];
            audio.Play();
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
