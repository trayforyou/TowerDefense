using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class GameInteractionMenu : MonoBehaviour
    {
        protected const string CurrencySymbol = "$";

        private CanvasGroup _canvasGroup;

        public bool IsActive { get; private set; }

        protected virtual void Awake() =>
            _canvasGroup = GetComponent<CanvasGroup>();

        protected virtual void Start() =>
            Hide();

        protected virtual void OnDestroy() { }

        public void Show()
        {
            _canvasGroup.Show();
            IsActive = true;
        }

        public void Hide()
        {
            _canvasGroup.Hide();
            IsActive = false;
        }

        protected void SetCostText(TextMeshProUGUI text, int cost) =>
            text.text = cost + CurrencySymbol;

        protected void SetAvailabilityView(TextMeshProUGUI text, bool canAfford) =>
            text.color = canAfford ? Color.green : Color.red;

        protected void SubscribeButton(Button button, UnityEngine.Events.UnityAction action) =>
            button.onClick.AddListener(action);

        protected void UnsubscribeButton(Button button, UnityEngine.Events.UnityAction action) =>
            button.onClick.RemoveListener(action);
    }
}