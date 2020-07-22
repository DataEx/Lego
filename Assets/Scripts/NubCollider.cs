using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NubCollider : MonoBehaviour, IMouseInteractible
{
    private new Renderer renderer;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
    }

    public void OnMouseClick(){ print("click"); }

    public void OnMouseEnter()
    {
        renderer.enabled = true;
    }

    public void OnMouseExit()
    {
        renderer.enabled = false;
    }
}
