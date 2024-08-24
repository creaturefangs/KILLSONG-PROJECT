using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(CRTRenderer), PostProcessEvent.AfterStack, "Custom/CRT")]
public sealed class CRT : PostProcessEffectSettings
{
    public FloatParameter Curvature = new FloatParameter() { value = 10 };
    public FloatParameter VingetteWidth = new FloatParameter() { value = 5 };
}
public sealed class CRTRenderer : PostProcessEffectRenderer<CRT>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/CRT"));
        sheet.properties.SetFloat("_Curvature", settings.Curvature);
        sheet.properties.SetFloat("_VingetteWidth", settings.VingetteWidth);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}