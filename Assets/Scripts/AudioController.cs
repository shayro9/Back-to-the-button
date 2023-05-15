using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [System.Serializable]
    public struct Effect
    {
        public string name;
        public AudioClip clip;
    }
    public Effect[] effects;
    Dictionary<string, AudioClip> effects_dic;
    AudioSource source;
    bool playing;

    static AudioController instance;

    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this; // In first scene, make us the singleton.
            DontDestroyOnLoad(gameObject.transform);
        }
        else if (instance != this)
            Destroy(gameObject); // On reload, singleton already set, so destroy duplicate.

        source = GetComponent<AudioSource>();
        effects_dic = new Dictionary<string, AudioClip>();
        foreach(Effect e in effects)
        {
            effects_dic.Add(e.name, e.clip);
        }
    }
    private void Update()
    {
        if (playing)
            playing = false;
    }
    public void PlayEffect(string name)
    {
        if (!playing)
        {
            playing = true;
            source.PlayOneShot(effects_dic[name]);
        }
    }
}
