using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopRenderer : MonoBehaviour
{
    public GameObject itemPrefab;
    public GameObject shopContent;
    public TMP_Text shopName;
    public Button closeButton;

    public CursorMessage currentMessage;

    private void Start()
    {
        closeButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            CursorRenderer.Instance.SetMessageIndex(0);
        });
    }

    private void ClearShop()
    {
        foreach(Transform child in shopContent.transform)
        {
            Destroy(child.gameObject);
        }
    }
    public void RenderShop(Shop shop, Player player)
    {
        gameObject.SetActive(true);
        ClearShop();
        CursorRenderer.Instance.SetMessageIndex(1);

        shopName.text = shop.shopName;
        foreach(ShopStock stock in shop.buyableItems)
        {
            GameObject newItemPrefab = Instantiate(itemPrefab, shopContent.transform);

            UnityAction OnPointerClick = () =>
            {
                if (!player.HasMoney(stock.buyPrice)) return;
                player.AddMoney(-1 * stock.buyPrice);
                stock.AddStock(player);
            };

            UnityAction OnPointerEnter = () =>
            {
                currentMessage = new CursorMessage($"{stock.GetStockName()}", stock.GetDescription(), 1);
                EventManager.TriggerEvent(new OnCursorMessageRequest(currentMessage, true));
            };

            UnityAction OnPointerExit = () =>
            {
                ClearMessage();
            };

            newItemPrefab.GetComponent<ItemRenderer>().Render(stock.GetStockIcon(), stock.buyPrice, OnPointerClick, OnPointerEnter, OnPointerExit);
        }
    }

    private void ClearMessage()
    {
        if (currentMessage == null) return;
        EventManager.TriggerEvent(new OnCursorMessageRequest(currentMessage, false));
        currentMessage = null;
    }
}
