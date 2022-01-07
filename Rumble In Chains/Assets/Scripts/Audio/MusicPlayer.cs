using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour // Allows to choose which instruments to play from the main theme and to synchronize voices
{
    private MusicPlayer _instance;
    public MusicPlayer Instance { get => _instance; private set { _instance = value; } }
    public MusicPlayer(){}

    [SerializeField]
    private AudioSource[] audioSources; // The audiosource 0 will be used to synchronize all sounds
    [SerializeField]
    private float[] audioVolumes;
    

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject); 
    }

    public void chooseInstruments(List<Instruments> instruments)
    {
        List<int> allInstruments = new List<int> { 0, 1, 2, 3, 4, 5, 6 };
        for (int i = 0; i<instruments.Count; i++)
        {   
            int instrument = (int)instruments[i];
            allInstruments.Remove(instrument);
            var machin = audioVolumes[instrument];
            audioSources[instrument].volume = machin;
        }
        foreach(Instruments instrument in allInstruments)
        {
            audioSources[(int)instrument].volume = 0;
        }
    }
}
