using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
    private BuildingProps buildingProps;
  //  public bool inFloor;
    // Start is called before the first frame update
    void Start()
    {
        buildingProps = transform.GetComponentInParent<BuildingProps>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
  
   /* private void OnTriggerEnter(Collider other)
    {
        print("Collision" + buildingProps.stackManager.StackIndex);
         if (other.transform.parent.gameObject == buildingProps.stackManager.TheStack[buildingProps.stackManager.StackIndex])
         {
            if (buildingProps.props.Contains(gameObject) )
            {
                buildingProps.props.Remove(gameObject);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        print("OutCollision");
        if (!buildingProps.props.Contains(gameObject) && inFloor == true)
        {
            buildingProps.props.Add(gameObject);
        }
       
    }*/
    private void OnTriggerStay(Collider other)
    {
       
        
        if(other.gameObject == transform.parent.parent.GetChild(0).gameObject)
        {
           // inFloor = true;
            if (!buildingProps.props.Contains(gameObject) )
            {
                buildingProps.props.Add(gameObject);
            }
        }
        else
        {
            if (buildingProps.props.Contains(gameObject))
            {
                buildingProps.props.Remove(gameObject);
            }
           // inFloor = false;
        }
    }

}
