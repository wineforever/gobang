using UnityEngine;
using System.Collections;

public class collider : MonoBehaviour {

    void OnMouseDown()
    {
        int X = int.Parse(this.name.Substring(8, 2));
        int Y = int.Parse(this.name.Substring(10, 2));
        GameObject.Find("board").GetComponent<logic>().Action["Event Handle - Move position - X"] = X.ToString();
        GameObject.Find("board").GetComponent<logic>().Action["Event Handle - Move position - Y"] = Y.ToString();
        GameObject.Find("board").GetComponent<logic>().Action["Event Handle - Recent event"] = "Event - [Any Player] Move";
        GameObject.Find("board").GetComponent<logic>().Trigger = true;//触发事件
    }
    public void Move(int X,int Y)
    {
        int Player = GameObject.Find("board").GetComponent<logic>().Player;
        GameObject go = (GameObject)Instantiate(GameObject.Find("chess_" + Player.ToString()));
        go.transform.position = GameObject.Find("collider" + X.ToString("00") + Y.ToString("00")).transform.position;
        go.transform.parent = GameObject.Find("chess").transform;
        GameObject.Find("board").GetComponent<AudioSource>().Play();
        GameObject.Find("board").GetComponent<logic>().Action["Event Handle - Recent event"] = "Event - [Any Player] Round End";
        GameObject.Find("board").GetComponent<logic>().Trigger = true;
    }
}
