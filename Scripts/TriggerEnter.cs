using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnter : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var ai = transform.parent.GetComponent<AISnakeController>();
        if (collision.tag== "AIBorder")
        {
            
            var vec3 = ai.direction.eulerAngles;
            //if (vec3.z < 90)
            //{
            //    vec3.z = Random.Range(180f, 270f);
            //}
            //else if (vec3.z < 180)
            //{
            //    vec3.z = Random.Range(270f, 360f);
            //}
            //else if (vec3.z < 270)
            //{
            //    vec3.z = Random.Range(0f, 90f);
            //}
            //else
            //    vec3.z = Random.Range(90f, 180f);
            var zAxis = vec3.z;
            vec3.z=Random.Range(zAxis + 180 - 10, zAxis + 180 + 10);
            ai.direction = Quaternion.Euler(vec3);
        }if (collision.tag=="Player")
        {
            if (ai._bodys.Contains(collision.gameObject))
                return;

            var vec3 = ai.direction.eulerAngles;
            var zAxis = vec3.z;
            vec3.z = Random.Range(zAxis + 180 - 10, zAxis + 180 + 10);
            ai.direction = Quaternion.Euler(vec3);

        }
    }
}
