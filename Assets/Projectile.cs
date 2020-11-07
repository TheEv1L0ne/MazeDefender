using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public void Fly(Vector3 target)
    {
        StartCoroutine(IFly(target));
    }

    public IEnumerator IFly(Vector3 target)
    {
        float step = 0;
        float speed = 0.5f;

        Debug.Log($"target {target}");

        Vector3 currentPos = transform.position;
        if (this != null)
            while (transform.position != target)
        {
            yield return null;
            step += speed * Time.deltaTime;
            
                transform.position = Vector2.MoveTowards(currentPos, target, step);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "City")
        {
            Debug.Log("PRPJECTILE HIT");
            StopAllCoroutines();
            Destroy(this.gameObject);
        }
    }
}
