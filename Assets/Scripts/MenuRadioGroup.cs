using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuRadioGroup : RadioGroup<RectTransform>
{
    Image image;

    protected override void InitRadios()
    {
        image = GetComponent<Image>();

        radios = new List<RectTransform>();

        for (int i = 0; i < transform.childCount; i++)
        {
            radios.Add(transform.GetChild(i).GetComponent<RectTransform>());
        }
    }

    public void AllToggleOff()
    {
        image.enabled = false;

        foreach (var item in radios)
            ToggleOff(item);
    }

    protected override void ToggleOff(RectTransform obj)
    {
        obj.gameObject.SetActive(false);
    }

    protected override void ToggleOn(RectTransform obj)
    {
        image.enabled = true;
        obj.gameObject.SetActive(true);
    }

}
