using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public float baseShootRecovery;
    private float lastShootTime = -999.0f;
    [SerializeField] private GameObject projectilePrefab;

    public enum LifeStage {YOUNG, MATURE, OLD}

    private float birthTime;

    public float lifeSpan;
    public int price;
    public int sellMoney;

    public Color youngColor;
    public Color matureColor;
    public Color oldColor;
    [Multiline]
    [SerializeField] public string description;

    public LifeStage lifeStage() {
        var age = Time.time - birthTime;
        if (age < lifeSpan * 0.35f) {
            return LifeStage.YOUNG;
        } else if (age < lifeSpan * 0.8f) {
            return LifeStage.MATURE;
        } else {
            return LifeStage.OLD;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        birthTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        var shootRecovery = lifeStage() == LifeStage.MATURE ? baseShootRecovery * 0.5f : baseShootRecovery;
        if (Time.time >= lastShootTime + shootRecovery) {
            Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            lastShootTime = Time.time;
        }
        switch (lifeStage())
        {
            case(LifeStage.YOUNG):
                GetComponent<SpriteRenderer>().color = youngColor;
                break;
            case(LifeStage.MATURE):
                GetComponent<SpriteRenderer>().color = matureColor;
                break;
            case(LifeStage.OLD):
                GetComponent<SpriteRenderer>().color = oldColor;
                break;
        }
        if (Time.time - birthTime > lifeSpan) {
            Destroy(gameObject);
        }
    }


}
