
// Type: FruitNinja2.Coin
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Mortar;
using System;
using System.Collections.Generic;

#nullable disable
namespace FruitNinja2
{
  public class Coin : Entity
  {
    protected int m_worth;
    protected int m_state;
    protected float m_stateTime;
    protected bool m_useModel;
    protected uint m_trailHash;
    protected uint m_burstHash;
    protected Vector3 m_destination;
    protected Coin.CoinArrivedCallback m_coinArrivedCall;
    protected float m_speed;
    protected ushort m_rollyAngle;
    protected PSPParticleEmitter m_trailEmitter;
    protected PSPParticleEmitter m_burstEmitter;
    private static Model s_coinModel = (Model) null;
    private static bool s_isContentLoaded = false;
    public static int PiIdx = 32768;
    public static int TwoPiIdx = 65536;

    public static float COIN_INIT_VEL_MIN => 500f;

    public static float COIN_INIT_VEL_MAX => 1050f;

    public static float COIN_INIT_VEL_RANGE => Coin.COIN_INIT_VEL_MAX - Coin.COIN_INIT_VEL_MIN;

    public static float COIN_INIT_VEL
    {
      get => Coin.COIN_INIT_VEL_MIN + Mortar.Math.g_random.RandF(Coin.COIN_INIT_VEL_RANGE);
    }

    public static float COIN_CORNER_POS_X => (float) ((double) Game.SCREEN_WIDTH / 2.0 - 20.0);

    public static float COIN_CORNER_POS_Y => (float) (-(double) Game.SCREEN_HEIGHT / 2.0 + 20.0);

    public static Vector3 COIN_CORNER_POS
    {
      get => new Vector3(Coin.COIN_CORNER_POS_X, Coin.COIN_CORNER_POS_Y, 0.0f);
    }

    public static float COIN_WAIT_TIME => 0.05f;

    public static float ROT_SPEED => 0.065f;

    public static float SCALE_NORMAL => 0.5f;

    public static float SCALE_POP => 1f;

    public int GetWorth() => this.m_worth;

    public static void LoadContent()
    {
      Coin.s_coinModel = MeshManager.GetInstance().Load("models/Fruit/coin.mmd");
      Coin.s_isContentLoaded = true;
    }

    public static void UnLoadContent()
    {
      Coin.s_coinModel = (Model) null;
      Coin.s_isContentLoaded = false;
    }

    public void InitCoin(
      Vector3 pos,
      Vector3 dest,
      ushort angle,
      int worth,
      uint trailHash,
      uint burstHash)
    {
      this.InitCoin(pos, dest, angle, worth, trailHash, burstHash, new Coin.CoinArrivedCallback(Coin.CoinArrived));
    }

    public void InitCoin(
      Vector3 pos,
      Vector3 dest,
      ushort angle,
      int worth,
      uint trailHash,
      uint burstHash,
      Coin.CoinArrivedCallback call)
    {
      this.InitCoin(pos, dest, angle, worth, trailHash, burstHash, call, 0.0f);
    }

    public void InitCoin(
      Vector3 pos,
      Vector3 dest,
      ushort angle,
      int worth,
      uint trailHash,
      uint burstHash,
      Coin.CoinArrivedCallback call,
      float wait)
    {
      this.InitCoin(pos, dest, angle, worth, trailHash, burstHash, call, wait, true);
    }

    public void InitCoin(
      Vector3 pos,
      Vector3 dest,
      ushort angle,
      int worth,
      uint trailHash,
      uint burstHash,
      Coin.CoinArrivedCallback call,
      float wait,
      bool useModel)
    {
      this.m_destroy = false;
      this.m_dormant = false;
      this.m_speed = Coin.COIN_INIT_VEL;
      this.m_pos = pos;
      this.m_destination = dest;
      this.m_dir_angle = angle;
      this.m_worth = worth;
      this.m_state = 0;
      this.m_stateTime = -wait;
      this.m_vel = Vector3.Zero;
      this.m_cur_scale = Vector3.Multiply(Vector3.One, Coin.SCALE_NORMAL);
      this.m_trailEmitter = (PSPParticleEmitter) null;
      this.m_burstEmitter = (PSPParticleEmitter) null;
      this.m_trailHash = trailHash;
      this.m_burstHash = burstHash;
      this.m_useModel = useModel;
      this.m_coinArrivedCall = call;
      this.m_rollyAngle = (ushort) 0;
    }

    public Coin()
    {
      if (!Coin.s_isContentLoaded)
        Coin.LoadContent();
      this.m_worth = 1;
      this.m_state = 0;
      this.m_stateTime = 0.0f;
      this.m_pos = Vector3.Zero;
      this.m_vel = Vector3.Zero;
      this.m_destroy = false;
      this.m_dormant = false;
      this.m_speed = 0.0f;
      this.m_useModel = true;
      this.m_destination = Coin.COIN_CORNER_POS;
    }

    public override void Release()
    {
      if (this.m_trailEmitter == null)
        return;
      PSPParticleManager.GetInstance().ClearEmitter(this.m_trailEmitter);
      this.m_trailEmitter = (PSPParticleEmitter) null;
    }

    private static int GetSmallestDelta(int to, int from)
    {
      int t = to - from;
      if (Mortar.Math.ABS(t) > Coin.PiIdx)
        t = to > from ? t - Coin.TwoPiIdx : to + Coin.TwoPiIdx - from;
      return t;
    }

    public override void Update(float dt)
    {
      float dt1 = Game.game_work.dt;
      Game.game_work.dt = 0.0166666675f;
      this._Update(0.0166666675f);
      if (!this.m_dormant)
        this._Update(0.0166666675f);
      Game.game_work.dt = dt1;
    }

    public void _Update(float dt)
    {
      dt *= 0.25f;
      if (this.m_burstEmitter != null)
      {
        PSPParticleManager.GetInstance().ClearEmitter(this.m_burstEmitter);
        this.m_burstEmitter = (PSPParticleEmitter) null;
      }
      switch (this.m_state)
      {
        case 0:
          this.m_stateTime -= Game.game_work.dt;
          if ((double) this.m_stateTime <= 0.0)
          {
            this.m_state = 2;
            this.m_vel = Vector3.Multiply(new Vector3(Mortar.Math.SinIdx(this.m_dir_angle) * this.m_speed, Mortar.Math.CosIdx(this.m_dir_angle) * this.m_speed, 0.0f), 1.5f);
            if (!this.m_useModel)
            {
              this.m_stateTime = 0.0f;
              this.m_state = 4;
              if (PSPParticleManager.GetInstance().EmitterExists(this.m_trailHash))
              {
                this.m_trailEmitter = PSPParticleManager.GetInstance().AddEmitter(this.m_trailHash, (Action<PSPParticleEmitter>) null);
                break;
              }
              break;
            }
            break;
          }
          break;
        case 1:
          this.Arrived();
          return;
        case 2:
          Coin coin1 = this;
          coin1.m_vel = Vector3.Multiply(coin1.m_vel, 0.7f);
          if ( (double) this.m_vel.LengthSquared() < 900.0 )
          {
            this.m_state = 3;
            if (PSPParticleManager.GetInstance().EmitterExists(this.m_trailHash))
            {
              this.m_trailEmitter = PSPParticleManager.GetInstance().AddEmitter(this.m_trailHash, (Action<PSPParticleEmitter>) null);
              break;
            }
            break;
          }
          break;
        case 3:
          float stateTime = this.m_stateTime;
          this.m_stateTime += Game.game_work.dt;
          if ((double) stateTime <= 0.0099999997764825821 && (double) this.m_stateTime > 0.0099999997764825821 && (ActorManager.GetInstance().GetNumEntities(2) < 20U ? 1 : (Mortar.Math.g_random.Rand32(3) == 0 ? 1 : 0)) != 0 && PSPParticleManager.GetInstance().EmitterExists(this.m_burstHash))
          {
            this.m_burstEmitter = PSPParticleManager.GetInstance().AddEmitter(this.m_burstHash, (Action<PSPParticleEmitter>) null);
            if (this.m_burstEmitter != null)
              this.m_burstEmitter.pos = this.m_pos;
          }
          if ((double) this.m_stateTime >= (double) Coin.COIN_WAIT_TIME)
          {
            this.m_stateTime = 0.0f;
            this.m_state = 4;
            this.m_dir_angle += (ushort) (uint) (ushort) ((double) Coin.GetSmallestDelta((int) Mortar.Math.Atan2Idx(this.m_destination.X - this.m_pos.X, this.m_destination.Y - this.m_pos.Y), (int) this.m_dir_angle) * 0.25);
            break;
          }
          break;
        case 4:
          this.m_stateTime += Game.game_work.dt;
          float y = this.m_destination.X - this.m_pos.X;
          float x = this.m_destination.Y - this.m_pos.Y;
          float num1 = Mortar.Math.Sqrt((float) ((double) y * (double) y + (double) x * (double) x));
          if ((double) num1 < 40.0 || (double) this.m_stateTime > 1.5)
          {
            if ((ActorManager.GetInstance().GetNumEntities(2) < 20U ? 1 : (Mortar.Math.g_random.Rand32(3) == 0 ? 1 : 0)) != 0 && PSPParticleManager.GetInstance().EmitterExists(this.m_burstHash))
            {
              this.m_burstEmitter = PSPParticleManager.GetInstance().AddEmitter(this.m_burstHash, (Action<PSPParticleEmitter>) null);
              if (this.m_burstEmitter != null)
                this.m_burstEmitter.pos = this.m_pos;
            }
            this.m_state = 1;
            return;
          }
          ushort to = Mortar.Math.Atan2Idx(y, x);
          float num2 = Mortar.Math.MIN(Coin.ROT_SPEED * (float) (1.0 + (double) this.m_stateTime * 5.0 + (double) Mortar.Math.MAX(100f - num1, 0.0f) / 5.0), (float) (0.85000002384185791 + (double) Mortar.Math.MAX(100f - num1, 0.0f) * 0.15000000596046448 / 100.0));
          this.m_dir_angle += (ushort) (uint) (ushort) (int) ((double) Coin.GetSmallestDelta((int) to, (int) this.m_dir_angle) * (double) num2);
          float num3 = Mortar.Math.MIN((float) (((double) this.m_stateTime + 0.05000000074505806) * 2.0), 1f);
          this.m_rollyAngle += (ushort) (int) ((double) num3 * (double) Mortar.Math.DEGREE_TO_IDX(180f) * (double) dt * 500.0);
          float num4 = num3 * (this.m_speed * 2f);
          this.m_vel = new Vector3(Mortar.Math.SinIdx(this.m_dir_angle) * num4, Mortar.Math.CosIdx(this.m_dir_angle) * num4, 0.0f);
          break;
      }
      Coin coin2 = this;
      coin2.m_pos = Vector3.Add(coin2.m_pos, Vector3.Multiply(this.m_vel, Game.game_work.dt));
      if (this.m_trailEmitter == null)
        return;
      this.m_trailEmitter.pos = this.m_pos;
      this.m_trailEmitter.sinz = Mortar.Math.SinIdx(this.m_dir_angle);
      this.m_trailEmitter.cosz = Mortar.Math.CosIdx(this.m_dir_angle);
    }

    public static void CoinArrived(Coin theCoin) => Game.AddCoins(theCoin.GetWorth());

    public override void Draw()
    {
      if (!this.m_useModel || Coin.s_coinModel == null || this.m_state <= 1)
        return;
      Matrix matrix = Matrix.Multiply(Matrix.Multiply(Matrix.Multiply(Matrix.CreateScale(this.m_cur_scale), Matrix.CreateRotationY(Mortar.Math.IDX_TO_RADIANS(this.m_rollyAngle))), Matrix.CreateRotationZ(Mortar.Math.IDX_TO_RADIANS(this.m_dir_angle))), Matrix.CreateTranslation(this.m_pos));
      Coin.s_coinModel.Draw(new Matrix?(matrix));
    }

    public override void DrawUpdate(float dt)
    {
    }

    public void Arrived()
    {
      this.m_coinArrivedCall(this);
      if (this.m_trailEmitter != null)
      {
        PSPParticleManager.GetInstance().ClearEmitter(this.m_trailEmitter);
        this.m_trailEmitter = (PSPParticleEmitter) null;
      }
      this.m_dormant = true;
      this.m_destroy = true;
    }

    public static void MakeCoins(
      int numberOfCoins,
      int coinWorth,
      Vector3 pos,
      ushort direction,
      ushort range)
    {
      Coin.MakeCoins(numberOfCoins, coinWorth, pos, direction, range, new Vector3?());
    }

    public static void MakeCoins(
      int numberOfCoins,
      int coinWorth,
      Vector3 pos,
      ushort direction,
      ushort range,
      Vector3? dest)
    {
      Coin.MakeCoins(numberOfCoins, coinWorth, pos, direction, range, dest, 0.02f);
    }

    public static void MakeCoins(
      int numberOfCoins,
      int coinWorth,
      Vector3 pos,
      ushort direction,
      ushort range,
      Vector3? dest,
      float delay)
    {
      Coin.MakeCoins(numberOfCoins, coinWorth, pos, direction, range, dest, delay, 0.15f);
    }

    public static void MakeCoins(
      int numberOfCoins,
      int coinWorth,
      Vector3 pos,
      ushort direction,
      ushort range,
      Vector3? dest,
      float delay,
      float maxTotalDelay)
    {
      Coin.MakeCoins(numberOfCoins, coinWorth, pos, direction, range, dest, delay, maxTotalDelay, (string) null);
    }

    public static void MakeCoins(
      int numberOfCoins,
      int coinWorth,
      Vector3 pos,
      ushort direction,
      ushort range,
      Vector3? dest,
      float delay,
      float maxTotalDelay,
      string trailEmmiter)
    {
      Coin.MakeCoins(numberOfCoins, coinWorth, pos, direction, range, dest, delay, maxTotalDelay, trailEmmiter, (string) null);
    }

    public static void MakeCoins(
      int numberOfCoins,
      int coinWorth,
      Vector3 pos,
      ushort direction,
      ushort range,
      Vector3? dest,
      float delay,
      float maxTotalDelay,
      string trailEmmiter,
      string burstEmmiter)
    {
      Coin.MakeCoins(numberOfCoins, coinWorth, pos, direction, range, dest, delay, maxTotalDelay, trailEmmiter, burstEmmiter, new Coin.CoinArrivedCallback(Coin.CoinArrived));
    }

    public static void MakeCoins(
      int numberOfCoins,
      int coinWorth,
      Vector3 pos,
      ushort direction,
      ushort range,
      Vector3? dest,
      float delay,
      float maxTotalDelay,
      string trailEmmiter,
      string burstEmmiter,
      Coin.CoinArrivedCallback call)
    {
      Coin.MakeCoins(numberOfCoins, coinWorth, pos, direction, range, dest, delay, maxTotalDelay, trailEmmiter, burstEmmiter, call, true);
    }

    public static void MakeCoins(
      int numberOfCoins,
      int coinWorth,
      Vector3 pos,
      ushort direction,
      ushort range,
      Vector3? dest,
      float delay,
      float maxTotalDelay,
      string trailEmmiter,
      string burstEmmiter,
      Coin.CoinArrivedCallback call,
      bool useModel)
    {
      if (numberOfCoins <= 0)
        return;
      int num1 = numberOfCoins / coinWorth + 1;
      if ((double) num1 * (double) delay > (double) maxTotalDelay)
        delay = maxTotalDelay / (float) num1;
      Vector3 dest1 = dest.HasValue ? dest.Value : Coin.COIN_CORNER_POS;
      uint trailHash = StringFunctions.StringHash(trailEmmiter != null ? trailEmmiter : "coin_fly");
      uint burstHash = StringFunctions.StringHash(burstEmmiter != null ? burstEmmiter : "coin_collect");
      int num2 = 0;
      for (int index1 = 0; index1 < numberOfCoins; index1 += coinWorth)
      {
        Coin coin = (Coin) ActorManager.GetInstance().Add(EntityTypes.ENTITY_COIN);
        int num3 = Mortar.Math.g_random.Rand32((int) range) - ((int) range >> 1);
        ushort num4 = (ushort) ((uint) direction + (uint) num3);
        float num5 = pos.X + Mortar.Math.SinIdx(num4) * 100f;
        float num6 = pos.Y + Mortar.Math.CosIdx(num4) * 100f;
        for (int index2 = 1; ((double) num5 < -(double) Game.SCREEN_WIDTH / 2.0 || (double) num5 > (double) Game.SCREEN_WIDTH / 2.0 || (double) num6 < -(double) Game.SCREEN_HEIGHT / 2.0 || (double) num6 > (double) Game.SCREEN_HEIGHT / 2.0) && index2 < 10; num6 = pos.Y + Mortar.Math.CosIdx(num4) * 100f)
        {
          ++index2;
          int num7 = Mortar.Math.g_random.Rand32((int) range) - ((int) range >> 1);
          num4 = (ushort) ((uint) direction + (uint) num7);
          num5 = pos.X + Mortar.Math.SinIdx(num4) * 100f;
        }
        coin.InitCoin(pos, dest1, num4, Mortar.Math.MIN(coinWorth, numberOfCoins - index1), trailHash, burstHash, call, (float) num2 * delay, useModel);
        ++num2;
      }
    }

    public static void ClearCoins(bool callDelegate)
    {
      LinkedListNode<Entity> iterator = (LinkedListNode<Entity>) null;
      for (Coin coin = (Coin) ActorManager.GetInstance().GetEntityFirst(EntityTypes.ENTITY_COIN, ref iterator); coin != null; coin = (Coin) ActorManager.GetInstance().GetEntityNext(EntityTypes.ENTITY_COIN, ref iterator))
      {
        if (callDelegate)
        {
          coin.Arrived();
        }
        else
        {
          coin.m_dormant = true;
          coin.m_destroy = true;
        }
      }
    }

    public delegate void CoinArrivedCallback(Coin sdf);

    public enum COIN_STATE
    {
      COIN_STATE_START,
      COIN_STATE_DEAD,
      COIN_STATE_POPOUT,
      COIN_STATE_WAIT,
      COIN_STATE_HEADH_TO_CORNER,
    }
  }
}
