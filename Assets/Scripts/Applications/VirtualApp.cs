using UnityEngine;
using System.Collections;

[RequireComponent(typeof(VirtualOS))]
public class VirtualFunction : MonoBehaviour
{
    public VirtualOS vos;
    public int[] pins;
    public VirtualBUS bus;
    Outline outline;

    public void Start ()
    {
        outline = gameObject.AddComponent<Outline>();
        outline.OutlineWidth = 2;
        outline.OutlineColor = Color.black;
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.enabled = false;

        if (vos == null)
        {
            vos = gameObject.GetComponent<VirtualOS>();
        }

        bus = new VirtualBUS(vos);

        SetBus();
    }

    public void SetBus()
    {
        for (int i = 0; i < pins.Length; i++)
        {
            bus.AddPin(pins[i]);
        }
    }

    public void AttachedToEditor()
    {
        outline.enabled = true;
    }

    public void DettachedToEditor()
    {
        outline.enabled = false;
    }
}
