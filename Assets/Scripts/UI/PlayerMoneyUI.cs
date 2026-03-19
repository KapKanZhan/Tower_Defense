using TMPro;
using UnityEngine;

public class PlayerMoneyUI : MonoBehaviour
{
    [SerializeField] private PlayerMoney playerMoney;
    [SerializeField] private TMP_Text moneyText;

    private void Start()
    {
        if (playerMoney == null)
            return;

        playerMoney.OnMoneyChanged += UpdateMoneyText;
        UpdateMoneyText(playerMoney.CurrentMoney);
    }

    private void OnDestroy()
    {
        if (playerMoney == null)
            return;

        playerMoney.OnMoneyChanged -= UpdateMoneyText;
    }

    private void UpdateMoneyText(int money)
    {
        moneyText.text = $"{money}";
    }
}