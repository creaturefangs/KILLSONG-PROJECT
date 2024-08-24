using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(SharpenEffectRenderer), PostProcessEvent.AfterStack, "Custom/Sharpen")]
public sealed class SharpenEffect : PostProcessEffectSettings
{
    public FloatParameter SharpenAmount = new FloatParameter { value = 1 };
}
public sealed class SharpenEffectRenderer : PostProcessEffectRenderer<SharpenEffect>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Sharpen"));
        sheet.properties.SetFloat("_SharpnessAmount", settings.SharpenAmount);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}