using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get => _instance; private set { _instance = value; } }

    public int winner = 0;

    public string characterPlayer1;
    public string characterPlayer2;

    private Character character1;
    private Character character2;
    public Character Character1 { get => character1; private set { character1 = value; } }
    public Character Character2 { get => character2; private set { character2 = value; } }
    public GameManager(){}

    private void Awake()
    {
        Application.targetFrameRate = 60;
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        Instance = this;
        print(Instance);
        DontDestroyOnLoad(this.gameObject);
        
        //FOR DEBUGGING PURPOSES
    }

    private void Start()
    {
        LoadScene("MenuScene");
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    private void OnLevelWasLoaded(int level)
    {        
        if (level == SceneManager.GetSceneByName("FightScene").buildIndex)
        {         
            character1 = (Character)Resources.Load("Characters/" + characterPlayer1);
            character2 = (Character)Resources.Load("Characters/" + characterPlayer2);
            GameObject.FindGameObjectWithTag("Player1").GetComponent<SpriteRenderer>().sprite = character1.spriteNormal;
            GameObject.FindGameObjectWithTag("Player2").GetComponent<SpriteRenderer>().sprite = character2.spriteNormal;
            MusicPlayer.Instance.chooseInstruments(new List<Instruments>() { Instruments.Trumpet, Instruments.BassDrums, Instruments.Guitare, Instruments.Handclaps});
        }
        else
        {
            MusicPlayer.Instance.chooseInstruments(new List<Instruments>() { Instruments.BassDrums, Instruments.Guitare, Instruments.Handclaps, Instruments.Maracas, Instruments.Castanets });
        }
    }

    public void setCharacter(string myCharacter, int player)
    {
        if (player == 0)
        {
            characterPlayer1 = myCharacter;
        }
        else
        {
            characterPlayer2 = myCharacter;
        }
    }
}
