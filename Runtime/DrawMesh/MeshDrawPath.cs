using System.Collections;
using UnityEngine;

public class MeshDrawPath : MonoBehaviour
{

    public MeshRenderer meshArrow;
    public float offset = 1f;
    public float rateScale = 1f;

    private Transform target;
    private Coroutine routine;

    private void OnDisable()
    {
        StopArrow();
    }

    void OnEnable()
    {
        if (routine != null)
            StopCoroutine(routine);

        if (target != null)
            routine = StartCoroutine(RunArrow());
    }

    /// <summary>
    /// Chỉ đổi target cho mũi tên.
    /// </summary>
    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void StopArrow()
    {
        if (routine != null)
            StopCoroutine(routine);

        routine = null;
        target = null;
    }

    IEnumerator RunArrow()
    {
        while (true)
        {
            if (target == null)
            {
                StopArrow();
                yield break;
            }

            // Quay hướng về target
            transform.LookAt(target);

            // Tính độ dài mũi tên
            float dist = Vector3.Distance(transform.position, target.position);

            // Scale texture
            meshArrow.material.mainTextureScale = new Vector2(dist * offset, 1);

            // Scale chiều dài mũi tên
            transform.localScale = new Vector3(
                transform.localScale.x,
                transform.localScale.y,
                dist * rateScale
            );

            yield return null;
        }
    }
}
