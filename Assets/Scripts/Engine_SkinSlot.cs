public class Engine_SkinSlot : SkinSlot
{
    void OnEnable()
    {
        if (Wardrobe.instance.engineParticleSkin.name != currentSkin.name)
            SetSkin(Wardrobe.instance.engineParticleSkin);
    }
}
