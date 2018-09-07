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
    Rigidbody2D rb;
    GameObject projectile;
    float force;
    float avelocity;

    [SerializeField] Text currencyText;
    int currency = 0;

	void Start () {
        currencyText.text = "$ " + PlayerPrefs.GetInt("currency", 0).ToString();
        spawnProjectile();
        spawnWalls();
    }
	
	void Update () {
        force = weaponInUse.GetDamage() + (Mathf.PingPong(Time.time, 1) + 1); //TODO alter this
        if (Input.GetKeyDown("space") && panel.activeSelf == false)
        {
            shootProjectile();
            SetForce();
        }

        if(projectile.transform.position.y < -2f || projectile.transform.position.x > 11f)
        {
            panel.SetActive(true);
        }

        //if (Input.GetKeyDown("r"))
        //{
        //    PlayerPrefs.DeleteAll();
        //    print("Score Reset");
        //}
    }

    void spawnProjectile(){
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
            thisWall.transform.position = new Vector3(firstWall.transform.position.x + (i * 2), firstWall.transform.position.y, firstWall.transform.position.z);
        }
    }

    void SetForce(){
        pc.force = force;
    }

    void shootProjectile()
    {
        //move projectile positive x times speed which is equivalent to damage
        rb.velocity = projectile.transform.right * force;
        rb.angularVelocity = avelocity;
    }

    public void levelOverPanel(){
        currencyText.text = "$ " + PlayerPrefs.GetInt("currency").ToString();
        Destroy(projectile);
        spawnProjectile();
        spawnWalls();
        panel.SetActive(false);
    }

}
