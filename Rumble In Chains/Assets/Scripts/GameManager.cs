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
    public GameManager(){}

    private void Awake()
    {
        Application.targetFrameRate = 60;
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    
    public void LoadScene(string sceneName)
    {
        Debug.Log("Loading Scene : " + sceneName);
        SceneManager.LoadScene(sceneName);
        if(sceneName == "FightScene")
        {
            character1 = AssetDatabase.LoadAssetAtPath<Character>("Assets/Characters/" + characterPlayer1);
            character2 = AssetDatabase.LoadAssetAtPath<Character>("Assets/Characters/" + characterPlayer2);
            EmitDataFromCharacter();
        }
    }

    public void setCharacter(string myCharacter, int player)
    {
        if (player == 0) {
            characterPlayer1 = myCharacter;
        }
        else
        {
            characterPlayer2 = myCharacter;
        }
    }

    private void EmitDataFromCharacter()
    {
        //
    }
}