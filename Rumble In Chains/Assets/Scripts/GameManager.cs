using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get => _instance; private set { _instance = value; } }

    public string characterPlayer1;
    public string characterPlayer2;

    private Character character1;
    private Character character2;
    public GameManager(){}

    private void Awake()
    {
        if(Instance != null && Instance != this)
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
        Debug.Log("characterP1 : " + characterPlayer1.ToString());
        Debug.Log("characterP2 : " + characterPlayer2.ToString());
    }

    private void EmitDataFromCharacter()
    {
        //
    }
}
