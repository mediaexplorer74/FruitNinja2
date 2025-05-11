
// Type: Mortar.DisplayManager
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#nullable disable
namespace Mortar
{
  public class DisplayManager
  {
    public Matrix currentViewMtx;
    public Matrix currentWorldMtx;
    public Matrix currentProjMtx;
    public Matrix currentTextureMtx;
    public Point Res;
    public static DisplayManager instance = new DisplayManager();
    private Color clearcolor = Color.Black;
    public Matrix topMatrix;
    public Texture currentTexture;
    private DepthStencilState currentDstate;
    private DepthStencilState lastDstate;
    private BlendState currentBstate;
    private BlendState lastBstate;
    private RasterizerState currentRstate;
    private RasterizerState lastRstate;
    private bool denabled;
    private bool dwriteenabled = true;
    private DepthStencilState[] depthStates;
    private BlendState bsOff;
    private BlendState bsDef;
    private RasterizerState rsCullOff;
    private RasterizerState rsCullCwise;

    public DisplayManager()
    {
      this.Res = new Point(800, 480);
      this.depthStates = ArrayInit.CreateFilledArray<DepthStencilState>(4);
      this.depthStates[0].DepthBufferEnable = false;
      this.depthStates[0].DepthBufferWriteEnable = false;
      this.depthStates[1].DepthBufferEnable = false;
      this.depthStates[1].DepthBufferWriteEnable = true;
      this.depthStates[2].DepthBufferEnable = true;
      this.depthStates[2].DepthBufferWriteEnable = false;
      this.depthStates[3].DepthBufferEnable = true;
      this.depthStates[3].DepthBufferWriteEnable = true;
      this.bsOff = new BlendState();
      this.bsDef = new BlendState();
      this.bsDef.ColorBlendFunction = (BlendFunction) 0;
      this.bsDef.ColorSourceBlend = (Blend) 4;
      this.bsDef.ColorDestinationBlend = (Blend) 5;
      this.bsDef.AlphaBlendFunction = (BlendFunction) 0;
      this.bsDef.AlphaSourceBlend = (Blend) 4;
      this.bsDef.AlphaDestinationBlend = (Blend) 5;
      this.rsCullCwise = new RasterizerState();
      this.rsCullCwise.CullMode = (CullMode) 1;
      this.rsCullOff = new RasterizerState();
      this.rsCullOff.CullMode = (CullMode) 0;
    }

    public static DisplayManager GetInstance() => DisplayManager.instance;

    public void SetWindowSize(int xpos, int xsize, int ypos, int ysize)
    {
    }

    public void Init(string name)
    {
      this.topMatrix = Matrix.CreateRotationZ(MathHelper.ToRadians(270f));
      this.topMatrix = Matrix.CreateScale(new Vector3(1.66666663f, 1.5f, 1f));
    }

    public void SetClearColor(Color color) => this.clearcolor = Color.Red;

    public void SetLightDirection(Vector3 ld)
    {
    }

    public void SetGlobalAmbience(Color col)
    {
    }

    public void DidUploadMatrixies()
    {
    }

    public void BeginFrame()
    {
      Game1.instance.GraphicsDevice.Clear((ClearOptions) 3, this.clearcolor, 1f, 0);
      Mesh.newframe = true;
      this.SetBlendStateOff();
      Mesh.vertsCurrentOffset = 0;
    }

    public void EndFrame()
    {
    }

    public void SwapBuffers()
    {
    }

    public void SetNewDeptStensileState(DepthStencilState d)
    {
      Game1.instance.GraphicsDevice.DepthStencilState = d;
      this.lastDstate = this.currentDstate;
      this.currentDstate = d;
    }

    public void SetNewBlendState(BlendState b)
    {
      Game1.instance.GraphicsDevice.BlendState = b;
      this.lastBstate = this.currentBstate;
      this.currentBstate = b;
    }

    public void SetNewRasterizeState(RasterizerState r)
    {
      Game1.instance.GraphicsDevice.RasterizerState = r;
      this.lastRstate = this.currentRstate;
      this.currentRstate = r;
    }

    private void GetNewDepthStensileState()
    {
      this.SetNewDeptStensileState(this.depthStates[(this.denabled ? 2 : 0) + (this.dwriteenabled ? 1 : 0)]);
    }

    public void SetBlendStateOff() => this.SetNewBlendState(this.bsOff);

    public void SetBlendStateDefault() => this.SetNewBlendState(BlendState.NonPremultiplied);

    public void SetBlendStateDefault2() => this.SetNewBlendState(BlendState.Additive);

    public void SetRasterizeStateCullOff() => this.SetNewRasterizeState(this.rsCullOff);

    public void SetRasterizeStateCullCwise() => this.SetNewRasterizeState(this.rsCullCwise);

    public void SetDepthBuffer(bool en)
    {
      this.denabled = en;
      this.GetNewDepthStensileState();
    }

    public void SetDepthBufferWrite(bool en)
    {
      this.dwriteenabled = en;
      this.GetNewDepthStensileState();
    }

    public MortarRectangle GetWindowSize()
    {
      MortarRectangle windowSize;
      windowSize.left = 0;
      windowSize.top = 0;
      windowSize.right = this.Res.X;
      windowSize.bottom = this.Res.Y;
      return windowSize;
    }
  }
}
