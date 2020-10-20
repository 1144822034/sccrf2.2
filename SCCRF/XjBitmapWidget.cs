using Engine;
using Engine.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Game
{
    public class XjBitmapWidget:Widget
    {
        public static Vector2 textcora= new Vector2(1, 1);
        
        public Texture2D Texture_;
        public Texture2D Texture
        {
            get
            {
                return Texture_;
            }
            set
            {
                Texture_ = value;
            }
        }

        public XjBitmapWidget()
        {
            IsHitTestVisible = false;
            IsDrawRequired = true;
         }
        public override void Draw(DrawContext drawContext)
        {
            if (Texture != null)
            {
                
                TexturedBatch2D texturedBatch2D = drawContext.PrimitivesRenderer2D.TexturedBatch(Texture, useAlphaTest: false, 0, DepthStencilState.None, null, BlendState.NonPremultiplied, SamplerState.PointWrap);
                int count = texturedBatch2D.TriangleVertices.Count;
                texturedBatch2D.QueueQuad(Vector2.Zero, base.ActualSize, 1f, Vector2.Zero, textcora, base.GlobalColorTransform);
                texturedBatch2D.TransformTriangles(base.GlobalTransform, count);
            }
        }
        public override void MeasureOverride(Vector2 parentAvailableSize)
        {
            base.MeasureOverride(parentAvailableSize);
        }
        public override void Update()
        {
            
        }
        public override void ArrangeOverride()
        {
            base.ArrangeOverride();

        }

    }
}
