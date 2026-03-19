using System;
using UnityEngine;

public class PlayerMoney : MonoBehaviour
{
    [SerializeField] private int startMoney = 100;

    private int currentMoney;

    public int CurrentMoney => currentMoney;

    public event Action<int> OnMoneyChanged;

    private void Awake()
    {
        currentMoney = startMoney;
        OnMoneyChanged?.Invoke(currentMoney);
    }

    public bool CanSpend(int amount)
    {
        return currentMoney >= amount;
    }

    public bool TrySpend(int amount)
    {
        if (amount < 0)
            return false;

        if (currentMoney < amount)
            return false;

        currentMoney -= amount;
        OnMoneyChanged?.Invoke(currentMoney);
        return true;
    }

    public void AddMoney(int amount)
    {
        if (amount <= 0)
            return;

        currentMoney += amount;
        OnMoneyChanged?.Invoke(currentMoney);
    }
}