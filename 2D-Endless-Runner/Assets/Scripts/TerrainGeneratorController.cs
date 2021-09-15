using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneratorController : MonoBehaviour
{
[Header("Templates")]
public List<TerrainTemplateController> terrainTemplates;
public float terrainTemplateWidth;

[Header("Generator Area")]
public Camera gameCamera;
public float areaStartOffset;
public float areaEndOffset;

[Header("Force Early Template")]
public List<TerrainTemplateController> earlyTerrainTemplates;


// pool list
private Dictionary<string, List<GameObject>> pool;

private float lastRemovedPositionX;

private const float debugLineHeight = 10.0f;

private List<GameObject> spawnedTerrain;

private float lastGeneratedPositionX;


    // Start is called before the first frame update
    private void Start()
{
    // init pool
    pool = new Dictionary<string, List<GameObject>>();

    spawnedTerrain = new List<GameObject>();

    lastGeneratedPositionX = GetHorizontalPositionStart();
    lastRemovedPositionX = lastGeneratedPositionX - terrainTemplateWidth;


    GameObject newTerrain = Instantiate(earlyTerrainTemplates[0].gameObject, transform);
        newTerrain.transform.position = new Vector2(lastGeneratedPositionX, -4.4f);
        spawnedTerrain.Add(newTerrain);



    while (lastGeneratedPositionX < GetHorizontalPositionEnd())
    {
        GenerateTerrain(lastGeneratedPositionX);
        lastGeneratedPositionX += terrainTemplateWidth;
    }
}

private void GenerateTerrain(float posX, TerrainTemplateController forceterrain = null)
{
    GameObject newTerrain = Instantiate(terrainTemplates[Random.Range(0, terrainTemplates.Count)].gameObject, transform);

    newTerrain.transform.position = new Vector2(posX, -4.5f);

    spawnedTerrain.Add(newTerrain);
}

 

    // Update is called once per frame
    private void Update()
{
    while (lastGeneratedPositionX < GetHorizontalPositionEnd())
    {
        GenerateTerrain(lastGeneratedPositionX);
        lastGeneratedPositionX += terrainTemplateWidth;
    }
   
    while (lastRemovedPositionX + terrainTemplateWidth < GetHorizontalPositionStart())
    {
        lastRemovedPositionX += terrainTemplateWidth;
        RemoveTerrain(lastRemovedPositionX);
    }
}

    private void RemoveTerrain(float posX)
{
    GameObject terrainToRemove = null;

    // find terrain at posX
    foreach (GameObject item in spawnedTerrain)
    {
        if (item.transform.position.x == posX)
        {
            terrainToRemove = item;
            break;
        }
    }
 


    // after found;
    if (terrainToRemove != null)
    {
        spawnedTerrain.Remove(terrainToRemove);
        Destroy(terrainToRemove);
    }
}


private float GetHorizontalPositionStart()
{
    return gameCamera.ViewportToWorldPoint(new Vector2(0f, 0f)).x + areaStartOffset;
}

private float GetHorizontalPositionEnd()
{
    return gameCamera.ViewportToWorldPoint(new Vector2(1f, 0f)).x + areaEndOffset;
}

// debug
private void OnDrawGizmos()
{
    Vector3 areaStartPosition = transform.position;
    Vector3 areaEndPosition = transform.position;

    areaStartPosition.x = GetHorizontalPositionStart();
    areaEndPosition.x = GetHorizontalPositionEnd();

    Debug.DrawLine(areaStartPosition + Vector3.up * debugLineHeight / 2, areaStartPosition + Vector3.down * debugLineHeight / 2, Color.red);
    Debug.DrawLine(areaEndPosition + Vector3.up * debugLineHeight / 2, areaEndPosition + Vector3.down * debugLineHeight / 2, Color.red);
}
// pool function
private GameObject GenerateFromPool(GameObject item, Transform parent)
{
    if (pool.ContainsKey(item.name))
    {
        // if item available in pool
        if (pool[item.name].Count > 0)
        {
            GameObject newItemFromPool = pool[item.name][0];
            pool[item.name].Remove(newItemFromPool);
            newItemFromPool.SetActive(true);
            return newItemFromPool;
        }
    }
    else
    {
        // if item list not defined, create new one
        pool.Add(item.name, new List<GameObject>());
    }


    // create new one if no item available in pool
    GameObject newItem = Instantiate(item, parent);
    newItem.name = item.name;
    return newItem;
}
   
private void ReturnToPool(GameObject item)
{
    if (!pool.ContainsKey(item.name))
    {
        Debug.LogError("INVALID POOL ITEM!!");
    }


    pool[item.name].Add(item);
    item.SetActive(false);
}
}
