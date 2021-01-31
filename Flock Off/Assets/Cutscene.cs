using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    CanvasGroup[] images;
    int index = 1;

    private void Awake() {
        images = GetComponentsInChildren<CanvasGroup>();
        StartCoroutine(cutscene());
    }

    IEnumerator cutscene() {                 // Starts with black
        index++; StartCoroutine(FadeNext()); // First panel 2
        yield return new WaitForSeconds(7f);
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
            if (index != 7) previous.alpha -= Time.deltaTime;
            current.alpha += Time.deltaTime;
            yield return new WaitForSeconds(0.02f);
        }
    }
}
