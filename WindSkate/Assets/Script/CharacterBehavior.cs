using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterBehavior : MonoBehaviour {

    private GameObject Armature;
    //public bool ragdollFlag = false;
    public List<GameObject> CharacterArmatureList;
    private List<GameObject> currentTreeGameObject;
    public GameObject animationGameObject;


    // Use this for initialization
    void Start() {
        Armature = recusiveSearch("Armature", this.gameObject);
        animationGameObject = this.gameObject;

        List < GameObject > templistGameObjects= getSubChildren(Armature);
        foreach (GameObject child in templistGameObjects)
        {
            List<GameObject> templistGameObjects1 = getSubChildren(child);
            foreach (GameObject subchild in templistGameObjects1)
            {
                List<GameObject> templistGameObjects2 = getSubChildren(subchild);
                foreach (GameObject subsubchild in templistGameObjects2)
                {
                    List<GameObject> templistGameObjects3 = getSubChildren(subsubchild);

                }
            }

        }
        DisableRagdoll();
        

    }

    public List<GameObject> getSubChildren(GameObject currentObject)
    {
        currentTreeGameObject = new List<GameObject>();
        foreach (Transform child in currentObject.transform)
        {
            currentTreeGameObject.Add(child.gameObject);
            CharacterArmatureList.Add(child.gameObject);
            
        }
            return currentTreeGameObject;
    }
    
    public void DisableRagdoll()
    {
        foreach (GameObject currentBone in CharacterArmatureList)
        {
            if (currentBone.GetComponent<Rigidbody>() != null)
            {
                currentBone.GetComponent<Rigidbody>().isKinematic = true;
                //currentBone.GetComponent<Rigidbody>().detectCollisions = true;
            }
            if (currentBone.GetComponent<Collider>() != null)
            {
                if (currentBone.name == "Torso_00" || currentBone.name == "Head")
                {
                    currentBone.GetComponent<Collider>().enabled = true;
                    currentBone.GetComponent<Collider>().isTrigger = false;
                    
                }
                else
                {
                    currentBone.GetComponent<Collider>().enabled = false;
                    currentBone.GetComponent<Collider>().isTrigger = false;
                }
            }
        }
        //animCharacter.enabled= true;
    }

    public void EnableRagdoll(Vector3 velocity, Vector3 angularVelocity)
    {
      
        foreach (GameObject currentBone in CharacterArmatureList)
        {
            if (currentBone.GetComponent<Rigidbody>() != null)
            {
                currentBone.GetComponent<Rigidbody>().isKinematic = false;
                currentBone.GetComponent<Rigidbody>().detectCollisions = true;
                
                currentBone.GetComponent<Rigidbody>().velocity = velocity;
                currentBone.GetComponent<Rigidbody>().angularVelocity = angularVelocity;
            }
            

        if (currentBone.GetComponent<Collider>() != null)
        {
            if (currentBone.name == "Torso__" || currentBone.name == "Head")
            {
                currentBone.GetComponent<Collider>().enabled = true;
                currentBone.GetComponent<Collider>().isTrigger = false;
            }
            else
            {
                currentBone.GetComponent<Collider>().enabled = true;
                currentBone.GetComponent<Collider>().isTrigger = false;
            }
        }
    }
    //animCharacter.enabled= false;
}   

public GameObject recusiveSearch(string nameToFind, GameObject gameObjectToBrowse)
    {

        foreach (Transform child in gameObjectToBrowse.transform)
        {
            if (child.gameObject.name == nameToFind)
            {
                return child.gameObject;
            }
            else
            {
                foreach (Transform subchild in child)
                {
                    if (subchild.gameObject.name == nameToFind)
                    {
                        return subchild.gameObject;
                    }
                    else
                    {
                        foreach (Transform subsubchild in subchild)
                        {

                            if (subsubchild.gameObject.name == nameToFind)
                            {
                                return subsubchild.gameObject;
                            }
                        }
                    }
                }
            }
        }
        return this.gameObject;
    }

}
