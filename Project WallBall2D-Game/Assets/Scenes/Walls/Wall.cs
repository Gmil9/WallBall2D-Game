using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

    [SerializeField] int health;
    [SerializeField] GameObject brokenWall;

    public int GetHealth(){
        return health;
    }

    public GameObject GetBrokenWall(){
        return brokenWall;
    }
}
