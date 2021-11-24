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


    private static UIController instance;
    private UIController() { } //au cas o� certains fous tenteraient qd m�me d'utiliser le mot cl� "new"

    // M�thode d'acc�s statique (point d'acc�s global)
    public static UIController Instance { get => instance; set { instance = value; } }

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);    // Suppression d'une instance pr�c�dente potentielle

        Instance = this;
    }
    // Start is called before the first frame update

    public void ChangePercentages(int player, float percentages)
    {
        TextMeshProUGUI text = player == 1 ? player1Percentages : player2Percentages;
        text.text = "Player " + (player == 1 ? "1" : "2") + " : " + percentages + "%";
    }
}
