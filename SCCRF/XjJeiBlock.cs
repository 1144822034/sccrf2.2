using Engine;
using Engine.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
   public class XjJeiBlock : Block
    {

        public const int Index = 1010;
        private Texture2D texture2D;
        public override void Initialize()
        {
            texture2D = ContentManager.Get<Texture2D>("JEITextures/JEI");
        }
        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {
            BlocksManager.DrawFlatBlock(primitivesRenderer,value,size,ref matrix, texture2D, color,false, environmentData);
        }

        public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
        {
        }
    }
}
