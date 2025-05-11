
// Type: Mortar.Texture
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework.Graphics;

#nullable disable
namespace Mortar
{
  public class Texture
  {
    private const int MRI_CURRENT_VERSION = 4;
    private const int MRI_HEADER_SIZE = 16;
    public uint w;
    public uint h;
    public uint mipmaps;
    public Texture2D intex;
    public bool cached;
    public bool hasAlpha;
    public bool localise;
    public string texture_filename;

    private void SetUnCached()
    {
    }

    private void UnSetUnCached()
    {
    }

    public Texture() => this.localise = false;

    public static Texture Load(string Filename) => Texture._Load(new Texture(), Filename);

    public static Texture _Load(Texture tex, string Filename)
    {
      Texture texture = tex;
      try
      {
        string str = Filename.Replace(".tex", ".tga");
        Texture2D texture2D = Game1.instance.Content.Load<Texture2D>(str);
        texture.h = (uint) texture2D.Height;
        texture.w = (uint) texture2D.Width;
        texture.hasAlpha = true;
        texture.mipmaps = 1U;
        texture.cached = false;
        texture.intex = texture2D;
      }
      catch
      {
        return (Texture) null;
      }
      return texture;
    }

    public uint GetWidth() => this.w;

    public uint GetHeight() => this.h;

    public void Set() => DisplayManager.GetInstance().currentTexture = this;

    public void UnSet() => DisplayManager.GetInstance().currentTexture = (Texture) null;

    public static void Reload(string Filename, Texture texture) => Texture._Load(texture, Filename);

    public enum TextureType
    {
      TT_2D,
      TT_3D,
      TT_Cube,
    }

    public enum TextureFormat
    {
      TF_UNKNOWN,
      TF_CUSTOM,
      TF_A8,
      TF_L8,
      TF_R5G6B5,
      TF_X1R5G5B5,
      TF_A1R5G5B5,
      TF_A4R4G4B4,
      TF_A8L8,
      TF_V8U8,
      TF_L16,
      TF_A8R8G8B8,
      TF_X8R8G8B8,
      TF_G16R16,
      TF_A2R10G10B10,
      TF_A2B10G10R10,
      TF_X8L8V8U8,
      TF_V16U16,
      TF_A16B16G16R16,
      TF_Q8W8V8U8,
      TF_DXT1,
      TF_DXT2,
      TF_DXT3,
      TF_DXT4,
      TF_DXT5,
      TF_R16F,
      TF_G16R16F,
      TF_A16B16G16R16F,
      TF_R32F,
      TF_G32R32F,
      TF_A32B32G32R32F,
      TF_RGB,
      TF_RGBA,
      TF_MAX,
    }

    public enum TextureUsage
    {
      TU_None = 0,
      TU_CanRead = 1,
      TU_Dynamic = 2,
      TU_RenderTarget = 4,
    }

    private enum MRImage2Formats
    {
      MRI_COLOR_FORMAT_SHIFT = 0,
      MRI_CF_BGR_888 = 1,
      MRI_CF_ABGR_8888 = 2,
      MRI_CF_RGBA_5551 = 3,
      MRI_CF_RGBA_4444 = 4,
      MRI_CF_RGB_565 = 5,
      MRI_CF_RGB_888 = 6,
      MRI_CONTAINER_FORMAT_SHIFT = 6,
      MRI_CF_ARGB_1555 = 7,
      MRI_CF_ARGB_8888 = 8,
      MRI_NATIVE_COMPRESSION_SHIFT = 8,
      MRI_CF_XRGB_8888 = 9,
      MRI_CF_RGBX_5551 = 10, // 0x0000000A
      MRI_CF_ARGB_4444 = 11, // 0x0000000B
      MRI_COLOR_FORMAT_MASK = 15, // 0x0000000F
      MRI_VERTICAL_FLIP = 16, // 0x00000010
      MRI_HORIZONTAL_FLIP = 32, // 0x00000020
      MRI_CONTF_WORKING = 64, // 0x00000040
      MRI_CONTF_MORTAR = 128, // 0x00000080
      MRI_CONTAINER_FORMAT_MASK = 192, // 0x000000C0
      MRI_NC_RAW = 256, // 0x00000100
      MRI_NC_PVRTC_2BPP = 512, // 0x00000200
      MRI_NC_PVRTC_4BPP = 768, // 0x00000300
      MRI_NC_A3I5 = 1024, // 0x00000400
      MRI_NC_A5I3 = 1280, // 0x00000500
      MRI_NC_PAL256 = 1536, // 0x00000600
      MRI_NC_PAL16 = 1792, // 0x00000700
      MRI_NC_PAL4 = 2048, // 0x00000800
      MRI_NATIVE_COMPRESSION_MASK = 3840, // 0x00000F00
      MRI_PALETTE_FORMAT_0_COLOR_TRANS = 4096, // 0x00001000
      MRI_LINES_32BIT_ALIGNED = 8192, // 0x00002000
      MRI_MIPMAPS_SHARE_PALETTE = 16384, // 0x00004000
    }

    public struct MRImage2Mortar
    {
      public byte pad01;
      public byte pad00;
      public ushort version;
      public uint format;
      public byte x;
      public byte y;
      public byte pad03;
      public byte mipmapLevels;
      public ushort apparentX;
      public ushort apparentY;
    }
  }
}
