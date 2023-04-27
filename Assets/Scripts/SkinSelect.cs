using UnityEngine;
using UnityEngine.UI;
using Zenject;
using TMPro;

public class SkinSelect : MonoBehaviour
{
    [SerializeField] public Skin skin;
    [Space]
    [SerializeField] public Color selectionColor;

    SkinSelect_RadioGroup skinSelectRadio;
    Vendible vendible;
    Button button;
    Image buttonImage;
    TextMeshProUGUI costText;

    void Awake()
    {
        skinSelectRadio = GetComponentInParent<SkinSelect_RadioGroup>();

        vendible = GetComponent<Vendible>();
        vendible.Initialize(UpdateInteractable);

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
        SkinSlot[] skinSlots = skin.subtype switch
            {
                Skin.Subtype.plane => FindObjectsOfType<Plane_SkinSlot>(),
                Skin.Subtype.engine => FindObjectsOfType<Engine_SkinSlot>(),

                (_) => throw new System.ArgumentException()
            };

        foreach(var item in skinSlots)
        {
            item.SetSkin(skin);
        }

        skinSelectRadio.RadioToggle(this);
    }

    public void ToggleSelection(bool toggle)
    {
        buttonImage.color = (toggle) ? selectionColor : Color.white;
    }
}
