using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileController : MonoBehaviour {

    GameController gc;
    Wall wall;
    public float force;

    private void Start()
    {
        gc = Camera.main.GetComponent<GameController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //need to check if it hits wall
        if (collision.gameObject.tag == "wall")
        {
            wall = collision.gameObject.GetComponent<Wall>();
            force -= wall.GetHealth();
            if(force <= 0){
                gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0;
                gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
            }
            else{ //force must be greater than 0, which means it can continue to the next wall
                //remove collision from list
                gc.currentWalls.Remove(collision.gameObject);
                destroyWall(collision.gameObject);
            }
        }
    }

    void destroyWall(GameObject currentWall)
    {
        PlayerPrefs.SetInt("currency", PlayerPrefs.GetInt("currency") + currentWall.GetComponent<Wall>().GetHealth());
        //destroys wall object
        Destroy(currentWall);
        //spawns wall broken object which has rigidbody and falls to ground
        //make it one layer behind projectile
    }
}
