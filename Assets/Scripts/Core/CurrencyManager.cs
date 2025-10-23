using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager I { get; private set; }
    private void Awake() => I = this;

    public double Fame { get; private set; }
    public double Fans { get; private set; }
    public double Cash { get; private set; }

    [Header("Base rates per second")] // amount/s
    public double famePerSec = 1.0 / 0.3;  // was 1/6s ~ 0.1667
    public double fansPerSec = 1.0 / 0.3; // was 50.0 ~ 0.0083
    public double baseCashPerSec = 0.0;  // no cash at start

    [Header("Additive multipliers from upgrades")]
    public double fameMult = 1;
    public double fansMult = 1;
    public double cashMult = 1;

    [Header("Unlocks")]
    public bool fansUnlocked = false;

    [Header("Coupling")]
    public double cashFromFansFactor = 10.0;

    private void Update()
    {
        double dt = Time.deltaTime;
        Fame += famePerSec * fameMult * dt;
        if (fansUnlocked)
        {
            Fans += fansPerSec * fansMult * dt; // cash starts once fans exist
            double cashRate = baseCashPerSec + (fansPerSec * cashFromFansFactor);
            Cash += cashRate * cashMult * dt;
        }
    }

    public bool TrySpendCash (double amount)
    {
        if (Cash + 1e-9< amount) return false;
        Cash -= amount;
        return true;
    }

    public bool TrySpendFame (double amount)
    {
        if (Fame + 1e-9 < amount)
        {
            return false;
        }
        Fame -= amount;
        return true;
    }

    void Start() {
        if (PrestigeManager.I != null)
            PrestigeManager.I.ApplyPermanentMultiplier(this);
    }
}
