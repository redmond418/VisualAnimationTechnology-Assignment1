using TweenPlayables;

namespace VAT.TweenPlayables
{
    public sealed class TweenMaterialFloatPropertyControllerMixerBehaviour : TweenAnimationMixerBehaviour<MaterialFloatPropertyController, TweenMaterialFloatPropertyControllerBehaviour>
    {
        readonly FloatValueMixer valueMixer = new();

        public override void Blend(MaterialFloatPropertyController binding, TweenMaterialFloatPropertyControllerBehaviour behaviour, float weight, float progress)
        {
            valueMixer.TryBlend(behaviour.value, binding, progress, weight);
        }

        public override void Apply(MaterialFloatPropertyController binding)
        {
            valueMixer.TryApplyAndClear(binding, (x, binding) => binding.Value = x);
        }
    }
}