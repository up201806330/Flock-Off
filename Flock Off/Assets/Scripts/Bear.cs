using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear : MonoBehaviour
{
    AudioSource audio;

    [SerializeField]
    AudioClip[] sounds;

    Animator animator;
    int attackingHsh = Animator.StringToHash("attacking");

    GameObject obj;
    GameObject targetSheep;

    [Header("Stats")]
    [SerializeField]
    float cooldownTime;
    float counter = 0;

    private void Awake() {
        obj = transform.GetChild(0).gameObject;
        animator = obj.GetComponent<Animator>();

        audio = GetComponent<AudioSource>();
    }

    private void Update() {
        if (counter > 0) counter -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other) {

        int nSound = UnityEngine.Random.Range(0, 2);
        audio.clip = sounds[nSound];
        audio.Play();

        if (other.tag == "Entity" && other.GetComponent<Sheep>() != null) {
            if (counter <= 0 && !animator.GetBool(attackingHsh)) {
                counter = cooldownTime;
                animator.SetBool(attackingHsh, true);
                targetSheep = other.gameObject;
            }
        }
    }

    public void resetAttacking() {
        animator.SetBool(attackingHsh, false);
        Destroy(targetSheep);
    }

    public void grabSheep() {
        targetSheep.transform.SetParent(obj.transform.GetChild(0));
        targetSheep.GetComponent<Sheep>().kill(false);
        audio.clip = sounds[2];
        audio.Play();
    }
}
