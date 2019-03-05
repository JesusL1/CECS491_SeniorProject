using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGscroll : MonoBehaviour
{
    public float scroll_speech = 0.5f;

    private MeshRenderer mesh_Renderer;
    // Start is called before the first frame update
    void Awake()
    {
        mesh_Renderer = GetComponent<MeshRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        float x = Time.time * scroll_speech;

        Vector2 offset = new Vector2(x, 0);

        mesh_Renderer.sharedMaterial.SetTextureOffset("_MainTex", offset);
    }
}
