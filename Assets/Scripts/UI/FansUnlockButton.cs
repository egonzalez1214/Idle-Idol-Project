using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FansUnlockButton : MonoBehaviour
{
    public TMP_Text label;
    public Button button;

    [Header("Unlock (cost in Fame)")]
    public double unlockCostFame = 200;

    [Header("Post-unlock upgrades (cost in Cash")]
    public double baseCostCash = 50;
    public double costMultiplier = 1.3;
    public double amountPerLevel = 0.1;
    private int level;

    void Reset()
    {
        button = GetComponent<Button>();
        if (label == null)
        {
            label = GetComponentInChildren<TMP_Text>();
        }
    }

    void Update()
    {
        var cm = CurrencyManager.I;
        if (!cm.fansUnlocked)
        {
            if (label)
            {
                label.text = $"Unlock Fans\nRequires: {unlockCostFame:0} Fame";
            }
            if (button)
            {
                button.interactable = cm.Fame >= unlockCostFame;
            }
        }
        else
        {
            double cost = baseCostCash * System.Math.Pow(costMultiplier, level);
            if (label)
            {
                label.text = $"Street Teams Lv {level}\n Cost: {cost:0}";
            }
            if (button)
            {
                button.interactable = cm.Cash >= cost;
            }
        }
    }
    public void OnClick()
    {
        var cm = CurrencyManager.I;
        if (!cm.fansUnlocked)
        {
            if (cm.Fame >= unlockCostFame)
            {
                cm.fansUnlocked = true;
            }
            return;
        }
        double cost = baseCostCash * System.Math.Pow(costMultiplier, level);
        if (cm.TrySpendCash(cost))
        {
            level++;
            cm.fansPerSec *= (2.0 + amountPerLevel);
        }
    }
}
