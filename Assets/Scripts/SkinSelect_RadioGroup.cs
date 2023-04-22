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
        obj.ToggleSelection(true);
    }

    protected override void ToggleOff(SkinSelect obj)
    {
        obj.ToggleSelection(false);
    }
}
