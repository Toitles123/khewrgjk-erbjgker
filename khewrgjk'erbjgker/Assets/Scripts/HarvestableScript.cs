using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestableScript : MonoBehaviour
{
    [System.Serializable]
    public class Drop
    {
        public string name;
        public GameObject dropGameobject;
        public int count;
    }

    [Header("Health Settings")]

    [SerializeField] int maxHealth;
    public int health;

    [Header("Drop Settings")]

    [SerializeField] Drop[] drops;

    [Header("Healthbar Settings")]
    [SerializeField] InGameHealthbar inGameHealthbar;
    [SerializeField] Transform healthbarPosition;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            List<GameObject> dropGameobjects = new List<GameObject>(); 
            foreach (Drop drop in drops)
            {
                for (int i = 0; i < drop.count; i++)
                {
                    dropGameobjects.Add(Instantiate(drop.dropGameobject, healthbarPosition.position, Quaternion.identity));
                }
            }

            foreach (GameObject dropGameobject in dropGameobjects)
            {
                dropGameobject.GetComponent<Rigidbody>().AddExplosionForce(100, transform.position, 5);
            }
            Destroy(gameObject);
        }
    }

    public void EnterLook()
    {
        if (inGameHealthbar == null)
        {
            GameObject healthBar = Instantiate(GameManager.instance.inGameHealthbar, healthbarPosition.position, Quaternion.identity);
            healthBar.transform.SetParent(transform);
            inGameHealthbar = healthBar.GetComponent<InGameHealthbar>();
        }
        else
        {
            inGameHealthbar.healthbarImage.fillAmount = (float) health / (float) maxHealth;
            inGameHealthbar.healthbarText.text = health + " / " + maxHealth;
        }
    }

    void OnMouseExit()
    {
        if (inGameHealthbar != null)
        {
            Destroy(inGameHealthbar.gameObject);
            inGameHealthbar = null;
        }
    }

}
