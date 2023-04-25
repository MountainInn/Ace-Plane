using UnityEngine;
using UnityEngine.UI;
using Zenject;
using TMPro;

public class SkinSelect : MonoBehaviour
{
    [SerializeField] public Skin skin;
    [SerializeField] public SkinContainer skinContainer;
    [SerializeField] public Color selectionColor;

    SkinSelect_RadioGroup skinSelectRadio;
    Vendible vendible;
    Button button;
    Image buttonImage;
    TextMeshProUGUI costText;

    void Awake()
    {
        skinSelectRadio = GetComponentInParent<SkinSelect_RadioGroup>();

        vendible =
            GetComponent<Vendible>()
            .Initialize(UpdateInteractable);

        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);

        buttonImage = GetComponent<Image>();

        costText = GetComponentInChildren<TextMeshProUGUI>();
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

        SelectSkin();
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
