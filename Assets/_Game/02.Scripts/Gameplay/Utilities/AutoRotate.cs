using UnityEngine;

namespace _Game._02.Scripts.Gameplay
{
    public class AutoRotate : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;

        private void Update()
        {
            transform.Rotate(Vector3.up, speed * Time.deltaTime);
        }
    }
}