using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitsList : MonoBehaviour
{
    public Fruits[] fruitsList;
    public GameObject line;

    public bool canSpawn{get;set;} = true;

    public void SpawnFruit()
    {
        if (canSpawn)
        {
            int num = Random.Range(0, 5);
            Instantiate(fruitsList[num], transform.position, Quaternion.identity);
            line.SetActive(true);
            Debug.Log($"{num}番のフルーツが生成!");
            canSpawn = false;
        }

    }

    public void FusionFruit()
    {

    }

    public void Move()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += new Vector3(3f * Time.deltaTime, 0, 0);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position -= new Vector3(3f * Time.deltaTime, 0, 0);
        }
    }

    void Start()
    {
        SpawnFruit();
    }

    void Update()
    {
        Move();
    }
}
