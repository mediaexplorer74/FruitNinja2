
// Type: Mortar.MortarCamera
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;

#nullable disable
namespace Mortar
{
  public class MortarCamera
  {
    protected Matrix m_view_mtx;
    protected Matrix m_proj_mtx;
    protected Matrix m_ortho_view_mtx;
    protected Matrix m_ortho_mtx;
    protected Vector3 m_eye;
    protected Vector3 m_look_at;
    protected Vector3 m_up;
    protected bool m_changed;
    protected bool m_recalc_ortho;
    protected MortarRectangle m_oldwnd;
    protected float m_fov_x;
    protected float m_fov_y;
    protected float m_nearClip;
    protected float m_farClip;

    public MortarCamera()
    {
      this.m_changed = true;
      this.m_look_at = Vector3.Zero;
      this.m_eye = new Vector3(0.0f, 0.0f, 1f);
      this.m_up = new Vector3(0.0f, 1f, 0.0f);
      this.m_fov_x = 0.0f;
      this.m_fov_y = 0.0f;
      this.m_nearClip = 1f;
      this.m_farClip = 1000f;
      this.m_view_mtx = Matrix.Identity;
      this.m_proj_mtx = Matrix.Identity;
      this.m_ortho_view_mtx = Matrix.Identity;
      this.m_ortho_mtx = Matrix.Identity;
      this.m_recalc_ortho = true;
      this.m_oldwnd = new MortarRectangle();
    }

    public virtual void Init(float snear, float sfar, float xFOV, float yFOV)
    {
      this.m_nearClip = snear;
      this.m_farClip = sfar;
      this.m_fov_x = xFOV;
      this.m_fov_y = yFOV;
      this.m_recalc_ortho = true;
    }

    public virtual void UpdateCamera(float dt)
    {
    }

    public virtual void SetupPerspective()
    {
      if (this.m_changed)
      {
        MatrixManager.GetInstance().SetupLookAt(this.m_eye, this.m_up, this.m_look_at);
        this.m_view_mtx = MatrixManager.GetInstance().GetMatrix(MatrixManager.MatrixStackTypes.MATRIXSTACK_VIEW);
        MatrixManager.GetInstance().SetupPerspective(this.m_fov_y, this.m_fov_x / this.m_fov_y, this.m_nearClip, this.m_farClip);
        this.m_proj_mtx = MatrixManager.GetInstance().GetMatrix(MatrixManager.MatrixStackTypes.MATRIXSTACK_PROJECTION);
      }
      else
      {
        MatrixManager.GetInstance().SetMatrix(this.m_view_mtx, MatrixManager.MatrixStackTypes.MATRIXSTACK_VIEW);
        MatrixManager.GetInstance().SetMatrix(this.m_proj_mtx, MatrixManager.MatrixStackTypes.MATRIXSTACK_PROJECTION);
      }
      MatrixManager.GetInstance().Reset(MatrixManager.MatrixStackTypes.MATRIXSTACK_WORLD);
    }

    public virtual void SetupOrtho()
    {
      MortarRectangle windowSize = DisplayManager.GetInstance().GetWindowSize();
      if (this.m_recalc_ortho || windowSize.Height() != this.m_oldwnd.Height() && windowSize.Width() != this.m_oldwnd.Width())
      {
        MatrixManager.GetInstance().SetupLookAt(new Vector3(0.0f, 0.0f, 1f), new Vector3(0.0f, 1f, 0.0f), Vector3.Zero);
        this.m_ortho_view_mtx = MatrixManager.GetInstance().GetMatrix(MatrixManager.MatrixStackTypes.MATRIXSTACK_VIEW);
        MatrixManager.GetInstance().SetupOrtho((float) (windowSize.bottom >> 1), (float) -(windowSize.bottom >> 1), (float) -(windowSize.right >> 1), (float) (windowSize.right >> 1), -1f, 1000f);
        this.m_ortho_mtx = MatrixManager.GetInstance().GetMatrix(MatrixManager.MatrixStackTypes.MATRIXSTACK_PROJECTION);
      }
      else
      {
        MatrixManager.GetInstance().SetMatrix(this.m_ortho_view_mtx, MatrixManager.MatrixStackTypes.MATRIXSTACK_VIEW);
        MatrixManager.GetInstance().SetMatrix(this.m_ortho_mtx, MatrixManager.MatrixStackTypes.MATRIXSTACK_PROJECTION);
      }
      this.m_oldwnd = windowSize;
      MatrixManager.GetInstance().Reset(MatrixManager.MatrixStackTypes.MATRIXSTACK_WORLD);
    }

    public virtual float GetAspectRatio() => this.m_fov_x / this.m_fov_y;

    public virtual float GetFOVx() => this.m_fov_x;

    public virtual float GetFOVy() => this.m_fov_y;

    public virtual void SetLookAt(Vector3 pos)
    {
      this.m_look_at = pos;
      this.m_changed = true;
    }

    public virtual Vector3 GetLookAt() => this.m_look_at;

    public virtual void SetPos(Vector3 pos)
    {
      this.m_eye = pos;
      this.m_changed = true;
    }

    public virtual Vector3 GetPos() => this.m_eye;

    public virtual void SetUp(Vector3 pos)
    {
      this.m_up = pos;
      this.m_changed = true;
    }

    public virtual Vector3 GetUp() => this.m_up;
  }
}
