using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(OutlinesRenderer), PostProcessEvent.AfterStack, "Custom/Outlines")]
public sealed class Outlines : PostProcessEffectSettings
{
    public FloatParameter Threshold = new FloatParameter() { value = .5f };
    public FloatParameter EdgeWidth = new FloatParameter() { value = 2 };

}
public sealed class OutlinesRenderer : PostProcessEffectRenderer<Outlines>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Outlines"));
        sheet.properties.SetFloat("_Threshold", settings.Threshold);
        sheet.properties.SetFloat("_EdgeWidth", settings.EdgeWidth);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}