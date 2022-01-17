using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField]
    List<TextMeshProUGUI> blueBubblesText;
    [SerializeField]
    List<TextMeshProUGUI> redBubblesText;
    [SerializeField]
    List<GameObject> blueBubbles;
    [SerializeField]
    List<GameObject> redBubbles;
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
    TextMeshProUGUI playerWins;
    [SerializeField]
    int mandatoryPoints;
    [SerializeField]
    float baseWidth;

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

    public void ChangePoints(int player, float points)
    {
        float pourcentageOfMaxPoints = points / (float)mandatoryPoints;
        float newWidth = pourcentageOfMaxPoints * baseWidth;
        Rect rect = player == 1 ? blueBarre.rect : redBarre.rect;
        if (player == 1)
        {
            blueBarre.sizeDelta = new Vector2(newWidth, rect.height);
            blueBarre.localPosition = new Vector2(newWidth / 2 + baseWidth * (player == 1 ? -1 : 1), redBarre.localPosition.y);
        }
        else {
            redBarre.sizeDelta = new Vector2(newWidth, rect.height);
            redBarre.localPosition = new Vector2(-newWidth / 2 + baseWidth * (player == 1 ? -1 : 1), redBarre.localPosition.y);
        }

    }

    public void MakeBubbleAppear(int player, float points)
    {
        int rd = Random.Range(0, 3);
        TextMeshProUGUI text = player == 1 ? blueBubblesText[rd] : redBubblesText[rd];
        GameObject go = player == 1 ? blueBubbles[rd] : redBubbles[rd];
        text.SetText("+" + points);
        go.transform.localScale *= 1 + ((points - 1) / 10);
        go.SetActive(true);
        StopCoroutine("MakeBubbleDisappear");
        StartCoroutine(MakeBubbleDisappear(go.GetComponent<Image>(), text));
    }

    IEnumerator MakeBubbleDisappear(Image image, TMP_Text text)
    {
        for (int i = 0; i < 50; i++)
        {
            Color c = new Color(image.color.r, image.color.g, image.color.b, 255 - i * 5f);
            image.color = c;
            text.color = c;
            yield return null;
        }
        image.gameObject.SetActive(false);
        image.color = new Color(image.color.r, image.color.g, image.color.b, 255);
        text.color = new Color(image.color.r, image.color.g, image.color.b, 255);
    }
}
