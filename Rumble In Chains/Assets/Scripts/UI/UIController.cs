using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI player1Percentages;
    [SerializeField]
    TextMeshProUGUI player2Percentages;
    [SerializeField]
    TextMeshProUGUI player1Points;
    [SerializeField]
    TextMeshProUGUI player2Points;
    [SerializeField]
    TextMeshProUGUI playerWins;
    [SerializeField]
    int mandatoryPoints;

    private static UIController instance;
    private UIController() { } //au cas où certains fous tenteraient qd même d'utiliser le mot clé "new"

    // Méthode d'accès statique (point d'accès global)
    public static UIController Instance { get => instance; set { instance = value; } }

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);    // Suppression d'une instance précédente potentielle

        Instance = this;
    }
    // Start is called before the first frame update

    public void ChangePercentages(int player, float percentages)
    {
        TextMeshProUGUI text = player == 1 ? player1Percentages : player2Percentages;
        text.text = "Player " + (player == 1 ? "1" : "2") + " : " + percentages + "%";
    }

    public void ChangePoints(int player, int points)
    {
        TextMeshProUGUI text = player == 1 ? player1Points : player2Points;
        text.text = points + " points";
        if(points >= mandatoryPoints)
        {
            playerWins.text = "Player " + player + " wins !";
            playerWins.transform.parent.gameObject.SetActive(true);
        }
    }
}
