using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBinder: MonoBehaviour
{
    [Header("HUD")]
    public TMP_Text fameText;
    public TMP_Text fansText;
    public TMP_Text cashText;

    [Header("Upgrades")]
    public UpgradeService service;
    public Button[] upgradeButtons;
    public TMP_Text[] upgradeLabels;

    private void Update()
    {
        var cm = CurrencyManager.I;
        fameText.text = $"Fame: {cm.Fame:0}";
        fansText.text = $"Fans: {cm.Fans:0}";
        cashText.text = $"Cash: ${cm.Cash:0}";

        for (int i = 0; i < service.upgrades.Length && i < upgradeLabels.Length; i++)
        {
            var u = service.upgrades[i];
            double cost = service.CurrentCost(u);
            upgradeLabels[i].text = $"{u.displayName} Lv {u.level}\nCost: {cost:0}";
            if (i < upgradeButtons.Length)
            {
                upgradeButtons[i].interactable = cm.Cash >= cost;
            }
        }
    }

    public void OnBuyUpgrade(int index)
    {
        if (index < 0 || index >= service.upgrades.Length)
        {
            return;
        }
        service.TryBuy(service.upgrades[index]);
    }
}
