using System.Collections.Generic;
using TweenPlayables.Editor;
using UnityEngine;
using UnityEditor;
using UnityEditor.Timeline;

namespace VAT.TweenPlayables.Editor
{
    [CustomTimelineEditor(typeof(TweenMaterialFloatPropertyControllerTrack))]
    public sealed class TweenMaterialFloatPropertyControllerTrackEditor : TweenAnimationTrackEditor
    {
        public override Color TrackColor => Styles.RendererColor;
        public override Texture2D TrackIcon => Styles.SpriteRendererIcon;
        public override string DefaultTrackName => "Tween MaterialFloatPropertyController Track";
    }

    [CustomTimelineEditor(typeof(TweenMaterialFloatPropertyControllerClip))]
    public sealed class TweenMaterialFloatPropertyControllerClipEditor : TweenAnimationClipEditor
    {
        public override string DefaultClipName => "Tween MaterialFloatPropertyController";
        public override Color ClipColor => Styles.RendererColor;
        public override Texture2D ClipIcon => Styles.SpriteRendererIcon;
    }

    [CustomPropertyDrawer(typeof(TweenMaterialFloatPropertyControllerBehaviour))]
    public sealed class TweenMaterialFloatPropertyControllerBehaviourDrawer : TweenAnimationBehaviourDrawer
    {
        static readonly string[] parameters = new string[]
        {
            "value",
        };

        protected override IEnumerable<string> GetPropertyNames() => parameters;
    }
}
