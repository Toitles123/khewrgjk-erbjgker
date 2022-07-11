using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    [System.Serializable]
    public class Item
    {
        public string id;
        public int count;
        public GameObject gameObject;
        public int maxStack;
        public Vector3 UIPosition;
        public Vector3 UIRotation;
        public Vector3 UIScale = Vector3.one * 100;
        
        public bool checkMaxStack()
        {
            bool isMaxStack = false;
            if (count == 0) return false;
            if (count == maxStack) isMaxStack = true;
            return isMaxStack;
        }
    }

    [System.Serializable]
    public class Recipe
    {
        public string name;
        public string description;
        public Item[] requirements;
        public Item outcome;
    }

    [SerializeField] PlayerController playerController;
    public Recipe[] recipes;
    public List<Item> inventory;
    public GameObject inventoryGameobject;
    public static bool inventoryOpen;
    public Recipe selectedRecipe;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            playerController.lockCursor = !playerController.lockCursor;
            inventoryGameobject.SetActive(!inventoryGameobject.activeSelf);
            inventoryOpen = !inventoryOpen;
        }
    }

    public void Craft()
    {
        List<string> inventoryIDs = new List<string>(10);
        
        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryIDs.Add(inventory[i].id);
        }

        for (int i = 0; i < selectedRecipe.requirements.Length; i++)
        {
            if (inventoryIDs.Contains(selectedRecipe.requirements[i].id) && inventory[inventoryIDs.IndexOf(selectedRecipe.requirements[i].id)].count >= selectedRecipe.requirements[i].count) continue;
            else return;
        }
        if (inventory.Contains(selectedRecipe.outcome))
        {
            if (inventory[inventoryIDs.IndexOf(selectedRecipe.outcome.id)].checkMaxStack())
            {
                bool addedItem = false;
                foreach (Item item in inventory)
                {
                    if (item.id == "")
                    {
                        item.id = selectedRecipe.outcome.id;
                        item.count = selectedRecipe.outcome.count;
                        item.gameObject = selectedRecipe.outcome.gameObject;
                        item.maxStack = selectedRecipe.outcome.maxStack;
                        addedItem = true;
                        break;
                    }
                }
                if (!addedItem) return;
            }
            else inventory[inventoryIDs.IndexOf(selectedRecipe.outcome.id)].count += selectedRecipe.outcome.count;
        }
        else
        {
            bool addedItem = false;
            foreach (Item item in inventory)
            {
                if (item.id == "")
                {
                    item.id = selectedRecipe.outcome.id;
                    item.count = selectedRecipe.outcome.count;
                    item.gameObject = selectedRecipe.outcome.gameObject;
                    item.maxStack = selectedRecipe.outcome.maxStack;
                    addedItem = true;
                    break;
                }
            }
            if (addedItem)
            {
                List<Item> tempInventory = new List<Item>(inventory);
                List<string> itemsRemovedIDs = new List<string>(); 
                int itemsRemoved = 0;
                foreach (Item requirement in selectedRecipe.requirements)
                {
                    foreach (Item item in tempInventory)
                    {
                        if (requirement.id == item.id && !itemsRemovedIDs.Contains(item.id))
                        {
                            if (item.count - requirement.count == 0) 
                            {
                                int index = tempInventory.IndexOf(item) - itemsRemoved;
                                inventory[index].id = "";
                                inventory[index].count = 0;
                                inventory[index].gameObject = null;
                                inventory[index].maxStack = 0;
                                itemsRemoved++;
                            }
                            else item.count -= requirement.count;
                            itemsRemovedIDs.Add(item.id);
                        }
                    }
                }
            }
        }
    }
}
