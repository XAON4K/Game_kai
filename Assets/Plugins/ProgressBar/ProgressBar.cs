using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Transform holder;

    [Range(0, 1)] public float value;

    private void Update()
    {
        holder.localScale = new Vector2(value, holder.localScale.y);
    }
}
