using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrestigeBinder : MonoBehaviour {
    public Button button;
    public TMP_Text buttonLabel;
    public TMP_Text infoLabel;

    void Reset() {
        button = GetComponent<Button>();
        if (!buttonLabel) buttonLabel = GetComponentInChildren<TMP_Text>();
    }

    void Update() {
        var cm = CurrencyManager.I;
        var pm = PrestigeManager.I;
        if (!cm || !pm) return;

        bool maxed = pm.IsMaxed;
        bool can = pm.CanPrestige(cm.Fame);

        if (button) button.interactable = can && !maxed;
        if (buttonLabel) buttonLabel.text = maxed ? "MAX" : "PRESTIGE";

        if (infoLabel) {
            int p = pm.prestigeLevel;

            if (maxed) {
                infoLabel.textWrappingMode = TextWrappingModes.NoWrap;
                infoLabel.text = $"P {p}";
            }
            else if (can) {
                infoLabel.textWrappingMode = TextWrappingModes.NoWrap;
                string next = $"{pm.NextGainPreview():0.##}x";
                infoLabel.text = $"P {p} • Next {next}";
            }
            else {
                infoLabel.textWrappingMode = TextWrappingModes.NoWrap;
                string req = NumberFormat.Abbr(pm.CurrentRequirement());
                infoLabel.text = $"Req {req} • P {p}";
            }
        }
    }

    public void OnClick() {
        PrestigeManager.I.DoPrestige();
    }
}
