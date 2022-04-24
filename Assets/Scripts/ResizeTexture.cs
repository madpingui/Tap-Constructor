using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeTexture : MonoBehaviour
{

    public int meshType = 0;
    public Renderer _renderer;
    private MaterialPropertyBlock _propBlock;
    // Start is called before the first frame update
    private void Awake()
    {
        _propBlock = new MaterialPropertyBlock();
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ResizeTextureUV(float sizeX, float sizeZ)
    {
        _renderer.GetPropertyBlock(_propBlock);
        
        _propBlock.SetFloat("_ScaleX", sizeX);
        _propBlock.SetFloat("_ScaleZ", sizeZ);
        _renderer.SetPropertyBlock(_propBlock);
    }

    
}
