using System;
using _Project.Scripts.Session;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts
{
    public class MainUpgraderMenu : GameInteractionMenu, IUIElement
    {
        private Button _closeMenu;
        private CardButton _buttonCastleHealth;
        private CardButton _buttonCastleSpeed;
        private CardButton _buttonCastleDamage;
        private CardButton _buttonTowerSpeed;
        private CardButton _buttonTowerDamage;

        private MetaMoneyBank _metaMoneyBank;

        public event Action OnCloseMenu;
        public event Action OnUpCastleSpeed;
        public event Action OnUpCastleDamage;
        public event Action OnUpCastleHealth;
        public event Action OnUpTowersDamage;
        public event Action OnUpTowersSpeed;

        protected override void OnDestroy()
        {
            UnSubscribeAll();
        }

        public void Initialize(MetaMoneyBank bank, CardButton buttonCastleHealth, CardButton buttonCastleSpeed,
            CardButton buttonCastleDamage, CardButton buttonTowerSpeed, CardButton buttonTowerDamage, Button  closeMenu)
        {
            _closeMenu = closeMenu;
            _buttonCastleSpeed = buttonCastleSpeed;
            _buttonCastleDamage = buttonCastleDamage;
            _buttonTowerSpeed = buttonTowerSpeed;
            _buttonTowerDamage = buttonTowerDamage;
            _buttonCastleHealth = buttonCastleHealth;

            _metaMoneyBank = bank;
            
            SubscribeAll();
        }
        
        public void SetCastleHealthLevel(int level, float growthMultiplier)
        {
            
        }
        
        public void SetCastleSpeedLevel(int level, float growthMultiplier)
        {
            
        }
        
        public void SetCastleDamageLevel(int level, float growthMultiplier)
        {
            
        }
        
        public void SetTowerSpeedLevel(int level, float growthMultiplier)
        {
            
        }
        
        public void SetTowerDamageLevel(int level, float growthMultiplier)
        {
            
        }
        
        // private void ChangeOpportunitiesBuy(int count)
        // {
        //     _buttonCastleSpeed.SetCanBuy(count >= _buttonCastleSpeed.Cost);
        //     _buttonCastleDamage.SetCanBuy(count >= _buttonCastleDamage.Cost);
        //     _buttonTowerSpeed.SetCanBuy(count >= _buttonTowerSpeed.Cost);
        //     _buttonTowerDamage.SetCanBuy(count >= _buttonTowerDamage.Cost);
        //     _buttonCastleHealth.SetCanBuy(count >= _buttonCastleHealth.Cost);
        // }
        
        private void SubscribeAll()
        {
            _buttonCastleHealth.OnClick += (UpHealth);
            _buttonCastleSpeed.OnClick += (UpCastleSpeed);
            _buttonCastleDamage.OnClick += (UpCastleDamage);
            _buttonTowerSpeed.OnClick += (UpTowersSpeed);
            _buttonTowerDamage.OnClick += (UpTowersDamage);
            _closeMenu.onClick.AddListener(CloseMenu);
            // _metaMoneyBank.OnValueChanged += ChangeOpportunitiesBuy;
        }

        private void UnSubscribeAll()
        {
            _buttonCastleHealth.OnClick -= (UpHealth);
            _buttonCastleSpeed.OnClick -= (UpCastleSpeed);
            _buttonCastleDamage.OnClick -= (UpCastleDamage);
            _buttonTowerSpeed.OnClick -= (UpTowersSpeed);
            _buttonTowerDamage.OnClick -= (UpTowersDamage);
            _closeMenu.onClick.RemoveListener(CloseMenu);
            // _metaMoneyBank.OnValueChanged -= ChangeOpportunitiesBuy;
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
    }
}