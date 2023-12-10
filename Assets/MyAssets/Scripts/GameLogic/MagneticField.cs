using UnityEngine;

namespace MyAssets.Scripts.GameLogic
{
    public class MagneticField : MonoBehaviour
    {
        [SerializeField] private float _magneticForce;

        private void OnTriggerStay2D(Collider2D other)
        {
            if(!other.CompareTag("Proton")) return;
            var forceDirection = transform.position - other.transform.position;
            other.GetComponent<Rigidbody2D>().AddForce(_magneticForce * forceDirection);
        }
    }
}