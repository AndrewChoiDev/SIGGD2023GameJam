using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float deathTime = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        deathTime = Time.time + 5.0f;
        GetComponent<Rigidbody2D>().velocity = Vector2.right * 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > deathTime) {
            used = true;
            Destroy(gameObject);
        }
    }

    private bool used = false;

    private void OnCollisionEnter2D(Collision2D other) {
        if (used == false && other.gameObject.GetComponent<Enemy>() != null) {
            used = true;
            other.gameObject.GetComponent<Enemy>().Damage(1);
            Destroy(gameObject);
            // Destroy(other.gameObject);
        }
    }
}
