
// Type: Mortar.MatrixManager
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;

#nullable disable
namespace Mortar
{
  public class MatrixManager
  {
    public static MatrixManager instance = new MatrixManager();
    protected MatrixStack[] m_stacks = ArrayInit.CreateFilledArray<MatrixStack>(4);
    protected uint[] m_stack_versions = new uint[4];

    private MatrixManager()
    {
      this.m_stacks = ArrayInit.CreateFilledArray<MatrixStack>(4);
      this.m_stack_versions = new uint[4];
      for (int index = 0; index < 4; ++index)
        this.m_stacks[index] = new MatrixStack();
    }

    private void _UploadCurrentMatrices(bool quick)
    {
      if (!quick)
      {
        DisplayManager.instance.currentViewMtx = this.m_stacks[1].GetCurrentMatrix();
        DisplayManager.instance.currentProjMtx = Matrix.Multiply(this.m_stacks[0].GetCurrentMatrix(), DisplayManager.instance.topMatrix);
      }
      DisplayManager.instance.currentWorldMtx = this.m_stacks[2].GetCurrentMatrix();
      DisplayManager.instance.currentTextureMtx = this.m_stacks[3].GetCurrentMatrix();
      DisplayManager.instance.DidUploadMatrixies();
    }

    public static MatrixManager GetInstance() => MatrixManager.instance;

    public void Init() => this.ResetAllStacks();

    public void ResetAllStacks()
    {
      for (int index = 0; index < 4; ++index)
        this.m_stacks[index].Reset();
    }

    public void UploadCurrentMatrices() => this.UploadCurrentMatrices(true);

    public void UploadCurrentMatrices(bool quick) => this._UploadCurrentMatrices(quick);

    public void Push() => this.m_stacks[2].Push();

    public void Pop() => this.m_stacks[2].Pop(1);

    public void Store(byte num) => this.m_stacks[2].Store((int) num);

    public void Restore(byte num) => this.m_stacks[2].Restore((int) num);

    public void Reset() => this.m_stacks[2].Reset();

    public Matrix GetMatrix() => this.m_stacks[2].GetCurrentMatrix();

    public void SetMatrix(Matrix mtx) => this.m_stacks[2].SetCurrentMatrix(mtx);

    public void Translate(Vector3 amount) => this.m_stacks[2].Translate(amount);

    public void TranslateGlobal(Vector3 amount)
    {
      this.Translate(amount, MatrixManager.MatrixStackTypes.MATRIXSTACK_WORLD);
    }

    public void TranslateLocal(Vector3 amount) => this.m_stacks[2].TranslateLocal(amount);

    public void Scale(Vector3 amount) => this.m_stacks[2].Scale(amount);

    public void RotX(float amount) => this.m_stacks[2].RotX(amount);

    public void RotY(float amount) => this.m_stacks[2].RotY(amount);

    public void RotZ(float amount) => this.m_stacks[2].RotZ(amount);

    public void Translate2D(Vector2 amount) => this.m_stacks[2].Translate2D(amount);

    public void Push(MatrixManager.MatrixStackTypes type) => this.m_stacks[(int) type].Push();

    public void Pop(int num, MatrixManager.MatrixStackTypes type)
    {
      this.m_stacks[(int) type].Pop(num);
    }

    public void Store(int num, MatrixManager.MatrixStackTypes type)
    {
      this.m_stacks[(int) type].Store(num);
    }

    public void Restore(int num, MatrixManager.MatrixStackTypes type)
    {
      this.m_stacks[(int) type].Restore(num);
    }

    public void Reset(MatrixManager.MatrixStackTypes type) => this.m_stacks[(int) type].Reset();

    public Matrix GetMatrix(MatrixManager.MatrixStackTypes type)
    {
      return this.m_stacks[(int) type].GetCurrentMatrix();
    }

    public void SetMatrix(Matrix mtx, MatrixManager.MatrixStackTypes type)
    {
      this.m_stacks[(int) type].SetCurrentMatrix(mtx);
    }

    public void Translate(Vector3 amount, MatrixManager.MatrixStackTypes type)
    {
      this.m_stacks[(int) type].Translate(amount);
    }

    public void TranslateGlobal(Vector3 amount, MatrixManager.MatrixStackTypes type)
    {
      this.Translate(amount, type);
    }

    public void TranslateLocal(Vector3 amount, MatrixManager.MatrixStackTypes type)
    {
      this.m_stacks[(int) type].TranslateLocal(amount);
    }

    public void Scale(Vector3 amount, MatrixManager.MatrixStackTypes type)
    {
      this.m_stacks[(int) type].Scale(amount);
    }

    public void RotX(float amount, MatrixManager.MatrixStackTypes type)
    {
      this.m_stacks[(int) type].RotX(amount);
    }

    public void RotY(float amount, MatrixManager.MatrixStackTypes type)
    {
      this.m_stacks[(int) type].RotY(amount);
    }

    public void RotZ(float amount, MatrixManager.MatrixStackTypes type)
    {
      this.m_stacks[(int) type].RotZ(amount);
    }

    public void Translate2D(Vector2 amount, MatrixManager.MatrixStackTypes type)
    {
      this.m_stacks[(int) type].Translate2D(amount);
    }

    public void SetupPerspective(float fovy, float aspect, float n, float f)
    {
      this.SetupPerspective(fovy, aspect, n, f, out Matrix _);
    }

    public void SetupPerspective(float fovy, float aspect, float n, float f, out Matrix mtx)
    {
      Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(fovy), aspect, n, f, out mtx);
      this.m_stacks[0].SetCurrentMatrix(mtx);
      this.UploadCurrentMatrices(false);
    }

    public void SetupOrtho(float t, float b, float l, float r, float n, float f)
    {
      this.SetupOrtho(t, b, l, r, n, f, out Matrix _);
      this.UploadCurrentMatrices(false);
    }

    public void SetupOrtho(
      float t,
      float b,
      float l,
      float r,
      float n,
      float f,
      out Matrix mtx)
    {
      Matrix.CreateOrthographicOffCenter(l, r, b, t, n, f, out mtx);
      this.m_stacks[0].SetCurrentMatrix(mtx);
    }

    public void SetupLookAt(Vector3 camPos, Vector3 camUp, Vector3 target)
    {
      this.SetupLookAt(camPos, camUp, target, out Matrix _);
    }

    public void SetupLookAt(Vector3 camPos, Vector3 camUp, Vector3 target, out Matrix mtx)
    {
      Matrix.CreateLookAt(ref camPos, ref target, ref camUp, out mtx);
      this.m_stacks[1].SetCurrentMatrix(mtx);
    }

    public void WorldTranslate(Vector3 amount)
    {
      this.Translate(amount, MatrixManager.MatrixStackTypes.MATRIXSTACK_WORLD);
    }

    public void WorldTranslateGlobal(Vector3 amount)
    {
      this.TranslateGlobal(amount, MatrixManager.MatrixStackTypes.MATRIXSTACK_WORLD);
    }

    public void WorldScale(Vector3 amount)
    {
      this.Scale(amount, MatrixManager.MatrixStackTypes.MATRIXSTACK_WORLD);
    }

    public void WorldRotX(float amount)
    {
      this.RotX(amount, MatrixManager.MatrixStackTypes.MATRIXSTACK_WORLD);
    }

    public void WorldRotY(float amount)
    {
      this.RotY(amount, MatrixManager.MatrixStackTypes.MATRIXSTACK_WORLD);
    }

    public void WorldRotZ(float amount)
    {
      this.RotZ(amount, MatrixManager.MatrixStackTypes.MATRIXSTACK_WORLD);
    }

    public enum MatrixStackTypes
    {
      MATRIXSTACK_PROJECTION,
      MATRIXSTACK_VIEW,
      MATRIXSTACK_WORLD,
      MATRIXSTACK_TEXTURE,
      MATRIXSTACK_MAX,
    }
  }
}
