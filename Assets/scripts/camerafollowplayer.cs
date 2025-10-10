using System.Collections;
using UnityEngine;

public class camerafollowplayer : MonoBehaviour
{
    public Transform player;
    public Transform anchorAbove;
    public Transform anchorBelow;

    public float sizeIncreaseAmount = 2f; // How much to increase orthographic size
    public float transitionSpeed = 2f;    // How fast the transition happens
    public float fovIncreaseDuration = 3f; // How long the FOV increase lasts

    private Camera mainCamera;
    private float baseOrthoSize;
    private float targetOrthoSize;
    private float fovBoostOffset = 0f;
    private void Start()
    {
        if (player == null || anchorAbove == null || anchorBelow == null)
        {
            Debug.LogError("Player, AnchorAbove, or AnchorBelow not assigned!");
            enabled = false;
            return;
        }

        mainCamera = GetComponent<Camera>();

        if (!mainCamera.orthographic)
        {
            Debug.LogError("This script is meant for an orthographic camera!");
            enabled = false;
            return;
        }

        baseOrthoSize = mainCamera.orthographicSize;
        targetOrthoSize = baseOrthoSize;
    }

    private void LateUpdate()
    {
        AdjustCameraView();
        SmoothCameraSizeTransition();
    }

    private void AdjustCameraView()
    {
        Bounds bounds = CalculateBounds();
        AdjustOrthographicCamera(bounds);

        Vector3 targetPosition = bounds.center;
        transform.position = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
    }

    private Bounds CalculateBounds()
    {
        Bounds bounds = new Bounds(player.position, Vector3.zero);
        bounds.Encapsulate(anchorAbove.position);
        bounds.Encapsulate(anchorBelow.position);
        return bounds;
    }

    private void AdjustOrthographicCamera(Bounds bounds)
    {
        float verticalSize = bounds.size.y / 2f;
        float horizontalSize = bounds.size.x / mainCamera.aspect / 2f;
        baseOrthoSize = Mathf.Max(verticalSize, horizontalSize);
    }

    private void SmoothCameraSizeTransition()
    {
        targetOrthoSize = baseOrthoSize + fovBoostOffset;
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetOrthoSize, Time.deltaTime * transitionSpeed);
    }

    // Call this function when the particle system is instantiated
    public void IncreaseFOVTemporarily()
    {
        StartCoroutine(FoVIncrease());
    }


    IEnumerator FoVIncrease()
    {
        fovBoostOffset = sizeIncreaseAmount;
        yield return new WaitForSeconds(fovIncreaseDuration);
        fovBoostOffset  = 0f;
    }

}

