using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    Animator[] animators;
    [SerializeField]
    float duration;

    private void Awake() {
        animators = GetComponentsInChildren<Animator>();
    }

    public void reload() {
        StartCoroutine(loadIndex(SceneManager.GetActiveScene().buildIndex));
    }

    public void nextLevel() {
        StartCoroutine(loadIndex(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator loadIndex(int index) {
        foreach(Animator animator in animators) animator.SetTrigger("Start");

        yield return new WaitForSeconds(duration);

        SceneManager.LoadScene(index);
    }
}
