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
                GameObject broken = (GameObject) Instantiate(wall.GetBrokenWall(), wall.transform.position, Quaternion.identity);
                var count = 0;
                foreach(Transform child in broken.transform){
                    count++;
                    if(count == 1){
                        child.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-2f, 0f), -0.25f);
                        child.GetComponent<Rigidbody2D>().angularVelocity = Random.Range(-150, -50);
                    }
                    else{
                        child.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-2f, 0f), Random.Range(4, 2));
                        child.GetComponent<Rigidbody2D>().angularVelocity = Random.Range(150, 50);
                    }
                }
                StartCoroutine(killWall(broken));

            }
        }
    }

    IEnumerator killWall(GameObject thiswall)
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(thiswall);
    }

    void destroyWall(GameObject currentWall)
    {
        PlayerPrefs.SetInt("currency", PlayerPrefs.GetInt("currency") + currentWall.GetComponent<Wall>().GetHealth());
        Destroy(currentWall);
    }
}
