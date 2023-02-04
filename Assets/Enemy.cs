using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    [SerializeField] private int health = 10;

    private void FixedUpdate() {
        var speed = 0.2f;
        GetComponent<Rigidbody2D>().velocity = Vector2.left * speed;
    }

    public void Damage(int amount) {
        health -= amount;
        if (health <= 0) {
            Destroy(gameObject);
        }
    }

    bool isAttacking = false;

    IEnumerator Attack() {
        isAttacking = true;
        var startTime = Time.time;
        var duration = 1.0f;
        var renderer = GetComponent<SpriteRenderer>();
        while (Time.time < startTime + duration) {
            renderer.color = Color.Lerp(Color.white, Color.red, (Time.time - startTime) / duration);
            yield return null;
        }
        // attack
        renderer.color = Color.white;
        isAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.name == "enemyGoal") {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }
}
