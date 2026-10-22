using System;
using UnityEngine;
using UnityEngine.UI;
using _Project.Scripts.Session;

namespace _Project.Scripts.MainMenuScene
{
    public class MainUpgraderMenu : GameInteractionMenu, IUIElement
    {
        [SerializeField] private Button _closeMenu;
        [SerializeField] private CardButton _buttonCastleHealth;
        [SerializeField] private CardButton _buttonCastleSpeed;
        [SerializeField] private CardButton _buttonCastleDamage;
        [SerializeField] private CardButton _buttonTowerSpeed;
        [SerializeField] private CardButton _buttonTowerDamage;

        public event Action OnCloseMenu;
        public event Action OnUpCastleSpeed;
        public event Action OnUpCastleDamage;
        public event Action OnUpCastleHealth;
        public event Action OnUpTowersDamage;
        public event Action OnUpTowersSpeed;

        protected override void OnDestroy() =>
            UnSubscribeAll();

        public void Initialize(int costCastleHealth, int costCastleSpeed, int costCastleDamage, int costTowerSpeed,
            int costTowerDamage)
        {
            _buttonCastleHealth.Initialize(costCastleHealth);
            _buttonCastleSpeed.Initialize(costCastleSpeed);
            _buttonCastleDamage.Initialize(costCastleDamage);
            _buttonTowerSpeed.Initialize(costTowerSpeed);
            _buttonTowerDamage.Initialize(costTowerDamage);

            SubscribeAll();
        }

        public void SetCastleHealthLevel(int level, float growthMultiplier) =>
            _buttonCastleHealth.SetLevel(level, growthMultiplier);

        public void SetCastleSpeedLevel(int level, float growthMultiplier) =>
            _buttonCastleSpeed.SetLevel(level, growthMultiplier);

        public void SetCastleDamageLevel(int level, float growthMultiplier) =>
            _buttonCastleDamage.SetLevel(level, growthMultiplier);

        public void SetTowerSpeedLevel(int level, float growthMultiplier) =>
            _buttonTowerSpeed.SetLevel(level, growthMultiplier);

        public void SetTowerDamageLevel(int level, float growthMultiplier) =>
            _buttonTowerDamage.SetLevel(level, growthMultiplier);

        public void SetCanBuyCastleHealth(bool value) =>
            _buttonCastleHealth.SetCanBuy(value);

        public void SetCanBuyCastleSpeed(bool value) =>
            _buttonCastleSpeed.SetCanBuy(value);

        public void SetCanBuyCastleDamage(bool value) =>
            _buttonCastleDamage.SetCanBuy(value);

        public void SetCanBuyTowerSpeed(bool value) =>
            _buttonTowerSpeed.SetCanBuy(value);

        public void SetCanBuyTowerDamage(bool value) =>
            _buttonTowerDamage.SetCanBuy(value);

        private void SubscribeAll()
        {
            _buttonCastleHealth.OnClick += (UpCastleHealth);
            _buttonCastleSpeed.OnClick += (UpCastleSpeed);
            _buttonCastleDamage.OnClick += (UpCastleDamage);
            _buttonTowerSpeed.OnClick += (UpTowersSpeed);
            _buttonTowerDamage.OnClick += (UpTowersDamage);
            _closeMenu.onClick.AddListener(CloseMenu);
        }

        private void UnSubscribeAll()
        {
            _buttonCastleHealth.OnClick -= (UpCastleHealth);
            _buttonCastleSpeed.OnClick -= (UpCastleSpeed);
            _buttonCastleDamage.OnClick -= (UpCastleDamage);
            _buttonTowerSpeed.OnClick -= (UpTowersSpeed);
            _buttonTowerDamage.OnClick -= (UpTowersDamage);
            _closeMenu.onClick.RemoveListener(CloseMenu);
        }

        private void UpTowersDamage() =>
            OnUpTowersDamage?.Invoke();

        private void UpTowersSpeed() =>
            OnUpTowersSpeed?.Invoke();

        private void UpCastleDamage() =>
            OnUpCastleDamage?.Invoke();

        private void UpCastleSpeed() =>
            OnUpCastleSpeed?.Invoke();

        private void UpCastleHealth() => 
            OnUpCastleHealth?.Invoke();

        private void CloseMenu() =>
            OnCloseMenu?.Invoke();
    }
}