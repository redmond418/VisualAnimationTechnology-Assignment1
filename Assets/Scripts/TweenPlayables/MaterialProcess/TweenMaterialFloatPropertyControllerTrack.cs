#if UNITY_EDITOR
using System.ComponentModel;
#endif
using TweenPlayables;
using UnityEngine.Timeline;

namespace VAT.TweenPlayables
{
    [TrackBindingType(typeof(MaterialFloatPropertyController))]
    [TrackClipType(typeof(TweenMaterialFloatPropertyControllerClip))]
#if UNITY_EDITOR
    [DisplayName("Tween Playables/VAT/Material Float Property Controller")]
#endif
    public sealed class TweenMaterialFloatPropertyControllerTrack : TweenAnimationTrack<MaterialFloatPropertyController, TweenMaterialFloatPropertyControllerMixerBehaviour, TweenMaterialFloatPropertyControllerBehaviour> { }
}