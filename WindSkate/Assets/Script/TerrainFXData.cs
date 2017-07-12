using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TerrainFXData : MonoBehaviour {

	public int currentGroundType  = 0;
    public List<bool> permanentSkids = new List<bool> { false };
    public List<bool> EmitParticles = new List<bool> { false };
    public List<Material> TerrainSkidMaterials = new List<Material> ();
    public List<Color> TerrainParticleColor = new List<Color> { new Color(0, 0, 0, 1) };
    public List<float> surfaceGripFactors = new List<float>() { 1.0f, 0.6f, 0.8f, 0.6f, 0.5f };
    public List<float> surfaceDragFactors = new List<float>() { 1.0f, 1.2f, 1.5f, 1.2f, 1.5f };

}
