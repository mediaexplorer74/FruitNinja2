
// Type: FruitNinja2.EffectImage
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Mortar;
using System.Xml.Linq;

#nullable disable
namespace FruitNinja2
{
  public class EffectImage
  {
    public Texture texture;
    public HUDControl3d control;
    public bool added;
    public bool deferer;
    public Vector3 pos;
    public HUD.HUD_ORDER drawOrder;
    public ushort pulse;
    public float pulseSpeed;
    public float pulseScalePositive;
    public float pulseScaleNegative;
    public float transitionAmount;
    public float transitionTime;
    public Vector3 transitionMoveIn;
    public Vector3 transitionMoveOut;
    public float timeStart;
    public float timeEnd;
    public Vector3 scale;
    public Color colour;
    public uint transitionMask;
    private static uint[] hashes = new uint[2]
    {
      StringFunctions.StringHash(nameof (scale)),
      StringFunctions.StringHash("fade")
    };
    private static uint[] drawderhashes = new uint[5]
    {
      StringFunctions.StringHash("normal"),
      StringFunctions.StringHash("post"),
      StringFunctions.StringHash("before_splats"),
      StringFunctions.StringHash("after_splats"),
      StringFunctions.StringHash("after_bomb")
    };

    public EffectImage Duplicate()
    {
      return new EffectImage()
      {
        texture = this.texture,
        control = this.control,
        added = this.added,
        deferer = this.deferer,
        pos = this.pos,
        drawOrder = this.drawOrder,
        pulse = this.pulse,
        pulseSpeed = this.pulseSpeed,
        pulseScalePositive = this.pulseScalePositive,
        pulseScaleNegative = this.pulseScaleNegative,
        transitionAmount = this.transitionAmount,
        transitionTime = this.transitionTime,
        transitionMoveIn = this.transitionMoveIn,
        transitionMoveOut = this.transitionMoveOut,
        timeStart = this.timeStart,
        timeEnd = this.timeEnd,
        scale = this.scale,
        colour = this.colour,
        transitionMask = this.transitionMask
      };
    }

    public EffectImage()
    {
      this.deferer = false;
      this.drawOrder = HUD.HUD_ORDER.HUD_ORDER_NORMAL;
      this.pulse = (ushort) 0;
      this.colour = Color.White;
      this.scale = Vector3.Zero;
      this.pulseScalePositive = 0.0f;
      this.pulseScaleNegative = 0.0f;
      this.pulseSpeed = 0.0f;
      this.timeStart = 1f;
      this.timeEnd = 0.0f;
      this.added = true;
      this.transitionAmount = 0.0f;
      this.transitionTime = 0.0f;
      this.transitionMoveOut = this.transitionMoveIn = Vector3.Zero;
      this.pos = Vector3.Zero;
      this.texture = (Texture) null;
      this.control = (HUDControl3d) null;
    }

    public void Parse(XElement parent)
    {
      this.pos = Save.ParseVector(parent.AttributeStr("pos"));
      this.transitionMoveOut = this.transitionMoveIn = Save.ParseVector(parent.AttributeStr("transitionMove"));
      Save.ParseVector(parent.AttributeStr("transitionMoveIn"), ref this.transitionMoveIn);
      Save.ParseVector(parent.AttributeStr("transitionMoveOut"), ref this.transitionMoveOut);
      string str1 = parent.AttributeStr("localise");
      bool flag = false;
      if (str1 == null)
      {
        this.texture = StringFunctions.LoadTexture(parent.AttributeStr("texture"));
      }
      else
      {
        string str2 = parent.AttributeStr("texture") + ".tex";
        this.texture = TextureManager.GetInstance().Load(str2, true);
        if (string.Compare(str2, "arcade_go.tex") == 0)
          flag = true;
        if (string.Compare(str2, "arcade_60seconds.tex") == 0)
          flag = true;
      }
      if (this.texture != null)
      {
        this.scale = new Vector3((float) this.texture.GetWidth(), (float) this.texture.GetHeight(), 0.0f);
        if (flag)
        {
          EffectImage effectImage = this;
          effectImage.scale = Vector3.Multiply(effectImage.scale, new Vector3(0.8f, 0.8f, 0.8f));
        }
      }
      Save.ParseVector(parent.AttributeStr("scale"), ref this.scale);
      EffectImage effectImage1 = this;
      effectImage1.scale = Vector3.Multiply(effectImage1.scale, Game.GAME_MODE_SCALE_FIX);
      float num = 1f;
      parent.QueryFloatAttribute("slowHardwareScale", ref num);
      if (!Game.IsFastHardware())
      {
        EffectImage effectImage2 = this;
        effectImage2.scale = Vector3.Multiply(effectImage2.scale, num);
      }
      parent.QueryFloatAttribute("pulseSpeed", ref this.pulseSpeed);
      parent.QueryFloatAttribute("pulseScale", ref this.pulseScalePositive);
      this.pulseScaleNegative = this.pulseScalePositive;
      parent.QueryFloatAttribute("pulseScalePositive", ref this.pulseScalePositive);
      parent.QueryFloatAttribute("pulseScaleNegative", ref this.pulseScaleNegative);
      parent.QueryFloatAttribute("transitionTime", ref this.transitionTime);
      parent.QueryFloatAttribute("timeStart", ref this.timeStart);
      parent.QueryFloatAttribute("timeEnd", ref this.timeEnd);
      StringFunctions.ParseColour(ref this.colour, parent.AttributeStr("colour"));
      this.transitionMask = StringFunctions.ParseMaskWords(parent.AttributeStr("transition"), EffectImage.hashes, EffectImage.hashes.Length);
      this.drawOrder = (HUD.HUD_ORDER) StringFunctions.FindIndex(parent.AttributeStr("drawOrder"), EffectImage.drawderhashes, EffectImage.drawderhashes.Length);
      this.deferer = StringFunctions.CompareWords(parent.AttributeStr("deferPoints"), "true");
    }
  }
}
