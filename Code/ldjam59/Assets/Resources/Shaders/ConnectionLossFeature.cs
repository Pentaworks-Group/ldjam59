using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.RenderGraphModule;
using Assets.Scripts.Scenes.Game;

public class ConnectionLossFeature : ScriptableRendererFeature
{
    class ConnectionLossFeaturePass : ScriptableRenderPass
    {
        private Material _mat;

        public ConnectionLossFeaturePass(Material mat)
        {
            _mat = mat;
            renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        }

        private class PassData
        {
            public TextureHandle src;
            public TextureHandle dst;
            public Material mat;
        }

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            var fx = ConnectionLossEffect.Instance;
            if (fx == null || !fx.IsEffectActive) return;

            var cameraData = frameData.Get<UniversalCameraData>();
            if (cameraData.cameraType != CameraType.Game)
            {
                return;
            }

            UpdateMaterial(fx);

            var resourceData = frameData.Get<UniversalResourceData>();
            TextureHandle src = resourceData.activeColorTexture;


            //Create a temp texture with the same descriptor
            var desc = renderGraph.GetTextureDesc(src);
            desc.name = "_ConnectionLossTemp";
            desc.clearBuffer = false;
            TextureHandle tmp = renderGraph.CreateTexture(desc);

            // Pass 1: blit src -> tmp with effect material

            using (var builder = renderGraph.AddUnsafePass<PassData>("ConnectionLoss_Blit", out var passData))
            {
                passData.src = src;
                passData.dst = tmp;
                passData.mat = _mat;

                builder.UseTexture(passData.src, AccessFlags.Read);
                builder.UseTexture(passData.dst, AccessFlags.Write);

                builder.SetRenderFunc((PassData data, UnsafeGraphContext ctx) =>
                {
                    CommandBuffer cmd = CommandBufferHelpers.GetNativeCommandBuffer(ctx.cmd);
                    Blitter.BlitCameraTexture(cmd, data.src, data.dst, data.mat, 0);
                });
            }

            // Pass 2: blit tmp back -> src (copy back to active color)
            using (var builder = renderGraph.AddUnsafePass<PassData>("ConnectionLoss_CopyBack", out var passData))
            {
                passData.src = tmp;
                passData.dst = src;
                passData.mat = null;

                builder.UseTexture(passData.src, AccessFlags.Read);
                builder.UseTexture(passData.dst, AccessFlags.Write);

                builder.SetRenderFunc((PassData data, UnsafeGraphContext ctx) =>
                {
                    CommandBuffer cmd = CommandBufferHelpers.GetNativeCommandBuffer(ctx.cmd);
                    Blitter.BlitCameraTexture(cmd, data.src, data.dst );
                });
            }
        }

        private void UpdateMaterial(ConnectionLossEffect fx)
        {
            float t = 1f - fx.CurrentBlend;
            _mat.SetFloat("_StaticIntensity", fx.staticIntensity * t);
            _mat.SetFloat("_StaticSpeed", fx.staticSpeed);
            _mat.SetFloat("_ScanlineIntensity", fx.scanlineIntensity * t);
            _mat.SetFloat("_ScanlineCount", fx.scanlineCount);
            _mat.SetFloat("_JitterAmount", fx.jitterAmount * t);
            _mat.SetFloat("_RollSpeed", fx.rollSpeed);
            _mat.SetFloat("_VignetteStrength", fx.vignetteStrength * t);
            _mat.SetFloat("_Time2", Time.time);
        }

        public void Dispose() { }
    }

    private ConnectionLossFeaturePass _pass;
    private Material _mat;

    public override void Create()
    {
        var shader = Shader.Find("Hidden/ConnectionLoss");

        if (shader == null)
        {
            Debug.LogError("Hidden/ConnectionLoss shader not found!");
            return;
        }

        _mat = new Material(shader);
        _pass = new ConnectionLossFeaturePass(_mat);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData data)
    {
        if (_pass != null)
            renderer.EnqueuePass(_pass);
    }

    protected override void Dispose(bool disposing)
    {
        _pass?.Dispose();
        if (_mat) CoreUtils.Destroy(_mat);
    }
}
