using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    [SerializeField] Projectile weaponInUse;
    [SerializeField] GameObject[] walls;

    [SerializeField] Transform firstWall;

    [SerializeField] GameObject panel;

    ProjectileController pc;

    GameObject projectile;
    float force;

	void Start () {
        spawnProjectile();
        spawnWalls();
    }
	
	void Update () {
        force = weaponInUse.GetDamage() + (Mathf.PingPong(Time.time, 1) + 1); //TODO alter this
        if (Input.GetKeyDown("space"))
        {
            shootProjectile();
            SetForce();
        }

        if(projectile.transform.position.y < -2f || projectile.transform.position.x > 11f)
        {
            panel.SetActive(true);
        }
    }

    void spawnProjectile(){
        var projectilePrefab = weaponInUse.GetWeapon();
        projectile = Instantiate(projectilePrefab);
        projectile.transform.position = weaponInUse.spawnPosition.position;
        pc = projectile.GetComponent<ProjectileController>();
    }

    void spawnWalls(){
        for (int i = 0; i < walls.Length; i++){
            var wallPrefab = walls[i];
            var thisWall = Instantiate(wallPrefab);
            thisWall.transform.position = new Vector3(firstWall.transform.position.x + (i * 2), firstWall.transform.position.y, firstWall.transform.position.z);
        }
    }

    void SetForce(){
        pc.force = force;
    }

    void shootProjectile()
    {
        //move projectile positive x times speed which is equivalent to damage
        projectile.GetComponent<Rigidbody2D>().velocity = projectile.transform.up * force;

    }

    public void levelOverPanel(){
        Destroy(projectile);
        spawnProjectile();
        spawnWalls();
        panel.SetActive(false);
    }

}
