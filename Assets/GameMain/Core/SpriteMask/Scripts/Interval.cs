using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKit;
using GameKit.Timer;
using DG.Tweening;

public enum IntervalType
{
    Horizontal,
    Vertical
}
public class Interval : MonoBehaviour
{
    public List<Transform> betweenings = new List<Transform>();
    public IntervalType intervalType = IntervalType.Horizontal;
    public Mask maskA, maskB;
    public Transform leftEdge, rightEdge;
    public bool CanTravel = false;
    public bool IsTravelActive = true;
    private float initOffset, initScaleA, initScaleB;
    private SpriteRenderer mySpriteRenderer;
    private Ticker_Auto ticker;
    private float activateTime = 2f;
    private float activateDistanceOffset = 2f;
    public string WorldName;
    public string WorldBName;
    public bool isMoving = false;

    private void Start()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        ticker = new Ticker_Auto(activateTime);
        ticker.Register(EnableTravel);
        ticker.Start();
        ticker.Pause();

        if (intervalType == IntervalType.Horizontal)
        {
            initOffset = this.transform.position.x;
            initScaleA = maskA.transform.localScale.x;
            initScaleB = maskB.transform.localScale.x;
        }
    }

    private void Update()
    {
        if (intervalType == IntervalType.Horizontal)
        {
            float substract = this.transform.position.x - initOffset;
            maskA.transform.localScale = new Vector2(initScaleA + (substract / 10), maskA.transform.localScale.y);
            maskB.transform.localScale = new Vector2(initScaleB - (substract / 10), maskB.transform.localScale.y);

            for (int i = 0; i < betweenings.Count; i++)
            {
                if (betweenings[i] == null)
                    continue;

                float disToJuncture = Mathf.Abs(betweenings[i].position.x - this.transform.position.x);

                if (isMoving)
                {
                    float volume = Mathf.Clamp(5 / disToJuncture, 0.1f, 1f);
                    GlobalSound.current.PlaySound("????????????", volume);
                }

                // Debug.Log(disToJuncture);
                if (IsTravelActive && disToJuncture < activateDistanceOffset)
                {
                    ticker.Resume();
                    break;
                }
                else
                {
                    ticker.Pause();

                    // ??????????????????????????????Inverval??? ???????????????Interval??????????????????????????????
                    if (!IsTravelActive && disToJuncture >= activateDistanceOffset)
                        IsTravelActive = true;

                    // ?????????????????????Inverval????????????Inverval??? ????????????????????????????????????
                    if (CanTravel)
                    {
                        CanTravel = false;
                        this.transform.DOScaleX(2f, 0.1f).SetEase(Ease.Flash);
                    }
                }
            }

        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            float playerX = other.gameObject.transform.position.x;
            float intervalX = this.transform.position.x;
            if (CanTravel && Mathf.Abs(playerX - intervalX) <= 0.1f)
            {
                other.GetComponent<PlayerAgent>().SwitchWorld(WorldName, WorldBName);
                CanTravel = false;
                IsTravelActive = false;
                this.transform.DOScaleX(2f, 0.1f).SetEase(Ease.Flash);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {

    }

    private void EnableTravel()
    {
        Utility.Debugger.LogWarning("EnableTravel");
        CanTravel = true;
        this.transform.DOScaleX(0.5f, 0.1f).SetEase(Ease.InOutBounce);
    }

    private void PlaySound()
    {

    }
}
