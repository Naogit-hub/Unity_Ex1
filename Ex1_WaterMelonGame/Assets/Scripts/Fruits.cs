using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruits : MonoBehaviour
{
    [SerializeField] private string _fruitName;
    [SerializeField] private int level;
    [SerializeField] private int point;
    // x軸方向の移動範囲の最小値
    private float _minX = -2.5f;

    // x軸方向の移動範囲の最大値
    private float _maxX = 2.5f;

    private Rigidbody2D rb2d{get;set;}

    public float speed = 3f;
    public bool canMove = true;
    public bool canFusion = true;
    public bool firstCollision = true;

    void Awake()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        rb2d.isKinematic = true;  //物理演算を止める。
    }

    void Update()
    {
        Move();
    }

    public void Move()
    {
        if (canMove)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb2d.isKinematic = false;
                this.canMove = false;
                //                this.enabled = false;
                GameManager.instance.fl.line.SetActive(false);
                GameManager.instance.fl.canSpawn = true;
                //                GameManager.instance.fl.SpawnFruit();
            }

            var pos = transform.position;

            // x軸方向の移動範囲制限
            pos.x = Mathf.Clamp(pos.x, _minX, _maxX);

            transform.position = pos;
        }


    }

    public void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.CompareTag("Floor"))
        {
            Debug.Log($"{_fruitName}が床に衝突");
        }
        else if (c.gameObject.tag == "Fruit")
        {
            if (c.gameObject.GetComponent<Fruits>().level == this.level)
            {
                if (c.gameObject.GetComponent<Fruits>().canFusion)  //相手が融合可能なら、
                {
                    Debug.Log($"{_fruitName}が融合!");
                    canFusion = false;  //自身を融合済みにする。
                    Fruits f = Instantiate(GameManager.instance.fl.fruitsList[level + 1], c.transform.position, Quaternion.identity);
                    f.rb2d.isKinematic = false;
                    f.canMove = false;
                    Destroy(c.gameObject);
                    Destroy(gameObject);
                }
                else return;
            }
        }
        else
        {
            return;
        }
        if(firstCollision)
        {
            GameManager.instance.fl.SpawnFruit();
            firstCollision = false;
        }

    }
}
