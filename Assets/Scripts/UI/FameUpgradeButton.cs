using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FameUpgradeButton : MonoBehaviour
{
    public TMP_Text label;
    public Button button;

    [Header("Requirement uses Fame (not spent)")]
    public double baseRequirementFame = 50;
    public double requirementMultiplier = 1.3;
    public double amountPerLevel = 0.1;
    private int level;

    void Reset()
    {
        button = GetComponent<Button>();
        if (label == null) label = GetComponentInChildren<TMP_Text>();
    }

    double Requirement() => baseRequirementFame * System.Math.Pow(requirementMultiplier, level);

    void Update()
    {
        var cm = CurrencyManager.I;
        double req = Requirement();
        if (label) label.text = $"Fandom Buzz Lv {level}\nRequires: {req:0} Fame";
        if (button) button.interactable = cm.Fame >= req;
    }

    public void OnClick()
    {
        var cm = CurrencyManager.I;
        double req = Requirement();
        if (cm.Fame >= req)
        {
            level++;
            cm.famePerSec += amountPerLevel;
        }
    }
}
