using System;
using UnityEngine;

[CreateAssetMenuAttribute(menuName="Wardrobe")]
public class Wardrobe : ScriptableObject
{
    static public Wardrobe instance => _inst ??= Resources.Load<Wardrobe>("Wardrobe");
    static Wardrobe _inst;

    private Skin _planeSkin;
    private Skin _engineParticleSkin;

    public Action onPlaneSkinChanged;
    public Action onEngineParticleSkinChanged;

    public Skin planeSkin
    {
        get => _planeSkin;
        set
        {
            _planeSkin = value;
            onPlaneSkinChanged?.Invoke();
        }
    }
    public Skin engineParticleSkin
    {
        get => _engineParticleSkin;
        set
        {
            _engineParticleSkin = value;

            onEngineParticleSkinChanged?.Invoke();

        }
    }

}
