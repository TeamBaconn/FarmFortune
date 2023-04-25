using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    public TextAsset itemConfig;

    public List<Item> itemList;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this; 
    }

#if UNITY_EDITOR
    [ContextMenu("Load Data")]
    public void LoadData()
    {
        itemList = new List<Item>();
        string[] guids = AssetDatabase.FindAssets("t:Item", new[] { Global.ITEM_CONFIG_PATH });

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            Item item = AssetDatabase.LoadAssetAtPath<Item>(assetPath);

            if (item != null)
            {
                itemList.Add(item);
            }
        }
        Debug.Log($"Loaded {itemList.Count} items");
    }
#endif
    
    // I demo the load CSV here
    //private void LoadData()
    //{
    //    itemList = new List<Item>();
    //    ConfigData config = GameConfigLoader.LoadData(itemConfig);
    //    if (config == null) return;
    //    for(int i = 0; i < config.GetLength(); i++)
    //    {
    //        if (TryParse(config, i, out Item newItem)) itemList.Add(newItem);
    //        else Destroy(newItem);
    //    }
    //    Debug.Log($"[SYSTEM] Loaded {itemList.Count} items");
    //}

    //private bool TryParse(ConfigData config, int i, out Item newItem)
    //{
    //    newItem = null;
    //    try
    //    {
    //        string itemType = config.GetDataAttribute(i, "item_type");
    //        if (itemType == null) return false;

    //        switch (itemType)
    //        {
    //            case "SEED_ITEM":
    //                SeedItem seedItem = ScriptableObject.CreateInstance<SeedItem>();
    //                break;
    //            default:
    //                newItem = ScriptableObject.CreateInstance<Item>();
    //                break;
    //        };


    //        string id = config.GetDataAttribute(i, "id");
    //        string itemName = config.GetDataAttribute(i, "item_name");
    //        string itemIcon = config.GetDataAttribute(i, "item_icon");
    //        if (itemName == null || itemIcon == null || id == null)
    //            return false;
    //        newItem.itemName = config.GetDataAttribute(i, "item_name");

    //        Sprite sprite = Resources.Load<Sprite>(itemIcon);
    //        if (sprite == null)
    //            return false;
    //        newItem.itemIcon = sprite;

    //        int itemID = int.Parse(id);
    //        newItem.id = itemID;
    //        return true;
    //    }
    //    catch(Exception) { return false; }
    //}

    public static Item GetItem(int id)
    {
        foreach (Item item in Instance.itemList) if (item.id == id) return item;
        return null;
    }

    public static Item GetSeedFromPlant(FarmEntityData data)
    {
        if (data == null) return null;
        foreach (Item item in Instance.itemList) if (item is SeedItem seedItem && seedItem.seedData.Equals(data)) return item;
        return null;
    }

    public static bool ContainsItem(Item item)
    {
        return Instance.itemList.Contains(item);
    }
}
