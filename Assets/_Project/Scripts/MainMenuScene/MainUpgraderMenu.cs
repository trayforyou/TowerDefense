using System;
using System.Collections.Generic;
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

        private Dictionary<MetaUpgradeType, CardButton> _cardButtons;

        public event Action OnClickCloseMenu;
        public event Action<MetaUpgradeType> OnClickUpgrade;

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
            InitializeCardButtons();
            SubscribeAll();
        }

        private CardButton GetCardButton(MetaUpgradeType type)
        {
            if (!_cardButtons.TryGetValue(type, out var button))
                throw new ArgumentException($"Нет кнопки с типом {type.ToString()}");
            
            return button;
        }
        
        public void SetLevel(MetaUpgradeType type, int level, float growthMultiplier) => 
            GetCardButton(type).SetLevel(level, growthMultiplier);

        public void SetCanBuy(MetaUpgradeType type, bool value) => 
            GetCardButton(type).SetCanBuy(value);
        
        private void InitializeCardButtons()
        {
            _cardButtons = new();
            _cardButtons[MetaUpgradeType.CastleHealth] = _buttonCastleHealth;
            _cardButtons[MetaUpgradeType.CastleSpeed] = _buttonCastleSpeed;
            _cardButtons[MetaUpgradeType.CastleDamage] = _buttonCastleDamage;
            _cardButtons[MetaUpgradeType.TowerSpeed] = _buttonTowerSpeed;
            _cardButtons[MetaUpgradeType.TowerDamage] = _buttonTowerDamage;
        }
        
        private void SubscribeAll()
        {
            _buttonCastleHealth.OnClick += () => OnClickUpgrade?.Invoke(MetaUpgradeType.CastleHealth);
            _buttonCastleSpeed.OnClick += () => OnClickUpgrade?.Invoke(MetaUpgradeType.CastleSpeed);
            _buttonCastleDamage.OnClick += () => OnClickUpgrade?.Invoke(MetaUpgradeType.CastleDamage);
            _buttonTowerSpeed.OnClick += () => OnClickUpgrade?.Invoke(MetaUpgradeType.TowerSpeed);
            _buttonTowerDamage.OnClick += () => OnClickUpgrade?.Invoke(MetaUpgradeType.TowerDamage);
            _closeMenu.onClick.AddListener(( )=> OnClickCloseMenu?.Invoke());
        }

        private void UnSubscribeAll()
        {
            _buttonCastleHealth.ClearSubscribers();
            _buttonCastleSpeed.ClearSubscribers();
            _buttonCastleDamage.ClearSubscribers();
            _buttonTowerSpeed.ClearSubscribers();
            _buttonTowerDamage.ClearSubscribers();
            _closeMenu.onClick.RemoveAllListeners();
        }
    }
}