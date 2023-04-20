using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SkinSelect_RadioGroup : RadioGroup<SkinSelect>
{
    protected override void InitRadios()
    {
        radios = GetComponentsInChildren<SkinSelect>().ToList();
    }

    protected override void ToggleOn(SkinSelect obj)
    {
        Debug.Log($"Selected Skin: {obj.skin.name}");
    }

    protected override void ToggleOff(SkinSelect obj)
    {
        Debug.Log($"Deselected Skin: {obj.skin.name}");
    }
}
