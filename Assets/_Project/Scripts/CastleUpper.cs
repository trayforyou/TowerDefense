using System;
using UnityEngine;

public class CastleUpper : MonoBehaviour
{
    [SerializeField] private UpgradeMenu _upgradeMenu;

    private Wallet _wallet;
    private Castle _castle;
    private GameConfig _config;
    private int _castleHealthLevel = 1;
    private int _castleSpeedLevel = 1;
    private int _castleStrongLevel = 1;
    private int _currentCostUpgradeSpeed;
    private int _currentCostUpgradeForce;
    private int _currentCostUpgradeHealth;
    private bool _isInitialize = false;

    public bool IsActive => _upgradeMenu.IsActive;

    private void OnDestroy() =>
        UnSubscribeAll();

    public void Initialize(GameConfig config, Wallet wallet, Castle castle)
    {
        if (_isInitialize)
            throw new InvalidOperationException("Апгрейдер уже инициализирован");

        _isInitialize = true;
        _config = config;
        _wallet = wallet;
        _castle = castle;
        
        _upgradeMenu.SetStartCost(_config.UpgradeCastleCost);
        _currentCostUpgradeSpeed = _config.UpgradeCastleCost;
        _currentCostUpgradeForce = _config.UpgradeCastleCost;
        _currentCostUpgradeHealth = _config.UpgradeCastleCost;
        SubscribeAll();
    }

    public void Activate() => 
        _upgradeMenu.Show();

    private void ChangeOpportunitiesBuy(int count)
    {
        _upgradeMenu.SetCanUpHealth(CheckOnOpportunity(count, _castleHealthLevel));
        _upgradeMenu.SetCanUpSpeed(CheckOnOpportunity(count, _castleSpeedLevel));
        _upgradeMenu.SetCanUpStrong(CheckOnOpportunity(count, _castleStrongLevel));
    }

    private bool CheckOnOpportunity(int count, int currentLevel)
    {
        int tempCost = _config.UpgradeCastleCost;

        for (int i = 1; i < currentLevel; i++)
            tempCost = (int)(tempCost * _config.CostMultiplier);

        return count >= tempCost;
    }

    private void SubscribeAll()
    {
        _upgradeMenu.TriedUpHealth += UpCastleHealth;
        _upgradeMenu.TriedUpStrong += UpCastleStrong;
        _upgradeMenu.TriedUpSpeed += UpCastleSpeed;
        _wallet.ValueChanged += ChangeOpportunitiesBuy;
    }

    private void UnSubscribeAll()
    {
        _upgradeMenu.TriedUpHealth -= UpCastleHealth;
        _upgradeMenu.TriedUpStrong -= UpCastleStrong;
        _upgradeMenu.TriedUpSpeed -= UpCastleSpeed;
        _wallet.ValueChanged -= ChangeOpportunitiesBuy;
    }

    private void UpCastleHealth()
    {
        if (_wallet.TryTakeMoney(_currentCostUpgradeHealth) && _castleHealthLevel <= _config.MaxCastleLevel)
        {
            _castle.UpHealth();
            _currentCostUpgradeHealth = (int)(_currentCostUpgradeHealth * _config.CostMultiplier);
            _upgradeMenu.ChangeCostUpgradeHealth(_currentCostUpgradeHealth);
            _castleHealthLevel++;
        }

        _upgradeMenu.Hide();
    }

    private void UpCastleStrong()
    {
        if (_wallet.TryTakeMoney(_currentCostUpgradeForce) && _castleStrongLevel <= _config.MaxCastleLevel)
        {
            _castle.UpStrong();
            _currentCostUpgradeForce = (int)(_currentCostUpgradeForce * _config.CostMultiplier);
            _upgradeMenu.ChangeCostUpgradeStrong(_currentCostUpgradeForce);
            _castleStrongLevel++;
        }

        _upgradeMenu.Hide();
    }

    private void UpCastleSpeed()
    {
        if (_wallet.TryTakeMoney(_currentCostUpgradeSpeed) && _castleSpeedLevel <= _config.MaxCastleLevel)
        {
            _castle.UpSpeed();
            _currentCostUpgradeSpeed = (int)(_currentCostUpgradeSpeed * _config.CostMultiplier);
            _upgradeMenu.ChangeCostUpgradeSpeed(_currentCostUpgradeSpeed);
            _castleSpeedLevel++;
        }

        _upgradeMenu.Hide();
    }
}