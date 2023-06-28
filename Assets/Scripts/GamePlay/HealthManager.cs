using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public float health = 100f;
    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;
    public Text textObject;

    private int score;

    void Start() {
        score = 0;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene(0);
        }

        if(score > 300 || score == 300) {
            SceneManager.LoadScene(2);
        }
    }

     private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Trigger the function when the player collides with an enemy
            EnemyCollision();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
   if(other.CompareTag("Enemy")){
    // Handle the collision
    EnemyCollision();

   }else if(other.CompareTag("ten")){
        score += 10;
        textObject.text = "" + score;
        Destroy(other.gameObject);
   }else if(other.CompareTag("twenty")){
        score += 20;
        textObject.text = "" + score;
        Destroy(other.gameObject);
   }else if(other.CompareTag("fifty")){
        score += 50;
        textObject.text = "" + score;
        Destroy(other.gameObject);
   }
   else if(other.CompareTag("minus")){
         health -= 33.5f;
            if(health < 100){
                heart3.SetActive(false);
            }
            if(health < 66) {
                heart2.SetActive(false);
            }
            if(health < 33) {
                heart1.SetActive(false);
                SceneManager.LoadScene(3);
            }
        Destroy(other.gameObject);
   }else if(other.CompareTag("plus")){
        if(health == 100 || health > 100) {

        }else {
        health += 33.5f;
            if(health > 33){
                heart3.SetActive(true);
            }
            if(health > 66) {
                heart2.SetActive(true);
            }
            if(health > 33) {
                heart1.SetActive(true);
            }
        }

        Destroy(other.gameObject);
   }
}

    private void EnemyCollision()
    {
        heart3.SetActive(false);
        heart2.SetActive(false);
        heart1.SetActive(false);
        SceneManager.LoadScene(3);
    }
}
