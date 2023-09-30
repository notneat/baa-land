using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScript : MonoBehaviour
{
    [SerializeField] private int chopCount;

    public void Chop(int chopAmount)
    {
        this.chopCount -= chopAmount;

        if(this.chopCount <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
