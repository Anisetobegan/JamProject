using DG.Tweening;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Vector3 offset;
    float baseZDistance = -35.3f;
    float smoothTime = 3f;
    Transform targetToFollow;
    //Vector3 currentVelocity = Vector3.zero;

    [SerializeField] Transform playerTransform;
    Transform levelCameraPos;

    private void FixedUpdate()
    {
        Vector3 targetPosition = targetToFollow.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothTime * Time.fixedDeltaTime);
    }

    public void SetCameraBehaviour(Transform cameraPos, Vector3 playerStartPos)
    {
        levelCameraPos = cameraPos;
        if (levelCameraPos != null)
        {
            targetToFollow = levelCameraPos;
            //offset = Vector3.zero;
            if (GameManager.Instance.IsStartOfGame)
            {
                transform.position = levelCameraPos.position;
                offset = Vector3.zero;
            }
            else
            {
                transform.DOMove(levelCameraPos.position, GameManager.Instance.TransitionTime).SetEase(Ease.InOutSine).OnComplete(() => offset = Vector3.zero);
            }
        }
        else
        {
            //transform.position = new Vector3 (playerTransform.position.x, playerTransform.position.y, transform.position.z);
            targetToFollow = playerTransform;
            playerStartPos.z = baseZDistance;
            if (GameManager.Instance.IsStartOfGame)
            {
                transform.position = new Vector3(targetToFollow.position.x, targetToFollow.position.y, baseZDistance);
                offset = offset = transform.position - targetToFollow.position;
            }
            else
            {
                transform.DOMove(playerStartPos, GameManager.Instance.TransitionTime).SetEase(Ease.InOutSine).OnComplete(() => offset = transform.position - targetToFollow.position);
            }
        }
    }
}
