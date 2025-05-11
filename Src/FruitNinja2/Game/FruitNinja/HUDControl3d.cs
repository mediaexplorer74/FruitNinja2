
// Type: GameManager.HUDControl3d
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Mortar;

#nullable disable
namespace GameManager
{
  public class HUDControl3d : HUDControl
  {
    public Texture m_texture;

    public override void Save()
    {
    }

    public override void Release()
    {
    }

    public override void PreDraw(float[] tintChannels)
    {
    }

    public override void Draw(float[] tintChannels)
    {
      if (this.m_texture == null)
        return;
      this.m_texture.Set();
      MatrixManager.GetInstance().Reset();
      Matrix matrix = Matrix.CreateScale(this.m_scale);
      if ((double) this.m_rotation != 0.0)
        matrix = Matrix.Multiply(matrix, Matrix.CreateRotationZ(MathHelper.ToRadians(this.m_rotation)));
      Matrix mtx = Matrix.Multiply(matrix, Matrix.CreateTranslation(this.m_pos));
      MatrixManager.GetInstance().SetMatrix(mtx);
      MatrixManager.GetInstance().UploadCurrentMatrices();
      Mesh.DrawQuad(HUDControl.TintColor(this.m_color, tintChannels), this.m_uvs[0].X, this.m_uvs[1].X, this.m_uvs[0].Y, this.m_uvs[1].Y);
      this.m_texture.UnSet();
    }

    public override void Update(float dt) => base.Update(dt);

    public override HUD_TYPE GetType() => HUD_TYPE.HUD_TYPE_3D;

    public override void Skip()
    {
    }
  }
}
