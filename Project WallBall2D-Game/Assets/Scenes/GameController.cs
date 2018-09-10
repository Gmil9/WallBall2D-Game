using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    [SerializeField] Projectile weaponInUse;
    [SerializeField] GameObject[] walls;
    [SerializeField] Transform[] wallPositions;
    [SerializeField] Transform WallOrigin;
    [SerializeField] float speed = 3;

    ProjectileController pc;
    Rigidbody2D rb;
    GameObject projectile;
    float force;
    float avelocity;

    bool projectileIsSpawned = false;

    [SerializeField] Text currencyText;
    int currency = 0;

	void Start () {
        currencyText.text = "$ " + PlayerPrefs.GetInt("currency", 0).ToString();
        spawnProjectile();
        spawnWalls();
    }
	
	void Update () {
        force = weaponInUse.GetDamage() + (Mathf.PingPong(Time.time, 1) + 1); //TODO alter this
        if (Input.GetKeyDown("space") && projectileIsSpawned)
        {
            shootProjectile();
            SetForce();
        }
                 
        if (projectile.transform.position.y < -5f || projectile.transform.position.x > 13f)
        {

            currencyText.text = "$ " + PlayerPrefs.GetInt("currency").ToString();
            Destroy(projectile);
            spawnProjectile();
        }

        if(Input.GetKeyDown("s")){
            spawnWalls();
        }

        //if (Input.GetKeyDown("r"))
        //{
        //    PlayerPrefs.DeleteAll();
        //    print("Score Reset");
        //}
    }

    void spawnProjectile(){
        projectileIsSpawned = true;
        var projectilePrefab = weaponInUse.GetWeapon();
        avelocity = weaponInUse.GetAngVelocity();
        projectile = Instantiate(projectilePrefab);
        rb = projectile.GetComponent<Rigidbody2D>();
        projectile.transform.position = weaponInUse.spawnPosition.position;
        pc = projectile.GetComponent<ProjectileController>();
    }

    void spawnWalls(){
        for (int i = 0; i < walls.Length; i++){
            var wallPrefab = walls[i];
            var thisWall = Instantiate(wallPrefab);
            while(thisWall.transform.position.x > wallPositions[i].position.x){
                thisWall.transform.Translate(Vector3.left * Time.deltaTime * speed);
            }
        }
    }

    void SetForce(){
        pc.force = force;
    }

    void shootProjectile()
    {
        projectileIsSpawned = false;
        //move projectile positive x times speed which is equivalent to damage
        rb.velocity = projectile.transform.right * force;
        rb.angularVelocity = avelocity;
    }

    public void changeWeapon(Projectile newWeapon){
        Destroy(projectile);
        weaponInUse = newWeapon;
        spawnProjectile();
    }

}
