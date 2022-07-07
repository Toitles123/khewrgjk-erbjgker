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

        List<string> inventoryIDs = new List<string>(inventory.Count);
        
        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryIDs[i] = inventory[i].id;
        }

        print(inventoryIDs);

        foreach (Recipe recipe in recipes)
        {
            if (recipe.name == name) selectedRecipe = recipe;
        }
        foreach (Item requirement in selectedRecipe.requirements)
        {
            if (inventoryIDs.Contains(requirement.id) && inventory[inventoryIDs.IndexOf(requirement.id)].count >= requirement.count) break;
            else canCraft = false;
        }
        if (canCraft)
        {
            foreach (Item requirement in selectedRecipe.requirements)
            {
                foreach (Item item in inventory)
                {
                    if (inventory[inventory.IndexOf(item)].count - requirement.count == 0) inventory.Remove(requirement);
                    else inventory[inventory.IndexOf(item)].count -= requirement.count;
                }
            }
            inventory.Add(selectedRecipe.outcome);
        }
    }

}
