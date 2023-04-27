using UnityEngine;

abstract public class SkinSlot : MonoBehaviour
{
    protected Skin currentSkin;

    protected void Awake()
    {
        currentSkin = GetComponentInChildren<Skin>();
    }

    public void SetSkin(Skin skin)
    {
        switch(skin.subtype)
        {
            case Skin.Subtype.plane:
                Wardrobe.instance.planeSkin = skin;
                break;

            case Skin.Subtype.engine:
                Wardrobe.instance.engineParticleSkin = skin;
                break;

            default:
                throw new System.ArgumentException();
        }

        if (currentSkin is not null)
        {
            Destroy(currentSkin.gameObject);
        }

        currentSkin =
            Instantiate(skin, transform)
            .GetComponent<Skin>();

        currentSkin.name = skin.name;
        currentSkin.transform.localPosition = Vector3.zero;
        currentSkin.transform.localRotation = Quaternion.identity;
    }
}
