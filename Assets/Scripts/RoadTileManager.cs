using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadTileManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public Dictionary<GameObject, bool> tiles; // bool - tile used
    public GameObject car;
    public GameObject firstTile;
    public GameObject carPre;
    public float time;
    public GameObject otherCars;
    public float yPre, zPre;

    // Start is called before the first frame update
    void Start()
    {
        tiles = new Dictionary<GameObject, bool>();
        tiles.Add(firstTile, false); // add the first tile to the dict   
    }

    // Update is called once per frame
    void Update()
    {
        
        Dictionary<GameObject, bool> tileClone = CloneDict(tiles); // clone to not changing the dict in foreach
        foreach(var a in tileClone)
        {
            GameObject t = a.Key;
            Bounds bou = t.GetComponent<Collider>().bounds;

            if (bou.center.x >= car.transform.position.x && !a.Value) // if passed the half of the tile
            {
                Vector3 loc = new Vector3(bou.center.x - bou.size.x, 0, 0);
                CreateTile(loc);
                tiles[t] = true;
                t.GetComponentInChildren<Terrain>().enabled = false;
            }
            else if (car.transform.position.x <= bou.center.x - bou.size.x) // if passed 1.5 tiles remove it
            {
                tiles.Remove(t);
                Destroy(t);
            }

            if(car.transform.position.x >= bou.center.x && Random.Range(0, 1f) >= 0.995) // spwan a new car
            {
                if(Time.time - time >= 7)
                {
                    SpwanCar(bou.center.x - bou.size.x/2f + 2f);
                }
            }

        }

            Transform[] tr = otherCars.GetComponentsInChildren<Transform>();
            foreach (Transform tra in tr)
            {
                if(car.transform.position.x <= tra.position.x - 12)
                {
                   if (tra.gameObject.CompareTag("Car"))
                    {
                        Destroy(tra.gameObject);
                    }
                }
            }
    }

    Dictionary<GameObject, bool> CloneDict(Dictionary<GameObject, bool> d)
    {
        Dictionary<GameObject, bool> ret = new Dictionary<GameObject, bool>();
        foreach (var a in d)
        {
            ret.Add(a.Key, a.Value);
        }
        return ret;
    }

    void CreateTile(Vector3 pos)
    {
        GameObject g = Instantiate(tilePrefab, pos, Quaternion.identity);
        tiles.Add(g, false);
        g.transform.parent = transform;
    }

    void SpwanCar(float x)
    {
        Vector3 pos = new Vector3(x, yPre, zPre);
        GameObject g = Instantiate(carPre, pos, Quaternion.Euler(0, 90, 0));
        g.transform.parent = otherCars.transform;
        time = Time.time;
    }
}
