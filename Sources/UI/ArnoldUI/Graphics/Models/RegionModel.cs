﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoodAI.Arnold.Graphics;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GoodAI.Arnold.Graphics.Models
{
    public class RegionModel : CompositeModelBase<IModel>
    {
        private Vector3 m_size;
        public const float RegionMargin = 2f;

        public Vector3 Size
        {
            get { return m_size; }
            private set
            {
                m_size = value;
                HalfSize = Size/2;
            }
        }

        public CompositeModel<ExpertModel> Experts { get; } = new CompositeModel<ExpertModel>();
        public CompositeModel<SynapseModel> Synapses { get; } = new CompositeModel<SynapseModel>();

        public Vector3 HalfSize { get; private set; }

        public RegionModel(Vector3 position)
        {
            Position = position;

            Size = new Vector3
            {
                X = 10f,
                Y = 10f,
                Z = 10f
            };

            Translucent = true;

            AddChild(Experts);
            AddChild(Synapses);
        }

        public void AddExpert(ExpertModel expert) => Experts.AddChild(expert);

        public void AddSynapse(SynapseModel synapse) => Synapses.AddChild(synapse);

        public void AdjustSize()
        {
            float minX = 0;
            float maxX = 0;

            // The experts spread into both Y and Z.
            float minY = 0;
            float maxY = 0;

            float minZ = 0;
            float maxZ = 0;

            foreach (ExpertModel expert in Experts)
            {
                maxX = Math.Max(expert.Position.X, maxX);
                maxY = Math.Max(expert.Position.Y, maxY);
                maxZ = Math.Max(expert.Position.Z, maxZ);

                minX = Math.Min(expert.Position.Z, minX);
                minY = Math.Min(expert.Position.Y, minY);
                minZ = Math.Min(expert.Position.Z, minZ);
            }

            Size = new Vector3
            {
                X = maxX - minX + 2*RegionMargin,
                Y = maxY - minY + 2*RegionMargin,
                Z = maxZ - minZ + 2*RegionMargin
            };
        }

        protected override void UpdateModel(float elapsedMs)
        {
        }

        protected override void RenderModel(float elapsedMs)
        {
            using (Blender.MultiplicativeBlender())
            {
                GL.Color4(0, 0.2, 0.4, 0.6);

                GL.LineWidth(3f);

                GL.Begin(PrimitiveType.Lines);

                // Face one.

                GL.Vertex3(-HalfSize.X, -HalfSize.Y, -HalfSize.Z);
                GL.Vertex3(-HalfSize.X, HalfSize.Y, -HalfSize.Z);

                GL.Vertex3(-HalfSize.X, HalfSize.Y, -HalfSize.Z);
                GL.Vertex3(HalfSize.X, HalfSize.Y, -HalfSize.Z);

                GL.Vertex3(HalfSize.X, HalfSize.Y, -HalfSize.Z);
                GL.Vertex3(HalfSize.X, -HalfSize.Y, -HalfSize.Z);

                GL.Vertex3(HalfSize.X, -HalfSize.Y, -HalfSize.Z);
                GL.Vertex3(-HalfSize.X, -HalfSize.Y, -HalfSize.Z);

                // Face two.

                GL.Vertex3(-HalfSize.X, -HalfSize.Y, HalfSize.Z);
                GL.Vertex3(-HalfSize.X, HalfSize.Y, HalfSize.Z);

                GL.Vertex3(-HalfSize.X, HalfSize.Y, HalfSize.Z);
                GL.Vertex3(HalfSize.X, HalfSize.Y, HalfSize.Z);

                GL.Vertex3(HalfSize.X, HalfSize.Y, HalfSize.Z);
                GL.Vertex3(HalfSize.X, -HalfSize.Y, HalfSize.Z);

                GL.Vertex3(HalfSize.X, -HalfSize.Y, HalfSize.Z);
                GL.Vertex3(-HalfSize.X, -HalfSize.Y, HalfSize.Z);

                // Face connectors.

                GL.Vertex3(HalfSize.X, -HalfSize.Y, -HalfSize.Z);
                GL.Vertex3(HalfSize.X, -HalfSize.Y, HalfSize.Z);

                GL.Vertex3(-HalfSize.X, -HalfSize.Y, -HalfSize.Z);
                GL.Vertex3(-HalfSize.X, -HalfSize.Y, HalfSize.Z);

                GL.Vertex3(-HalfSize.X, HalfSize.Y, -HalfSize.Z);
                GL.Vertex3(-HalfSize.X, HalfSize.Y, HalfSize.Z);

                GL.Vertex3(HalfSize.X, HalfSize.Y, -HalfSize.Z);
                GL.Vertex3(HalfSize.X, HalfSize.Y, HalfSize.Z);

                GL.End();
            }
        }
    }
}