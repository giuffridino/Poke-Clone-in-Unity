using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Singleton instance
    private static SoundManager instance;

    [SerializeField] AudioClip actionClick;
    private AudioSource audioSource;

    // Public reference to the instance (accessible from other scripts)
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                // If no instance exists, try to find one in the scene
                instance = FindObjectOfType<SoundManager>();

                // If still no instance, create a new GameObject and attach the SoundManager script
                if (instance == null)
                {
                    GameObject soundManagerObject = new GameObject("SoundManager");
                    instance = soundManagerObject.AddComponent<SoundManager>();
                }
            }

            return instance;
        }
    }

    // Other sound-related methods and variables can be added here

    private void Awake()
    {
        // Ensure there is only one instance of the SoundManager
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // Example method for playing a sound
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void PlayClick()
    {
        if (actionClick != null)
        {
            audioSource.PlayOneShot(actionClick);
        }
    }
}

