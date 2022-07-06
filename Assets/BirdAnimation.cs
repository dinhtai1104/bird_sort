using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using System;
using Random = UnityEngine.Random;

public class BirdAnimation : MonoBehaviour
{
    public static readonly string IDLE = "idle";
    public static readonly string GROUNDING = "grounding";
    public static readonly string FLY = "fly";
    public static readonly string TOUCHING = "touching";

    public SkeletonAnimation skeletonBird;
    private System.Action callback;

    private Spine.TrackEntry lastTrackEntry;

    private void Start()
    {
        skeletonBird.state.Event += OnEvent;
    }


    public void SetSkin(string skin)
    {
        skeletonBird.state.Event -= OnEvent;
        skeletonBird.Skeleton.SetSkin(skin);
        skeletonBird.state.Event += OnEvent;
    }

    public void SetAnim(string anim, System.Action callback = null, bool loop = false)
    {
        this.callback = callback;
        skeletonBird.AnimationState.ClearTracks();
        if (lastTrackEntry != null && anim != FLY)
        {
            lastTrackEntry.Loop = false;
        }
        lastTrackEntry = skeletonBird.AnimationState.SetAnimation(0, anim, loop);
        float length = lastTrackEntry.Animation.Duration;
        //lastTrackEntry.MixDuration = 0.5f;
        lastTrackEntry.AnimationStart = length * Random.Range(0, 0.2f);

        if (anim == GROUNDING)
        {
            lastTrackEntry.Loop = false;
            lastTrackEntry.Complete += (e) =>
            {
                GameController.Instance.state = STATE.NONE;
                lastTrackEntry = skeletonBird.AnimationState.AddAnimation(0, IDLE, true, 0);
            };
        }
    }

    public void AddAnimation(string anim)
    {
        Spine.TrackEntry track = skeletonBird.AnimationState.AddAnimation(1, anim, true, 0);
    }

    private void OnEvent(TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data.Name == "touching")
        {
            Debug.Log("Event");
            callback?.Invoke();
        }
    }

    public void SetSkeletonData(SkeletonDataAsset data)
    {
        if (skeletonBird == null)
        {
            skeletonBird = GetComponent<SkeletonAnimation>();
        }
        skeletonBird.skeletonDataAsset = data;

        skeletonBird.Initialize(true);

    }

    public void ClearTrack()
    {
        skeletonBird.state.SetEmptyAnimation(1, 0);
    }
}
