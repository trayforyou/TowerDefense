using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class PumpingMenu : MonoBehaviour
{
    private const string CurrencySymbol = "$";

    [SerializeField] private Button _buttonHealthier;
    [SerializeField] private Button _buttonFaster;
    [SerializeField] private Button _buttonStronger;
    [SerializeField] private TextMeshProUGUI _tMPHealthier;
    [SerializeField] private TextMeshProUGUI _tMPFaster;
    [SerializeField] private TextMeshProUGUI _tMPStronger;

    private CanvasGroup _canvasGroup;
    private bool _isActive = false;

    public event Action TriedUpHealth;
    public event Action TriedUpSpeed;
    public event Action TriedUpStrong;

    public bool IsActive => _isActive;

    private void Awake() => 
        _canvasGroup = GetComponent<CanvasGroup>();

    private void Start()
    {
        Hide();

        ChangeView(_tMPHealthier, false);
        ChangeView(_tMPFaster, false);
        ChangeView(_tMPStronger, false);
    }

    private void OnEnable()
    {
        _buttonFaster.onClick.AddListener(TryUpSpeed);
        _buttonHealthier.onClick.AddListener(TryUpHealth);
        _buttonStronger.onClick.AddListener(TryUpStrong);
    }

    public void Show()
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        _isActive = true;
    }

    public void Hide()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        _isActive = false;
    }

    public void SetStartCost(int cost)
    {
        ChangeCostUpgradeHealth(cost);
        ChangeCostUpgradeSpeed(cost);
        ChangeCostUpgradeStrong(cost);
    }

    public void ChangeCostUpgradeHealth(int cost) =>
        _tMPHealthier.text = cost + CurrencySymbol;

    public void ChangeCostUpgradeSpeed(int cost) =>
        _tMPFaster.text = cost + CurrencySymbol;

    public void ChangeCostUpgradeStrong(int cost) =>
        _tMPStronger.text = cost + CurrencySymbol;

    public void SetCanUpHealth(bool value) =>
        ChangeView(_tMPHealthier, value);

    public void SetCanUpSpeed(bool value) =>
        ChangeView(_tMPFaster, value);

    public void SetCanUpStrong(bool value) =>
        ChangeView(_tMPStronger, value);

    private void TryUpHealth() =>
        TriedUpHealth?.Invoke();

    private void TryUpSpeed() =>
        TriedUpSpeed?.Invoke();

    private void TryUpStrong() =>
        TriedUpStrong?.Invoke();

    private void ChangeView(TextMeshProUGUI tMPTower, bool value)
    {
        if (value)
            tMPTower.color = Color.green;
        else
            tMPTower.color = Color.red;
    }
}