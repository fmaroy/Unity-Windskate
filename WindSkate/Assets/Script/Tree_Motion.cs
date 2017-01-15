using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tree_Motion : MonoBehaviour {
	
	private SkinnedMeshRenderer TreeSkinnedMesh;
	private Mesh TreeMesh ;

	// Use this for initialization
	void Start () 
	{
		
		foreach (Transform child in transform)
		{
            if (child.gameObject.CompareTag("Tree"))
            {
                //Debug.Log("Tree Found");
                TreeSkinnedMesh = child.gameObject.GetComponent<SkinnedMeshRenderer>();
                TreeMesh = child.gameObject.GetComponent<MeshFilter>().mesh;
                for (var n = 2; n < TreeMesh.blendShapeCount ; n++)
                {
                   TreeSkinnedMesh.SetBlendShapeWeight(n, (Random.value * 100));
                }
            }
               /* treeShapeList = new List<float>();
				
				
					TreeList.Add(child.gameObject);
				    TreeSkinnedMeshList.Add(child.gameObject.GetComponent<SkinnedMeshRenderer>());
						TreePhase.Add(Random.value*10);
						TreeMesh = child.gameObject.GetComponent<MeshFilter>().mesh;
						for(var n = 2; n < TreeMesh.blendShapeCount; n++)
						{
							treeShapeList.Add(Random.value * 100);
						}
						treeId ++;
					}
					TreeSkinnedMesh = child.gameObject.GetComponent<SkinnedMeshRenderer>();
                    //Debug.Log(subchild.gameObject.name + TreeMesh.blendShapeCount);
					for(var n = 2; n < TreeMesh.blendShapeCount; n++)
					{
						TreeSkinnedMesh.SetBlendShapeWeight(n, treeShapeList[n-2]);
					}	
				*/
			
			
		}
	}
	
	// Update is called once per frame
	/*void Update () {
		int treeId = 0;
		foreach (GameObject tree in TreeList)
		{
				//Debug.Log(tree.name);
				float local_time_tree_phase=Time.time+TreePhase[treeId];
				TreeSkinnedMeshList[treeId].SetBlendShapeWeight(0, (Mathf.Sin(local_time_tree_phase*10)*50)+50);
				TreeSkinnedMeshList[treeId].SetBlendShapeWeight(1, (Mathf.Sin(local_time_tree_phase*2)*50)+50);
			
		treeId ++;
		}
	}*/

}
