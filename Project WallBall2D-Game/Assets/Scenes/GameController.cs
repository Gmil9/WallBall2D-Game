using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    [SerializeField] Projectile weaponInUse;

    [SerializeField] GameObject[] possibleWalls;
    public List<GameObject> currentWalls;
    [SerializeField] Transform WallOrigin;
    [SerializeField] Transform firstWall;

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
            if(currentWalls.Count != 0){
                foreach(GameObject wall in currentWalls){
                    StartCoroutine(moveWall(wall));
                }
            }
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
        int temp = currentWalls.Count;
        for (int i = 0; i < (4 - temp); i++){
            var wallPrefab = possibleWalls[Random.Range(0, possibleWalls.Length)]; 
            var thisWall = Instantiate(wallPrefab);
            currentWalls.Add(thisWall);
            thisWall.transform.position = new Vector3(WallOrigin.position.x + (i * 2),0,WallOrigin.position.z);
            StartCoroutine(moveWall(thisWall));
        }
    }

    IEnumerator moveWall(GameObject wall){
        Rigidbody2D rbWall = wall.GetComponent<Rigidbody2D>();
        Vector3 pos = new Vector3(firstWall.position.x + (currentWalls.IndexOf(wall) * 2), 0, firstWall.position.z);

        rbWall.velocity = Vector3.left * speed;

        yield return new WaitUntil(() => wall.transform.position.x <= pos.x);
        rbWall.velocity = Vector3.zero;
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
