using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UIController : MonoBehaviour
{
    public static UIController Instance;

    public TextMeshProUGUI PlayerMana;
    public TextMeshProUGUI EnemyMana;
    public TextMeshProUGUI PlayerHP;
    public TextMeshProUGUI EnemyHP;

    public GameObject ResultGO;
    public TextMeshProUGUI ResultTxt;

    public TextMeshProUGUI TurnTime;
    public Button EndTurnButton;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);
    }
    public void StartGame()
    {
        EndTurnButton.interactable = true;
        ResultGO.SetActive(false);
        UpdateHPAndMana();
    }
    public void UpdateHPAndMana()
    {
        PlayerMana.text = GameManagerScript.Instance.CurrentGame.Player.Mana.ToString();
        EnemyMana.text = GameManagerScript.Instance.CurrentGame.Enemy.Mana.ToString();
        PlayerHP.text = GameManagerScript.Instance.CurrentGame.Player.HP.ToString();
        EnemyHP.text = GameManagerScript.Instance.CurrentGame.Enemy.HP.ToString();
    }
    public void ShowResult()
    {
        ResultGO.SetActive(true);
        if (GameManagerScript.Instance.CurrentGame.Enemy.HP == 0)
            ResultTxt.text = "YOU WIN";
        else
            ResultTxt.text = "YOU LOSE, LOSER";
    }
    public void UpdateTurnTime(int time)
    {
        TurnTime.text = time.ToString();
    }
    public void DisableTurnButton()
    {
        EndTurnButton.interactable = GameManagerScript.Instance.isPlayerTurn;
    }
}
