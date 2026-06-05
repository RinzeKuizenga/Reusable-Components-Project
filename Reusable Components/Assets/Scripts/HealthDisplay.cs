using TMPro;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public enum TargetPlayer
{
    player1,
    player2, 
}

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    //[SerializeField] private TextMeshProUGUI staminaText;
    public Player player;
    [SerializeField] private TargetPlayer targetPlayer;

    void Start()
    {
        if(targetPlayer == TargetPlayer.player1) player = GameObject.FindWithTag("Player1").GetComponent<Player>();

        else if (targetPlayer == TargetPlayer.player2) player = GameObject.FindWithTag("Player2").GetComponent<Player>();

        player.GetComponent<Player>();

        UpdateHealth();

        player.OnDamageTaken += UpdateHealth;
    }

    public void UpdateHealth()
    {
        //healthText.text = $"{player.health.ToString()}/ {player.maxHealth.ToString()}";
        healthText.text = $"{player.health.ToString()}";
        //staminaText.text = player1.stamina.ToString();
    }

}
