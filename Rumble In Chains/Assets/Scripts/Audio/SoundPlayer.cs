using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundPlayer : MonoBehaviour // Allows to choose which instruments to play from the main theme and to synchronize voices
{
    private static SoundPlayer _instance;
    public static SoundPlayer Instance { get => _instance; private set { _instance = value; } }
    public SoundPlayer(){}

    [SerializeField]
    private AudioClip[] audioClips;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private float[] audioVolumes;
    [SerializeField]
    private int[] numberOfClipsForThisSound;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject); 
    }

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlaySound(int numberOfTheSound)
    {

        print(audioSource);
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        print(audioSource);

        int zero = 0;
        for(int i = 0; i<numberOfTheSound; i++)
        {
            zero += numberOfClipsForThisSound[i];
        }
        int clipToPlay = Random.Range(zero, zero + numberOfClipsForThisSound[numberOfTheSound]);
        audioSource.volume = audioVolumes[clipToPlay];
        audioSource.PlayOneShot(audioClips[clipToPlay]);
    }

    //FOR DEBUGGING PURPOSES
    public void PlayClip(int number)
    {
        print(number);
        audioSource.PlayOneShot(audioClips[number]);
    }
}
