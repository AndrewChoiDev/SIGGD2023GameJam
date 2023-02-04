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
    [SerializeField] private int[] itemCosts;
    [SerializeField] private GameObject[] shopPrefabs;

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
        if (gridY == -1 && gridX >= 0 && gridY < itemCosts.Length) {
            gridHoverTransform.gameObject.SetActive(true);
            gridHoverTransform.position = gridPosToWorldPos(new Vector2Int(gridX, gridY));
            if (Input.GetMouseButtonDown(0) && gridSelect.HasValue) {
                var placePos = gridPosToWorldPos(gridSelect.Value);
                var colliders = Physics2D.OverlapCircleAll(placePos, 0.1f);
                if (System.Array.Find(colliders, (c) => c.GetComponent<Plant>() != null) == null) {
                    Instantiate(shopPrefabs[gridX], placePos, Quaternion.identity);
                }
            }
        }

        if (gridSelect.HasValue) {
            gridSelectTransform.gameObject.SetActive(true);
            gridSelectTransform.position = gridPosToWorldPos(gridSelect.Value);

        } else {
            gridSelectTransform.gameObject.SetActive(false);
        }
        
        // if (Bounds)
    }
}
