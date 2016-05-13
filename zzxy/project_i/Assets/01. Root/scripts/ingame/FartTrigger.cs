using UnityEngine;
using System.Collections;

public class FartTrigger : MonoBehaviour
{
    public GameObject explosion;		// Prefab of explosion effect.
    public int m_DamageValue = 1;

    // Use this for initialization
    void Start()
    {

    }


    void OnExplode()
    {
        //// Create a quaternion with a random rotation in the z-axis.
        //Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        //// Instantiate the explosion where the rocket is with the random rotation.
        //Instantiate(explosion, transform.position, randomRotation);
    }

    void OnTriggerEnter(Collider col)
    {
        // If it hits an enemy...
        if (col.tag == "Enemy")
        {

            // ... find the Enemy script and call the Hurt function.
            col.gameObject.GetComponent<Enemy>().Hurt(m_DamageValue);

            // Call the explosion instantiation.
            OnExplode();

        }
    }
}
