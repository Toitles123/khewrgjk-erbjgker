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
    [SerializeField] Recipe[] recipes;
    [SerializeField] List<Item> inventory;  

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
        }
    }

    public void Craft(string name)
    {
        bool canCraft = true;
        Recipe selectedRecipe = null;

        List<string> inventoryIDs = new List<string>();
        
        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryIDs.Add(inventory[i].id);
        }

        foreach (Recipe recipe in recipes)
        {
            if (recipe.name == name) selectedRecipe = recipe;
        }
        for (int i = 0; i < selectedRecipe.requirements.Length; i++)
        {
            if (inventoryIDs.Contains(selectedRecipe.requirements[i].id) && inventory[inventoryIDs.IndexOf(selectedRecipe.requirements[i].id)].count >= selectedRecipe.requirements[i].count) break;
            else canCraft = false;
            print(inventoryIDs.Contains(selectedRecipe.requirements[i].id));
            print(i == inventoryIDs.IndexOf(selectedRecipe.requirements[i].id));
            print(inventory[inventoryIDs.IndexOf(selectedRecipe.requirements[i].id)].count >= selectedRecipe.requirements[i].count);
        }
        if (canCraft)
        {
            List<Item> tempInventory = new List<Item>(inventory);
            foreach (Item requirement in selectedRecipe.requirements)
            {
                foreach (Item item in tempInventory)
                {
                    if (requirement.id == item.id)
                    {
                        if (item.count - requirement.count == 0) tempInventory.RemoveAt(tempInventory.IndexOf(item));
                        else item.count -= requirement.count;
                    }
                }
            }
            inventory = tempInventory;
            inventory.Add(selectedRecipe.outcome);
        }
    }

}