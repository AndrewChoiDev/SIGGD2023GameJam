using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GameObject gridSpacePrefab;
    [SerializeField] private Transform gridHoverTransform;
    [SerializeField] private Transform shopHoverTransform;
    [SerializeField] private Transform gridSelectTransform;
    // [SerializeField] private GameObject[] shopItems;
    [SerializeField] private GameObject[] shopPrefabs;
    [Multiline]
    [SerializeField] private TMPro.TMP_Text moneyText;
    [SerializeField] private TMPro.TMP_Text timeText;
    [SerializeField] private TMPro.TMP_Text shopDescriptionText;

    private int money = 120;

    int gridYSize = 4;
    int gridXSize = 10;

    Vector2Int? gridSelect;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    [ContextMenu("Generate Spaces")]
    private void generateSpaces() {
            
        for (int y = 0; y < gridYSize; y++)
        {
            for (int x = 0; x < gridXSize; x++)
            {
                var pos = transform.position + Vector3.right * (x + 0.5f) + Vector3.up * (y + 0.5f);
                Instantiate(gridSpacePrefab, pos, Quaternion.identity, transform);
            }
        }
    }
    public Vector3 gridPosToWorldPos(Vector2Int gridPos) {
        return transform.position + new Vector3(gridPos.x, gridPos.y, 0.0f) 
            + Vector3.right * 0.5f
            + Vector3.up * 0.5f;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos = Vector3.ProjectOnPlane(mousePos, Vector3.forward);
        var disp = mousePos - transform.position;
        var gridX = Mathf.FloorToInt(disp.x);
        var gridY = Mathf.FloorToInt(disp.y);
        if (gridX >= 0 && gridX < gridXSize && gridY >= 0 && gridY < gridYSize) {
            gridHoverTransform.gameObject.SetActive(true);
            gridHoverTransform.position = gridPosToWorldPos(new Vector2Int(gridX, gridY));
            
            if (Input.GetMouseButtonDown(0)) {
                gridSelect = new Vector2Int(gridX, gridY);
            }

        } else {
            gridHoverTransform.gameObject.SetActive(false);
            if (Input.GetMouseButtonDown(0)) {
                // gridSelect = null;
            }
        }
        // shop space hovered
        if (gridY == -1 && gridX >= 0 && gridX <= shopPrefabs.Length) {
            // update grid hover to appropriate position
            gridHoverTransform.gameObject.SetActive(true);
            gridHoverTransform.position = gridPosToWorldPos(new Vector2Int(gridX, gridY));

            // find whether a plant is currently selected in the field grid
            Plant plantSelected = null;
            if (gridSelect.HasValue)
            {
                var placePos = gridPosToWorldPos(gridSelect.Value);
                var colliders = Physics2D.OverlapCircleAll(placePos, 0.1f);
                var find = System.Array.Find(colliders, (c) => c.GetComponent<Plant>() != null);
                plantSelected = find?.GetComponent<Plant>();
            }


            // plant item hovered
            if (gridX < shopPrefabs.Length) {
                var plantPrefab = shopPrefabs[gridX];
                var plantComp = plantPrefab.GetComponent<Plant>();
                shopDescriptionText.text = plantComp.description;

                // plant the plant
                if (Input.GetMouseButtonDown(0) 
                    && plantSelected == null && plantComp.price <= money && gridSelect.HasValue) {
                    money -= plantComp.price;
                    var placePos = gridPosToWorldPos(gridSelect.Value);
                    Instantiate(shopPrefabs[gridX], placePos, Quaternion.identity);
                }
            // sell option hovered
            } else {
                shopDescriptionText.text = "<b>Uproot</b>\nRemoves plant.\nIf the plant is a healhty green, it will be sold for money";
                // perform sell
                if (Input.GetMouseButtonDown(0) && plantSelected != null) {
                    if (plantSelected.lifeStage() == Plant.LifeStage.MATURE) {
                        money += plantSelected.sellMoney;
                    }
                    Destroy(plantSelected.gameObject);
                }
            }
        } else {
            shopDescriptionText.text = "";
        }

        if (gridSelect.HasValue) {
            gridSelectTransform.gameObject.SetActive(true);
            gridSelectTransform.position = gridPosToWorldPos(gridSelect.Value);

        } else {
            gridSelectTransform.gameObject.SetActive(false);
        }

        moneyText.text = "money: " + money;
        timeText.text = "time: " + Mathf.Floor(Time.timeSinceLevelLoad);
        
        // if (Bounds)
    }
}
