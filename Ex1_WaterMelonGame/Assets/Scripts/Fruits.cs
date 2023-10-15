using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruits : MonoBehaviour
{
    [SerializeField] private string _fruitName;
    [SerializeField] private int level;
    [SerializeField] private int point;
    // x軸方向の移動範囲の最小値
    [SerializeField] private float _minX = -2.5f;

    // x軸方向の移動範囲の最大値
    [SerializeField] private float _maxX = 2.49f;

    private Rigidbody2D rb2d { get; set; }
    private CircleCollider2D cc2d { get; set; }

    public float speed = 3f;
    public bool canMove = true;
    public bool canFusion = true;
    public bool firstCollision = true;

    void Awake()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        //cc2d = gameObject.GetComponent<CircleCollider2D>();
        rb2d.isKinematic = true;  //物理演算を止める。
        //cc2d.isTrigger = true;
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
                cc2d.isTrigger = false;
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
                    GameManager.instance.score+=this.point;
                    Debug.Log($"現在{GameManager.instance.score}ポイント!");
                    Fruits f = Instantiate(GameManager.instance.fl.fruitsList[level + 1], c.transform.position, Quaternion.identity);
                    //f.cc2d = f.gameObject.GetComponent<CircleCollider2D>();
                    //f.cc2d.isTrigger = true;
                    f.rb2d.isKinematic = false;
                    f.canMove = false;
                    Destroy(c.gameObject);
                    Destroy(gameObject);
                }
                else return;
            }
        }

        if (firstCollision)  //初回の衝突のみ作動する。
        {
            GameManager.instance.fl.SpawnFruit();
            firstCollision = false;
        }

    }

    public void OnTriggerEnter2D(Collider2D c)  //融合したフルーツthisに周りのオブジェクトcが侵入する
    {
        Fruits oppFruit = c.gameObject.GetComponent<Fruits>();
        
        Debug.Log($"{this.name}が{c.name}に貫通");
        this.cc2d.isTrigger = false;
        
        if (c.CompareTag("Fruit"))
        {
            Debug.Log("fruitと衝突");
            Vector3 distination = (c.transform.position - transform.position).normalized;
            oppFruit.rb2d.AddForce(distination * 10, ForceMode2D.Impulse);
            rb2d.AddForce(distination * 10, ForceMode2D.Impulse);
        }
    }

}
