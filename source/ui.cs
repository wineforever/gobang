using UnityEngine;
using System.Collections;

public class ui : MonoBehaviour {
    public int board_Scale = 19;
    // Use this for initialization
    void Start () {
        //创建碰撞器
        for (int i = 0; i < board_Scale; i++)
            for (int j = 0; j < board_Scale; j++)
            {
                GameObject Collider = new GameObject("collider" + i.ToString("00") + j.ToString("00"));
                Collider.transform.position = new Vector3(-4.225f + 0.47f * i, 4.225f - 0.47f * j);
                Collider.transform.parent = GameObject.Find("colliders").transform;
                Collider.transform.localScale = new Vector3(0.5f,0.5f);
                Collider.AddComponent<BoxCollider2D>();
                Collider.AddComponent<SpriteRenderer>();
                Collider.AddComponent<collider>();
            }
	}
}
