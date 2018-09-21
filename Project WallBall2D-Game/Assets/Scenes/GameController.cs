using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    bool canShoot = false;

    [SerializeField] GameObject nextLevelPanel;
    [SerializeField] Button buyWeaponButton;
    Projectile purchaseWeapon;
    [SerializeField] Text currencyText;
    int currency = 0;

	void Start () {
        currencyText.text = "$ " + PlayerPrefs.GetInt("currency", 0).ToString();
        spawnProjectile();
        spawnWalls();
    }
	
	void Update () {
        force = weaponInUse.GetDamage() + (Mathf.PingPong(Time.time, 1) + 2); //TODO alter this
        currency = PlayerPrefs.GetInt("currency");
        if (Input.GetKeyDown("space") && projectileIsSpawned)
        {
            shootProjectile();
            SetForce();
        }
                 
        if (projectile.transform.position.y < -15f || projectile.transform.position.x > 20f)
        {
            currencyText.text = "$ " + PlayerPrefs.GetInt("currency").ToString();
            Destroy(projectile);
            spawnProjectile();
            if(currentWalls.Count != 0){
                foreach(GameObject wall in currentWalls){
                    StartCoroutine(moveWall(wall));
                }
                spawnWalls();
            }
            else{
                nextLevelPanel.SetActive(true);
            }
        }

        if (Input.GetKeyDown("r"))
        {
            PlayerPrefs.DeleteAll();
            print("Score Reset");
        }
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
        canShoot = false;
        //move projectile positive x times speed which is equivalent to damage
        rb.velocity = projectile.transform.right * force;
        rb.angularVelocity = avelocity;
    }

    public void changeWeapon(Projectile newWeapon){
        Button thisButton = GameObject.Find(newWeapon.name).GetComponent<Button>();
        print("here");
        print(currency);
        print(newWeapon.GetPrice());
        if (newWeapon.isUnlocked){
            print("here 2");
            Destroy(projectile);
            weaponInUse = newWeapon;
            spawnProjectile();
        }else if (currency > newWeapon.GetPrice()){
            print("here 3");
            purchaseWeapon = newWeapon;
            buyWeaponButton.transform.position = new Vector3(thisButton.transform.position.x, thisButton.transform.position.y - 90, thisButton.transform.position.z);
            buyWeaponButton.gameObject.SetActive(true);
        }else{
            print("here 4");
            //show you dont have enough moneey text
        }
    }

    public void buyWeapon(){
        currency -= purchaseWeapon.GetPrice();
        purchaseWeapon.isUnlocked = true;
        buyWeaponButton.gameObject.SetActive(false);
    }

    public void nextScene(string answer){
        if(answer == "yes"){
            var s = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(s + 1);
            nextLevelPanel.SetActive(false);
        }
        else
        {
            nextLevelPanel.SetActive(false);
            spawnWalls();
        }
    }
}
