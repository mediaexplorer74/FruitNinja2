
// Type: FruitNinja2.BombFlash
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Mortar;
using System;

#nullable disable
namespace FruitNinja2
{
  public class BombFlash
  {
    protected float m_time;
    protected Color m_startColor;
    protected Color m_colour;
    protected float m_sinA;
    protected float m_cosA;
    protected Texture m_texture;
    public Vector3 m_pos;
    public Vector3 m_vel;
    public Vector3 m_cur_scale;
    public bool m_update;
    public ushort m_dir_angle;
    public static BombFlash[] pool;
    public static int poolCount;
    public static int currentFree;

    public static float TOTAL_FLASH_TIME => 0.6f;

    public static float FLASH_FADE_IN_TIME => 0.02f;

    public static float FLASH_FADE_OUT_TIME => 0.4f;

    public static float FLASH_START_SCALE_X => 150f;

    public static float FLASH_FULL_SCALE_X => 200f;

    public static float FLASH_START_SCALE_Y => 100f;

    public static float FLASH_FULL_SCALE_Y => 300f;

    public void Destroy() => this.m_update = false;

    public virtual void Init(byte[] tpl_data, out Vector3 size) => size = new Vector3();

    public virtual void Update(float dt)
    {
      this.m_time += dt;
      float num1 = this.m_time / BombFlash.TOTAL_FLASH_TIME;
      this.m_cur_scale = new Vector3(BombFlash.FLASH_START_SCALE_X 
          + (BombFlash.FLASH_FULL_SCALE_X - BombFlash.FLASH_START_SCALE_X) * num1 * num1, 
          BombFlash.FLASH_START_SCALE_Y + (BombFlash.FLASH_FULL_SCALE_Y 
          - BombFlash.FLASH_START_SCALE_Y) * num1 * num1, 0.0f);

      if ((double) this.m_time < (double) BombFlash.FLASH_FADE_IN_TIME)
        this.m_colour.A = (byte) Mortar.Math.CLAMP(this.m_startColor.A
            * (this.m_time / BombFlash.FLASH_FADE_IN_TIME), 0.0f,
            (float) this.m_startColor.A );
      else if ((double) this.m_time > (double) BombFlash.TOTAL_FLASH_TIME
                - (double) BombFlash.FLASH_FADE_OUT_TIME)
      {
        float num2 = (BombFlash.TOTAL_FLASH_TIME - this.m_time) / BombFlash.FLASH_FADE_OUT_TIME;
        this.m_colour.A = (byte) Mortar.Math.CLAMP(
            this.m_startColor.A * (num2 * num2),
            0.0f, (float) this.m_startColor.A  );
      }
      else
        this.m_colour.A = this.m_startColor.A;
      if ((double) this.m_time <= (double) BombFlash.TOTAL_FLASH_TIME)
        return;
      this.m_update = false;
      this.m_time = 0.0f;
    }

    public virtual void Draw()
    {
      if (this.m_texture == null)
        return;
      MatrixManager.instance.SetMatrix(Matrix.Identity);
      MatrixManager.instance.Scale(this.m_cur_scale);
      MatrixManager.instance.RotZ(Mortar.Math.IDX_TO_RADIANS(this.m_dir_angle));
      MatrixManager.instance.TranslateGlobal(this.m_pos);
      MatrixManager.instance.UploadCurrentMatrices();
      this.m_texture.Set();
      Mesh.DrawQuad(this.m_colour);
      this.m_texture.UnSet();
    }

    public virtual void DrawUpdate(float dt)
    {
    }

    public void MakeFlash(Color colour, Vector3 pos, Vector3 vel, Texture texture)
    {
      this.m_texture = texture;
      this.m_pos = pos;
      this.m_pos.Z = -5400f;
      this.m_vel = vel;
      this.m_colour = colour;
      this.m_startColor = colour;
      BombFlash bombFlash = this;
      bombFlash.m_pos = Vector3.Add(bombFlash.m_pos, Vector3.Multiply(vel, 7.5f));
      this.m_pos.X = (float) ((double) Game.SCREEN_WIDTH * (double) Mortar.Math.MATH_SIGN(
          this.m_pos.X) / 2.0);

      this.m_cur_scale = new Vector3(128f, 128f, 128f);
      this.m_dir_angle = Mortar.Math.Atan2Idx(this.m_vel.X, -this.m_vel.Y);
      this.m_time = 0.0f;
      this.m_update = true;
      this.m_sinA = Mortar.Math.SinIdx(this.m_dir_angle);
      this.m_cosA = Mortar.Math.CosIdx(this.m_dir_angle);
      this.Update(0.0f);
    }

    public static void CreatePool() => BombFlash.CreatePool(512);

    public static void CreatePool(int num)
    {
    }

    public static BombFlash GetFree()
    {
      if (BombFlash.pool == null)
        return (BombFlash) null;
      for (int index = 0; BombFlash.pool[BombFlash.currentFree].m_update && index < BombFlash.poolCount; ++index)
        BombFlash.currentFree = (BombFlash.currentFree + 1) % BombFlash.poolCount;
      return BombFlash.pool[BombFlash.currentFree];
    }

    public static void CleanUp()
    {
      Delete.SAFE_DELETE_ARRAY<BombFlash[]>(ref BombFlash.pool);
      BombFlash.poolCount = 0;
    }

    public static void UpdateActiveFlashes(float dt)
    {
    }

    public static void DrawActiveFlashes()
    {
    }

    public static void RemoveAllFlashes()
    {
      for (int index = 0; index < BombFlash.poolCount; ++index)
        BombFlash.pool[index].Destroy();
    }

    public static int NumActiveFlashes() => throw new MissingMethodException();
  }
}
