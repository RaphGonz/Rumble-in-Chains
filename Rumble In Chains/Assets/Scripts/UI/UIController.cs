using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class UIController : MonoBehaviour
{
    [SerializeField]
    Image char1Image;
    [SerializeField]
    Image char2Image;
    [SerializeField]
    TextMeshProUGUI player1Character;
    [SerializeField]
    TextMeshProUGUI player2Character;
    [SerializeField]
    TextMeshProUGUI player1Percentages;
    [SerializeField]
    TextMeshProUGUI player2Percentages;
    [SerializeField]
    Slider redBarre;
    [SerializeField]
    Slider blueBarre;
    [SerializeField]
    GameObject gameOver;
    [SerializeField]
    int mandatoryPoints;
    [SerializeField]
    float baseWidth;

    float points1;
    float points2;

    bool won = false;

    static UIController _instance;
    public static UIController Instance { get => _instance; private set { _instance = value; } }
    UIController() { }

    List<Instruments> instrumentsQuarter;
    List<Instruments> instrumentsHalf;
    List<Instruments> instrumentsQuarterTo;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        Instance = this;
    }

    void Start()
    {
        instrumentsQuarter = new List<Instruments>() { Instruments.Trumpet, Instruments.BassDrums, Instruments.Guitare, Instruments.Handclaps, Instruments.Maracas };
        instrumentsHalf = new List<Instruments>() { Instruments.Trumpet, Instruments.BassDrums, Instruments.Guitare, Instruments.Handclaps, Instruments.Maracas, Instruments.Trombone};
        instrumentsQuarterTo = new List<Instruments>() { Instruments.BassDrums, Instruments.Castanets, Instruments.Guitare,Instruments.Handclaps, Instruments.Maracas, Instruments.Trombone, Instruments.Trumpet};

        player1Character.SetText(GameManager.Instance.characterPlayer1);
        player2Character.SetText(GameManager.Instance.characterPlayer2);
        print(char1Image);
        print((Sprite)AssetDatabase.LoadAssetAtPath("Assets/Graphics/Perso/El Jaguar/El-Jaguar_Open_Boder.png", typeof(Sprite)));

        switch (GameManager.Instance.characterPlayer1)
        {
            case "El Jaguar": char1Image.sprite = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Graphics/Perso/El Jaguar/El-Jaguar_Open_Boder.png", typeof(Sprite)); break;
            case "José de la Muerte": char1Image.sprite = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Graphics/Perso/Jose de la Muerte/Jose-de-la-Muerte_Open_Border.png", typeof(Sprite)); break;
            case "Vaquero Del Paso": char1Image.sprite = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Graphics/Perso/Vaquero Del Paso/Vaquero-Del-Paso_Open_Border.png", typeof(Sprite)); break;
        }
        switch (GameManager.Instance.characterPlayer2)
        {
            case "El Jaguar": char2Image.sprite = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Graphics/Perso/El Jaguar/El-Jaguar_Open_Boder.png", typeof(Sprite)); break;
            case "José de la Muerte": char2Image.sprite = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Graphics/Perso/Jose de la Muerte/Jose-de-la-Muerte_Open_Border.png", typeof(Sprite)); break;
            case "Vaquero Del Paso": char2Image.sprite = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Graphics/Perso/Vaquero Del Paso/Vaquero-Del-Paso_Open_Border.png", typeof(Sprite)); break;
        }

        //blueBarre.maxValue = mandatoryPoints;
        redBarre.maxValue = mandatoryPoints;
    }
    // Start is called before the first frame update

    public void ChangePercentages(int player, float percentages)
    {
        TextMeshProUGUI text = player == 1 ? player1Percentages : player2Percentages;
        text.text = percentages.ToString() + "%";
    }

    public void ChangePoints(int player, float points)
    {
        if(points > mandatoryPoints)
        {
            Win(player);
        }
        else
        {
            float pourcentageOfMaxPoints = points / (float)mandatoryPoints;
            float newWidth = pourcentageOfMaxPoints * baseWidth;
            
            if(player == 1)
            {
                blueBarre.value += points;
                points1 = points;
            }
            else {
                redBarre.value += points;
                points2 = points;
            }
        }
        if (Mathf.Max(points1, points2) > 3 * mandatoryPoints / 4)
        {
            MusicPlayer.Instance.chooseInstruments(instrumentsQuarter);
        }
        else if (Mathf.Max(points1, points2) > mandatoryPoints / 2)
        {
            MusicPlayer.Instance.chooseInstruments(instrumentsHalf);
        }
        else if (Mathf.Max(points1, points2) > mandatoryPoints / 4)
        {
            MusicPlayer.Instance.chooseInstruments(instrumentsQuarterTo);
        }
    }
    public void Win(int player)
    {
        if (!won)
        {
            gameOver.SetActive(true);
            won = true;
            GameManager.Instance.winner = player;
            StartCoroutine(ChangeScene());
        }
        SoundPlayer.Instance.PlaySound(5);
    }

    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(2);
        GameManager.Instance.LoadScene("WinnerScene");
    }

    
    
}
