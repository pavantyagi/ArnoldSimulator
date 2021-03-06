﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace GoodAI.Arnold.Visualization.Models
{
    public class CompositeNeuronId
    {
        private const int RegionIndexOffset = 22;

        public uint RegionIndex { get; }
        public uint NeuronIndex { get; }

        public CompositeNeuronId(uint regionIndex, uint neuronIndex)
        {
            RegionIndex = regionIndex;
            NeuronIndex = neuronIndex;
        }

        public override bool Equals(object obj)
        {
            var other = obj as CompositeNeuronId;
            if (other != null)
                return GetHashCode() == other.GetHashCode();

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            // Based on the way indexes are stored in core.
            return (int) ((RegionIndex << RegionIndexOffset) | NeuronIndex);
        }
    }

    public class NeuronModel : ModelBase, IPickable
    {
        public static int NeuronTexture;

        /// <summary>
        /// The position relative to region in the 0;1 interval.
        /// </summary>
        public Vector3 ProportionalPosition { get; set; }

        public const float MinAlpha = 0.4f;
        public const float SpikeAlpha = 1f;
        public const float AlphaReductionPerMs = 1/1000f;

        public const float SpriteSize = 1f;

        private float m_alpha = MinAlpha;

        public uint Index { get; set; }

        // TODO(HonzaS): Cache this.
        public CompositeNeuronId NeuronId => new CompositeNeuronId(RegionModel.Index, Index);

        public string Type { get; set; }

        public ICamera Camera { get; set; }

        public RegionModel RegionModel { get; }

        // The key is the remote neuron's index.
        public IDictionary<uint, SynapseModel> Outputs { get; } = new Dictionary<uint, SynapseModel>();
        public IDictionary<uint, SynapseModel> Inputs { get; } = new Dictionary<uint, SynapseModel>();

        public NeuronModel(uint index, string type, RegionModel regionModel, Vector3 position)
        {
            Index = index;
            Type = type;
            RegionModel = regionModel;
            ProportionalPosition = position;

            UpdatePosition();

            Translucent = true;
        }

        // Neurons are rendered as billboards - they turn towards the camera.
        // Their world space rotation is equal to the camera's inverse rotation.
        protected override Matrix4 RotationMatrix
            => Camera.CurrentFrameViewMatrix.ClearScale().ClearTranslation().Inverted();

        public bool Picked { get; set; }

        protected override void UpdateModel(float elapsedMs)
        {
            if (m_alpha > MinAlpha)
                m_alpha -= AlphaReductionPerMs*elapsedMs;

            if (m_alpha < MinAlpha)
                m_alpha = MinAlpha;

            if (Picked)
            {
                m_alpha = 1;
                Scale = new Vector3(2, 2, 2);
            }
            else
            {
                Scale = Vector3.One;
            }
        }

        private void Spike()
        {
            m_alpha = SpikeAlpha;

            foreach (SynapseModel synapse in Outputs.Values)
                synapse.Spike();
        }

        protected override void RenderModel(float elapsedMs)
        {
            GL.BindTexture(TextureTarget.Texture2D, NeuronTexture);

            const float halfSize = SpriteSize/2;

            Color4 color = new Color4(255, 255, 255, (byte) (255 * m_alpha));

            GL.Enable(EnableCap.Texture2D);

            using (Blender.AveragingBlender())
            {
                GL.Color4(color);

                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(0, 0);
                GL.Vertex2(-halfSize, -halfSize);
                GL.TexCoord2(1, 0);
                GL.Vertex2(halfSize, -halfSize);
                GL.TexCoord2(1, 1);
                GL.Vertex2(halfSize, halfSize);
                GL.TexCoord2(0, 1);
                GL.Vertex2(-halfSize, halfSize);
                GL.End();
            }

            GL.Disable(EnableCap.Texture2D);

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public float DistanceToRayOrigin(PickRay pickRay)
        {
            var r = SpriteSize/2;

            Vector3 m = pickRay.Position - CurrentWorldMatrix.ExtractTranslation();
            float b = Vector3.Dot(m, pickRay.Direction);
            float r2 = r*r;
            float c = Vector3.Dot(m, m) - r2;

            // Ray starting outside of the sphere and pointing away.
            if (c > 0 && b > 0)
                return float.MaxValue;

            float discriminant = b * b - c;

            // Ray missing the sphere.
            if (discriminant < 0)
                return float.MaxValue;

            float t = -b - (float)Math.Sqrt(discriminant);

            // Ray starting inside sphere.
            if (t < 0)
                t = 0;

            return t;
        }

        public void UpdatePosition()
        {
            Position = ProportionalPosition*RegionModel.InnerSize - RegionModel.InnerHalfSize;
        }
    }
}
