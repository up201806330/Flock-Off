using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animatorEvent : MonoBehaviour
{
    [SerializeField]
    Player player;

    public void resetShout() {
        player.resetShout();
    }
}
