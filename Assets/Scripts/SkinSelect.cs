using UnityEngine;
using UnityEngine.UI;
using Zenject;
using TMPro;

public class SkinSelect : MonoBehaviour
{
    [SerializeField] public Skin skin;
    [SerializeField] public Color selectionColor;

    [Inject] Vault vault;
    [Inject] SkinContainer skinContainer;

    SkinSelect skinSelect;
    SkinSelect_RadioGroup skinSelectRadio;
    Vendible vendible;
    Button button;
    Image buttonImage;
    TextMeshProUGUI costText;

    void Awake()
    {
        skinSelect = GetComponent<SkinSelect>();
        skinSelectRadio = GetComponentInParent<SkinSelect_RadioGroup>();
        vendible = GetComponent<Vendible>();
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();
        costText = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Start()
    {
        vendible.onUpdateAffordable += (_) => UpdateInteractable();
        vendible.onBought += (_) => UpdateInteractable();
        UpdateInteractable();

        button.onClick.AddListener(OnClick);

        costText.text = vendible.cost.ToString();
    }

    void UpdateInteractable()
    {
        button.interactable = vendible.isAffordable || vendible.isBought;
    }

    void OnClick()
    {
        if (!vendible.isBought)
        {
            if (!vendible.isAffordable) return;

            vendible.Buy();
        }

        skinSelect.SelectSkin();
    }

    public void SelectSkin()
    {
        skinContainer.SetSkin(skin);
        skinSelectRadio.RadioToggle(this);
    }

    public void ToggleSelection(bool toggle)
    {
        buttonImage.color = (toggle) ? selectionColor : Color.white;
    }
}
