using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMover : MonoBehaviour
{
    public float speed = 0f, dirTorque = 10f, turnTorque = 800f,yoTorque = 500f, Gosa = 0.0f;
    //public Vector3 myforward = new Vector3(100f, 24f, -66f);
    public Vector3 myforward = DestinationScript.myforward;
    public Vector3 preforward = Vector3.zero;
    private Rigidbody rigidbody_;
    public float getaway_p = 50f, awayS = 200f;
    //GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

    void Start()
    {
        rigidbody_ = GetComponent<Rigidbody>();
        preforward = myforward;
        //create();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //GameObject nearest_away = null;
        myforward = DestinationScript.myforward;
        var my_pos = transform.position;
        var forward = transform.TransformVector(Vector3.forward);//transformのワールド空間への変換

        //var dir = new Vector3(500f, 24f, 0f) - my_pos;
        var dir = myforward - my_pos; 
       // Debug.Log(dir);
        //Debug.Log(forward);
        rigidbody_.AddTorque(Vector3.Cross(forward, dir) * dirTorque);//方向制御
        rigidbody_.AddTorque(Vector3.Cross(forward, -my_pos) * Gosa);//誤差入力

        var left = transform.TransformVector(Vector3.left);
        var horizontal_forward = new Vector3(forward.x, 0f, forward.z).normalized;
        var horizontal_left = Vector3.Cross(Vector3.up, horizontal_forward);
        rigidbody_.AddTorque(Vector3.Cross(forward, horizontal_forward) * yoTorque);//ピッチトルク
        rigidbody_.AddTorque(Vector3.Cross(horizontal_left, left) * turnTorque);//ロールトルク

        //他個体によるベクトル加算↓↓
        //var min_dist = Mathf.Infinity;
        var anothers = GameObject.FindGameObjectsWithTag("FishTag");
        //Debug.Log(anothers.Length);
        for (var i = 0; i < anothers.Length; ++i)
        {
            var another = anothers[i];
            var diff = another.transform.position - my_pos;
            var mag2 = diff.magnitude;
            //Debug.Log(mag2);
            var dir2 = (another.transform.position - my_pos).normalized;
            if (mag2 > 0)
            {
                rigidbody_.AddTorque(Vector3.Cross(forward, -diff) * awayS / (mag2 + anothers.Length));
            }
                
            /*if (mag2 > 0 && mag2 < min_dist)
            {
                //Debug.Log(mag2);
                nearest_away = another;
                //rigidbody_.AddTorque(Vector3.Cross(forward, -diff) * awayS);
                min_dist = mag2;
            }*/
        }
        var decos = GameObject.FindGameObjectsWithTag("Decoration");
        for (var i = 0; i < decos.Length; ++i)
        {
            var deco = decos[i];
            var diff2 = deco.transform.position - my_pos;
            var mag3 = diff2.magnitude;
            //Debug.Log(mag3);
            var dir2 = (deco.transform.position - my_pos).normalized;
            if (mag3 > 0)
            {
                rigidbody_.AddTorque(Vector3.Cross(forward, -diff2) * 15f / (mag3 + anothers.Length));
            }
            
        }

        float dt = Time.fixedDeltaTime;
        float drag = rigidbody_.drag;
        float mass = rigidbody_.mass;
        speed = dir.magnitude*250/750 + 50;
        float force = (speed / 3600f * 1000f) * (drag * mass / (1f - drag * dt));
        rigidbody_.AddForce(transform.TransformVector(new Vector3(0f, 0f, force)));

        if (preforward != myforward)
        {
            //create();
        }
    }

    void create()
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = myforward;
        preforward = myforward;
    }

}
