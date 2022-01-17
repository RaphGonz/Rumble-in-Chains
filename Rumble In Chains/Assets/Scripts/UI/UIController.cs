using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI player1Character;
    [SerializeField]
    TextMeshProUGUI player2Character;
    [SerializeField]
    TextMeshProUGUI player1Percentages;
    [SerializeField]
    TextMeshProUGUI player2Percentages;
    [SerializeField]
    RectTransform redBarre;
    [SerializeField]
    RectTransform blueBarre;
    [SerializeField]
    GameObject gameOver;
    [SerializeField]
    int mandatoryPoints;
    [SerializeField]
    float baseWidth;

    bool won = false;

    static UIController _instance;
    public static UIController Instance { get => _instance; private set { _instance = value; } }
    UIController() { }

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
        player1Character.SetText(GameManager.Instance.characterPlayer1);
        player2Character.SetText(GameManager.Instance.characterPlayer2);
    }
    // Start is called before the first frame update

    public void ChangePercentages(int player, float percentages)
    {
        TextMeshProUGUI text = player == 1 ? player1Percentages : player2Percentages;
        text.text = percentages.ToString() + "%";
    }

    public void ChangePoints(int player, int points)
    {
        if(points > mandatoryPoints)
        {
            Win(player);
        }
        else
        {
            float pourcentageOfMaxPoints = (float)points / (float)mandatoryPoints;
            float newWidth = pourcentageOfMaxPoints * baseWidth;
            Rect rect = player == 1 ? blueBarre.rect : redBarre.rect;
            Debug.Log(baseWidth / 2 - newWidth / 2 * (player == 1 ? -1 : 1));
            Debug.Log(newWidth);
            if(player == 1)
            {
                blueBarre.sizeDelta = new Vector2(newWidth, rect.height); 
                blueBarre.localPosition = new Vector2(newWidth / 2 + baseWidth * (player == 1 ? -1 : 1), redBarre.localPosition.y) ; 
            }
            else {
                redBarre.sizeDelta = new Vector2(newWidth, rect.height);
                redBarre.localPosition = new Vector2(-newWidth / 2 + baseWidth * (player == 1 ? -1 : 1), redBarre.localPosition.y);
            }
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
    }

    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(2);
        GameManager.Instance.LoadScene("WinnerScene");
    }

    
    
}
