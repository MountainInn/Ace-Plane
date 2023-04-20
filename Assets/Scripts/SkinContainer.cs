using UnityEngine;

public class SkinContainer : MonoBehaviour
{
    Skin currentSkin;

    void Awake()
    {
        currentSkin = GetComponentInChildren<Skin>();
    }

    public void SetSkin(Skin skin)
    {
        if (currentSkin is not null)
        {
            Destroy(currentSkin.gameObject);
        }

        currentSkin =
            Instantiate(skin, transform)
            .GetComponent<Skin>();

        currentSkin.transform.localPosition = Vector3.zero;
        currentSkin.transform.localRotation = Quaternion.identity;
    }
}
