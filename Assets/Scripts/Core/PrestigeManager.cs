using UnityEngine;
using System;

public class PrestigeManager : MonoBehaviour {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
    [ContextMenu("DEBUG: Reset Prestige")]
    public void DebugResetPrestige() {
        prestigeLevel = 0;
        nextThresholdIndex = 0;
        ApplyPermanentMultiplier(CurrencyManager.I);
        PlayerPrefs.DeleteKey("prestige_level");
        PlayerPrefs.DeleteKey("prestige_next_idx");
        PlayerPrefs.Save();
    }
#endif

    public static PrestigeManager I { get; private set; }
    void Awake() { I = this; }

    [Header("Fame thresholds (unlock order)")]
    public double[] fameThresholds = new double[] { 100_000, 500_000, 1_000_000, 5_000_000 };

    [Header("Permanent boost per prestige")]
    [Tooltip("0.5 = +50% per prestige. Total = 1 + prestigeLevel * multPerLevel")]
    public double multPerLevel = 0.5;

    [Header("State (persists if you choose)")]
    public int prestigeLevel = 0; // prestige count
    public int nextThresholdIndex = 0;

    public event Action OnPrestige; // hook video here

    public bool IsMaxed => nextThresholdIndex >= fameThresholds.Length;

    public bool CanPrestige(double currentFame) {
        if (IsMaxed) return false;
        return currentFame >= fameThresholds[nextThresholdIndex];
    }


    public double CurrentRequirement() {
        if (nextThresholdIndex >= fameThresholds.Length) return double.PositiveInfinity;
        return fameThresholds[nextThresholdIndex];
    }

    public double CurrentPermanentMultiplier() {
        return 1.0 + prestigeLevel * multPerLevel;
    }

    public double NextGainPreview() {
        return 1.0 + (prestigeLevel + 1) * multPerLevel;
    }

    public void ApplyPermanentMultiplier(CurrencyManager cm) {
        double perm = CurrentPermanentMultiplier();
        cm.fameMult = perm;
        cm.fansMult = perm;
        cm.cashMult = perm;
    }

    const string K_LEVEL = "prestige_level";
    const string K_NEXT = "prestige_next_idx";

    void Start() {
        prestigeLevel = PlayerPrefs.GetInt(K_LEVEL, prestigeLevel);
        nextThresholdIndex = PlayerPrefs.GetInt(K_NEXT, nextThresholdIndex);
        if (CurrencyManager.I) ApplyPermanentMultiplier(CurrencyManager.I);
    }

    public void DoPrestige() {
        var cm = CurrencyManager.I;
        if (!CanPrestige(cm.Fame)) return;

        prestigeLevel++;
        nextThresholdIndex = Mathf.Min(nextThresholdIndex + 1, fameThresholds.Length);
        ApplyPermanentMultiplier(cm);

        PlayerPrefs.SetInt(K_LEVEL, prestigeLevel);
        PlayerPrefs.SetInt(K_NEXT, nextThresholdIndex);
        PlayerPrefs.Save();

        OnPrestige?.Invoke();
    }
}
