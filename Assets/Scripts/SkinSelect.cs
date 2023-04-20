using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SkinSelect : MonoBehaviour
{
    [SerializeField] public Skin skin;

    SkinSelect skinSelect;
    Vendible vendible;
    Button button;

    [Inject] Vault vault;
    [Inject] SkinContainer skinContainer;

    public void SelectSkin()
    {
        skinContainer.SetSkin(skin);
    }

    void Awake()
    {
        vendible = GetComponent<Vendible>();
        skinSelect = GetComponent<SkinSelect>();
        button = GetComponent<Button>();

        button.onClick.AddListener(OnClick);

        vendible.onUpdateAffordable += SetInteractable;
    }

    void SetInteractable(bool canAfford)
    {
        button.interactable = canAfford;
    }

    void OnClick()
    {
        if (!vendible.bought)
        {
            vendible.Buy();
            return;
        }

        skinSelect.SelectSkin();
    }
}
