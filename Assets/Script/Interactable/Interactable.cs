using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Interactable : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    protected bool isInteractable = false;
    protected int enableThickness = 1;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material.SetFloat("_OutlineThickness", 0);
    }

    protected virtual void OnMouseEnter()
    {
        EventManager.TriggerEvent(new OnInteractableHighlight(this, true));
        spriteRenderer.material.SetFloat("_OutlineThickness", 1);
        isInteractable = true;
    }

    protected virtual void OnMouseExit()
    { 
        EventManager.TriggerEvent(new OnInteractableHighlight(this, false));
        spriteRenderer.material.SetFloat("_OutlineThickness", 0);
        isInteractable = false;
    }

    protected void SetColor(Color color)
    {
        spriteRenderer.material.SetColor("_OutlineColor", color);
    }
}
