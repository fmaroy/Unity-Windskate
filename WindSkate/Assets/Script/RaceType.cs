using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class RaceType : MonoBehaviour {

}

[System.Serializable]
public class TrackList
{
    public int trackId;
    public string trackName;
    public string sceneName;
    public Texture2D RacePreview;
    //public Material look2;

    // public Sprite image;
    public TrackList(int id, string name)
    {
        trackId = id;
        trackName = name;
    }
}

