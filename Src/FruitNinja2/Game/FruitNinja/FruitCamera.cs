
// Type: GameManager.FruitCamera
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Mortar;

#nullable disable
namespace GameManager
{
  public class FruitCamera : MortarCamera
  {
    private const float CAMERA_SHAKE_RADIUS = 9f;
    private const float CAMERA_SHAKE_SPEED = 0.2f;
    protected Entity m_target;
    protected FruitCamera.CameraState m_state;
    protected ushort m_rot_z;
    protected ushort m_rot_x;
    protected Vector2 m_cameraShakeDest;
    protected ushort m_cameraShakeDirection;
    public Vector2 m_cameraShake;
    public Vector3 Eye;
    public Vector3 LookAt;
    public float m_cameraShakeTime;
    public float m_cameraShakeMaxTime;

    protected void UpdateIdle(float dt)
    {
    }

    protected void UpdateFollow(float dt)
    {
      if (this.m_target != null)
      {
        Vector3 vector3 = Vector3.Subtract(this.m_look_at, this.m_target.m_pos);
        this.m_look_at = this.m_target.m_pos;
        FruitCamera fruitCamera = this;
        fruitCamera.m_eye = Vector3.Subtract(fruitCamera.m_eye, vector3);
      }
      else
        this.IdleCamera();
    }

    public FruitCamera() => this.m_state = FruitCamera.CameraState.CAMERA_STATE_IDLE;

    public override void UpdateCamera(float dt)
    {
      this.Eye.X = (float) this.m_rot_x;
      this.Eye.Y = (float) this.m_rot_z;
      ref Vector3 local = ref this.Eye;
      Vector3 vector3 = Vector3.Subtract(this.m_eye, this.m_look_at);
      double num = (double) vector3.Length();
      local.Z = (float) num;
      this.LookAt = this.m_look_at;
      switch (this.m_state)
      {
        case FruitCamera.CameraState.CAMERA_STATE_IDLE:
          this.UpdateIdle(dt);
          break;
        case FruitCamera.CameraState.CAMERA_STATE_FOLLOW:
          this.UpdateFollow(dt);
          break;
      }
    }

    public void UpdateShake(float dt)
    {
      if ((double) this.m_cameraShakeTime > 0.0)
      {
        this.m_cameraShakeTime -= dt;
        Vector2 vector2 = Vector2.Subtract(this.m_cameraShakeDest, this.m_cameraShake);
        if ((double) vector2.LengthSquared() < 16.0)
        {
          this.m_cameraShakeDirection += (ushort) ((uint) Math.DEGREE_TO_IDX(140f) + (uint) Math.g_random.Rand32((int) Math.DEGREE_TO_IDX(80f)));
          float num = (float) (9.0 * ((double) this.m_cameraShakeTime / (double) this.m_cameraShakeMaxTime));
          this.m_cameraShakeDest.X = Math.CosIdx(this.m_cameraShakeDirection) * num;
          this.m_cameraShakeDest.Y = Math.SinIdx(this.m_cameraShakeDirection) * num;
        }
        FruitCamera fruitCamera = this;
        fruitCamera.m_cameraShake = Vector2.Add(
            fruitCamera.m_cameraShake, Vector2.Multiply(Vector2.Multiply(Vector2.Subtract(this.m_cameraShakeDest, this.m_cameraShake), 0.2f), (float) (1.0 + (double) this.m_cameraShakeTime / (double) this.m_cameraShakeMaxTime)));
        this.m_changed = true;
      }
      else
      {
        if ((double) Math.ABS(this.m_cameraShake.X) > 0.0099999997764825821)
          this.m_cameraShake.X *= 0.8f;
        else
          this.m_cameraShake.X = 0.0f;
        if ((double) Math.ABS(this.m_cameraShake.Y) > 0.0099999997764825821)
          this.m_cameraShake.Y *= 0.8f;
        else
          this.m_cameraShake.Y = 0.0f;
      }
    }

    public void CreateCameraShake(Vector3 origin, float length)
    {
      this.CreateCameraShake(origin, length, 1f);
    }

    public void CreateCameraShake(Vector3 origin, float length, float strength)
    {
      this.m_cameraShakeDirection = Math.Atan2Idx(origin.Y, origin.X);
      this.m_cameraShakeDest.X = Math.CosIdx(this.m_cameraShakeDirection) * 9f;
      this.m_cameraShakeDest.Y = Math.SinIdx(this.m_cameraShakeDirection) * 9f;
      FruitCamera fruitCamera = this;
      fruitCamera.m_cameraShakeDest = Vector2.Multiply(fruitCamera.m_cameraShakeDest, strength);
      this.m_cameraShakeMaxTime = this.m_cameraShakeTime = length;
    }

    public Entity GetFollowEntity()
    {
      Entity followEntity = (Entity) null;
      if (this.m_state == FruitCamera.CameraState.CAMERA_STATE_FOLLOW)
        followEntity = this.m_target;
      return followEntity;
    }

    public void FollowEntity(Entity ent)
    {
      if (ent != null)
      {
        this.m_state = FruitCamera.CameraState.CAMERA_STATE_FOLLOW;
        this.m_target = ent;
      }
      this.m_rot_x = (ushort) 0;
      this.m_rot_z = (ushort) 0;
      this.m_up = new Vector3(0.0f, 1f, 0.0f);
    }

    public void IdleCamera()
    {
      this.m_state = FruitCamera.CameraState.CAMERA_STATE_IDLE;
      this.m_target = (Entity) null;
    }

    public void SetupPerspective(FruitCamera.PERSPECIVE_TYPE orientation, bool changed)
    {
      if (this.m_changed || changed)
      {
        switch (orientation)
        {
          case FruitCamera.PERSPECIVE_TYPE.ORIENTATION_NORMAL:
            MatrixManager.instance.SetupLookAt(Vector3.Add(new Vector3(0.0f, 0.0f, 1f), new Vector3(this.m_cameraShake.X, this.m_cameraShake.Y, 0.0f)), new Vector3(0.0f, 1f, 0.0f), new Vector3(this.m_cameraShake.X, this.m_cameraShake.Y, 0.0f));
            this.m_view_mtx = MatrixManager.instance.GetMatrix(MatrixManager.MatrixStackTypes.MATRIXSTACK_VIEW);
            MatrixManager.instance.SetupOrtho((float) (DisplayManager.GetInstance().Res.Y >> 1), (float) -(DisplayManager.GetInstance().Res.Y >> 1), (float) -(DisplayManager.GetInstance().Res.X >> 1), (float) (DisplayManager.GetInstance().Res.X >> 1), -6000f, 6000f);
            this.m_proj_mtx = MatrixManager.instance.GetMatrix(MatrixManager.MatrixStackTypes.MATRIXSTACK_PROJECTION);
            break;
          case FruitCamera.PERSPECIVE_TYPE.ORIENTATION_LEFT:
            MatrixManager.instance.SetupLookAt(Vector3.Add(new Vector3(0.0f, 0.0f, 1f), new Vector3(this.m_cameraShake.X, this.m_cameraShake.Y, 0.0f)), new Vector3(1f, 0.0f, 0.0f), new Vector3(this.m_cameraShake.X, this.m_cameraShake.Y, 0.0f));
            this.m_view_mtx = MatrixManager.instance.GetMatrix(MatrixManager.MatrixStackTypes.MATRIXSTACK_VIEW);
            MatrixManager.instance.SetupOrtho((float) -(DisplayManager.GetInstance().Res.X >> 1), (float) (DisplayManager.GetInstance().Res.X >> 1), (float) (DisplayManager.GetInstance().Res.Y >> 1), (float) -(DisplayManager.GetInstance().Res.Y >> 1) - (float) DisplayManager.GetInstance().Res.Y, -6000f, 6000f);
            break;
          case FruitCamera.PERSPECIVE_TYPE.ORIENTATION_RIGHT:
            MatrixManager.instance.SetupLookAt(Vector3.Add(new Vector3(0.0f, 0.0f, 1f), new Vector3(this.m_cameraShake.X, this.m_cameraShake.Y, 0.0f)), new Vector3(1f, 0.0f, 0.0f), new Vector3(this.m_cameraShake.X, this.m_cameraShake.Y, 0.0f));
            this.m_view_mtx = MatrixManager.instance.GetMatrix(MatrixManager.MatrixStackTypes.MATRIXSTACK_VIEW);
            MatrixManager.instance.SetupOrtho((float) (DisplayManager.GetInstance().Res.X >> 1), (float) -(DisplayManager.GetInstance().Res.X >> 1), (float) -(DisplayManager.GetInstance().Res.Y >> 1) - (float) DisplayManager.GetInstance().Res.Y, (float) (DisplayManager.GetInstance().Res.Y >> 1), -6000f, 6000f);
            this.m_proj_mtx = MatrixManager.instance.GetMatrix(MatrixManager.MatrixStackTypes.MATRIXSTACK_PROJECTION);
            break;
          case FruitCamera.PERSPECIVE_TYPE.ORIENTATION_NORMAL_NO_SHAKE:
            MatrixManager.instance.SetupLookAt(new Vector3(0.0f, 0.0f, 1f), new Vector3(0.0f, 1f, 0.0f), Vector3.Zero);
            this.m_view_mtx = MatrixManager.instance.GetMatrix(MatrixManager.MatrixStackTypes.MATRIXSTACK_VIEW);
            MatrixManager.instance.SetupOrtho((float) ((int) Game.SCREEN_HEIGHT >> 1), (float) -((int) Game.SCREEN_HEIGHT >> 1), (float) -((int) Game.SCREEN_WIDTH >> 1), (float) ((int) Game.SCREEN_WIDTH >> 1), 2000f, -6000f);
            this.m_proj_mtx = MatrixManager.instance.GetMatrix(MatrixManager.MatrixStackTypes.MATRIXSTACK_PROJECTION);
            break;
        }
      }
      else
      {
        MatrixManager.instance.SetMatrix(this.m_view_mtx, MatrixManager.MatrixStackTypes.MATRIXSTACK_VIEW);
        MatrixManager.instance.SetMatrix(this.m_proj_mtx, MatrixManager.MatrixStackTypes.MATRIXSTACK_PROJECTION);
      }
      MatrixManager.instance.Reset(MatrixManager.MatrixStackTypes.MATRIXSTACK_WORLD);
    }

    public void SetupPerspective(FruitCamera.PERSPECIVE_TYPE orientation)
    {
      this.SetupPerspective(orientation, false);
    }

    public new void SetupPerspective()
    {
      this.SetupPerspective(FruitCamera.PERSPECIVE_TYPE.ORIENTATION_NORMAL, false);
    }

    public enum CameraState
    {
      CAMERA_STATE_IDLE,
      CAMERA_STATE_FOLLOW,
    }

    public enum PERSPECIVE_TYPE
    {
      ORIENTATION_NORMAL,
      ORIENTATION_LEFT,
      ORIENTATION_RIGHT,
      ORIENTATION_NORMAL_NO_SHAKE,
    }
  }
}
