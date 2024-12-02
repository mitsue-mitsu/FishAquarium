using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateObject : MonoBehaviour
{
    public Texture tex;
    GameObject plane;

    // Start is called before the first frame update
    void Start()
    {
        GameObject plane_prefab = Resources.Load<GameObject>("PhotoOBJ");
        plane = Instantiate(plane_prefab);

        Renderer rend = plane.GetComponent<Renderer>();
        rend.material = new Material(Shader.Find("Unlit/PhotoShader"));
        rend.material.SetTexture("_MainTex", tex);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
