using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Counter : MonoBehaviour {
    [SerializeField]
    TextMeshProUGUI leftNumber;
    [SerializeField]
    TextMeshProUGUI rightNumber;

    [Header("Colors")]
    [SerializeField]
    float duration;
    float leftCounter = 0;
    float rightCounter = 0;
    [SerializeField]
    Color idleColor;
    [SerializeField]
    Color leftActivationColor;
    [SerializeField]
    Color rightActivationColor;

    private void Awake() {
        leftNumber = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        rightNumber = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public void setLeft(int n) {
        string x;
        if (n < 0) x = " 0";
        else if (n < 10) x = " " + n;
        else x = n + "";
        leftCounter = duration;
        leftNumber.text = x;
    }

    public void setRight(int n) {
        string x;
        if (n < 0) x = " 0";
        else if (n < 10) x = " " + n;
        else x = n + "";
        rightCounter = duration;
        rightNumber.text = x;
    }

    private void Update() {
        if (leftCounter > 0) {
            leftCounter -= Time.deltaTime;
            leftNumber.color = Color.Lerp(idleColor, leftActivationColor, leftCounter / duration);
        }
        if (rightCounter > 0) {
            rightCounter -= Time.deltaTime;
            rightNumber.color = Color.Lerp(idleColor, rightActivationColor, rightCounter / duration);
        }
    }

}
