using NUnit.Framework.Internal.Commands;
using UnityEngine;

public enum UpgradeType
{
    FameRate,
    FansRate,
    CashRate,
    FameMult,
    FansMult,
    CashMult
}

[System.Serializable]
public class Upgrade
{
    public string displayName;
    public double baseCost = 100;
    public double costMultiplier = 1.3;
    public int level;
    public UpgradeType type;
    public double amountPerLevel = 0.1;
}

public class UpgradeService : MonoBehaviour
{
    public Upgrade[] upgrades;

    public double CurrentCost(Upgrade u) =>
        u.baseCost * System.Math.Pow(u.costMultiplier, u.level);

    public bool TryBuy(Upgrade u)
    {
        var cost = CurrentCost(u);
        if (!CurrencyManager.I.TrySpendCash(cost)) return false;
        u.level++;
        Apply(u);
        return true;
    }

    void Apply(Upgrade u)
    {
        var cm = CurrencyManager.I;
        switch (u.type)
        {
            case UpgradeType.FameRate:
                cm.famePerSec += u.amountPerLevel;
                break;
            case UpgradeType.FansRate:
                cm.fansPerSec += u.amountPerLevel;
                break;
            case UpgradeType.CashRate:
                cm.baseCashPerSec += u.amountPerLevel;
                break;
            case UpgradeType.FameMult:
                cm.fameMult += u.amountPerLevel;
                break;
            case UpgradeType.FansMult:
                cm.fansMult += u.amountPerLevel;
                break;
            case UpgradeType.CashMult:
                cm.cashMult += u.amountPerLevel;
                break;
        }
    }
}
