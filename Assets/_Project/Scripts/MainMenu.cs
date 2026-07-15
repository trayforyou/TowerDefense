using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _start;
    [SerializeField] private TextMeshProUGUI _metaCoins;

    private string _metaCoinsText;

    private void Awake() => 
        _metaCoinsText = _metaCoins.text;

    public void ChangeMetaCoins(int value) => 
        _metaCoins.text = _metaCoinsText + value;

    private void OnEnable()
    {
        ChangeMetaCoins(PlayerPrefs.GetInt(GameManager.META_CURRENCY, 0));
        
        _start.onClick.AddListener(ClickButton);
    }

    private void OnDisable() => 
        _start.onClick.RemoveListener(ClickButton);

    private void ClickButton() => 
        GameManager.Instance.LoadGame();
}
