using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class UIFormatterScript : MonoBehaviour
{
    [Header("Recipe Settings")]
    public InventoryScript.Recipe recipe;
    InventoryScript.Recipe lastRecipe;
    [SerializeField] GameObject ingredientDisplay;
    List<GameObject> ingredients;

    [Header("Text Settings")]
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI descriptionText;
    
    [Header("Rect Transforms")]
    [SerializeField] RectTransform descriptionTransform;
    [SerializeField] RectTransform materialListTransform;
    [SerializeField] RectTransform backgroundTransform;

    [Header("Other Settings")]
    public InventoryScript inventoryScript;
    [SerializeField] Button craftButton;

    bool initialized;

    // Start is called before the first frame update
    void Start()
    {
        if (!initialized)
        {
            ingredients = new List<GameObject>();
            recipe = new InventoryScript.Recipe();
            recipe.id = "";   
            initialized = true;
        }
    }


    // Update is called once per frame
    void LateUpdate()
    {
        if (recipe.id == "")
        {
            gameObject.SetActive(false);
            backgroundTransform.gameObject.SetActive(false);
        }
        if (ingredients.Count != 0)
        {
            foreach (InventoryScript.InventoryItem requirement in recipe.requirements)
            {
                foreach (GameObject ingredient in ingredients)
                {
                    int count = 0;
                    foreach (InventoryScript.InventoryItem item in inventoryScript.inventory)
                    {
                        if (item.id == requirement.id) count += item.count;
                    }
                    if (ingredient.GetComponent<UIRecipeScript>().nameText.text == requirement.id) ingredient.GetComponent<UIRecipeScript>().countText.text = count + "/" + requirement.count;
                }
            }
        }
        if (recipe != lastRecipe && recipe.id != "")
        {
            if (ingredients != null)
            {
                for (int i = 0; i < ingredients.Count; i++)
                {
                    Destroy(ingredients[i]);
                }
                ingredients = new List<GameObject>();
            }

            backgroundTransform.gameObject.SetActive(true);
            itemName.text = recipe.id;
            descriptionText.text = recipe.description;

            foreach (InventoryScript.InventoryItem requirement in recipe.requirements)
            {
                GameObject ingredient = Instantiate(ingredientDisplay, materialListTransform);
                ingredient.GetComponent<UIRecipeScript>().nameText.text = requirement.id;
                GameObject display = Instantiate(requirement.gameObject, ingredient.GetComponent<UIRecipeScript>().objectAnchor);
                display.transform.position += requirement.UIPosition;
                display.transform.localEulerAngles = requirement.UIRotation;
                display.transform.localScale = requirement.UIScale;

                ingredients.Add(ingredient);
            }
        
            descriptionText.ForceMeshUpdate();
            descriptionTransform.sizeDelta = new Vector2(descriptionTransform.sizeDelta.x, descriptionText.textInfo.lineCount * 22.5f + 5);
            materialListTransform.sizeDelta = new Vector2(materialListTransform.sizeDelta.x, recipe.requirements.Length * 90);
            backgroundTransform.sizeDelta = new Vector2(backgroundTransform.sizeDelta.x, materialListTransform.sizeDelta.y + descriptionTransform.sizeDelta.y + 225);

            lastRecipe = recipe;
        }
    }

    public void SetRecipe(int i)
    {
        recipe = inventoryScript.recipes[i];
        inventoryScript.selectedRecipe = recipe;
        gameObject.SetActive(true);
    }
}
