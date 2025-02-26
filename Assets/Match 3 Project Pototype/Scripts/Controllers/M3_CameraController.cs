using UnityEngine;

namespace SNGames.M3
{
    public class M3_CameraController : MonoBehaviour
    {
        [SerializeField] private Camera mainGameCamera;
        [SerializeField] private float cameraFOVDecisionOffset;

        #region Public Region

        /// <summary>
        /// Sets the camera position and field of view based on the board size
        /// </summary>
        /// <param name="boardWidth"> Board Width </param>
        /// <param name="boardHeight"> Board Heigth </param>
        public void SetCameraPositionAndFOV(int boardWidth, int boardHeight)
        {
            if (mainGameCamera == null)
            {
                TryGetCameraFromScene();
            }

            SetCameraPosition(boardWidth, boardHeight);
            SetCameraFOV(boardWidth, boardHeight);
        }

        #endregion

        #region  Private Region

        private void TryGetCameraFromScene()
        {
            mainGameCamera = Camera.main;
            if (mainGameCamera == null)
            {
                Debug.LogError("No camera found in the scene");
                return;
            }
        }

        private void SetCameraPosition(int boardWidth, int boardHeight)
        {
            mainGameCamera.transform.position = new Vector3((boardWidth / 2f) - 0.5f, (boardHeight / 2f) - 0.5f, -10);
        }

        private void SetCameraFOV(int boardWidth, int boardHeight)
        {
            //Set Camera FOV
            float orthographicSize_basedOnHeight = (boardHeight + cameraFOVDecisionOffset) / 2f;  //Why? The total height visible is always orthographicSize * 2. So, we need to divide by 2 to get the half height.
            Debug.Log($"Orthographic Size based on height: {orthographicSize_basedOnHeight}");

            float aspectRatio = (float)Screen.width / (float)Screen.height;
            float orthographicSize_basedOnWidth = ((boardWidth / aspectRatio) + cameraFOVDecisionOffset) / 2f;
            Debug.Log($"Aspect Ratio: {aspectRatio}, Orthographic Size based on width: {orthographicSize_basedOnWidth}");

            mainGameCamera.orthographicSize = orthographicSize_basedOnHeight > orthographicSize_basedOnWidth ? orthographicSize_basedOnHeight : orthographicSize_basedOnWidth;
        }
        
        #endregion
    }
}
