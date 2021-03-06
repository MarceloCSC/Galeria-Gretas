using Gretas.User;
using UnityEngine;

namespace Gretas.Core
{
    public class ScenesManager : MonoBehaviour
    {
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private Camera _secondaryCamera;
        [SerializeField] private GameObject _navigationPanel;

        [Header("Secondary Camera")]
        [SerializeField] private Vector3 _initialPosition;
        [SerializeField] private Quaternion _initialRotation;
        [SerializeField] private float _speed = 2.5f;
        [SerializeField] private float _secondsBeforeStart = 1.0f;

        private bool _isCameraInPlace;

        private UserMovement _userMovement;
        private UserVision _userVision;

        private void Start()
        {
            var user = GameObject.FindGameObjectWithTag("User");
            _userMovement = user.GetComponent<UserMovement>();
            _userVision = user.GetComponent<UserVision>();
            _userMovement.CanMove = false;
            _userVision.CanLook = false;
            _mainCamera.enabled = false;
            _secondaryCamera.enabled = true;
            _secondaryCamera.transform.SetPositionAndRotation(_initialPosition, _initialRotation);
            _navigationPanel.SetActive(false);
        }

        private void LateUpdate()
        {
            if (!_isCameraInPlace && Time.timeSinceLevelLoad > _secondsBeforeStart)
            {
                ResettingCamera();
            }
        }

        private void ResettingCamera()
        {
            Vector3 position = Vector3.MoveTowards(_secondaryCamera.transform.position, _mainCamera.transform.position, _speed * Time.deltaTime);
            Quaternion rotation = Quaternion.Slerp(_secondaryCamera.transform.rotation, _mainCamera.transform.rotation, _speed * Time.deltaTime);

            _secondaryCamera.transform.SetPositionAndRotation(position, rotation);

            if (Vector3.Distance(_secondaryCamera.transform.position, _mainCamera.transform.position) <= 0.1f &&
                Quaternion.Angle(_secondaryCamera.transform.rotation, _mainCamera.transform.rotation) <= 0.1f)
            {
                _secondaryCamera.enabled = false;
                _mainCamera.enabled = true;
                _isCameraInPlace = true;
                _userMovement.CanMove = true;
                _userVision.CanLook = true;
                _navigationPanel.SetActive(true);
            }
        }
    }
}