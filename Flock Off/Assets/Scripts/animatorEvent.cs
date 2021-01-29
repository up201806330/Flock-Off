using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animatorEvent : MonoBehaviour
{
    Player player;
    Bear bear;

    private void Awake() {
        player = transform.parent.gameObject.GetComponent<Player>();
        bear = transform.parent.gameObject.GetComponent<Bear>();
    }

    public void resetShout() {
        if (player != null) player.resetShout();
    }

    public void resetAttacking() {
        if (bear != null) bear.resetAttacking();
    }

    public void grabSheep() {
        if (bear != null) bear.grabSheep();
    }
}
