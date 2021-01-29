using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour {
    List<GameObject> objectsInsideArea = new List<GameObject>();

    public List<GameObject> getObjectsInside() { return objectsInsideArea; }
    public GameObject getRandomInside() { return objectsInsideArea.Count == 0 ? null : objectsInsideArea[Random.Range(0, objectsInsideArea.Count)]; }
    public bool isInside(GameObject x) { return objectsInsideArea.Contains(x); }
    private void OnTriggerEnter(Collider other) {
        if(!objectsInsideArea.Contains(other.gameObject)) { objectsInsideArea.Add(other.gameObject); }
    }
    private void OnTriggerExit(Collider other) {
        objectsInsideArea.Remove(other.gameObject);
    }
}
