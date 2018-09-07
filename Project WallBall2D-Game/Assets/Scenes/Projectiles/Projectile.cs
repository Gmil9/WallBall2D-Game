using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Projectile")]
public class Projectile : ScriptableObject {

    [SerializeField] GameObject weapon;
    public Transform spawnPosition;
    [SerializeField] int damage;
    [SerializeField] float rotation;

    public GameObject GetWeapon(){
        return weapon;
    }

    public int GetDamage(){
        return damage;
    }

    public float GetAngVelocity(){
        return rotation;
    }

}
