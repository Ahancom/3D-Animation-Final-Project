using UnityEngine;
using UnityEngine.Playables;

public class CameraSwitchAfterTimeline : MonoBehaviour
{
    [Header("Timelines")]
    public PlayableDirector timelineA; // 初始Timeline
    public PlayableDirector timelineB; // 跳跃视角的Timeline

    [Header("Cameras")]
    public Camera cameraA; // TimelineA使用
    public Camera cameraB; // 跟随摄像机
    public Camera cameraC; // TimelineB使用

    [Header("Follow Settings")]
    public Transform player;
    private Vector3 followOffset;
    private Quaternion followRotation;
    private bool isFollowing = false;
    private float followTime = 0f;
    private bool triggeredSecondTimeline = false;

    [Header("Jump Animation")]
    public Animator playerAnimator;

    void Start()
    {
        // 初始化：只有TimelineA相机启用
        cameraA.enabled = true;
        cameraB.enabled = false;
        cameraC.enabled = false;

        timelineA.stopped += OnTimelineAFinished;
    }

    void OnTimelineAFinished(PlayableDirector director)
    {
        // 记录结束时的视角
        followOffset = cameraA.transform.position - player.position;
        followRotation = cameraA.transform.rotation;

        // 设置相机B位置角度
        cameraB.transform.position = cameraA.transform.position;
        cameraB.transform.rotation = followRotation;

        // 切换相机
        cameraA.enabled = false;
        cameraB.enabled = true;

        isFollowing = true;
    }

    void LateUpdate()
    {
        if (isFollowing)
        {
            followTime += Time.deltaTime;

            // 摄像机B 跟随位置
            cameraB.transform.position = player.position + followOffset;
            cameraB.transform.rotation = followRotation;

            // 达到5秒后切换到TimelineB
            if (followTime >= 7f && !triggeredSecondTimeline)

            {
                TriggerSecondTimeline();
            }
        }
    }

    void TriggerSecondTimeline()
    {
        // 停止跟随
        isFollowing = false;
        triggeredSecondTimeline = true;

        // 播放跳跃动画
        playerAnimator.SetTrigger("Jump");

        // 切换相机
        cameraB.enabled = false;
        cameraC.enabled = true;

        // 播放第二段 Timeline（控制 CameraC）
        timelineB.Play();
    }
}
