using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [NonSerialized]
    public float Weight = 1;

    public CollectibleResource Type;

    public Mesh foodMesh;
    public Mesh coinMesh;
    public Mesh capMesh;

    public Material foodMaterial;
    public Material coinMaterial;
    public Material capMaterial;

    public float BaseScale = 30.0f;

    public Collectible()
    {
    }

    public Collectible(CollectibleResource Type)
    {
        this.Type = Type;
    }

    public void Awake()
    {
        transform.localScale = new Vector3(BaseScale, BaseScale, BaseScale);
        switch (Type)
        {
            case CollectibleResource.Food:
                GetComponent<MeshFilter>().mesh = foodMesh;
                GetComponent<Renderer>().material = foodMaterial;
                Weight = GameBalance.instance.foodWeight;
                break;
            case CollectibleResource.Coin:
                GetComponent<MeshFilter>().mesh = coinMesh;
                GetComponent<Renderer>().material = coinMaterial;
                Weight = GameBalance.instance.coinWeight;
                break;
            case CollectibleResource.Cap:
                GetComponent<MeshFilter>().mesh = capMesh;
                GetComponent<Renderer>().material = capMaterial;
                Weight = GameBalance.instance.capWeight;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Consts.BirdTag)
        {
            if (PlayerManager.instance.CollectResource(this))
                Destroy(gameObject);
        }
    }
}
