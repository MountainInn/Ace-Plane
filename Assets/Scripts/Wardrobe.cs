using UnityEngine;

[CreateAssetMenuAttribute(menuName="Wardrobe")]
public class Wardrobe : ScriptableObject
{
    static public Wardrobe instance => _inst ??= Resources.Load<Wardrobe>("Wardrobe");
    static Wardrobe _inst;
    public Skin
        planeSkin,
        engineParticleSkin;

}
