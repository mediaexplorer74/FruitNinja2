
// Type: Mortar.Model
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

#nullable disable
namespace Mortar
{
  public class Model
  {
    public VertexPositionColorTexture[] vertecies;
    public Texture tex;
    public Matrix amatrix = Matrix.Identity;
    private static BasicEffect basicEffect;

    public void Draw(Matrix? mtx)
    {
      if (Model.basicEffect == null)
      {
        Model.basicEffect = new BasicEffect(TheGame.instance.GraphicsDevice);
        Model.basicEffect.VertexColorEnabled = true;
      }
      DisplayManager.GetInstance().SetRasterizeStateCullCwise();
      if (this.vertecies == null)
        throw new Exception();
      bool flag = false;
      Model.basicEffect.Projection = DisplayManager.GetInstance().currentProjMtx;
      Matrix identity = Matrix.Identity;
      if (mtx.HasValue)
        identity = mtx.Value;
      Model.basicEffect.World = Matrix.Multiply(this.amatrix, identity);
      Model.basicEffect.View = DisplayManager.GetInstance().currentViewMtx;
      if (this.tex == null)
      {
        Model.basicEffect.TextureEnabled = false;
      }
      else
      {
        Model.basicEffect.TextureEnabled = true;
        Model.basicEffect.Texture = this.tex.intex;
        flag = this.tex.hasAlpha;
      }
      ((Effect) Model.basicEffect).CurrentTechnique.Passes[0].Apply();
      if (flag)
        DisplayManager.GetInstance().SetBlendStateDefault();
      else
        DisplayManager.GetInstance().SetBlendStateOff();
      TheGame.instance.GraphicsDevice.DrawUserPrimitives<VertexPositionColorTexture>((PrimitiveType) 1, this.vertecies, 0, this.vertecies.Length - 2);
    }
  }
}
