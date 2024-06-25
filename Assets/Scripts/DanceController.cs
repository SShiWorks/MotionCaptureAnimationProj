using UnityEngine;
using UnityEngine.Playables;

public class DanceController : MonoBehaviour
{
    public Animator[] animator0; // 角色的动画控制器
    public Animator animator;
    public Transform initialPosition; // 初始位置和朝向
    public AudioSource audioSource;
    public AudioClip[] danceClips;
    public GameObject Timeline;
    public PlayableDirector playableDirector;
    public PlayableAsset[] TimeLine_Cam;
    public static DanceController API;

    // 定义每个舞蹈的初始帧
    public int[] danceStartFrames = new int[7];
    public int[] danceEndFrames = new int[7];

    private void Start()
    {
        API = this;
        // 确保角色初始位置和朝向设置正确
        ResetPositionAndRotation();
        //Timeline.SetActive(false);
    }

    private void Update()
    {
        /*
        // 监听小键盘数字键的输入
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            animator.SetTrigger("GoodTime");
            audioSource.clip = danceClips[0];
            audioSource.Play();
            playableDirector.Play(TimeLine_Cam[0]);
            Timeline.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            animator.SetTrigger("夏日妄想");
            audioSource.clip = danceClips[1];
            audioSource.Play();
            Timeline.SetActive(true);
            playableDirector.Play(TimeLine_Cam[1]);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            animator.SetTrigger("寄明月");
            audioSource.clip = danceClips[2];
            audioSource.Play();
            Timeline.SetActive(true);
            playableDirector.Play(TimeLine_Cam[2]);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            animator.SetTrigger("爱你");
            audioSource.clip = danceClips[3];
            audioSource.Play();
            Timeline.SetActive(true);
            playableDirector.Play(TimeLine_Cam[3]);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            animator.SetTrigger("恋爱告急");
            audioSource.clip = danceClips[4];
            audioSource.Play();
            Timeline.SetActive(true);
            playableDirector.Play(TimeLine_Cam[4]);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            animator.SetTrigger("热恋情节");
            audioSource.clip = danceClips[5];
            audioSource.Play();
            Timeline.SetActive(true);
            playableDirector.Play(TimeLine_Cam[5]);
        }

        CheckDanceState();
        */
    }

    public void DancePlay(int danceNum, int camNum, int peopleNum)
    {
        Debug.Log("DancePlay触发" + danceNum + "cam" + camNum + "peop" + peopleNum);
        switch (peopleNum)
        {
            case 1:
                animator = animator0[0];
                break;
            case 2:
                animator = animator0[1];
                break;
            case 3:
                animator = animator0[2];
                break;
            case 4:
                animator = animator0[3];
                break;
        }

        CameraControllerActive.API.isNeedController = false;
        switch (camNum)
        {
            case 1:
                playableDirector.Play(TimeLine_Cam[0]);
                break;
            case 2:
                playableDirector.Play(TimeLine_Cam[1]);
                break;
            case 3:
                playableDirector.Play(TimeLine_Cam[2]);
                break;
            case 4:
                playableDirector.Play(TimeLine_Cam[3]);
                break;
            case 5:
                playableDirector.Play(TimeLine_Cam[4]);
                break;
            case 6:
                playableDirector.Play(TimeLine_Cam[5]);
                break;
            case 7: //正机位
                playableDirector.playableAsset = null;
                CameraControllerActive.API.SetMainCameraPosition();
                break;
            case 8: //特写
                playableDirector.playableAsset = null;
                CameraControllerActive.API._isAutoRoundWithCharacter = true;
                break;
            case 9: //自由镜头
                playableDirector.playableAsset = null;
                CameraControllerActive.API.isNeedController = true;
                break;
        }


        switch (danceNum)
        {
            case 1:
                animator.SetTrigger("GoodTime");
                audioSource.clip = danceClips[0];
                audioSource.Play();
                Timeline.SetActive(true);
                break;
            case 2:
                animator.SetTrigger("夏日妄想");
                audioSource.clip = danceClips[1];
                audioSource.Play();
                Timeline.SetActive(true);
                break;
            case 3:
                animator.SetTrigger("寄明月");
                audioSource.clip = danceClips[2];
                audioSource.Play();
                break;
            case 4:
                animator.SetTrigger("爱你");
                audioSource.clip = danceClips[3];
                audioSource.Play();
                break;
            case 5:
                animator.SetTrigger("恋爱告急");
                audioSource.clip = danceClips[4];
                audioSource.Play();
                break;
            case 6:
                animator.SetTrigger("热恋情节");
                audioSource.clip = danceClips[5];
                audioSource.Play();
                break;
        }
    }


    private void ChangeDance(string danceTrigger, int startFrame, int endFrame)
    {
        // 先进入待机状态
        //animator.SetTrigger("待机");

        // 重置位置和朝向
        ResetPositionAndRotation();

        // 等待待机动画完成后切换舞蹈动画
        StartCoroutine(PlayDanceAfterIdle(danceTrigger, startFrame, endFrame));
    }

    private void ResetPositionAndRotation()
    {
        transform.position = initialPosition.position;
        transform.rotation = initialPosition.rotation;
    }

    private System.Collections.IEnumerator PlayDanceAfterIdle(string danceTrigger, int startFrame, int endFrame)
    {
        // 重置所有Trigger和位置
        ResetAllTriggers();


        ResetPositionAndRotation();

        // 设置舞蹈触发器
        animator.SetTrigger(danceTrigger);

        // 等待一帧以确保动画切换完成
        yield return new WaitForEndOfFrame();

        // 获取当前动画状态信息
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // 使用Play方法从指定帧开始播放动画
        float normalizedTime = startFrame / stateInfo.length / animator.GetCurrentAnimatorClipInfo(0).Length;
        animator.Play(stateInfo.fullPathHash, -1, normalizedTime);


        // 等待直到达到结束帧
        while (true)
        {
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            float currentNormalizedTime = stateInfo.normalizedTime * stateInfo.length *
                                          animator.GetCurrentAnimatorClipInfo(0).Length;
            if (currentNormalizedTime >= endFrame)
            {
                break;
            }

            yield return null;
        }

        // 进入待机状态
        //animator.SetTrigger("待机");
        ResetPositionAndRotation();
    }

    private void ResetAllTriggers()
    {
        // 重置Animator中所有的Trigger
        animator.ResetTrigger("GoodTime");
        animator.ResetTrigger("夏日妄想");
        animator.ResetTrigger("寄明月");
        animator.ResetTrigger("爱你");
        animator.ResetTrigger("恋爱告急");
        animator.ResetTrigger("热恋情节");
        //animator.ResetTrigger("待机");
    }

    bool isDancing = false;

    private void CheckDanceState()
    {
        //检测到当前动画状态
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("待机"))
        {
            SceneModManage.API._currentCharacter.transform.position = initialPosition.position;
            SceneModManage.API._currentCharacter.transform.rotation = initialPosition.rotation;
            //  isDancing = false;
        }
        /*
        if(stateInfo.IsName("GoodTime")&&isDancing==true)
        {

        float normalizedTime = danceStartFrames[0] / stateInfo.length / animator.GetCurrentAnimatorClipInfo(0).Length;
        //animator.Play(stateInfo.fullPathHash, -1, normalizedTime);
        animator.Play("GoodTime", -1, normalizedTime);
        isDancing=false;
        Debug.Log(normalizedTime);

        }*/
    }
}