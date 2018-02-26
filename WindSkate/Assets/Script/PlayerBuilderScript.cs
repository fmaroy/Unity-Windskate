using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuilderScript : MonoBehaviour {

    public PersistentParameters sceneData;
    public PlayerProperties thisPlayerProps;
    //Boards def
    public GameObject boardGeometryContainer;
    public GameObject boardModel;
    public int currentBoard = 0;


	// Use this for initialization
	void Start () {
        if (this.GetComponent<ExternalObjectsReference>().SceneData != null)
        {
            sceneData = this.GetComponent<ExternalObjectsReference>().SceneData;
        }
        else
        {
            Debug.Log("can't find scene object !");
        }

	}
	
    public void buildPlayer()
    {
         
        //instantiante board
        InstantiateGear(sceneData.boardList[0].prefab, boardGeometryContainer);
        //apply board looks


    }

    public void InstantiateGear (GameObject prefab, GameObject parent)
    {
        Debug.Log("Instantiating " + prefab.name);
        boardModel = (GameObject)Instantiate(prefab, new Vector3 (0f, 0f, 0f) , parent.transform.rotation);
        boardModel.transform.parent = parent.transform;
        applyBoardLooks(boardModel, thisPlayerProps.board, thisPlayerProps.boardLook);
    }

    public void applyBoardLooks(GameObject obj, int boardId, int lookId)
    {
        Material[] mats = obj.GetComponent<MeshRenderer>().materials;
        for (int j = 0; j < mats.Length; j++)
        {
            // for board, the first material slot is for the mast base. The if checks the first slot and aplies it's own material
            if (j == 0) { mats[j] = obj.GetComponent<MeshRenderer>().materials[0]; }
            else
            {
                mats[j] = sceneData.boardList[boardId].looks[lookId].lookList[j - 1];
            }
        }
        // for board the looks has to be appplyied on the sub element
        obj.GetComponent<MeshRenderer>().materials = mats;
    }

    public void destroyBoardModel ()
    {
        //boardModel.Destroy();
    }

	// Update is called once per frame
	void Update () {
		
	}
}
