
// Type: GameManager.BombBlast
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Mortar;
using System.Collections.Generic;

#nullable disable
namespace GameManager
{
  public class BombBlast : Entity
  {
    protected Vector3 m_xVec;
    protected Vector3 m_yVec;
    protected Vector3 m_origXVec;
    protected Vector3 m_origYVec;
    public float m_time;
    public static GameVertex[] m_points = ArrayInit.CreateFilledArray<GameVertex>(3072);
    public static int m_curr_drawing_blast;

    public override void Init(byte[] tpl_data, int tpl_size, Vector3? size)
    {
      this.m_time = 0.0f;
      this.m_dir_angle = Math.DEGREE_TO_IDX(Math.g_random.RandF(360f));
      this.m_origXVec = Vector3.Multiply(new Vector3(Math.CosIdx(this.m_dir_angle), Math.SinIdx(this.m_dir_angle), 0.0f), 0.5f);
      this.m_origYVec = new Vector3(Math.CosIdx((ushort) ((uint) this.m_dir_angle + (uint) Math.DEGREE_TO_IDX(90f))), Math.SinIdx((ushort) ((uint) this.m_dir_angle + (uint) Math.DEGREE_TO_IDX(90f))), 0.0f);
      this.m_xVec = this.m_origXVec;
      this.m_yVec = this.m_origYVec;
      this.m_cur_scale = new Vector3(5f, 50f, 1f);
      this.m_destroy = false;
      this.m_dormant = false;
    }

    public override void Update(float dt)
    {
      this.m_time += Game.game_work.dt;
      this.m_xVec = Vector3.Multiply(this.m_origXVec, this.m_cur_scale.X += 100f * Game.game_work.dt);
      this.m_yVec = Vector3.Multiply(this.m_origYVec, this.m_cur_scale.Y += 2500f * Game.game_work.dt);
      if ((double) this.m_time <= 3.0)
        return;
      this.m_destroy = true;
      this.m_destroy = true;
    }

    public override void Draw()
    {
    }

    public override void DrawUpdate(float dt)
    {
    }

    public virtual void DrawBlast()
    {
      int index1 = BombBlast.m_curr_drawing_blast * 6;
      float num1 = 0.0f;
      float num2 = 1f;
      float num3 = 0.0f;
      float num4 = 1f;
      BombBlast.m_points[index1].X = this.m_pos.X + (this.m_xVec.X + this.m_yVec.X);
      BombBlast.m_points[index1].Y = this.m_pos.Y + (this.m_xVec.Y + this.m_yVec.Y);
      BombBlast.m_points[index1].u = num2;
      BombBlast.m_points[index1].v = num3;
      BombBlast.m_points[index1 + 1].X = this.m_pos.X + (-this.m_xVec.X + this.m_yVec.X);
      BombBlast.m_points[index1 + 1].Y = this.m_pos.Y + (-this.m_xVec.Y + this.m_yVec.Y);
      BombBlast.m_points[index1 + 1].u = num1;
      BombBlast.m_points[index1 + 1].v = num3;
      BombBlast.m_points[index1 + 2].X = this.m_pos.X + this.m_xVec.X * 0.25f;
      BombBlast.m_points[index1 + 2].Y = this.m_pos.Y + this.m_xVec.Y * 0.25f;
      BombBlast.m_points[index1 + 2].u = num2;
      BombBlast.m_points[index1 + 2].v = num4;
      BombBlast.m_points[index1 + 3] = BombBlast.m_points[index1 + 2];
      BombBlast.m_points[index1 + 4] = BombBlast.m_points[index1 + 1];
      BombBlast.m_points[index1 + 5].X = this.m_pos.X + (float) (-(double) this.m_xVec.X * 0.25);
      BombBlast.m_points[index1 + 5].Y = this.m_pos.Y + (float) (-(double) this.m_xVec.Y * 0.25);
      BombBlast.m_points[index1 + 5].u = num1;
      BombBlast.m_points[index1 + 5].v = num4;
      Color white = Color.White;
      for (int index2 = 0; index2 < 6; ++index2)
      {
        BombBlast.m_points[index1 + index2].Z = 0.0f;
        BombBlast.m_points[index1 + index2].nx = 0.0f;
        BombBlast.m_points[index1 + index2].ny = 0.0f;
        BombBlast.m_points[index1 + index2].nz = 1f;
        BombBlast.m_points[index1 + index2].color = white;
      }
    }

    public static void DrawActiveBlasts()
    {
      if (Bomb.m_blastTexture == null)
        return;
      Bomb.m_blastTexture.Set();
      BombBlast.m_curr_drawing_blast = 0;
      LinkedListNode<Entity> iterator = (LinkedListNode<Entity>) null;
      for (BombBlast bombBlast = (BombBlast) ActorManager.GetInstance().GetEntityFirst(EntityTypes.ENTITY_BOMB_BLAST, ref iterator); bombBlast != null; bombBlast = (BombBlast) ActorManager.GetInstance().GetEntityNext(EntityTypes.ENTITY_BOMB_BLAST, ref iterator))
      {
        bombBlast.DrawBlast();
        ++BombBlast.m_curr_drawing_blast;
      }
      Bomb.m_blastTexture.Set();
      MatrixManager.instance.Reset();
      MatrixManager.instance.UploadCurrentMatrices();
      Mesh.DrawTriList(BombBlast.m_points, BombBlast.m_curr_drawing_blast * 6);
      Bomb.m_blastTexture.UnSet();
    }

    public static void CleanupBomb()
    {
      for (int index = 0; index < Game.MAX_PLAYERS * 2; ++index)
        Bomb.m_bombModel[index] = (Model) null;
      BombFlash.CleanUp();
      Bomb.s_flashTexture[0] = (Texture) null;
      Bomb.s_flashTexture[1] = (Texture) null;
      Bomb.m_blastTexture = (Texture) null;
      Bomb.s_minus_10 = (Texture) null;
    }
  }
}
