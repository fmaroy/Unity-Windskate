using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WheelsBehavior : MonoBehaviour {

    //WheelsPhysicsDetails 0 means that all 4 wheels gets the same values, 1 that physics a split front and rear axis, and 2 that all wheels are independantly calculated
	public GameObject RaceManager;
	public int WheelsPhysicsDetails = 2;
    public bool WheelSkidMarksEnabled = true;
    public bool tireParticlesEnabled = true;
    public List<GameObject> WheelsList;
	public List<int> WheelsSurfaceTypeList = new List<int> ();
    public List<WheelCollider> WheelsCollidersList = new List<WheelCollider>();
    public List<GameObject> SkidMarksAsphaltList = new List<GameObject>();
    public List<GameObject> SkidMarksSandList = new List<GameObject>();
    public List<GameObject> TireParticlesObject = new List<GameObject>();
    //public GameObject TireWideParticleObject;
    public List<ParticleSystem> TireParticlesSystem = new List<ParticleSystem>();
    private GameObject currentTerrain;
    private TerrainFXData currentTerrainData;
	private TerrainFXData surfaceTypeData;
    public float SkidSlipThreshold = 0.2f;
    //public float sandGripFactor = 0.6f;
    //public float mudGripFactor = 0.8f;
    private List<float> wheelsSurfaceGripFactors  ;
    private WheelFrictionCurve sidewaysFriction;
    public List<GameObject> ObjectForPhysicsUpdate = new List<GameObject>();
    private List<float> wheelsSurfaceDragFactors;
    private float BoardDragInit;

	public float height;

    // Use this for initialization
    void Start() {
		RaceManager = GameObject.Find ("RaceManager");
		surfaceTypeData = RaceManager.GetComponent<TerrainFXData> ();
		//currentTerrain = RaceManager.GetComponent <RaceManagerScript >().thisLevelTerrain;
        //currentTerrainData = currentTerrain.GetComponent<TerrainFXData>();
		wheelsSurfaceDragFactors = surfaceTypeData.surfaceDragFactors;
		wheelsSurfaceGripFactors = surfaceTypeData.surfaceGripFactors;
        //build WheelList

        buildWheelList();
        /*WheelsList = new List<GameObject>();
        WheelsList.Add(this.gameObject.GetComponent<BoardForces>().recusiveSearch("Wheel_FL", this.gameObject.GetComponent<BoardForces>().front_axis));
        WheelsList.Add(this.gameObject.GetComponent<BoardForces>().recusiveSearch("Wheel_FR", this.gameObject.GetComponent<BoardForces>().front_axis));
        WheelsList.Add(this.gameObject.GetComponent<BoardForces>().recusiveSearch("Wheel_RL", this.gameObject.GetComponent<BoardForces>().rear_axis));
        WheelsList.Add(this.gameObject.GetComponent<BoardForces>().recusiveSearch("Wheel_RR", this.gameObject.GetComponent<BoardForces>().rear_axis));
        */
        for (int i = 0; i < 4; i++)
        {
            WheelsCollidersList.Add(WheelsList[i].GetComponent<WheelCollider>());
			WheelsSurfaceTypeList.Add (0);
            foreach (Transform child in WheelsList[i].transform)
            {
                if (child.gameObject.name.Contains("SkidTrailAsphalt") == true)
                {
                    child.gameObject.SetActive(false);
                    SkidMarksAsphaltList.Add(child.gameObject);
                }
                if (child.gameObject.name.Contains("SkidTrailSand") == true)
                {
                    child.gameObject.SetActive(false);
                    SkidMarksSandList.Add(child.gameObject);
                }
                if (child.gameObject.name.Contains("Wheel Particle System") == true)
                {
                    //child.gameObject.SetActive(false);
                    TireParticlesObject.Add(child.gameObject);
                    TireParticlesSystem.Add(child.gameObject.GetComponent<ParticleSystem>());
                    child.gameObject.GetComponent<ParticleSystem>().Stop();
                }
            }
        }

        sidewaysFriction = WheelsCollidersList[0].sidewaysFriction;
        if (WheelsPhysicsDetails == 2)
        {
            ObjectForPhysicsUpdate = WheelsList;
        }
        if (WheelsPhysicsDetails == 1)
        {
            for (int i = 0; i < 4; i = i + 2)
            {
                ObjectForPhysicsUpdate.Add(WheelsList[i].transform.parent.gameObject);
            }
        }
        if (WheelsPhysicsDetails == 0)
        {
            ObjectForPhysicsUpdate.Add(this.gameObject);
        }

        BoardDragInit = this.gameObject.GetComponent<Rigidbody>().drag;
    }

    public void buildWheelList ()
    {
        WheelsList = new List<GameObject>();
        WheelsList.Add(this.gameObject.GetComponent<BoardForces>().recusiveSearch("Wheel_FL", this.gameObject.GetComponent<BoardForces>().front_axis));
        WheelsList.Add(this.gameObject.GetComponent<BoardForces>().recusiveSearch("Wheel_FR", this.gameObject.GetComponent<BoardForces>().front_axis));
        WheelsList.Add(this.gameObject.GetComponent<BoardForces>().recusiveSearch("Wheel_RL", this.gameObject.GetComponent<BoardForces>().rear_axis));
        WheelsList.Add(this.gameObject.GetComponent<BoardForces>().recusiveSearch("Wheel_RR", this.gameObject.GetComponent<BoardForces>().rear_axis));

    }

    public List<GameObject> updateWheelPhysicsDetails(int Wheeldetails)
    {
        List<GameObject> ObjectList = new List<GameObject>();

        buildWheelList();

        if (Wheeldetails == 2)
        {
            ObjectList = WheelsList;

        }
        if (Wheeldetails == 1)
        {
            for (int i = 0; i < 4; i = i + 2)
            {
                //Debug.Log("i in Wheel behavior: " + i);
                //Debug.Log("WheelList : " + WheelsList[i].name);
                ObjectList.Add(WheelsList[i].transform.parent.gameObject);
            }
        }
        if (Wheeldetails == 0)
        {
            ObjectList.Add(this.gameObject);
        }
        return ObjectList;
    }

	public List<float> getDistanceToGround ()
	{
		Vector3 posCenterFrontAxis = new Vector3(0.0f, 0.0f, 0.0f);
		Vector3 posCenterRearAxis = new Vector3(0.0f, 0.0f, 0.0f);

		List <float> heightList = new List<float>();

		WheelHit FLwheelHit;
		WheelHit FRwheelHit;
		WheelHit RLwheelHit;
		WheelHit RRwheelHit;

		// checking if any front wheel touches the floor
		WheelsCollidersList[0].GetGroundHit(out FLwheelHit);
		WheelsCollidersList[1].GetGroundHit(out FRwheelHit);
		WheelsCollidersList[2].GetGroundHit(out RLwheelHit);
		WheelsCollidersList[3].GetGroundHit(out RRwheelHit);

		if ((WheelsCollidersList [0].isGrounded == false) || (WheelsCollidersList [1].isGrounded == false)) {
			// the front wheels are not touching the ground, calculating the distance to ground
			posCenterFrontAxis = Vector3.Lerp (WheelsCollidersList [0].gameObject.transform.position, WheelsCollidersList [1].gameObject.transform.position, 0.5f);
			heightList.Add(posCenterRearAxis.y);
		} 
		else 
		{
			heightList.Add(0.0f);
		}

		if ((WheelsCollidersList[2].isGrounded == false) || (WheelsCollidersList[3].isGrounded == false)) {
			// the front wheels are not touching the ground, calculating the distance to ground
			posCenterRearAxis =  Vector3.Lerp(WheelsCollidersList[2].gameObject.transform.position,WheelsCollidersList[3].gameObject.transform.position, 0.5f);
			heightList.Add(posCenterFrontAxis.y);
		} 
		else 
		{
			heightList.Add(0.0f);
		}

		return heightList;
	}

	public void getLocalSurface(Vector3 pos, out float [] mix, out int surfType, out float height)
	{
		float[] surfaceTypeMix;
		float verticalPos = 0;
		RaycastHit hit;
		Vector3 castdir = Vector3.down;
		if (Physics.Raycast (pos + Vector3.up * 10.0f, castdir, out hit, 30)) {
			verticalPos = hit.collider.gameObject.transform.transform.position.y;
			if (hit.collider.GetComponent<Terrain> () != null) {
				mix = TerrainSurface.GetTextureMix (pos, hit.collider.GetComponent<Terrain> ());
				surfType = TerrainSurface.GetMainTexture (pos, hit.collider.GetComponent<Terrain> ());
			} else {
				if (hit.collider.GetComponent<surfaceTypeScript> () != null) {
					surfType = hit.collider.gameObject.GetComponent<surfaceTypeScript> ().surfaceType;
					mix = new float[0];
				} else {
					surfType = 0;
					mix = new float[0];
				}
			}
			height = pos.y - hit.point.y;
		} else {
			height = 0.0f;
			surfType = 0;
			mix = new float[0];
		}
	}
	public void getLocalSurfaceFromWheel(WheelCollider col, out float [] mix, out int surfType)
	{
		float[] surfaceTypeMix;
		float verticalPos = 0;
		WheelHit hit;
		col.GetGroundHit (out hit);
		if (hit.collider != null) {
			if (hit.collider.GetComponent<Terrain> () != null) {
				mix = TerrainSurface.GetTextureMix (hit.point, hit.collider.GetComponent<Terrain> ());
				surfType = TerrainSurface.GetMainTexture (hit.point, hit.collider.GetComponent<Terrain> ());
			} else {
				if (hit.collider.GetComponent<surfaceTypeScript> () != null) {
					surfType = hit.collider.gameObject.GetComponent<surfaceTypeScript> ().surfaceType;
					mix = new float[0];
				} else {
					surfType = 0;
					mix = new float[1];
				}
			}
		} else {
			surfType = 0;
			mix = new float[1];
		}
	}

	/// <summary>
	/// Return wheel side friction value from a terrain mix of textures
	/// </summary>
	/// <param name="mix">Mix.</param>
	/// <param name="sideFriction">Side friction.</param>
	/// <param name="dragFriction">Drag friction.</param>
	void getFrictionFromMix(float [] mix, out float sideFriction)
	{
		float tempsideFriction = 0.0f;
		if (mix.Length > 1) {
			//Debug.Log ("mix.Length " + mix.Length);
			//Debug.Log ("wheelsSurfaceGripFactors.Capacity : " +  wheelsSurfaceGripFactors.Capacity);
			for (int n = 0; n < mix.Length; n++) {
				//Debug.Log ("n: " + n);
				//Debug.Log ("wheelsSurfaceGripFactors : " + wheelsSurfaceGripFactors [n]);
				//Debug.Log ("mix : " + mix [n]);
				tempsideFriction = tempsideFriction + wheelsSurfaceGripFactors [n] * mix [n];
			}
		} else {
			tempsideFriction = 1.0f;
		}
		sideFriction = tempsideFriction;
	}
	/// <summary>
	/// Return wheel side friction value from a terrain mix of textures
	/// </summary>
	/// <param name="mix">Mix.</param>
	/// <param name="sideFriction">Side friction.</param>
	/// <param name="dragFriction">Drag friction.</param>
	void getDragFromMix(float [] mix, out float drag)
	{
		float tempdragFriction = 0.0f;
		if (mix.Length > 1) {
			for (int n = 0; n < mix.Length; n++) {
				tempdragFriction = tempdragFriction + wheelsSurfaceDragFactors [n] * mix [n];
			}
		} else {
			tempdragFriction = 1.0f;
		}
		drag = tempdragFriction;
	}

	// Update is called once per frame
	void Update () {
        //int i = 0;
		float[] surfMix;
		int surfType = 0;

        int RearWheelsCounter = 0;
        for (int i = 0; i < 4; i++)
        {
            ///Wheels Side friction change with terrain handling
            ///
            // following can be used to get the main type of terrain
            //var surfaceIndex = TerrainSurface.GetMainTexture(WheelsList[i].transform.position);

			float localDragValue = 1.0f;
            float localStiffness = 1.0f;
			
            if (WheelsPhysicsDetails == 2)
            {
				getLocalSurfaceFromWheel(WheelsCollidersList[i], out surfMix, out surfType);
				float friction = 1.0f;
				getFrictionFromMix (surfMix, out friction);
				sidewaysFriction.stiffness = friction;
				WheelsCollidersList[i].sidewaysFriction = sidewaysFriction;
            }
            if (WheelsPhysicsDetails == 1)
            {
				if ((i == 0) || (i == 2))
				{
					getLocalSurfaceFromWheel(WheelsCollidersList[i], out surfMix, out surfType);
					float friction = 1.0f;
					getFrictionFromMix (surfMix, out friction);
					sidewaysFriction.stiffness = friction;
					for (int wheelid = 0; wheelid < 2; wheelid++) {
						int id = wheelid + i;
						WheelsCollidersList[id].sidewaysFriction = sidewaysFriction;
					}
				}
            }
           	
			if (i == 0) 
			{
				getLocalSurface (this.transform.position, out surfMix, out surfType, out height);
				getDragFromMix (surfMix, out localDragValue);
				this.gameObject.GetComponent<Rigidbody>().drag = localDragValue * BoardDragInit;

				if (WheelsPhysicsDetails == 0)
				{
					float friction = 1.0f;
					getFrictionFromMix (surfMix, out friction);
					sidewaysFriction.stiffness = friction;
					for (int wheelid = 0; wheelid < 4; wheelid++) {
						WheelsCollidersList[wheelid].sidewaysFriction = sidewaysFriction;
					}
				}
			}

            ///Wheels Skid Marks handling
            ///
            if (WheelSkidMarksEnabled)
            {
				int surfaceIndex = surfType;
                WheelHit wheelHit;
                WheelsCollidersList[i].GetGroundHit(out wheelHit);

                ///Debug.Log(Mathf.Abs(wheelHit.sidewaysSlip));

				SkidMarksAsphaltList[i].GetComponent<TrailRenderer>().material = surfaceTypeData.TerrainSkidMaterials[surfaceIndex];
				if (surfaceTypeData.permanentSkids[surfaceIndex] == false)
                {
                    if (Mathf.Abs(wheelHit.sidewaysSlip) > SkidSlipThreshold)
                    {
                        SkidMarksAsphaltList[i].SetActive(true);
                        //Debug.Log(Mathf.Abs(wheelHit.sidewaysSlip));
                    }
                    else
                    {
                        SkidMarksAsphaltList[i].SetActive(false);
                    }
                }
                else
                {
                    SkidMarksAsphaltList[i].SetActive(true);
                }
                /*if (surfaceIndex == 1)
                {
                    SkidMarksAsphaltList[i].SetActive(true);
                     //Debug.Log(Mathf.Abs(wheelHit.sidewaysSlip));
                }
                else
                {
                    SkidMarksAsphaltList[i].SetActive(false);
                }*/
                
                if (tireParticlesEnabled)
                {
					if (surfaceTypeData.EmitParticles[surfaceIndex] == true)
                    {
                        //TireParticlesObject[RearWheelsCounter].SetActive(true);
                        //Debug.Log("Particle_emission_playing");
                        TireParticlesSystem[RearWheelsCounter].Play();
						TireParticlesSystem[RearWheelsCounter].startColor = surfaceTypeData.TerrainParticleColor[surfaceIndex];
                    }
                    else
                    {
                        TireParticlesSystem[RearWheelsCounter].Stop();
                        //TireParticlesObject[RearWheelsCounter].SetActive(false);
                    }
                }
                else
                {
                    TireParticlesSystem[RearWheelsCounter].Stop();
                    //TireParticlesObject[RearWheelsCounter].SetActive(false);
                }
                
            }
            if (i > 1)
            {
                RearWheelsCounter = RearWheelsCounter + 1;
            }
        }
    }
}
