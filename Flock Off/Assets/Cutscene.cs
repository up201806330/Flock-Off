using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    BGSoundScript soundtrack;

    CanvasGroup[] images;
    int index = 2;

    private void Awake() {
        images = GetComponentsInChildren<CanvasGroup>();
        soundtrack = GetDontDestroyOnLoadObjects()[0].GetComponent<BGSoundScript>();
        Debug.Log(soundtrack);
        soundtrack.fade();
        StartCoroutine(cutscene());
    }

    IEnumerator cutscene() {                 // Starts with black
        index++; StartCoroutine(FadeNext()); // First panel 2
        yield return new WaitForSeconds(5f);
        index++; StartCoroutine(FadeNext()); // Second panel 3
        yield return new WaitForSeconds(5f);
        index++; StartCoroutine(FadeNext()); // Third panel 4
        yield return new WaitForSeconds(5f);
        index++; StartCoroutine(FadeNext()); // Last panel 5
        yield return new WaitForSeconds(4f);
        index++; StartCoroutine(FadeNext()); // Black 6
        yield return new WaitForSeconds(3f);
        index++; StartCoroutine(FadeNext()); // Credits 7
    }

    IEnumerator FadeNext() {
        CanvasGroup previous = images[index - 1];
        CanvasGroup current = images[index];
        while (previous.alpha > 0 && current.alpha < 1) {
            if (index != 8 && index != 2) previous.alpha -= Time.deltaTime;
            current.alpha += Time.deltaTime;
            yield return new WaitForSeconds(0.02f);
        }
    }

    public static GameObject[] GetDontDestroyOnLoadObjects() {
        GameObject temp = null;
        try {
            temp = new GameObject();
            Object.DontDestroyOnLoad(temp);
            UnityEngine.SceneManagement.Scene dontDestroyOnLoad = temp.scene;
            Object.DestroyImmediate(temp);
            temp = null;

            return dontDestroyOnLoad.GetRootGameObjects();
        }
        finally {
            if (temp != null)
                Object.DestroyImmediate(temp);
        }
    }
}
