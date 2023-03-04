using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public CollectibleResource Type;

    private Animator animator;
    private bool CanCollect = true;
    public bool Generated = false;

    public void Awake()
    {
        CanCollect = false;
        StartCoroutine(CanCollectDelay());

        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Consts.BirdTag && CanCollect)
        {
            if ((other.transform.parent?.parent?.parent.GetComponent<BirdsFormation>().CollectResource(this)).Value)
                Destroy(gameObject);
        }
    }

    private IEnumerator CanCollectDelay()
    {
        yield return new WaitForSeconds(1);
        CanCollect = true;
        if (Generated)
            GetComponentInChildren<Rigidbody>().useGravity = true;
    }

    public void Drop()
    {
        animator.SetBool("DropCalled", true);
    }
}
