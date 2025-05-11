
// Type: FruitNinja2.PSPParticleManager
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Mortar;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

#nullable disable
namespace FruitNinja2
{
  public class PSPParticleManager
  {
    private static PSPParticleManager instance = new PSPParticleManager();
    public PSPParticle[] particles;
    public ushort particleFreeList;
    public ushort rotStart;
    public uint m_activeParticles;
    public LinkedList<PSPParticleEmitter> particleEmitters = new LinkedList<PSPParticleEmitter>();
    public int numTemplates;
    public LinkedList<PSPParticleTemplate> particleTemplates = new LinkedList<PSPParticleTemplate>();
    public int noet;
    public LinkedList<PSPEmitterTemplate> emitterTemplates = new LinkedList<PSPEmitterTemplate>();
    private static GameVertex[] vt = new GameVertex[6000];

    public static PSPParticleManager GetInstance() => PSPParticleManager.instance;

    public PSPEmitterTemplate GetEmitterTemplate(int idx)
    {
      int num = 0;
      foreach (PSPEmitterTemplate emitterTemplate in this.emitterTemplates)
      {
        if (num == idx)
          return emitterTemplate;
        ++num;
      }
      return (PSPEmitterTemplate) null;
    }

    public void Update(float update) => this.Update(update, false);

    public void Update(float dt, bool paused)
    {
      this._Update(0.0166666675f, paused);
      this._Draw(0.0166666675f, paused, 0, false);
      this._Update(0.0166666675f, paused);
    }

    private void _Update(float dt, bool paused)
    {
      int num = 0;
      LinkedListNode<PSPParticleEmitter> next;
      for (LinkedListNode<PSPParticleEmitter> node = this.particleEmitters.First; node != null; node = next)
      {
        PSPParticleEmitter pspParticleEmitter = node.Value;
        if (pspParticleEmitter.enabled != (ushort) 0 && (double) pspParticleEmitter.rateScale != 0.0 && (!paused || pspParticleEmitter.updateEvenIfPaused))
        {
          pspParticleEmitter.Update(dt);
          ++num;
        }
        next = node.Next;
        if ((double) pspParticleEmitter.time >= (double) pspParticleEmitter.tmplt.life && (double) pspParticleEmitter.tmplt.life != 0.0 && ((double) pspParticleEmitter.tmplt.life > 0.0 || pspParticleEmitter.tmplt.Ends()))
        {
          if (pspParticleEmitter.zeroer != null)
            pspParticleEmitter.zeroer((PSPParticleEmitter) null);
          this.particleEmitters.Remove(node);
        }
      }
    }

    public void Draw(float dt, bool paused, int withDepth)
    {
      this._Draw(0.0333333351f, paused, withDepth, true);
    }

    private void _Draw(float dt, bool paused, int withDepth, bool foreal)
    {
      if (foreal)
      {
        MatrixManager.instance.Reset();
        MatrixManager.instance.UploadCurrentMatrices();
      }
      int offset = 0;
      int index1 = 0;
      this.m_activeParticles = 0U;
      byte[] numArray = new byte[4];
      foreach (PSPParticleTemplate particleTemplate in this.particleTemplates)
      {
        int index2 = (int) particleTemplate.first_particle;
        if (index2 != 0 && (withDepth == particleTemplate.useDepth || !foreal))
        {
          ushort index3 = 0;
          float life = particleTemplate.life;
          do
          {
            PSPParticle particle = this.particles[index2];
            PSPParticle pspParticle = particle;
            if ((double) pspParticle.time <= (double) pspParticle.lifeScale)
            {
              ushort nextParticle = (ushort) pspParticle.next_particle;
              if (index3 != (ushort) 0)
                this.particles[(int) index3].next_particle = this.particles[index2].next_particle;
              else
                particleTemplate.first_particle = (ushort) this.particles[index2].next_particle;
              this.particles[index2].next_particle = (int) this.particleFreeList;
              this.particleFreeList = (ushort) index2;
              index2 = (int) nextParticle;
            }
            else
            {
              float t = (life - pspParticle.time) / life;
              float num1;
              if ((double) t < 0.5)
              {
                float num2 = t * 2f;
                numArray[0] = (byte) ((double) pspParticle.Cs[0] + (double) pspParticle.Ci[0] * (double) num2);
                numArray[1] = (byte) ((double) pspParticle.Cs[1] + (double) pspParticle.Ci[1] * (double) num2);
                numArray[2] = (byte) ((double) pspParticle.Cs[2] + (double) pspParticle.Ci[2] * (double) num2);
                numArray[3] = (byte) ((double) pspParticle.Cs[3] + (double) pspParticle.Ci[3] * (double) num2);
                num1 = (float) pspParticle.sizeS + (float) pspParticle.sizeInc * num2;
              }
              else
              {
                float num3 = (float) (((double) t - 0.5) * 2.0);
                numArray[0] = (byte) ((double) ((int) pspParticle.Cs[0] + (int) pspParticle.Ci[0]) + (double) pspParticle.Ci2[0] * (double) num3);
                numArray[1] = (byte) ((double) ((int) pspParticle.Cs[1] + (int) pspParticle.Ci[1]) + (double) pspParticle.Ci2[1] * (double) num3);
                numArray[2] = (byte) ((double) ((int) pspParticle.Cs[2] + (int) pspParticle.Ci[2]) + (double) pspParticle.Ci2[2] * (double) num3);
                numArray[3] = (byte) ((double) ((int) pspParticle.Cs[3] + (int) pspParticle.Ci[3]) + (double) pspParticle.Ci2[3] * (double) num3);
                num1 = (float) ((double) pspParticle.sizeS + (double) pspParticle.sizeInc + (double) pspParticle.sizeInc2 * (double) num3);
              }
              float num4 = num1 * particleTemplate.xScaleRatio;
              float num5 = num1;
              if (!paused || pspParticle.owner != null && pspParticle.owner.updateEvenIfPaused)
              {
                float num6 = Mortar.Math.floatLERP(pspParticle.spin_speed_start, pspParticle.spin_speed_end, t);
                if ((double) num6 != 0.0)
                {
                  particle.current_spin += (ushort) (int) ((double) num6 * 182.0 * 360.0 * (double) dt);
                  if (particle.current_spin == (ushort) 0)
                  {
                    particle.sinz = -1f;
                    particle.cosz = 1f;
                    particle.vecX = new Vector2(1f, 0.0f);
                    particle.vecY = new Vector2(0.0f, 1f);
                  }
                  else
                  {
                    particle.vecX = new Vector2(Mortar.Math.SinIdx((ushort) ((uint) particle.current_spin + 16384U)), Mortar.Math.CosIdx((ushort) ((uint) particle.current_spin + 16384U)));
                    particle.vecY = new Vector2(Mortar.Math.SinIdx(particle.current_spin), Mortar.Math.CosIdx(particle.current_spin));
                    ushort idx = (ushort) (((int) particle.current_spin + 57330) % 65520);
                    particle.sinz = Mortar.Math.SinIdx(idx) * 1.41f;
                    particle.cosz = Mortar.Math.CosIdx(idx) * 1.41f;
                  }
                }
                float num7 = Mortar.Math.floatLERP(pspParticle.cycleX_speed_start, pspParticle.cycleX_speed_end, t);
                if ((double) num7 != 0.0)
                  particle.current_x_scale_cycle += (ushort) (int) ((double) num7 * 182.0 * 360.0 * (double) dt);
                if (particle.current_x_scale_cycle != (ushort) 0)
                  num4 *= Mortar.Math.CosIdx(particle.current_x_scale_cycle);
                float num8 = Mortar.Math.floatLERP(pspParticle.cycleY_speed_start, pspParticle.cycleY_speed_end, t);
                if ((double) num8 != 0.0)
                  particle.current_y_scale_cycle += (ushort) (int) ((double) num8 * 182.0 * 360.0 * (double) dt);
                if (particle.current_y_scale_cycle != (ushort) 0)
                  num5 *= Mortar.Math.CosIdx(particle.current_y_scale_cycle);
              }
              Color color = new Color();
              color.R = numArray[0];
              color.G = numArray[1];
              color.B = numArray[2];
              color.A = numArray[3];
              double sinz = (double) pspParticle.sinz;
              double cosz = (double) pspParticle.cosz;
              Vector2 vector2_1 = Vector2.Multiply(particle.vecX, num4);
              Vector2 vector2_2 = Vector2.Multiply(particle.vecY, num5);
              Vector3 pos = pspParticle.pos;
              pos.X += particleTemplate.coord_type == (byte) 1 ? pspParticle.owner.pos.X : 0.0f;
              pos.Y += particleTemplate.coord_type == (byte) 1 ? pspParticle.owner.pos.Y : 0.0f;
              PSPParticleManager.vt[index1].X = pos.X + (vector2_1.X + vector2_2.X);
              PSPParticleManager.vt[index1].Y = pos.Y + (vector2_1.Y + vector2_2.Y);
              PSPParticleManager.vt[index1].u = 1f;
              PSPParticleManager.vt[index1].v = 0.0f;
              int index4 = index1 + 1;
              PSPParticleManager.vt[index4].X = pos.X + (-vector2_1.X + vector2_2.X);
              PSPParticleManager.vt[index4].Y = pos.Y + (-vector2_1.Y + vector2_2.Y);
              PSPParticleManager.vt[index4].u = 0.0f;
              PSPParticleManager.vt[index4].v = 0.0f;
              int index5 = index4 + 1;
              PSPParticleManager.vt[index5].X = pos.X + (vector2_1.X - vector2_2.X);
              PSPParticleManager.vt[index5].Y = pos.Y + (vector2_1.Y - vector2_2.Y);
              PSPParticleManager.vt[index5].u = 1f;
              PSPParticleManager.vt[index5].v = 1f;
              int index6 = index5 + 1;
              PSPParticleManager.vt[index6] = PSPParticleManager.vt[index6 - 1];
              int index7 = index6 + 1;
              PSPParticleManager.vt[index7] = PSPParticleManager.vt[index7 - 3];
              int index8 = index7 + 1;
              PSPParticleManager.vt[index8].X = pos.X + (-vector2_1.X - vector2_2.X);
              PSPParticleManager.vt[index8].Y = pos.Y + (-vector2_1.Y - vector2_2.Y);
              PSPParticleManager.vt[index8].u = 0.0f;
              PSPParticleManager.vt[index8].v = 1f;
              index1 = index8 + 1;
              for (int index9 = index1 - 6; index9 < index1; ++index9)
              {
                PSPParticleManager.vt[index9].Z = pos.Z;
                PSPParticleManager.vt[index9].color = color;
              }
              if (!paused || pspParticle.owner != null && pspParticle.owner.updateEvenIfPaused)
              {
                particle.time = pspParticle.time - dt;
                float num9 = dt;
                if ((double) dt > 0.02500000037252903)
                  dt /= 2f;
                particle.vel.X = (pspParticle.vel.X + pspParticle.gravity.X * dt) * Mortar.Math.floatLERP(particleTemplate.friction_start.X, particleTemplate.friction_end.X, t);
                particle.vel.Y = (pspParticle.vel.Y + pspParticle.gravity.Y * dt) * Mortar.Math.floatLERP(particleTemplate.friction_start.Y, particleTemplate.friction_end.Y, t);
                particle.vel.Z = (pspParticle.vel.Z + pspParticle.gravity.Z * dt) * Mortar.Math.floatLERP(particleTemplate.friction_start.Z, particleTemplate.friction_end.Z, t);
                particle.pos.X = pspParticle.pos.X + particle.vel.X * dt;
                particle.pos.Y = pspParticle.pos.Y + particle.vel.Y * dt;
                particle.pos.Z = pspParticle.pos.Z + particle.vel.Z * dt;
                dt = num9;
              }
              index3 = (ushort) index2;
              index2 = pspParticle.next_particle;
            }
          }
          while (index2 != 0);
          int numPoints = index1 - offset;
          ++index1;
          if (numPoints != 0)
          {
            this.m_activeParticles += (uint) ((index1 - offset) / 6);
            if (foreal)
            {
              if (particleTemplate.tex != null)
                particleTemplate.tex.Set();
              Mesh.DrawTriList(PSPParticleManager.vt, numPoints, true, offset);
            }
          }
          offset = index1;
        }
      }
    }

    public bool EmitterExists(uint hash)
    {
      foreach (PSPEmitterTemplate emitterTemplate in this.emitterTemplates)
      {
        if ((int) emitterTemplate.hash == (int) hash)
          return true;
      }
      return false;
    }

    public PSPParticleEmitter AddEmitter(uint hash, Action<PSPParticleEmitter> zeroer)
    {
      PSPEmitterTemplate pspEmitterTemplate = (PSPEmitterTemplate) null;
      foreach (PSPEmitterTemplate emitterTemplate in this.emitterTemplates)
      {
        if ((int) emitterTemplate.hash == (int) hash)
        {
          pspEmitterTemplate = emitterTemplate;
          break;
        }
      }
      PSPParticleEmitter pspParticleEmitter = new PSPParticleEmitter();
      pspParticleEmitter.updateEvenIfPaused = false;
      pspParticleEmitter.pos.X = 0.0f;
      pspParticleEmitter.pos.Y = 0.0f;
      pspParticleEmitter.pos.Z = 0.0f;
      pspParticleEmitter.tmplt = pspEmitterTemplate;
      pspParticleEmitter.time = 0.0f;
      pspParticleEmitter._vel.X = 0.0f;
      pspParticleEmitter._vel.Y = 0.0f;
      pspParticleEmitter._vel.Z = 0.0f;
      pspParticleEmitter.enabled = (ushort) 1;
      pspParticleEmitter.zeroer = zeroer;
      pspParticleEmitter.cosz = 1f;
      pspParticleEmitter.sinz = 0.0f;
      pspParticleEmitter.scale = 1f;
      pspParticleEmitter.rateScale = 1f;
      pspParticleEmitter.lifeScale = 1f;
      pspParticleEmitter.sizeScale = Game.IsMultiplayer() ? Game.SPLIT_SCREEN_SCALE : 1f;
      this.particleEmitters.AddFirst(pspParticleEmitter);
      return pspParticleEmitter;
    }

    public void ClearEmitters()
    {
      foreach (PSPParticleEmitter particleEmitter in this.particleEmitters)
      {
        if (particleEmitter.zeroer != null)
          particleEmitter.zeroer((PSPParticleEmitter) null);
      }
      this.particleEmitters.Clear();
      if (this.particles != null)
      {
        int num = 1024;
        for (int index = 1; index < num; ++index)
          this.particles[index].next_particle = index + 1;
        this.particles[num - 1].next_particle = 0;
      }
      this.particleFreeList = (ushort) 1;
      if (this.particleTemplates == null)
        return;
      foreach (PSPParticleTemplate particleTemplate in this.particleTemplates)
        particleTemplate.first_particle = (ushort) 0;
    }

    public void ClearEmitter(PSPParticleEmitter emitter)
    {
      if (emitter == null)
        return;
      if (emitter.zeroer != null)
        emitter.zeroer((PSPParticleEmitter) null);
      this.particleEmitters.Remove(emitter);
    }

    public void Destroy()
    {
      this.ClearEmitters();
      this.particleTemplates.Clear();
      this.emitterTemplates.Clear();
      Delete.SAFE_DELETE_ARRAY<PSPParticle[]>(ref this.particles);
    }

    private int[] GetNumberListI(string str)
    {
      string[] strArray = str.Split(new char[1]{ ' ' }, StringSplitOptions.RemoveEmptyEntries);
      int[] numberListI = new int[strArray.Length];
      for (int index = 0; index < strArray.Length; ++index)
        numberListI[index] = MParser.ParseInt(strArray[index]);
      return numberListI;
    }

    private float[] GetNumberListF(string str)
    {
      string[] strArray = str.Split(new char[1]{ ' ' }, StringSplitOptions.RemoveEmptyEntries);
      float[] numberListF = new float[strArray.Length];
      for (int index = 0; index < strArray.Length; ++index)
        numberListF[index] = MParser.ParseFloat(strArray[index]);
      return numberListF;
    }

    public void LoadFile(string texturedir, string filename)
    {
      if (this.particles == null)
      {
        int size = 1024;
        this.particles = ArrayInit.CreateFilledArray<PSPParticle>(size);
        for (int index = 1; index < size; ++index)
          this.particles[index].next_particle = index + 1;
        this.particles[size - 1].next_particle = 0;
        this.particleFreeList = (ushort) 1;
      }
      bool flag = false;
      do
      {
        XDocument element1 = MortarXml.Load(filename);
        if (element1 != null)
        {
          flag = true;
          XElement element2 = element1.FirstChildElement("particle_file").FirstChildElement("body");
          XElement element3 = element2.FirstChildElement("particleTemplate");
          this.numTemplates = 0;
          uint[] numArray = new uint[1024];
          while (element3 != null)
          {
            XElement element4 = element3;
            PSPParticleTemplate particleTemplate = new PSPParticleTemplate();
            this.particleTemplates.AddLast(particleTemplate);
            numArray[this.numTemplates] = StringFunctions.StringHash(element4.Attribute((XName) "name").Value);
            element4.QueryIntAttribute("useDepth", ref particleTemplate.useDepth);
            string texture = string.Format("{0}/{1}.tex", (object) texturedir, (object) element4.FirstChildElement("texture").Attribute((XName) "name").Value);
            try
            {
              particleTemplate.tex = TextureManager.GetInstance().Load(texture);
            }
            catch
            {
            }
            particleTemplate.xScaleRatio = (float) particleTemplate.tex.GetWidth() / (float) particleTemplate.tex.GetHeight();
            particleTemplate.life = MParser.ParseFloat(element4.FirstChildElement("life").Value) / 60f;
            if (element4.FirstChildElement("type") != null)
            {
              switch (element4.FirstChildElement("type").Value)
              {
                case "Point":
                  particleTemplate.particle_type = (byte) 0;
                  break;
                case "Vortex":
                  particleTemplate.particle_type = (byte) 1;
                  break;
                case "Direction":
                  particleTemplate.particle_type = (byte) 2;
                  break;
                case "Angular":
                  particleTemplate.particle_type = (byte) 3;
                  break;
                default:
                  particleTemplate.particle_type = (byte) 0;
                  break;
              }
            }
            else
              particleTemplate.particle_type = (byte) 0;
            if (element4.FirstChildElement("system") != null)
            {
              switch (element4.FirstChildElement("system").Value)
              {
                case "Local":
                  particleTemplate.coord_type = (byte) 1;
                  break;
                case "Global":
                  particleTemplate.coord_type = (byte) 0;
                  break;
                default:
                  particleTemplate.coord_type = (byte) 0;
                  break;
              }
            }
            else
              particleTemplate.coord_type = (byte) 0;
            int[] numberListI1 = this.GetNumberListI(element4.FirstChildElement("gravity").Value);
            particleTemplate.gravity_min.X = (float) numberListI1[0];
            particleTemplate.gravity_min.Y = (float) numberListI1[1];
            particleTemplate.gravity_min.Z = (float) numberListI1[2];
            if (element4.FirstChildElement("gravity_max") != null)
            {
              int[] numberListI2 = this.GetNumberListI(element4.FirstChildElement("gravity_max").Value);
              particleTemplate.gravity_max.X = (float) numberListI2[0];
              particleTemplate.gravity_max.Y = (float) numberListI2[1];
              particleTemplate.gravity_max.Z = (float) numberListI2[2];
            }
            else
              particleTemplate.gravity_max = particleTemplate.gravity_min;
            XElement xelement1 = element4.FirstChildElement("color");
            int[] numberListI3 = this.GetNumberListI(xelement1.Attribute((XName) "startMin").Value);
            int num1 = numberListI3[0];
            int num2 = numberListI3[1];
            int num3 = numberListI3[2];
            int num4 = numberListI3[3];
            particleTemplate.color_start_max[0] = (byte) ((double) num1 * 8.22599983215332);
            particleTemplate.color_start_max[1] = (byte) ((double) num2 * 8.22599983215332);
            particleTemplate.color_start_max[2] = (byte) ((double) num3 * 8.22599983215332);
            particleTemplate.color_start_max[3] = (byte) ((double) num4 * 8.22599983215332);
            int[] numberListI4 = this.GetNumberListI(xelement1.Attribute((XName) "startMax").Value);
            int num5 = numberListI4[0];
            int num6 = numberListI4[1];
            int num7 = numberListI4[2];
            int num8 = numberListI4[3];
            particleTemplate.color_start_min[0] = (byte) ((double) num5 * 8.22599983215332);
            particleTemplate.color_start_min[1] = (byte) ((double) num6 * 8.22599983215332);
            particleTemplate.color_start_min[2] = (byte) ((double) num7 * 8.22599983215332);
            particleTemplate.color_start_min[3] = (byte) ((double) num8 * 8.22599983215332);
            if (xelement1.Attribute((XName) "endMin") != null && xelement1.Attribute((XName) "endMax") != null)
            {
              int[] numberListI5 = this.GetNumberListI(xelement1.Attribute((XName) "endMin").Value);
              int num9 = numberListI5[0];
              int num10 = numberListI5[1];
              int num11 = numberListI5[2];
              int num12 = numberListI5[3];
              particleTemplate.color_end_max[0] = (byte) ((double) num9 * 8.22599983215332);
              particleTemplate.color_end_max[1] = (byte) ((double) num10 * 8.22599983215332);
              particleTemplate.color_end_max[2] = (byte) ((double) num11 * 8.22599983215332);
              particleTemplate.color_end_max[3] = (byte) ((double) num12 * 8.22599983215332);
              int[] numberListI6 = this.GetNumberListI(xelement1.Attribute((XName) "endMax").Value);
              int num13 = numberListI6[0];
              int num14 = numberListI6[1];
              int num15 = numberListI6[2];
              int num16 = numberListI6[3];
              particleTemplate.color_end_min[0] = (byte) ((double) num13 * 8.22599983215332);
              particleTemplate.color_end_min[1] = (byte) ((double) num14 * 8.22599983215332);
              particleTemplate.color_end_min[2] = (byte) ((double) num15 * 8.22599983215332);
              particleTemplate.color_end_min[3] = (byte) ((double) num16 * 8.22599983215332);
            }
            else
            {
              particleTemplate.color_end_min = particleTemplate.color_start_min;
              particleTemplate.color_end_max = particleTemplate.color_start_max;
            }
            if (xelement1.Attribute((XName) "midMax") != null && xelement1.Attribute((XName) "midMin") != null)
            {
              int[] numberListI7 = this.GetNumberListI(xelement1.Attribute((XName) "midMin").Value);
              int num17 = numberListI7[0];
              int num18 = numberListI7[1];
              int num19 = numberListI7[2];
              int num20 = numberListI7[3];
              particleTemplate.color_mid_max[0] = (byte) ((double) num17 * 8.22599983215332);
              particleTemplate.color_mid_max[1] = (byte) ((double) num18 * 8.22599983215332);
              particleTemplate.color_mid_max[2] = (byte) ((double) num19 * 8.22599983215332);
              particleTemplate.color_mid_max[3] = (byte) ((double) num20 * 8.22599983215332);
              int[] numberListI8 = this.GetNumberListI(xelement1.Attribute((XName) "midMax").Value);
              int num21 = numberListI8[0];
              int num22 = numberListI8[1];
              int num23 = numberListI8[2];
              int num24 = numberListI8[3];
              particleTemplate.color_mid_min[0] = (byte) ((double) num21 * 8.22599983215332);
              particleTemplate.color_mid_min[3] = (byte) ((double) num24 * 8.22599983215332);
              particleTemplate.color_mid_min[1] = (byte) ((double) num22 * 8.22599983215332);
              particleTemplate.color_mid_min[2] = (byte) ((double) num23 * 8.22599983215332);
            }
            else
            {
              for (int index = 0; index < 4; ++index)
              {
                particleTemplate.color_mid_min[index] = (byte) (((int) particleTemplate.color_start_min[index] + (int) particleTemplate.color_end_min[index]) / 2);
                particleTemplate.color_mid_max[index] = (byte) (((int) particleTemplate.color_start_max[index] + (int) particleTemplate.color_end_max[index]) / 2);
              }
            }
            particleTemplate.first_particle = (ushort) 0;
            XElement xelement2 = element4.FirstChildElement("friction");
            if (xelement2 != null && xelement2.Attribute((XName) "start") != null && xelement2.Attribute((XName) "end") != null)
            {
              particleTemplate.friction_start = Vector3.One;
              particleTemplate.friction_end = Vector3.One;
              float[] numberListF1 = this.GetNumberListF(xelement2.Attribute((XName) "start").Value);
              particleTemplate.friction_start.X = numberListF1[0];
              particleTemplate.friction_start.Y = numberListF1[1];
              particleTemplate.friction_start.Z = numberListF1[2];
              float[] numberListF2 = this.GetNumberListF(xelement2.Attribute((XName) "end").Value);
              particleTemplate.friction_end.X = numberListF2[0];
              particleTemplate.friction_end.Y = numberListF2[1];
              particleTemplate.friction_end.Z = numberListF2[2];
            }
            else
            {
              particleTemplate.friction_start = Vector3.One;
              particleTemplate.friction_end = Vector3.One;
            }
            XElement xelement3 = element4.FirstChildElement("size");
            particleTemplate.size_start_min = (byte) MParser.ParseInt(xelement3.Attribute((XName) "startMin").Value);
            particleTemplate.size_start_max = (byte) MParser.ParseInt(xelement3.Attribute((XName) "startMax").Value);
            particleTemplate.size_end_min = (byte) MParser.ParseInt(xelement3.Attribute((XName) "endMin").Value);
            particleTemplate.size_end_max = (byte) MParser.ParseInt(xelement3.Attribute((XName) "endMax").Value);
            if (xelement3.Attribute((XName) "midMax") != null && xelement3.Attribute((XName) "midMin") != null)
            {
              particleTemplate.size_mid_max = (byte) MParser.ParseInt(xelement3.Attribute((XName) "midMax").Value);
              particleTemplate.size_mid_min = (byte) MParser.ParseInt(xelement3.Attribute((XName) "midMin").Value);
            }
            else
            {
              particleTemplate.size_mid_max = (byte) (((int) particleTemplate.size_start_max + (int) particleTemplate.size_end_max) / 2);
              particleTemplate.size_mid_min = (byte) (((int) particleTemplate.size_start_min + (int) particleTemplate.size_end_min) / 2);
            }
            XElement xelement4 = element4.FirstChildElement("cycleX");
            if (xelement4 != null && xelement4.Attribute((XName) "startMin") != null && xelement4.Attribute((XName) "startMax") != null)
            {
              particleTemplate.cycleX_start_min = (short) MParser.ParseInt(xelement4.Attribute((XName) "startMin").Value);
              particleTemplate.cycleX_start_max = (short) MParser.ParseInt(xelement4.Attribute((XName) "startMax").Value);
              if (xelement4 != null && xelement4.Attribute((XName) "endMin") != null && xelement4.Attribute((XName) "endMax") != null)
              {
                particleTemplate.cycleX_end_min = (short) MParser.ParseInt(xelement4.Attribute((XName) "endMin").Value);
                particleTemplate.cycleX_end_max = (short) MParser.ParseInt(xelement4.Attribute((XName) "endMax").Value);
              }
              else
              {
                particleTemplate.cycleX_end_min = particleTemplate.cycleX_start_min;
                particleTemplate.cycleX_end_max = particleTemplate.cycleX_start_max;
              }
            }
            else
            {
              particleTemplate.cycleX_start_min = (short) 0;
              particleTemplate.cycleX_start_max = (short) 0;
              particleTemplate.cycleX_end_min = (short) 0;
              particleTemplate.cycleX_end_max = (short) 0;
            }
            XElement xelement5 = element4.FirstChildElement("cycleY");
            if (xelement5 != null && xelement5.Attribute((XName) "startMin") != null && xelement5.Attribute((XName) "startMax") != null)
            {
              particleTemplate.cycleY_start_min = (short) MParser.ParseInt(xelement5.Attribute((XName) "startMin").Value);
              particleTemplate.cycleY_start_max = (short) MParser.ParseInt(xelement5.Attribute((XName) "startMax").Value);
              if (xelement5 != null && xelement5.Attribute((XName) "endMin") != null && xelement5.Attribute((XName) "endMax") != null)
              {
                particleTemplate.cycleY_end_min = (short) MParser.ParseInt(xelement5.Attribute((XName) "endMin").Value);
                particleTemplate.cycleY_end_max = (short) MParser.ParseInt(xelement5.Attribute((XName) "endMax").Value);
              }
              else
              {
                particleTemplate.cycleY_end_min = particleTemplate.cycleY_start_min;
                particleTemplate.cycleY_end_max = particleTemplate.cycleY_start_max;
              }
            }
            else
            {
              particleTemplate.cycleY_start_min = (short) 0;
              particleTemplate.cycleY_start_max = (short) 0;
              particleTemplate.cycleY_end_min = (short) 0;
              particleTemplate.cycleY_end_max = (short) 0;
            }
            XElement xelement6 = element4.FirstChildElement("angle");
            if (xelement6 != null && xelement6.Attribute((XName) "min") != null && xelement6.Attribute((XName) "max") != null)
            {
              particleTemplate.angleMin = MParser.ParseInt(xelement6.Attribute((XName) "min").Value);
              particleTemplate.angleMax = MParser.ParseInt(xelement6.Attribute((XName) "max").Value);
            }
            else
            {
              particleTemplate.angleMin = 0;
              particleTemplate.angleMax = 0;
            }
            XElement xelement7 = element4.FirstChildElement("spin");
            if (xelement7 != null && xelement7.Attribute((XName) "startMin") != null && xelement7.Attribute((XName) "startMax") != null)
            {
              particleTemplate.spin_start_min = (short) MParser.ParseInt(xelement7.Attribute((XName) "startMin").Value);
              particleTemplate.spin_start_max = (short) MParser.ParseInt(xelement7.Attribute((XName) "startMax").Value);
              if (xelement7 != null && xelement7.Attribute((XName) "endMin") != null && xelement7.Attribute((XName) "endMax") != null)
              {
                particleTemplate.spin_end_min = (short) MParser.ParseInt(xelement7.Attribute((XName) "endMin").Value);
                particleTemplate.spin_end_max = (short) MParser.ParseInt(xelement7.Attribute((XName) "endMax").Value);
              }
              else
              {
                particleTemplate.spin_end_min = particleTemplate.spin_start_min;
                particleTemplate.spin_end_max = particleTemplate.spin_start_max;
              }
            }
            else
            {
              particleTemplate.spin_start_min = (short) 0;
              particleTemplate.spin_start_max = (short) 0;
              particleTemplate.spin_end_min = (short) 0;
              particleTemplate.spin_end_max = (short) 0;
            }
            string str = element4.FirstChildElement("SourceBlend").Value;
            element3 = element3.NextSiblingElement("particleTemplate");
            ++this.numTemplates;
          }
          this.noet = 0;
          for (XElement element5 = element2.FirstChildElement("emitter"); element5 != null; element5 = element5.NextSiblingElement("emitter"))
          {
            XElement element6 = element5;
            PSPEmitterTemplate pspEmitterTemplate = new PSPEmitterTemplate();
            this.emitterTemplates.AddLast(pspEmitterTemplate);
            pspEmitterTemplate.hash = StringFunctions.StringHash(element6.Attribute((XName) "name").Value);
            pspEmitterTemplate.name = element6.Attribute((XName) "name").Value;
            pspEmitterTemplate.life = MParser.ParseFloat(element6.FirstChildElement("life").Value) / 60f;
            int num25 = 0;
            XElement element7 = element6.FirstChildElement("particleSet");
            while (element7 != null)
            {
              XElement element8 = element7;
              PSPParticleSet pspParticleSet = new PSPParticleSet();
              pspEmitterTemplate.sets.Add(pspParticleSet);
              uint num26 = StringFunctions.StringHash(element8.Attribute((XName) "name").Value);
              int index = 0;
              foreach (PSPParticleTemplate particleTemplate in this.particleTemplates)
              {
                if ((int) numArray[index] == (int) num26)
                {
                  pspParticleSet.template_idx = particleTemplate;
                  break;
                }
                ++index;
              }
              XElement xelement8 = element8.FirstChildElement("time");
              pspParticleSet.time_start = MParser.ParseUShort(xelement8.Attribute((XName) "start").Value);
              pspParticleSet.time_end = MParser.ParseUShort(xelement8.Attribute((XName) "stop").Value);
              XElement xelement9 = element8.FirstChildElement("particleNumber");
              pspParticleSet.number_start = MParser.ParseByte(xelement9.Attribute((XName) "init").Value);
              pspParticleSet.number_per_second = MParser.ParseByte(xelement9.Attribute((XName) "perSec").Value);
              XElement xelement10 = element8.FirstChildElement("velocity");
              int[] numberListI9 = this.GetNumberListI(xelement10.Attribute((XName) "min").Value);
              pspParticleSet.vel_min.X = (float) numberListI9[0];
              pspParticleSet.vel_min.Y = (float) numberListI9[1];
              pspParticleSet.vel_min.Z = (float) numberListI9[2];
              int[] numberListI10 = this.GetNumberListI(xelement10.Attribute((XName) "max").Value);
              pspParticleSet.vel_max.X = (float) numberListI10[0];
              pspParticleSet.vel_max.Y = (float) numberListI10[1];
              pspParticleSet.vel_max.Z = (float) numberListI10[2];
              element7 = element7.NextSiblingElement("particleSet");
              ++num25;
            }
            pspEmitterTemplate.particle_set_num = (byte) num25;
            ++this.noet;
          }
        }
      }
      while (!flag);
    }

    public enum PARTICLE_TYPE
    {
      PARTICLE_TYPE_POINT,
      PARTICLE_TYPE_VORTEX,
      PARTICLE_TYPE_DIRECTIONAL,
      PARTICLE_TYPE_ANGULAR,
    }

    public enum PARTICLE_COORD
    {
      PARTICLE_COORD_GLOBAL,
      PARTICLE_COORD_LOCAL,
    }
  }
}
