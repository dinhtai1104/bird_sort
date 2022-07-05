using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using System;

public class BirdAnimation : MonoBehaviour
{
    public static readonly string IDLE = "idle";
    public static readonly string GROUNDING = "grounding";
    public static readonly string FLY = "fly";
    public static readonly string TOUCHING = "touching";

    public SkeletonAnimation skeletonBird;
    private System.Action callback;

    private void Start()
    {
        skeletonBird.state.Event += OnEvent;
    }

    public void SetSkin(string skin)
    {
        skeletonBird.Skeleton.SetSkin(skin);
    }

    public void SetAnim(string anim, System.Action callback = null)
    {
        this.callback = callback;
        Spine.TrackEntry track = skeletonBird.AnimationState.SetAnimation(0, anim, true);
        if (anim == GROUNDING)
        {
            track.Loop = false;
            track.Complete += (e) =>
            {
                SetAnim(IDLE);
            };
        }
    }

    private void OnEvent(TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data.Name == "touching")
        {
            Debug.Log("Event");
            callback?.Invoke();
        }
    }
}
