using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WheelsBehavior : MonoBehaviour {

    //WheelsPhysicsDetails 0 means that all 4 wheels gets the same values, 1 that physics a split front and rear axis, and 2 that all wheels are independantly calculated
    public int WheelsPhysicsDetails = 2;
    public bool WheelSkidMarksEnabled = true;
    public bool tireParticlesEnabled = true;
    public List<GameObject> WheelsList;
    public List<WheelCollider> WheelsCollidersList = new List<WheelCollider>();
    public List<GameObject> SkidMarksAsphaltList = new List<GameObject>();
    public List<GameObject> SkidMarksSandList = new List<GameObject>();
    public List<GameObject> TireParticlesObject = new List<GameObject>();
    //public GameObject TireWideParticleObject;
    public List<ParticleSystem> TireParticlesSystem = new List<ParticleSystem>();
    private GameObject currentTerrain;
    private TerrainFXData currentTerrainData;
    public float SkidSlipThreshold = 0.2f;
    //public float sandGripFactor = 0.6f;
    //public float mudGripFactor = 0.8f;
    private List<float> wheelsSurfaceGripFactors  ;
    private WheelFrictionCurve sidewaysFriction;
    public List<GameObject> ObjectForPhysicsUpdate = new List<GameObject>();
    private List<float> wheelsSurfaceDragFactors;
    private float BoardDragInit;
    // Use this for initialization
    void Start() {
        currentTerrain = GameObject.Find("RaceManager").GetComponent <RaceManagerScript >().thisLevelTerrain;
        currentTerrainData = currentTerrain.GetComponent<TerrainFXData>();
        wheelsSurfaceDragFactors = currentTerrainData.surfaceDragFactors;
        wheelsSurfaceGripFactors = currentTerrainData.surfaceGripFactors;
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
        /*foreach (Transform child in WheelsList[0].transform.parent)
        {
            if (child.gameObject.name == "Wheel Particle System")
            {
                TireWideParticleObject = child.gameObject;
            }
        }*/
        //TireWideParticleObject.GetComponent<ParticleSystem>().Stop();

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
        /*foreach (GameObject Object in  ObjectForPhysicsUpdate)
        { Debug.Log(Object.name); }*/

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
	// Update is called once per frame
	void Update () {
        //int i = 0;
        int RearWheelsCounter = 0;
        for (int i = 0; i < 4; i++)
        {
            ///Wheels Side friction change with terrain handling
            ///
            // following can be used to get the main type of terrain
            //var surfaceIndex = TerrainSurface.GetMainTexture(WheelsList[i].transform.position);

            float localStiffness = 1.0f;
            if (WheelsPhysicsDetails == 2)
            {
                // All wheels calculated independantly
                var surfaceMix = TerrainSurface.GetTextureMix(ObjectForPhysicsUpdate[i].transform.position);
                localStiffness = 0.0f;
                for (int n = 0; n < wheelsSurfaceGripFactors.Count; n++)
                {
                    localStiffness = localStiffness + wheelsSurfaceGripFactors[n] * surfaceMix[n];
                }
                sidewaysFriction.stiffness = localStiffness;
            }
            if (WheelsPhysicsDetails == 1)
            {
                if (i == 0 || i == 1)
                {
                    var surfaceMix = TerrainSurface.GetTextureMix(ObjectForPhysicsUpdate[i].transform.position);
                    localStiffness = 0.0f;
                    for (int n = 0; n < wheelsSurfaceGripFactors.Count; n++)
                    {
                        localStiffness = localStiffness + wheelsSurfaceGripFactors[n] * surfaceMix[n];
                    }
                    sidewaysFriction.stiffness = localStiffness;
                }
            }
            if (WheelsPhysicsDetails == 0)
            {
                if (i == 0)
                {
                    var surfaceMix = TerrainSurface.GetTextureMix(ObjectForPhysicsUpdate[i].transform.position);
                    localStiffness = 0.0f;
                    for (int n = 0; n < wheelsSurfaceGripFactors.Count; n++)
                    {
                        localStiffness = localStiffness + wheelsSurfaceGripFactors[n] * surfaceMix[n];
                    }
                    sidewaysFriction.stiffness = localStiffness;
                }
            }


            WheelsCollidersList[i].sidewaysFriction = sidewaysFriction;

            //managing drage based on ground texture
            var surfaceValuesForDrag = TerrainSurface.GetTextureMix(this.gameObject.transform.position);
            float localDragValue = 0.0f;
            for (int n = 0; n < wheelsSurfaceDragFactors.Count; n++)
            {
                localDragValue = localDragValue + wheelsSurfaceDragFactors[n] * surfaceValuesForDrag[n];
            }
            this.gameObject.GetComponent<Rigidbody>().drag = localDragValue * BoardDragInit;



            ///Wheels Skid Marks handling
            ///

            if (WheelSkidMarksEnabled)
            {
                var surfaceIndex = TerrainSurface.GetMainTexture(WheelsList[i].transform.position);
                WheelHit wheelHit;
                WheelsCollidersList[i].GetGroundHit(out wheelHit);

                ///Debug.Log(Mathf.Abs(wheelHit.sidewaysSlip));

                SkidMarksAsphaltList[i].GetComponent<TrailRenderer>().material = Terrain.activeTerrain.GetComponent<TerrainFXData>().TerrainSkidMaterials[surfaceIndex];
                if (Terrain.activeTerrain.GetComponent<TerrainFXData>().permanentSkids[surfaceIndex] == false)
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
                    if (Terrain.activeTerrain.GetComponent<TerrainFXData>().EmitParticles[surfaceIndex] == true)
                    {
                        //TireParticlesObject[RearWheelsCounter].SetActive(true);
                        //Debug.Log("Particle_emission_playing");
                        TireParticlesSystem[RearWheelsCounter].Play();
                        TireParticlesSystem[RearWheelsCounter].startColor = Terrain.activeTerrain.GetComponent<TerrainFXData>().TerrainParticleColor[surfaceIndex];
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
