using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    private Transform player;
    private Vector2 targetPos;
    private Rigidbody2D rig;
    private BoxCollider2D col;
    private Animator anim;
    public float speed = 50;
    public int lossFod = 15;
    public AudioClip attackClip_1;
    public AudioClip attackClip_2;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        targetPos = transform.position;
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        GameManager.Instance.enemylist.Add(this);
    }

    void Update()
    {
        rig.MovePosition(Vector2.Lerp(transform.position, targetPos, speed * Time.deltaTime));
        col = GetComponent<BoxCollider2D>();
    }

    public void Move()
    {
        Vector2 offset = player.position - transform.position;
        if(offset.magnitude<1.1f)//攻击
        {
            anim.SetTrigger("Attack");
            AudioManager.Instance.RandomPlay(attackClip_1, attackClip_2);
            player.SendMessage("takeDamage",lossFod);
        }
        else//移动
        {
            float x = 0, y = 0;
            if(Mathf.Abs(offset.y)>Mathf.Abs(offset.x))//竖直方向的距离大于水平距离
            {//进行竖直移动
                if(offset.y<0)//竖直向下移动
                {
                    y = -1;
                }
                else//竖直向上移动
                {
                    y = 1;
                }
            }
            else
            {//进行水平移动
                if (offset.x < 0)//水平向左移动
                {
                    x = -1;
                }
                else//水平向右移动
                {
                    x = 1;
                }
            }
            //设置运动位置之前先做检测
            col.enabled = false;
            RaycastHit2D hit = Physics2D.Linecast(targetPos, targetPos + new Vector2(x, y));
            col.enabled = true;
            if(hit.transform == null)
            {
                targetPos += new Vector2(x, y);
            }
            else
            {
                if(hit.collider.tag=="Food_1"||hit.collider.tag=="Food_2")
                {
                    targetPos += new Vector2(x, y);
                }
            }
        }
    }
}
