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

    // Start is called before the first frame update
    void Start()
    {
        recipe = inventoryScript.recipes[0];
        ingredients = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (recipe.name == "")
        {
            backgroundTransform.gameObject.SetActive(false);
        }
        if (recipe != lastRecipe)
        {
            if (ingredients != null)
            {
                foreach (GameObject ingredient in ingredients)
                {
                    Destroy(ingredient);
                }
            }

            backgroundTransform.gameObject.SetActive(true);
            itemName.text = recipe.name;
            descriptionText.text = recipe.description;

            foreach (InventoryScript.Item requirement in recipe.requirements)
            {
                int count = 0;
                foreach (InventoryScript.Item item in inventoryScript.inventory)
                {
                    if (item.id == requirement.id) count += item.count;
                }
                GameObject ingredient = Instantiate(ingredientDisplay, materialListTransform);
                ingredient.GetComponent<UIRecipeScript>().nameText.text = requirement.id;
                ingredient.GetComponent<UIRecipeScript>().countText.text = count + "/" + requirement.count;
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
        print(recipe);
        inventoryScript.selectedRecipe = recipe;
        print(inventoryScript.selectedRecipe);
        print(inventoryScript.selectedRecipe == recipe);
    }
}
