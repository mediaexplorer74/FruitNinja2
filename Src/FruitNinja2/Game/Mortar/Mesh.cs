
// Type: Mortar.Mesh
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#nullable disable
namespace Mortar
{
  public class Mesh
  {
    private static Matrix view;
    private static Matrix proj;
    public static bool newframe;
    private static BasicEffect basicEffect = (BasicEffect) null;
    private static VertexPositionColorTexture[] qverts = new VertexPositionColorTexture[4];
    public static VertexPositionColorTexture[] verts = new VertexPositionColorTexture[10000];
    public static int vertsCurrentOffset = 0;

    public static void DrawQuad(Color col) => Mesh.DrawQuad(col, 0.0f, 1f, 0.0f, 1f);

    public static void DrawQuad(Color col, float u0, float u1, float v0, float v1)
    {
      if (Mesh.basicEffect == null)
      {
        Mesh.basicEffect = new BasicEffect(Game1.instance.GraphicsDevice);
        Mesh.basicEffect.VertexColorEnabled = true;
      }
      DisplayManager.GetInstance().SetRasterizeStateCullOff();
      if (Mesh.newframe)
      {
        Mesh.view = DisplayManager.GetInstance().currentViewMtx;
        Mesh.proj = DisplayManager.GetInstance().currentProjMtx;
      }
      bool flag = false;
      Mesh.basicEffect.Projection = Mesh.proj;
      Mesh.basicEffect.World = DisplayManager.GetInstance().currentWorldMtx;
      Mesh.basicEffect.View = Mesh.view;
      if (DisplayManager.GetInstance().currentTexture == null)
      {
        Mesh.basicEffect.TextureEnabled = false;
      }
      else
      {
        Mesh.basicEffect.TextureEnabled = true;
        Mesh.basicEffect.Texture = DisplayManager.GetInstance().currentTexture.intex;
        flag = DisplayManager.GetInstance().currentTexture.hasAlpha;
      }
      ((Effect) Mesh.basicEffect).CurrentTechnique.Passes[0].Apply();
      if (col.A != byte.MaxValue || flag)
        DisplayManager.GetInstance().SetBlendStateDefault();
      else
        DisplayManager.GetInstance().SetBlendStateOff();
      Mesh.qverts[0].Color = col;
      Mesh.qverts[1].Color = col;
      Mesh.qverts[2].Color = col;
      Mesh.qverts[3].Color = col;
      Mesh.qverts[0].Position = new Vector3(-0.5f, 0.5f, 0.0f);
      Mesh.qverts[0].TextureCoordinate = new Vector2(u0, v0);
      Mesh.qverts[1].Position = new Vector3(-0.5f, -0.5f, 0.0f);
      Mesh.qverts[1].TextureCoordinate = new Vector2(u0, v1);
      Mesh.qverts[2].Position = new Vector3(0.5f, 0.5f, 0.0f);
      Mesh.qverts[2].TextureCoordinate = new Vector2(u1, v0);
      Mesh.qverts[3].Position = new Vector3(0.5f, -0.5f, 0.0f);
      Mesh.qverts[3].TextureCoordinate = new Vector2(u1, v1);
      Game1.instance.GraphicsDevice.DrawUserPrimitives<VertexPositionColorTexture>((PrimitiveType) 1, Mesh.qverts, 0, 2);
    }

    public static void DrawTriList(GameVertex[] data, int numPoints)
    {
      Mesh.DrawTriList(data, numPoints, false);
    }

    public static void DrawTriList(GameVertex[] data, int numPoints, bool hasAlpha)
    {
      Mesh.DrawTriList(data, numPoints, hasAlpha, 0);
    }

    public static void DrawTriList(GameVertex[] data, int numPoints, bool hasAlpha, int offset)
    {
      Mesh.DrawPrimitives((PrimitiveType) 0, data, numPoints, hasAlpha, numPoints / 3, offset);
    }

    public static void DrawTriStrip(GameVertex[] data, int numPoints)
    {
      Mesh.DrawTriStrip(data, numPoints, false);
    }

    public static void DrawTriStrip(GameVertex[] data, int numPoints, bool hasAlpha)
    {
      Mesh.DrawPrimitives((PrimitiveType) 1, data, numPoints, hasAlpha, numPoints - 2, 0);
    }

    private static void DrawPrimitives(
      PrimitiveType ptype,
      GameVertex[] data,
      int numPoints,
      bool hashAlpah,
      int numPrims,
      int offset)
    {
      if (numPrims <= 0)
        return;
      DisplayManager.GetInstance().SetRasterizeStateCullOff();
      Mesh.basicEffect.Projection = DisplayManager.GetInstance().currentProjMtx;
      Mesh.basicEffect.World = DisplayManager.GetInstance().currentWorldMtx;
      Mesh.basicEffect.View = DisplayManager.GetInstance().currentViewMtx;
      if (DisplayManager.GetInstance().currentTexture == null)
      {
        Mesh.basicEffect.TextureEnabled = false;
      }
      else
      {
        Mesh.basicEffect.TextureEnabled = true;
        Mesh.basicEffect.Texture = DisplayManager.GetInstance().currentTexture.intex;
        if (DisplayManager.GetInstance().currentTexture.hasAlpha)
          hashAlpah = true;
      }
      if (hashAlpah)
        DisplayManager.GetInstance().SetBlendStateDefault();
      else
        DisplayManager.GetInstance().SetBlendStateOff();
      ((Effect) Mesh.basicEffect).CurrentTechnique.Passes[0].Apply();
      int vertsCurrentOffset = Mesh.vertsCurrentOffset;
      for (int index = 0; index < numPoints; ++index)
      {
        Mesh.verts[Mesh.vertsCurrentOffset].Position = new Vector3(data[index + offset].X, data[index + offset].Y, data[index + offset].Z);
        Mesh.verts[Mesh.vertsCurrentOffset].Color = data[index + offset].color;
        Mesh.verts[Mesh.vertsCurrentOffset].TextureCoordinate = new Vector2(data[index + offset].u, data[index + offset].v);
        ++Mesh.vertsCurrentOffset;
      }
      Game1.instance.GraphicsDevice.DrawUserPrimitives<VertexPositionColorTexture>(ptype, Mesh.verts, vertsCurrentOffset, numPrims);
    }

    public static void DrawPrimitives2(
      PrimitiveType ptype,
      GameVertex[] data,
      int numPoints,
      bool hashAlpah,
      int numPrims,
      int offset)
    {
      if (numPrims <= 0)
        return;
      DisplayManager.GetInstance().SetRasterizeStateCullOff();
      Mesh.basicEffect.Projection = DisplayManager.GetInstance().currentProjMtx;
      Mesh.basicEffect.World = DisplayManager.GetInstance().currentWorldMtx;
      Mesh.basicEffect.View = DisplayManager.GetInstance().currentViewMtx;
      if (DisplayManager.GetInstance().currentTexture == null)
      {
        Mesh.basicEffect.TextureEnabled = false;
      }
      else
      {
        Mesh.basicEffect.TextureEnabled = true;
        Mesh.basicEffect.Texture = DisplayManager.GetInstance().currentTexture.intex;
        if (DisplayManager.GetInstance().currentTexture.hasAlpha)
          hashAlpah = true;
      }
      if (hashAlpah)
        DisplayManager.GetInstance().SetBlendStateDefault2();
      else
        DisplayManager.GetInstance().SetBlendStateOff();
      ((Effect) Mesh.basicEffect).CurrentTechnique.Passes[0].Apply();
      int vertsCurrentOffset = Mesh.vertsCurrentOffset;
      for (int index = 0; index < numPoints; ++index)
      {
        Mesh.verts[Mesh.vertsCurrentOffset].Position = new Vector3(data[index + offset].X, data[index + offset].Y, data[index + offset].Z);
        Mesh.verts[Mesh.vertsCurrentOffset].Color = data[index + offset].color;
        Mesh.verts[Mesh.vertsCurrentOffset].TextureCoordinate = new Vector2(data[index + offset].u, data[index + offset].v);
        ++Mesh.vertsCurrentOffset;
      }
      Game1.instance.GraphicsDevice.DrawUserPrimitives<VertexPositionColorTexture>(ptype, Mesh.verts, vertsCurrentOffset, numPrims);
    }
  }
}
