using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGSoundScript : MonoBehaviour
{
    AudioSource[] sources;
    [SerializeField]
    float factor;
    float maxVol;

    // Play Global
    private static BGSoundScript instance = null;
    public static BGSoundScript Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        sources = GetComponents<AudioSource>();
        maxVol = sources[1].volume;
        sources[1].volume = 0;
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public void fade() {
        Debug.Log("HEr");
        StartCoroutine(fadeC());
    }

    IEnumerator fadeC() {
        sources[1].Play();
        sources[0].volume -= Time.deltaTime / factor;
        if (sources[1].volume <= maxVol) sources[1].volume -= Time.deltaTime / factor;
        yield return new WaitForSeconds(0.1f);
    }
}
