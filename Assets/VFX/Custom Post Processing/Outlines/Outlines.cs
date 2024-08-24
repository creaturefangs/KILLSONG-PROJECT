using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(OutlinesRenderer), PostProcessEvent.AfterStack, "Custom/Outlines")]
public sealed class Outlines : PostProcessEffectSettings
{
    public FloatParameter DepthThreshold = new FloatParameter() { value = .5f };
    public FloatParameter NormalThreshold = new FloatParameter() { value = .5f };
    public FloatParameter EdgeWidth = new FloatParameter() { value = 2 };
    public FloatParameter FresnelEdgePower = new FloatParameter() { value = 1 };
    public FloatParameter GrazingAngleMaskPower = new FloatParameter() { value = 1 };
    [Range(0, 1)]
    public FloatParameter GrazingAngleMaskHardness = new FloatParameter() { value = 1 };
    public ColorParameter EdgeColor = new ColorParameter() { value = Color.black };

}
public sealed class OutlinesRenderer : PostProcessEffectRenderer<Outlines>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Outlines"));
        sheet.properties.SetFloat("_DepthThreshold", settings.DepthThreshold);
        sheet.properties.SetFloat("_NormalThreshold", settings.NormalThreshold);
        sheet.properties.SetFloat("_EdgeWidth", settings.EdgeWidth);
        sheet.properties.SetFloat("_Power", settings.FresnelEdgePower);
        sheet.properties.SetFloat("_GrazingAngleMaskPower", settings.GrazingAngleMaskPower);
        sheet.properties.SetFloat("_GrazingAngleMaskHardness", settings.GrazingAngleMaskHardness);
        sheet.properties.SetColor("_EdgeColor", settings.EdgeColor);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}