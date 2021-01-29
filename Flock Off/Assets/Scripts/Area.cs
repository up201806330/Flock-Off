using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour {
    public List<GameObject> objectsInsideArea = new List<GameObject>();

    public List<GameObject> getObjectsInside() { return objectsInsideArea; }
    public GameObject getRandomInside() { return objectsInsideArea.Count == 0 ? null : objectsInsideArea[Random.Range(0, objectsInsideArea.Count)]; }
    public bool isInside(GameObject x) { return objectsInsideArea.Contains(x); }
    public void add(Collider x) {
        if (!objectsInsideArea.Contains(x.gameObject) && x.gameObject.GetComponent<Sheep>() != null) {
            objectsInsideArea.Add(x.gameObject);
        }
    }
    private void OnTriggerEnter(Collider other) {
        add(other);
    }
    private void OnTriggerExit(Collider other) {
        objectsInsideArea.Remove(other.gameObject);
    }
}
