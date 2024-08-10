using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(PixelEffectRenderer), PostProcessEvent.AfterStack, "Custom/Pixel Effect")]
public sealed class PixelEffect : PostProcessEffectSettings
{
    public IntParameter SampleAmount = new IntParameter { value = 100 };
    public FloatParameter DitherAmount = new FloatParameter { value = 1 };
    public IntParameter QuantizationAmount = new IntParameter { value = 255 };
}
public sealed class PixelEffectRenderer : PostProcessEffectRenderer<PixelEffect>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Pixel Effect"));
        sheet.properties.SetInteger("_SampleAmount", settings.SampleAmount);
        sheet.properties.SetInteger("_QuantizationAmounts", settings.QuantizationAmount);
        sheet.properties.SetFloat("_DitherSpread", settings.DitherAmount);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}