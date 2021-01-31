using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orchestrator : MonoBehaviour
{
    Counter UICounter;

    int liveSheep = 0;
    int survivedSheep = 0;
    int deadSheep;

    int liveEagles = 0;
    int deadEagles;

    int liveBears = 0;

    public Rumbler rumbler;
    public LevelLoader levelLoader;

    private void Start() {
        for (int i = 0; i < transform.childCount; i++) {
            Transform x = transform.GetChild(i);
            if (x.GetComponent<Sheep>() != null) liveSheep++;
            if (x.GetComponent<Bird>() != null) liveEagles++;
            if (x.GetComponent<Bear>() != null) liveBears++;
            if (x.tag == "UI") UICounter = x.GetComponentInChildren<Counter>();
        }

        UICounter.setLeft(survivedSheep);
        UICounter.setRight(liveSheep);

        rumbler = GetComponent<Rumbler>();
        levelLoader = GetComponentInChildren<LevelLoader>();
    }

    public void markDead(GameObject x) {
        if (x.GetComponent<Sheep>() != null) { 
            liveSheep--; deadSheep++;
            UICounter.setRight(liveSheep);

            if (liveSheep == 0) levelLoader.reload();
            else if (liveSheep == survivedSheep) {
                StartCoroutine(nextLevel());
            }
        }
        if (x.GetComponent<Bird>() != null) { liveEagles--; deadEagles++; }
    }

    public void markSurvived(GameObject x) {
        if (x.GetComponent<Sheep>() != null) { 
            survivedSheep++;
            UICounter.setLeft(survivedSheep);
            if (liveSheep == survivedSheep) {
                StartCoroutine(nextLevel());
            }
        }
    }

    IEnumerator nextLevel() {
        yield return new WaitForSeconds(0.5f);
        // [SFX]
        yield return new WaitForSeconds(0.5f);
        levelLoader.nextLevel();
    }

}
