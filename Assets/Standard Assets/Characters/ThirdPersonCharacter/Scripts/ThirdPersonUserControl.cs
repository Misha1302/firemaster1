using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
        [SerializeField] private KeyCode keyCode = KeyCode.C;
        private readonly Vector2 _mCamForwardMultiplyVector = new Vector3(1, 0, 1);
        private bool _isMainCameraNotNull;
        private Transform _mCam;
        private Vector3 _mCamForward;
        private ThirdPersonCharacter _mCharacter;
        private bool _mJump;
        private Vector3 _mMove;


        private void Start()
        {
            _isMainCameraNotNull = _mCam != null;
            // get the transform of the main camera
            if (Camera.main != null)
            {
                _mCam = Camera.main.transform;
            }
            else
            {
                const string message =
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.";
                Debug.LogWarning(message, gameObject);
            }

            _mCharacter = GetComponent<ThirdPersonCharacter>();
        }


        private void Update()
        {
            if (!_mJump) _mJump = Input.GetButtonDown("Jump");
        }


        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            // read inputs
            var h = Input.GetAxis("Horizontal");
            var v = Input.GetAxis("Vertical");
            var crouch = Input.GetKey(keyCode);

            if (_isMainCameraNotNull)
            {
                _mCamForward = Vector3.Scale(_mCam.forward, _mCamForwardMultiplyVector).normalized;
                _mMove = v * _mCamForward + h * _mCam.right;
            }
            else
            {
                _mMove = v * Vector3.forward + h * Vector3.right;
            }
            
            if (Input.GetKey(KeyCode.LeftShift)) _mMove *= 0.5f;

            _mCharacter.Move(_mMove, crouch, _mJump);
            _mJump = false;
        }
    }
}