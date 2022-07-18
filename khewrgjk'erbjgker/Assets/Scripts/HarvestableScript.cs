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
        public int minCount;
        public int maxCount;
    }

    [Header("Health Settings")]

    public int maxHealth;
    public int health;

    [Header("Drop Settings")]

    [SerializeField] Drop[] drops;

    [Header("Healthbar Settings")]
    [SerializeField] public InGameHealthbar inGameHealthbar;
    [SerializeField] Transform healthbarPosition;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (health <= 0)
        {
            List<GameObject> dropGameobjects = new List<GameObject>(); 
            foreach (Drop drop in drops)
            {
                for (int i = 0; i < Random.Range(drop.minCount, drop.maxCount + 1); i++)
                {
                    dropGameobjects.Add(Instantiate(drop.dropGameobject, healthbarPosition.position, Quaternion.identity));
                }
            }

            foreach (GameObject dropGameobject in dropGameobjects)
            {
                dropGameobject.GetComponent<Rigidbody>().AddExplosionForce(100, transform.position, 5);
            }
            GameManager.instance.allHarvestableObjects.Remove(this);
            Destroy(gameObject);
        }
    }

    public void EnterLook()
    {
        if (inGameHealthbar == null)
        {
            GameObject healthBar = Instantiate(GameManager.instance.inGameHealthbar, healthbarPosition.position, Quaternion.identity);
            healthBar.transform.SetParent(GameManager.instance.transform);
            healthBar.transform.localEulerAngles = Vector3.zero;
            inGameHealthbar = healthBar.GetComponent<InGameHealthbar>();
            inGameHealthbar.harvestableScript = this;
        }
    }
}
