using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public float shootRecovery;
    private float timeOfShootRecovery = -999.0f;
    [SerializeField] private GameObject projectilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        timeOfShootRecovery = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= timeOfShootRecovery) {
            Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            timeOfShootRecovery += shootRecovery;
        }
    }


}
