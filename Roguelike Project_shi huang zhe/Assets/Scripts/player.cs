using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour {

	private Vector2 targetPos = new Vector2(1, 1);
	private Rigidbody2D rig;
	private BoxCollider2D col;
	private Animator anim;
	public float speed = 6;
	public float restTime = 0.5f;
	public float restTimer = 0;
	public AudioClip Chop1;
	public AudioClip Chop2;
	public AudioClip step1;
	public AudioClip step2;
	public AudioClip food_1_1;
	public AudioClip food_1_2;
	public AudioClip food_2_1;
	public AudioClip food_2_2;

	// Use this for initialization
	void Start () {
		rig = GetComponent<Rigidbody2D>();
		col = GetComponent<BoxCollider2D>();
		anim = GetComponent<Animator>();
	}

    // Update is called once per frame
    [System.Obsolete]
    void Update () {
		rig.MovePosition(Vector2.Lerp(transform.position, targetPos, speed * Time.deltaTime));

		restTimer += Time.deltaTime;
		if (restTimer < restTime) return;
		
		float h = Input.GetAxisRaw("Horizontal");
		float v = Input.GetAxisRaw("Vertical");
		if (h > 0) v = 0;
		
		if(h!=0||v!=0)
        {
			if (GameManager.Instance.food <= 0 || GameManager.Instance.isEnd == true) return;
			GameManager.Instance.DecreaseFood(1);
			col.enabled = false;
			RaycastHit2D hit =  Physics2D.Linecast(targetPos, targetPos+new Vector2(h,v));
			col.enabled = true;
			if(hit.transform == null)
            {
				targetPos += new Vector2(h, v);
				AudioManager.Instance.RandomPlay(step1,step2);
			}
            else
            {
				switch(hit.collider.tag)
                {
					case "OutWall":
						AudioManager.Instance.RandomPlay(step1, step2);
						break;
					case "Wall":
						anim.SetTrigger("Attack");
						AudioManager.Instance.RandomPlay(Chop1, Chop2);
						hit.collider.SendMessage("takeDamage");
						break;
					case "Food_1":
						GameManager.Instance.AddFood(10);
						targetPos += new Vector2(h, v);
						AudioManager.Instance.RandomPlay(food_1_1, food_1_2);
						Destroy(hit.transform.gameObject);
						break;
					case "Food_2":
						GameManager.Instance.AddFood(20);
						targetPos += new Vector2(h, v);
						AudioManager.Instance.RandomPlay(food_2_1, food_2_2);
						Destroy(hit.transform.gameObject);
						break;
					case "Enemy":
						break;
					case "Exit":
						GameManager.Instance.isEnd = true;
						targetPos += new Vector2(h, v);
						AudioManager.Instance.RandomPlay(step1, step2);
						print(1);
						break;
                }
            }
			GameManager.Instance.OnPlayerMove();
			restTimer = 0;
		}
	}

	public void takeDamage(int num)
    {
		GameManager.Instance.DecreaseFood(num);
		anim.SetTrigger("Damage");
    }
}
