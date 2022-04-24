using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingProps : MonoBehaviour
{
    public List <GameObject> props = new List<GameObject>();
    public StackManager stackManager;
    // Start is called before the first frame update
    void Start()
    {
        stackManager = GameObject.FindGameObjectWithTag("Stack").GetComponent<StackManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PutProps()
    {
        foreach(GameObject prop in props)
        {
            for(int i = 0; i <  prop.transform.childCount ; i++)
            {
                prop.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
    public void CleanProps()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            for (int o = 0; o < transform.GetChild(i).childCount; o++)
            {
                transform.GetChild(i).GetChild(o).gameObject.SetActive(false);
            }
               
        }
    }
   
    

    
}
