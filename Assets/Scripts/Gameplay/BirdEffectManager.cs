using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdEffectManager : MonoBehaviour
{
    public List<GameObject> DefenceSpheres;

    public GameObject CollectSphere;

    public GameObject AttackEffect;

    void Start()
    {
        DisableAll();
    }

    public void ChangeEffect(FormationType type)
    {
        DisableAll();
        switch (type)
        {
            case FormationType.AttackFormation:
                AttackEffect.SetActive(true);
                break;
            case FormationType.DefenceFormation:
                foreach (var sphere in DefenceSpheres)
                    sphere.SetActive(true);
                break;
            case FormationType.CollectFormation:
                CollectSphere.SetActive(true);
                break;
        }
    }

    public void DisableAll()
    {
        foreach (var sphere in DefenceSpheres)
            sphere.SetActive(false);

        AttackEffect.SetActive(false);

        CollectSphere.SetActive(false);
    }
}
