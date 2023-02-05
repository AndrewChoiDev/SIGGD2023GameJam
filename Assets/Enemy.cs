using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] private int health = 10;
    [SerializeField] private float speed = 10;

    private Color baseColor;

    private void Start() {
        baseColor = GetComponent<SpriteRenderer>().color;
    }

    private void FixedUpdate() {
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
        var duration = 2.0f;
        var renderer = GetComponent<SpriteRenderer>();
        while (Time.time < startTime + duration) {
            renderer.color = Color.Lerp(baseColor, Color.red, (Time.time - startTime) / duration);
            yield return null;
        }
        if (queuedAttackTarget != null) {
            Destroy(queuedAttackTarget);
        }
        renderer.color = baseColor;
        isAttacking = false;
    }
    private GameObject queuedAttackTarget = null;


    private void OnCollisionStay2D(Collision2D collision) {
        var plantComp = collision.gameObject.GetComponent<Plant>();
        if (plantComp != null && isAttacking == false) {
            queuedAttackTarget = collision.gameObject;
            StartCoroutine(Attack());
        }
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.name == "enemyGoal") {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }
}
