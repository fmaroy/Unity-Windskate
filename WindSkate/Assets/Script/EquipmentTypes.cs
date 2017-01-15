using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipmentTypes: MonoBehaviour {

    
}

[System.Serializable]
public class SailLook
{

    public int sailId;
    public Material look;
    public Sprite image;
    public SailLook(int id, Material l, Sprite i)
    {
        look = l;
        image = i;
    }
    public SailLook(int id)
    {
        sailId = id;
    }
}
[System.Serializable]
public class Sail
{
    public string name;
    public int id;
    public float power;
    public float powerRearWind;
    public float drag;
    public List<ObjectLookSet> looks;
    public int activeLook;

    public Sail(string n, int i, float pow, float rear, float dr)
    {
        name = n;
        id = i;
        power = pow;
        powerRearWind = rear;
        drag = dr;
    }
}

[System.Serializable]
public class ObjectLookSet
{
    public int boardId;
    public List<Material> lookList;
    //public Material look2;
    
   // public Sprite image;
    public ObjectLookSet(int id, List<Material> l)
    {
        boardId = id;
        lookList = l;
        //look2 = l2;
        //image = i;
    }
    public ObjectLookSet(int id)
    {
        boardId = id;
    }
}

[System.Serializable]
public class Board
{
    public string name;
    public int id;
    public float grip;
    public float gripLostOverGround;
    public float drag;
    public List<ObjectLookSet> looks;
    public int activeLook;
    public float frontAxlePos;
    public float rearAxlePos;
    //public GameObject frontAxleObject;
    //public GameObject rearAxleObject;
    public string axleName;

    public Board(string n, int i, float g, float gOG, float dr)
    {
        name = n;
        id = i;
        grip = g;
        gripLostOverGround = gOG;
        drag = dr;
    }
}

[System.Serializable]
public class Cloths
{
    public string typeName;
    //0 : Male, 1: Female
    public int forGender;
    public int id;
    public List<Material> materialList;
    public List<Texture> textureList;
    public Cloths(int i)
    {
        forGender = i;
    }
}



