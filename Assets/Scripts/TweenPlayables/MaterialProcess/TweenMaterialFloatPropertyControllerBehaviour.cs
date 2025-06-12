using System;
using TweenPlayables;

namespace VAT.TweenPlayables
{
    [Serializable]
    public sealed class TweenMaterialFloatPropertyControllerBehaviour : TweenAnimationBehaviour<MaterialFloatPropertyController>
    {
        public FloatTweenParameter value;

        public override void OnTweenInitialize(MaterialFloatPropertyController playerData)
        {
            value.SetInitialValue(playerData, playerData.Value);
        }
    }
}