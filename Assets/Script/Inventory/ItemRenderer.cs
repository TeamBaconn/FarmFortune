using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class ItemRenderer : MonoBehaviour
{
    public TMP_Text amountCount;
    public Image imageIcon;
    public Button button;

    private UnityAction onClick, onEnter, onExit;

    private void Awake()
    {
        Render((Item)null, 0);
    }

    public void Render(Item item, int count, UnityAction onClick = null, UnityAction onEnter = null, UnityAction onExit = null)
    {
        Render(item ? item.itemIcon : null, count, onClick, onEnter, onExit);
    }
    public void Render(Sprite icon, int count, UnityAction onClick = null, UnityAction onEnter = null, UnityAction onExit = null)
    {
        if (icon == null || count <= 0)
        {
            amountCount.text = "";
            imageIcon.gameObject.SetActive(false);
            this.onClick = null;
            return;
        }
        amountCount.text = NumberExtensions.FormatNumber(count, 0);
        imageIcon.sprite = icon;
        imageIcon.gameObject.SetActive(true);

        this.onClick = onClick;
        this.onEnter = onEnter;
        this.onExit = onExit;
    }

    public void OnPointerDown()
    {
        onClick?.Invoke();
    }
    public void OnPointerEnter()
    {
        onEnter?.Invoke();
    }
    public void OnPointerExit()
    {
        onExit?.Invoke();
    }
}
