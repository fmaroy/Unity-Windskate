using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TerrainFXData : MonoBehaviour {


    /*public List<TerrainMaterial> TerrainDataList = new List<TerrainMaterial> {
        new TerrainMaterial(0, new Color(0, 0, 0, 1), false, false, new Color(0, 0, 0, 1)),
        new TerrainMaterial(0, new Color(0, 0, 0, 1), false, false, new Color(0, 0, 0, 1))
    };*/
    public List<bool> permanentSkids = new List<bool> { false };
    public List<bool> EmitParticles = new List<bool> { false };
    public List<Material> TerrainSkidMaterials = new List<Material> ();
    public List<Color> TerrainParticleColor = new List<Color> { new Color(0, 0, 0, 1) };
    public List<float> surfaceGripFactors = new List<float>() { 1.0f, 0.6f, 0.8f, 0.6f, 0.5f };
    public List<float> surfaceDragFactors = new List<float>() { 1.0f, 1.2f, 1.5f, 1.2f, 1.5f };

    /*public class TerrainMaterial
    {
        int TextureID;
        Color SkidColor;
        bool permanentSkids;
        bool EmitParticles;
        Color ParticleColor;

        public TerrainMaterial(int ID, Color c1, bool b1, bool b2, Color c2)
        {
            this.TextureID = ID;
            this.SkidColor = c1;
            this.permanentSkids = b1;
            this.EmitParticles = b2;
            this.ParticleColor = c2;

        }
    }*/
    // Use this for initialization
    void Start () {
    //TerrainDataList.Add(new TerrainMaterial(0, new Color(0, 0, 0, 1), false, false, new Color(0, 0, 0, 1)));
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
