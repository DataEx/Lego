using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOver : MonoBehaviour
{
    private Camera activeCamera;
    private IMouseInteractible mouseOverObject = null;

    private void Awake()
    {
        activeCamera = Camera.main;
    }

    void Update()
    {
        Ray ray = activeCamera.ScreenPointToRay(Input.mousePosition);
        IMouseInteractible currentMouseOverObj = null;
        if (Physics.Raycast(ray, out RaycastHit hit)) {
            currentMouseOverObj = hit.transform.GetComponent<IMouseInteractible>();
        }

        if (mouseOverObject != currentMouseOverObj) {
            if (mouseOverObject != null) {
                mouseOverObject.OnMouseExit();
            }

            mouseOverObject = currentMouseOverObj;

            if (mouseOverObject != null)
            {
                mouseOverObject.OnMouseEnter();
            }
        }

        if (mouseOverObject != null && Input.GetMouseButtonDown(0)) {
            mouseOverObject.OnMouseClick();
        }

    }
}
