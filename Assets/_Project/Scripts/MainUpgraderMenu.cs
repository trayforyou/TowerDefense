using System;
using _Project.Scripts.Session;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts
{
    public class MainUpgraderMenu : GameInteractionMenu, IUIElement
    {
        [SerializeField] private Button _castleHP;
        [SerializeField] private TextMeshProUGUI _castleHPTextLevel;
        [SerializeField] private TextMeshProUGUI _castleHPTextCost;
        [SerializeField] private Button _castleAttackSpeed;
        [SerializeField] private TextMeshProUGUI _castleAttackSpeedTextLevel;
        [SerializeField] private TextMeshProUGUI _castleAttackSpeedTextCost;
        [SerializeField] private Button _castleDamage;
        [SerializeField] private TextMeshProUGUI _castleDamageTextLevel;
        [SerializeField] private TextMeshProUGUI _castleDamageTextCost;
        [SerializeField] private Button _towerAttackSpeed;
        [SerializeField] private TextMeshProUGUI _towerAttackSpeedTextLevel;
        [SerializeField] private TextMeshProUGUI _towerAttackSpeedTextCost;
        [SerializeField] private Button _towerDamage;
        [SerializeField] private TextMeshProUGUI _towerDamageTextLevel;
        [SerializeField] private TextMeshProUGUI _towerDamageTextCost;
        [SerializeField] private Button _closeMenu;
        [SerializeField] private TextMeshProUGUI _closeMenuTextLevel;
        [SerializeField] private TextMeshProUGUI _closeMenuTextCost;

        public event Action OnCloseMenu;
        public event Action OnUpCastleSpeed;
        public event Action OnUpCastleDamage;
        public event Action OnUpCastleHealth;
        public event Action OnUpTowersDamage;
        public event Action OnUpTowersSpeed;

        private void OnDestroy()
        {
            _castleHP.onClick.RemoveListener(UpHealth);
            _castleAttackSpeed.onClick.RemoveListener(UpCastleSpeed);
            _castleDamage.onClick.RemoveListener(UpCastleDamage);
            _towerAttackSpeed.onClick.RemoveListener(UpTowersSpeed);
            _towerDamage.onClick.RemoveListener(UpTowersDamage);
            _closeMenu.onClick.RemoveListener(CloseMenu);
        }

        public void Initialize()
        {
            _castleHP.onClick.AddListener(UpHealth);
            _castleAttackSpeed.onClick.AddListener(UpCastleSpeed);
            _castleDamage.onClick.AddListener(UpCastleDamage);
            _towerAttackSpeed.onClick.AddListener(UpTowersSpeed);
            _towerDamage.onClick.AddListener(UpTowersDamage);
            _closeMenu.onClick.AddListener(CloseMenu);
        }

        private void UpTowersDamage() =>
            OnUpTowersDamage?.Invoke();

        private void UpTowersSpeed() =>
            OnUpTowersSpeed?.Invoke();

        private void UpCastleDamage() =>
            OnUpCastleDamage?.Invoke();

        private void UpCastleSpeed() =>
            OnUpCastleSpeed?.Invoke();

        private void UpHealth() =>
            OnUpCastleHealth?.Invoke();

        private void CloseMenu() =>
            OnCloseMenu?.Invoke();

        public void SetValues(int healthCastleLevel, int damageCastleLevel, int speedCastleLevel, int damageTowersLevel,
            int speedTowersLevel)
        {
            throw
                new NotImplementedException(); // TO DO нужно сделать объявлдение для каждой кнопки (Выделить кнопки в отдельный класс, чтобы не расширялся сильно текст)
        }
    }
}