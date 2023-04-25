using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryRenderer : MonoBehaviour
{
    [Header("Config")]
    public float extendedWidth = 380;
    public float unextendedWidth = 145;
    public int extendedColumnCount = 3;
    public int unextendedColumnCount = 1;

    public Player targetPlayer;

    [Header("Prefab")]
    public GameObject itemRendererPrefab;
    public GameObject itemHolder;
    public Button extendButton;

    private bool extended = false;
    private GridLayoutGroup grid;
    private ItemRenderer[] itemRenderers;

    private CursorMessage currentMessage;

    private void Awake()
    {
        itemRenderers = new ItemRenderer[Global.MAX_SLOT];
        for(int i = 0; i < itemRenderers.Length; i++)
        {
            GameObject newPrefab = Instantiate(itemRendererPrefab, itemHolder.transform);
            newPrefab.SetActive(true);
            itemRenderers[i] = newPrefab.GetComponent<ItemRenderer>();
        }
        grid = itemHolder.GetComponent<GridLayoutGroup>();
        
        SetExtend();

        extendButton.onClick.AddListener(() => SetExtend(true));
    }

    private void OnEnable()
    { 
        EventManager.StartListening<OnInventoryUpdate>(OnInventoryUpdate);
    }

    private void OnDisable()
    {
        EventManager.StopListening<OnInventoryUpdate>(OnInventoryUpdate);
    }

    private void SetExtend(bool toggle = false)
    {
        if(toggle)
            extended = !extended;

        Rect rect = gameObject.GetComponent<RectTransform>().rect;
        if (extended)
        {
            rect.width = extendedWidth;
            grid.constraintCount = extendedColumnCount;
            extendButton.transform.localScale = new Vector3(1, 1, 1);

        }else
        {
            rect.width = unextendedWidth;
            grid.constraintCount = unextendedColumnCount;
            extendButton.transform.localScale = new Vector3(-1, 1, 1);
        }
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(rect.width, rect.height);
    }

    private void OnInventoryUpdate(EventParam param)
    { 
        OnInventoryUpdate eventParam = param as OnInventoryUpdate;
        if (!eventParam.player.Equals(targetPlayer)) return;
        ClearMessage();
        RenderItem(eventParam.updatedInventory);
    }

    public void RenderItem(Dictionary<Item, int> itemList)
    {
        int slotCursor = 0;
        itemList = SortDictionaryByItemId(itemList);
        foreach(Item item in itemList.Keys)
        {
            if(slotCursor >= itemRenderers.Length)
            {
                Debug.LogWarning("Too much item to hold");
                return;
            }
            int amount = itemList[item];
            UnityAction OnPointerClick = () =>
            {
                OnItemPickUp pickEvent = new OnItemPickUp(item, targetPlayer, amount);
                EventManager.TriggerEvent(pickEvent);
            };

            UnityAction OnPointerEnter = () =>
            {
                currentMessage = new CursorMessage($"{item.itemName}",$"Amount: {NumberExtensions.FormatNumber(amount)}\n" + item.GetDescription());
                EventManager.TriggerEvent(new OnCursorMessageRequest(currentMessage, true));
            };

            UnityAction OnPointerExit = () =>
            {
                ClearMessage();
            };

            itemRenderers[slotCursor].Render(item, amount, OnPointerClick, OnPointerEnter, OnPointerExit);
            slotCursor++;
        }
        for(int i = itemList.Keys.Count; i < itemRenderers.Length; i++)
        {
            itemRenderers[i].Render((Item)null, 0);
        }
    }

    private void ClearMessage()
    {
        if (currentMessage == null) return;
        EventManager.TriggerEvent(new OnCursorMessageRequest(currentMessage, false));
        currentMessage = null;
    }

    public static Dictionary<Item, int> SortDictionaryByItemId(Dictionary<Item, int> dictionary)
    {
        var sortedList = new List<KeyValuePair<Item, int>>(dictionary);
        sortedList.Sort((x, y) => x.Key.id.CompareTo(y.Key.id));

        var sortedDictionary = new Dictionary<Item, int>();
        foreach (var kvp in sortedList)
        {
            sortedDictionary.Add(kvp.Key, kvp.Value);
        }

        return sortedDictionary;
    }
}
