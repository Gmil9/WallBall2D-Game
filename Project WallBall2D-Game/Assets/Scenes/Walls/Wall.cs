using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

    [SerializeField] int health;

    public int GetHealth(){
        return health;
    }
}
