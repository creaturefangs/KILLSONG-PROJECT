using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(NightVisionRenderer), PostProcessEvent.AfterStack, "Custom/Night Vision")]
public sealed class NightVision : PostProcessEffectSettings
{
    public FloatParameter ViewDistance = new FloatParameter() { value = 10 };
    [Range(0,1)]
    public FloatParameter GrainAmount = new FloatParameter() { value = 10 };
    public ColorParameter NVGColorLow = new ColorParameter() { value = new Color(1, 1, 1) };
    public ColorParameter NVGColorHigh = new ColorParameter() { value = new Color(1, 1, 1) };
}
public sealed class NightVisionRenderer : PostProcessEffectRenderer<NightVision>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Night Vision"));
        sheet.properties.SetFloat("_ViewDistance", settings.ViewDistance);
        sheet.properties.SetFloat("_GrainAmount", settings.GrainAmount);
        sheet.properties.SetColor("_NightVisionColorLow", settings.NVGColorLow);
        sheet.properties.SetColor("_NightVisionColorHigh", settings.NVGColorHigh);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}