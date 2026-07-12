using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup _buildMenuCanvasGroup;
    [SerializeField] private PumpingMenu _pumpingMenu;
    [SerializeField] private BuildMenu _buildMenu;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _castleLayer;
    [SerializeField] private Tower _strongTowerPrefab;
    [SerializeField] private Tower _fastTowerPrefab;
    [SerializeField] private List<Tower> _towers = new();
    [SerializeField] private Castle _castle;

    private Wallet _wallet;
    private GameConfig _config;
    private Camera _mainCamera;
    private Vector3 _buildPosition;
    private int _castleHealthLevel = 1;
    private int _castleSpeedLevel = 1;
    private int _castleStrongLevel = 1;
    private int _currentCostUpgradeSpeed;
    private int _currentCostUpgradeForce;
    private int _currentCostUpgradeHealth;

    private void Awake()
    {
        _config = GameManager.Instance.Config;
        _currentCostUpgradeSpeed = _config.UpgradeCastleCost;
        _currentCostUpgradeForce = _config.UpgradeCastleCost;
        _currentCostUpgradeHealth = _config.UpgradeCastleCost;

        _buildMenu.SetCostTowers(_config.FastTowerCost, _config.StrongTowerCost);
        _pumpingMenu.SetStartCost(_config.UpgradeCastleCost);
    }

    private void Start() => 
        _mainCamera = Camera.main;

    private void Update()
    {
        if (_buildMenu.IsActive == false && _pumpingMenu.IsActive == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;

                if (_buildMenu.IsActive || _pumpingMenu.IsActive || !Application.isFocused)
                    return;

                HandleClick();
            }
        }
    }

    private void OnEnable()
    {
        _buildMenu.TriedBuyFastTower += BuildFastTower;
        _buildMenu.TriedBuyStrongTower += BuildStrongTower;
        _pumpingMenu.TriedUpHealth += UpCastleHealth;
        _pumpingMenu.TriedUpStrong += UpCastleStrong;
        _pumpingMenu.TriedUpSpeed += UpCastleSpeed;
    }

    private void OnDisable()
    {
        _buildMenu.TriedBuyFastTower -= BuildFastTower;
        _buildMenu.TriedBuyStrongTower -= BuildStrongTower;
        _pumpingMenu.TriedUpHealth -= UpCastleHealth;
        _pumpingMenu.TriedUpStrong -= UpCastleStrong;
        _pumpingMenu.TriedUpSpeed -= UpCastleSpeed;
        _wallet.ValueChanged -= ChangeOpportunitiesBuy;
    }

    public void SetParameters(Wallet wallet)
    {
        _wallet = wallet ?? throw new ArgumentNullException(nameof(wallet));

        _wallet.ValueChanged += ChangeOpportunitiesBuy;
    }

    public void BuildStrongTower()
    {
        if (_wallet.TryTakeMoney(_config.StrongTowerCost))
            BuildTower(_strongTowerPrefab);
        else
            _buildMenu.Hide();
    }

    public void BuildFastTower()
    {
        if (_wallet.TryTakeMoney(_config.FastTowerCost))
            BuildTower(_fastTowerPrefab);
        else
            _buildMenu.Hide();
    }

    public void UpCastleHealth()
    {
        if (_wallet.TryTakeMoney(_currentCostUpgradeHealth) && _castleHealthLevel <= _config.MaxCastleLevel)
        {
            _castle.UpHealth();
            _currentCostUpgradeHealth = (int)(_currentCostUpgradeHealth * _config.CostMultiplier);
            _pumpingMenu.ChangeCostUpgradeHealth(_currentCostUpgradeHealth);
            _castleHealthLevel++;
        }

        _pumpingMenu.Hide();

    }

    public void UpCastleStrong()
    {
        if (_wallet.TryTakeMoney(_currentCostUpgradeForce) && _castleStrongLevel <= _config.MaxCastleLevel)
        {
            _castle.UpStrong();
            _currentCostUpgradeForce = (int)(_currentCostUpgradeForce * _config.CostMultiplier);
            _pumpingMenu.ChangeCostUpgradeStrong(_currentCostUpgradeForce);
            _castleStrongLevel++;
        }

        _pumpingMenu.Hide();
    }

    public void UpCastleSpeed()
    {
        if (_wallet.TryTakeMoney(_currentCostUpgradeSpeed) && _castleSpeedLevel <= _config.MaxCastleLevel)
        {
            _castle.UpSpeed();
            _currentCostUpgradeSpeed = (int)(_currentCostUpgradeSpeed * _config.CostMultiplier);
            _pumpingMenu.ChangeCostUpgradeSpeed(_currentCostUpgradeSpeed);
            _castleSpeedLevel++;
        }

        _pumpingMenu.Hide();
    }

    public void StopAllTowers()
    {
        foreach (Tower tower in _towers)
            tower.Stop();
    }

    private void ChangeOpportunitiesBuy(int count)
    {
        _buildMenu.SetCanBuyFast(count >= _config.FastTowerCost);
        _buildMenu.SetCanBuyStrong(count >= _config.StrongTowerCost);
        _pumpingMenu.SetCanUpHealth(CheckOnOpportunity(count, _castleHealthLevel));
        _pumpingMenu.SetCanUpSpeed(CheckOnOpportunity(count, _castleSpeedLevel));
        _pumpingMenu.SetCanUpStrong(CheckOnOpportunity(count, _castleStrongLevel));
    }

    private bool CheckOnOpportunity(int count, int currentLevel)
    {
        int tempCost = _config.UpgradeCastleCost;

        for (int i = 1; i < currentLevel; i++)
            tempCost = (int)(tempCost * _config.CostMultiplier);

        return count >= tempCost;
    }

    private void HandleClick()
    {
        if (_castle.isActiveAndEnabled == false)
            return;

        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _castleLayer))
        {
            _pumpingMenu.Show();
        }
        else if (Physics.Raycast(ray, out hit, Mathf.Infinity, _groundLayer))
        {
            Vector3 clickedPoint = hit.point;

            float distance = Vector3.Distance(clickedPoint, _castle.transform.position);

            if (distance < _config.MinDistanceForBuilding)
                return;

            foreach (Tower tower in _towers)
            {
                distance = Vector3.Distance(clickedPoint, tower.transform.position);

                if (distance < _config.MinDistanceForBuilding)
                    return;
            }

            _buildPosition = clickedPoint;

            _buildMenu.Show();
        }
    }

    private void BuildTower(Tower prefab)
    {
        if (prefab == null)
            throw new NullReferenceException(nameof(prefab));

        _towers.Add(Instantiate(prefab, _buildPosition, Quaternion.identity));

        _buildMenu.Hide();
    }
}