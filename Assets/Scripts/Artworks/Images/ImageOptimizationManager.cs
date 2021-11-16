using System;
using UnityEngine;

namespace Gretas.Artworks.Images
{
    public class ImageOptimizationManager : MonoBehaviour
    {
        public event Action<string> OnProximity = delegate { };

        private bool _isOptimized;

        private void OnTriggerEnter(Collider other)
        {
            if (!_isOptimized && other.GetComponent<CharacterController>())
            {
                _isOptimized = true;
                OnProximity(GetComponentInParent<ImageDisplay>().Id);
            }
        }
    }
}