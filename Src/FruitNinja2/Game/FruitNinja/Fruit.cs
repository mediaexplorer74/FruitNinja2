
// Type: GameManager.Fruit
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Mortar;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

#nullable disable
namespace GameManager
{
  public class Fruit : Entity
  {
    public const int FINAL_FREEZE_TIME = 8;
    private static bool s_fruitModelsLoaded = false;
    private static bool s_sfxPlayedThisFrame = false;
    private static List<Model>[] s_fruitModels = ArrayInit.CreateFilledArray<List<Model>>(3);
    protected byte m_fruitType;
    protected bool m_disabled;
    protected byte PAD01;
    protected PSPParticleEmitter[] m_slicedEmitter = ArrayInit.CreateFilledArray<PSPParticleEmitter>(2);
    protected Vector3 m_splashEffectPos;
    protected Vector3 m_splashEffectScale;
    protected int m_splashTime;
    protected int m_splashFrameLength;
    protected int m_splashFrames;
    protected float m_sliceWait;
    protected ushort m_slicedAngle;
    protected float m_slicedMag;
    protected bool m_gravityEnabled;
    protected float m_chuckWait;
    protected Vector3 m_separation;
    protected int m_forPlayer;
    public float m_dtMod;
    public bool m_isLockedMenuButton;
    public float m_z;
    public Vector3 m_gravity;
    public Vector3 m_orig_scale;
    public static int MAX_FRUIT_TYPES = 0;
    public static int CRITICAL_SPLATS = 10;
    public static float CRITICAL_SPLAT_SCALE = 1.5f;
    public static float CRITICAL_SPLAT_SPREAD = 1.2f;
    public static float CRITICAL_DISAPPEAR_SPEED = 2f;
    public static Color CRITICAL_COLOUR = Color.Blue;
    public static int CRITICAL_SCORE = 5;
    public static int CRITICAL_CHANCE = 5;
    public static int CRITICAL_CHANCE_START_INC = 30;
    public static float CRITICAL_FLASH_TIME = 0.5f;
    public static float CRITICAL_FLASH_FULL = 0.4f;
    public static float CRITICAL_FLASH_START_FADE = 0.3f;
    public static Color CRITICAL_FLASH_COLOUR = new Color(128, 128, (int) byte.MaxValue, 128);
    public static int NEW_LIFE_AT = 100;
    public static FRUIT_INFO[] fruitInfo = (FRUIT_INFO[]) null;
    public static int s_consecutiveCount = 0;
    public static int s_consecutiveType = -1;
    public static int s_numActivePowerUpFruits = 0;
    public bool m_isSliced;
    public Vector3 m_pos2;
    public Vector3 m_vel2;
    public Quaternion[] m_rotation_piece = ArrayInit.CreateFilledArray<Quaternion>(2);
    public Vector3[] m_rotation_speed = ArrayInit.CreateFilledArray<Vector3>(2);
    public MenuButton m_hudControl;
    public bool m_isMenuItem;
    public bool m_isCritical;
    private static int[] outOfFruitTime = new int[Game2.MAX_PLAYERS];
    private static bool inited = false;
    private static int banana;
    private static ushort angleOffset = Mortar.Math.DEGREE_TO_IDX(0.0f);
    private static Vector3 vecX = new Vector3(Mortar.Math.CosIdx(Fruit.angleOffset), Mortar.Math.SinIdx(Fruit.angleOffset), 0.0f);
    private static Vector3 vecY = new Vector3(0.0f, Mortar.Math.CosIdx(Fruit.angleOffset), Mortar.Math.SinIdx(Fruit.angleOffset));
    private static Vector3 vecZ = new Vector3(Mortar.Math.SinIdx(Fruit.angleOffset), 0.0f, Mortar.Math.CosIdx(Fruit.angleOffset));
    public static int LOCKED_BANANA_TYPE = -1;
    private static int totalChance = 0;
    private static int totalChanceNonCoined = 0;
    private static int totalChanceOfCritical = 0;
    private static int totalChanceOfCriticalNonCoined = 0;
    private static int redApple = Fruit.FruitType("apple_red");
    private static int apple = Fruit.FruitType(nameof (apple));

    public static float MAX_ROTATION_SPEED => 5.5f;

    public static float MAX_ROTATION_ANGLE => 359f;

    public static float MIN_SPEED => 4f;

    public static float MAX_SPEED => 8f;

    public static int RANDOM_ANGLE_SLICE => (int) Mortar.Math.DEGREE_TO_IDX(120f);

    public static float SIXTY_FPS_DT => 0.0166666675f;

    public static float SLICE_DELAY => 0.03f;

    public static float FRUIT_CHUCK_WAIT => 0.125f;

    protected void KillFruit() => this.KillFruit(false);

    protected void KillFruit(bool dropped)
    {
      for (int index = 0; index < 2; ++index)
      {
        if (this.m_slicedEmitter[index] != null)
        {
          PSPParticleManager.GetInstance().ClearEmitter(this.m_slicedEmitter[index]);
          this.m_slicedEmitter[index] = (PSPParticleEmitter) null;
        }
      }
      if (!this.m_isSliced && !this.m_disabled && !Game2.InViewer() && Fruit.fruitInfo[(int) this.m_fruitType].score < 5)
      {
        if (Game2.game_work.gameMode == Game2.GAME_MODE.GM_ARCADE)
        {
          if (dropped)
          {
            uint hash = StringFunctions.StringHash(nameof (dropped));
            Game2.game_work.saveData.AddToTotal(nameof (dropped), hash, 1, false, false);
          }
        }
        else if (Game2.FailureEnabled() && dropped)
        {
          if (!Game2.game_work.gameOver)
          {
            MissControl.GetFree().MakeDisappear(this.m_pos, this.m_forPlayer);
            SoundManager.GetInstance().SFXPlay(SoundDef.SND_GANK);
            Game2.game_work.hasDroppedFruit = true;
          }
          ++Game2.game_work.currentMissCount;
          if ((int) Game2.game_work.currentMissCount >= (int) Game2.MAX_FRUIT_MISSES)
          {
            Game2.GameOver();
            Fruit.s_consecutiveCount = 0;
            Fruit.s_consecutiveType = -1;
          }
        }
      }
      if (this.m_hudControl != null && this.m_hudControl.m_entity == this)
        this.m_hudControl.m_entity = (Entity) null;
      if (!this.m_destroy && Fruit.fruitInfo[(int) this.m_fruitType].powers != null)
        Fruit.s_numActivePowerUpFruits = Mortar.Math.MAX(Fruit.s_numActivePowerUpFruits - 1, 0);
      this.m_destroy = true;
    }

    public int CheckFruitDropped()
    {
      bool flag = false;
      for (int player = 1; player < Game2.MAX_PLAYERS; ++player)
      {
        if (Fruit.outOfFruitTime[player] > 0)
        {
          --Fruit.outOfFruitTime[player];
          if (Fruit.outOfFruitTime[player] == 0)
          {
            flag = true;
            Game2.GameOver(-1, -1f, player);
          }
        }
      }
      if (!flag)
      {
        if (Fruit.outOfFruitTime[1] > 0 && Fruit.outOfFruitTime[2] > 0)
        {
          flag = true;
          Game2.GameOver(-1, -1f, 0);
        }
        else if (Fruit.outOfFruitTime[1] > 0)
        {
          flag = true;
          Game2.GameOver(-1, -1f, 1);
        }
        else if (Fruit.outOfFruitTime[2] > 0)
        {
          flag = true;
          Game2.GameOver(-1, -1f, 2);
        }
      }
      if (flag)
      {
        for (int index = 1; index < Game2.MAX_PLAYERS; ++index)
          Fruit.outOfFruitTime[index] = 0;
      }
      return !flag ? 0 : 1;
    }

    protected void UpdateBombAvoidance(float dt)
    {
      if (this.m_isSliced)
        return;
      LinkedListNode<Entity> iterator = (LinkedListNode<Entity>) null;
      for (Entity entity = ActorManager.GetInstance().GetEntityFirst(EntityTypes.ENTITY_BOMB, ref iterator); entity != null; entity = ActorManager.GetInstance().GetEntityNext(EntityTypes.ENTITY_BOMB, ref iterator))
      {
        if (((Bomb) entity).IsActive() && ((Bomb) entity).ForPlayer() == this.ForPlayer() && entity.m_col_box != null)
        {
          Vector3 vector3 = Vector3.Subtract(entity.m_pos, this.m_pos);
          float num1 = vector3.LengthSquared();
          float num2 = (float) (((double) this.m_vel.X - (double) entity.m_vel.X) * ((double) this.m_vel.X - (double) entity.m_vel.X) + ((double) this.m_vel.Y - (double) entity.m_vel.Y) * ((double) this.m_vel.Y - (double) entity.m_vel.Y));
          if ((double) num1 < 4900.0 && (double) num2 < 56.25)
            entity.m_vel.X += (float) ((double) Mortar.Math.MATH_SIGN(vector3.Y) * (double) dt * 12.0);
        }
      }
    }

    public Fruit()
    {
      this.m_col_box = (Col) null;
      this.m_hudControl = (MenuButton) null;
      this.m_isMenuItem = false;
      for (int index = 0; index < 2; ++index)
        this.m_slicedEmitter[index] = (PSPParticleEmitter) null;
    }

    public override void Init(byte[] tpl_data, int tpl_size, Vector3? size)
    {
      this.m_isLockedMenuButton = false;
      this.m_dtMod = 1f;
      this.m_fruitType = tpl_size < 0 || tpl_size >= Fruit.MAX_FRUIT_TYPES ? (byte) Fruit.RandomFruit(true) : (byte) tpl_size;
      if (Game2.game_work.gameMode == Game2.GAME_MODE.GM_ARCADE && (double) Game2.game_work.gameOverTransition < 1.0)
      {
        if (!Fruit.inited)
        {
          Fruit.banana = Fruit.FruitType("banana");
          Fruit.inited = true;
        }
        while ((int) this.m_fruitType == Fruit.banana)
          this.m_fruitType = (byte) Fruit.RandomFruit(true);
        if (Fruit.fruitInfo[(int) this.m_fruitType].powers != null)
        {
          uint num = StringFunctions.StringHash("freeze");
          if (Fruit.s_numActivePowerUpFruits > 0 || (double) Game2.game_work.saveData.timer < 8.0 && (int) Fruit.fruitInfo[(int) this.m_fruitType].powers.powerUps[0].powerHash != (int) num || Fruit.fruitInfo[(int) this.m_fruitType].powers.AnyActivePowers())
          {
            this.m_destroy = true;
            return;
          }
        }
      }
      if (Fruit.fruitInfo[(int) this.m_fruitType].powers != null)
        ++Fruit.s_numActivePowerUpFruits;
      this.SetFruitType((int) this.m_fruitType, size.HasValue ? size.Value.X : 1f);
      this.m_updateAlways = true;
      this.m_destroy = false;
      this.m_isSliced = false;
      this.m_disabled = false;
      this.m_sliceWait = -1f;
      this.m_chuckWait = 0.0f;
      this.m_forPlayer = 0;
      this.m_separation = Vector3.Zero;
      float num1 = Mortar.Math.g_random.RandF(Fruit.MAX_ROTATION_SPEED * 2f) - Fruit.MAX_ROTATION_SPEED;
      float num2 = Mortar.Math.g_random.RandF(Fruit.MAX_ROTATION_SPEED * 2f) - Fruit.MAX_ROTATION_SPEED;
      float num3 = Mortar.Math.g_random.RandF(Fruit.MAX_ROTATION_SPEED * 2f) - Fruit.MAX_ROTATION_SPEED;
      Quaternion q = new Quaternion();
      Fruit.RandomStartAngle(ref q);
      for (int index = 0; index < 2; ++index)
      {
        this.m_rotation_speed[index] = new Vector3(num1, num2, num3);
        this.m_rotation_piece[index] = q;
        this.m_slicedEmitter[index] = (PSPParticleEmitter) null;
      }
      this.m_splashTime = 0;
      this.m_splashFrameLength = 75;
      this.m_splashFrames = 4;
      this.m_gravityEnabled = true;
      this.m_hudControl = (MenuButton) null;
      this.m_isMenuItem = false;
      this.m_isCritical = true;
      this.m_z = Game2.GetFruitZPosition();
      this.m_gravity = new Vector3(0.0f, (float) -((double) Game2.GRAVITY / (double) Fruit.SIXTY_FPS_DT), 0.0f);
    }

    public override void Release()
    {
      for (int index = 0; index < 2; ++index)
      {
        if (this.m_slicedEmitter[index] != null)
        {
          PSPParticleManager.GetInstance().ClearEmitter(this.m_slicedEmitter[index]);
          this.m_slicedEmitter[index] = (PSPParticleEmitter) null;
        }
      }
      if (this.m_hudControl != null && this.m_hudControl.m_entity == this)
        this.m_hudControl.m_entity = (Entity) null;
      base.Release();
    }

    public override void Update(float dt)
    {
      dt *= this.m_dtMod;
      float num1 = dt / Fruit.SIXTY_FPS_DT;
      if (this.m_isSliced)
      {
        if (!this.m_isCritical)
        {
          float num2 = this.m_gravity.Length();
          this.m_gravity.Normalize();
          Fruit fruit = this;
          fruit.m_gravity = Vector3.Multiply(fruit.m_gravity, 
              num2 + (float) ((double) Game2.GRAVITY * (double) num1 * 4.5));
        }
        if (this.m_isMenuItem)
        {
          float num3 = this.m_gravity.Length();
          this.m_gravity.Normalize();
          Fruit fruit = this;
          fruit.m_gravity = Vector3.Multiply(fruit.m_gravity, num3 
              + (float) ((double) Game2.GRAVITY * (double) num1 * 6.5));
        }
        Fruit fruit1 = this;
        fruit1.m_vel = Vector3.Add(fruit1.m_vel, Vector3.Multiply(this.m_gravity, dt));
        Fruit fruit2 = this;
        fruit2.m_vel2 = Vector3.Add(fruit2.m_vel2, Vector3.Multiply(this.m_gravity, dt));
        Fruit fruit3 = this;
        fruit3.m_pos = Vector3.Add(fruit3.m_pos, Vector3.Multiply(this.m_vel, num1));
        Fruit fruit4 = this;
        fruit4.m_pos2 = Vector3.Add(fruit4.m_pos2, Vector3.Multiply(this.m_vel2, num1));
        this.m_splashTime += (int) ((double) dt * 1000.0);
      }
      else
      {
        if ((double) this.m_chuckWait > 0.0)
        {
          float chuckWait = this.m_chuckWait;
          if (!Game2.game_work.pause && (double) Game2.game_work.hitBombTime <= 0.0 && (Game2.game_work.gameMode == Game2.GAME_MODE.GM_ARCADE && (double) Game2.game_work.gameOverTransition < 1.0 || !Game2.game_work.gameOver))
            this.m_chuckWait -= Game2.game_work.dt;
          if ((double) this.m_chuckWait <= 0.20000000298023224 && (double) chuckWait > 0.20000000298023224 && !Fruit.s_sfxPlayedThisFrame && !Game2.game_work.gameOver)
          {
            SoundManager.GetInstance().SFXPlay(SoundDef.SND_THROW_FRUIT);
            Fruit.s_sfxPlayedThisFrame = true;
          }
          if ((double) this.m_chuckWait > 0.0)
            return;
          if (PSPParticleManager.GetInstance().EmitterExists(Fruit.fruitInfo[(int) this.m_fruitType].hash[2]))
          {
            this.m_slicedEmitter[0] = PSPParticleManager.GetInstance().AddEmitter(Fruit.fruitInfo[(int) this.m_fruitType].hash[2], (Action<PSPParticleEmitter>) null);
            if (this.m_slicedEmitter[0] != null)
              this.m_slicedEmitter[0].pos = Vector3.Add(this.m_pos, Vector3.Multiply(Vector3.UnitZ, this.m_z));
          }
          else if (Game2.IsFastHardware() && this.m_slicedEmitter[0] != null)
            this.m_slicedEmitter[0].pos = Vector3.Add(this.m_pos, Vector3.Multiply(Vector3.UnitZ, this.m_z - 20f));
          float num4 = WaveManager.GetInstance().FruitMultiplyer();
          int num5 = (int) num4;
          if ((double) num4 > (double) num5 + 0.0099999997764825821 && ((double) num4 - (double) num5) * 100.0 > (double) Mortar.Math.g_random.Rand32(100))
            ++num5;
          if (num5 <= 0)
          {
            this.m_chuckWait = 0.0f;
            this.m_pos.Y = -Game2.SCREEN_HEIGHT;
            this.m_vel = new Vector3(0.0f, -1f, 0.0f);
          }
          else if (num5 > 1)
          {
            SPAWNER_INFO[] spawnerInfoArray = new SPAWNER_INFO[3];
            spawnerInfoArray[0].placement = SPAWN_PLACEMENTS.SPAWNER_TOP;
            spawnerInfoArray[0].gravity = new Vector3(0.0f, -0.05f, 0.0f);
            spawnerInfoArray[0].delayWait = -3f;
            spawnerInfoArray[1].placement = SPAWN_PLACEMENTS.SPAWNER_LEFT;
            spawnerInfoArray[1].horizontalMin = -1f;
            spawnerInfoArray[1].horizontalMax = -0.5f;
            spawnerInfoArray[1].delayWait = -3f;
            spawnerInfoArray[2].placement = SPAWN_PLACEMENTS.SPAWNER_RIGHT;
            spawnerInfoArray[2].horizontalMin = -1f;
            spawnerInfoArray[2].horizontalMax = -0.5f;
            spawnerInfoArray[2].delayWait = -3f;
            WaveManager.GetInstance().SpawnFruit(num5 - 1, -1, spawnerInfoArray[Mortar.Math.g_random.Rand32(3)]);
          }
        }
        if (this.m_gravityEnabled)
        {
          Vector3 gravity = this.m_gravity;
          Fruit fruit5 = this;
          fruit5.m_vel = Vector3.Add(fruit5.m_vel, Vector3.Multiply(gravity, dt));
          Fruit fruit6 = this;
          fruit6.m_pos = Vector3.Add(fruit6.m_pos, Vector3.Multiply(this.m_vel, num1));
          if (Game2.game_work.gameMode == Game2.GAME_MODE.GM_ARCADE && ((int) SlashEntity.ModPowerMask & 32) == 32 && !Game2.game_work.gameOver && (double) this.m_pos.Y < -(double) Game2.SCREEN_HEIGHT / 2.0 + (double) this.m_cur_scale.Y * 50.0 && (double) this.m_vel.Y < 0.0)
          {
            this.m_pos.Y = (float) (-(double) Game2.SCREEN_HEIGHT / 2.0 + (double) this.m_cur_scale.Y * 50.0);
            this.m_vel.Y *= -1f;
          }
          Fruit fruit7 = this;
          fruit7.m_pos = Vector3.Add(fruit7.m_pos, Vector3.Multiply(this.m_separation, dt));
        }
        this.m_pos2 = this.m_pos;
        this.m_vel2 = this.m_vel;
        if ((double) this.m_sliceWait > 0.0)
        {
          this.m_sliceWait -= dt;
          if ((double) this.m_sliceWait <= 0.0)
          {
            this.m_sliceWait = 0.0f;
            this.Slice();
          }
        }
        this.UpdateBombAvoidance(dt);
      }
      for (int index = 0; index < 2; ++index)
      {
        Quaternion fromAxisAngle1 = Quaternion.CreateFromAxisAngle(Fruit.vecX, MathHelper.ToRadians(this.m_rotation_speed[index].X * num1));
        Quaternion fromAxisAngle2 = Quaternion.CreateFromAxisAngle(Fruit.vecY, MathHelper.ToRadians(this.m_rotation_speed[index].Y * num1));
        Quaternion fromAxisAngle3 = Quaternion.CreateFromAxisAngle(Fruit.vecZ, MathHelper.ToRadians(this.m_rotation_speed[index].Z * num1));
        this.m_rotation_piece[index] = Quaternion.Multiply(Quaternion.Multiply(Quaternion.Multiply(this.m_rotation_piece[index], fromAxisAngle1), fromAxisAngle2), fromAxisAngle3);
      }
      if (this.CheckHasGoneOffsceen())
        this.KillFruit(true);
      if (this.m_col_box != null)
      {
        this.m_col_box.centre = this.m_pos;
        this.m_col_box.centre.Z = 0.0f;
      }
      if ((double) dt == 0.0)
      {
        for (int index = 0; index < 2; ++index)
        {
          if (this.m_slicedEmitter[index] != null)
            PSPParticleManager.GetInstance().ClearEmitter(this.m_slicedEmitter[index]);
          this.m_slicedEmitter[index] = (PSPParticleEmitter) null;
        }
      }
      if (this.m_slicedEmitter[0] != null)
      {
        this.m_slicedEmitter[0].pos = Vector3.Add(this.m_pos, Vector3.Multiply(Vector3.UnitZ, this.m_z - 20f));
        this.m_slicedEmitter[0].pos.Z = -5000f;
        Matrix matrix;
        Matrix.CreateFromQuaternion(ref this.m_rotation_piece[0], out matrix);
        Vector3 unitZ = Vector3.UnitZ;
        Vector3 vector3;
        Vector3.Transform(ref unitZ, ref matrix, out vector3);
        ushort idx = Mortar.Math.Atan2Idx(vector3.X, vector3.Y);
        this.m_slicedEmitter[0].sinz = Mortar.Math.SinIdx(idx);
        this.m_slicedEmitter[0].cosz = Mortar.Math.CosIdx(idx);
      }
      if (this.m_slicedEmitter[1] == null)
        return;
      this.m_slicedEmitter[1].pos = Vector3.Add(this.m_pos2, Vector3.Multiply(Vector3.UnitZ, this.m_z));
      Matrix matrix1;
      Matrix.CreateFromQuaternion(ref this.m_rotation_piece[0], out matrix1);
      Vector3 unitZ1 = Vector3.UnitZ;
      Vector3 vector3_1 = new Vector3();
      Vector3.Transform(ref unitZ1, ref matrix1, out vector3_1);
      ushort idx1 = Mortar.Math.Atan2Idx(vector3_1.X, vector3_1.Y);
      this.m_slicedEmitter[1].sinz = Mortar.Math.SinIdx(idx1);
      this.m_slicedEmitter[1].cosz = Mortar.Math.CosIdx(idx1);
    }

    public override void Draw()
    {
      if (PopOverControl.IsInPopup && !this.partOfPopup)
        return;
      Fruit.s_sfxPlayedThisFrame = false;
      if ((double) this.m_chuckWait > 0.0)
        return;
      if (Fruit.LOCKED_BANANA_TYPE == -1)
        Fruit.LOCKED_BANANA_TYPE = Fruit.FruitType("banana_locked");
      int index1 = !this.m_isLockedMenuButton || !Game2.isWP7TrialMode() ? (int) this.m_fruitType : Fruit.LOCKED_BANANA_TYPE;
      if (!this.m_isSliced)
      {
        if (Fruit.s_fruitModels[2][index1] == null)
          return;
        Matrix matrix1;
        Matrix.CreateScale(ref this.m_cur_scale, out matrix1);
        Matrix matrix2 = Matrix.Multiply(Matrix.Multiply(matrix1, Matrix.CreateFromQuaternion(this.m_rotation_piece[0])), Matrix.CreateTranslation(Vector3.Add(this.m_pos, Vector3.Multiply(Vector3.UnitZ, this.m_z))));
        Fruit.s_fruitModels[2][index1].Draw(new Matrix?(matrix2));
      }
      else
      {
        for (int index2 = 0; index2 < 2; ++index2)
        {
          if (Fruit.s_fruitModels[index2][index1] != null)
          {
            Matrix matrix3;
            Matrix.CreateScale(ref this.m_cur_scale, out matrix3);
            Matrix matrix4 = Matrix.Multiply(matrix3, Matrix.CreateFromQuaternion(this.m_rotation_piece[index2]));
            Vector3 vector3 = this.m_pos;
            if (index2 != 0)
              vector3 = this.m_pos2;
            vector3.Z += this.m_z;
            matrix3 = Matrix.Multiply(matrix4, Matrix.CreateTranslation(vector3));
            Fruit.s_fruitModels[index2][index1].Draw(new Matrix?(matrix3));
          }
        }
      }
    }

    public override void DrawUpdate(float dt)
    {
      Fruit fruit = this;
      fruit.m_separation = Vector3.Multiply(fruit.m_separation, 0.9f);
      if (this.m_isSliced || (double) this.m_chuckWait > 0.0)
        return;
      if ((double) this.m_gravity.X == 0.0)
      {
        if (Game2.game_work.gameMode == Game2.GAME_MODE.GM_ARCADE && ((int) SlashEntity.ModPowerMask & 32) == 32)
        {
          if ((double) this.m_pos.X < -(double) Game2.SCREEN_WIDTH * 0.40000000596046448)
          {
            this.m_pos.X = (float) (-(double) Game2.SCREEN_WIDTH * 0.40000000596046448);
            this.m_vel.X *= -1f;
          }
          if ((double) this.m_pos.X <= (double) Game2.SCREEN_WIDTH * 0.40000000596046448)
            return;
          this.m_pos.X = Game2.SCREEN_WIDTH * 0.4f;
          this.m_vel.X *= -1f;
        }
        else
        {
          if ((double) this.m_pos.X < -(double) Game2.SCREEN_WIDTH * 0.40000000596046448)
          {
            this.m_vel.X += dt * 16f;
            this.m_separation.X += 20f;
          }
          if ((double) this.m_pos.X <= (double) Game2.SCREEN_WIDTH * 0.40000000596046448)
            return;
          this.m_vel.X -= dt * 16f;
          this.m_separation.X -= 20f;
        }
      }
      else
      {
        if ((double) this.m_gravity.Y != 0.0)
          return;
        if ((double) this.m_pos.Y < -(double) Game2.SCREEN_HEIGHT * 0.40000000596046448)
        {
          this.m_vel.Y += dt * 16f;
          this.m_separation.Y += 20f;
        }
        if ((double) this.m_pos.Y <= (double) Game2.SCREEN_HEIGHT * 0.40000000596046448)
          return;
        this.m_vel.Y -= dt * 16f;
        this.m_separation.Y -= 20f;
      }
    }

    public override bool CollisionResponse(
      Entity p_ent2,
      uint col_1,
      uint col_2,
      ref Vector3 proj)
    {
      if (PopOverControl.IsInPopup && !this.partOfPopup)
        return false;
      if (this.m_isSliced || (double) this.m_sliceWait > -1.0)
        return true;
      if (Game2.game_work.currentScore < 2 || !Fruit.fruitInfo[(int) this.m_fruitType].canBeCritical || Game2.game_work.gameOver || (double) Game2.game_work.hitBombTime > 0.0)
      {
        this.m_isCritical = false;
      }
      else
      {
        Game2.game_work.criticalChance = Mortar.Math.MAX(2, Game2.game_work.criticalChance - 1);
        this.m_isCritical = (double) WaveManager.GetInstance().GetCriticalChance() > 0.0 && Mortar.Math.g_random.Rand32((uint) Mortar.Math.MAX((float) Mortar.Math.MIN(Game2.game_work.criticalChance, Fruit.CRITICAL_CHANCE) / WaveManager.GetInstance().GetCriticalChance(), 1f)) == 0U;
        if (this.m_isCritical)
          Game2.game_work.criticalChance = Fruit.CRITICAL_CHANCE + Fruit.CRITICAL_CHANCE_START_INC;
      }
      if (!Game2.game_work.inRetrySequence || !this.IsOffscreen())
      {
        if (this.m_isCritical)
        {
          SoundManager.GetInstance().SFXPlay("Critical");
          if ((int) this.m_fruitType == Fruit.FruitType("mango"))
            AchievementManager.AwardAchievementWP7("Mango Magic");
        }
        else
        {
          for (int index = 0; index < Fruit.fruitInfo[(int) this.m_fruitType].numImpactSounds; ++index)
            SoundManager.GetInstance().SFXPlay(Fruit.fruitInfo[(int) this.m_fruitType].impactSounds[index].file);
        }
      }
      this.m_splashEffectPos = this.m_pos;
      this.m_sliceWait = Fruit.SLICE_DELAY;
      float v = proj.Length() * 0.1f;
      float num = Mortar.Math.CLAMP(v, Fruit.MIN_SPEED, Fruit.MAX_SPEED);
      if (this.m_isCritical)
      {
        num = Mortar.Math.CLAMP(v, Fruit.MAX_SPEED * 0.75f, Fruit.MAX_SPEED);
        this.m_sliceWait *= 2.5f;
        GameTask.CriticalFlash(this.m_pos, Fruit.CRITICAL_COLOUR);
      }
      else if (Fruit.fruitInfo[(int) this.m_fruitType].score == 50 && !Game2.game_work.gameOver)
      {
        num = Mortar.Math.CLAMP(v, Fruit.MAX_SPEED * 0.75f, Fruit.MAX_SPEED);
        this.m_sliceWait *= 0.5f;
        GameTask.CriticalFlash(this.m_pos, new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 128));
        MissControl.GetFree().MakeRare(this.m_pos);
      }
      this.m_slicedAngle = Mortar.Math.Atan2Idx(proj.X, proj.Y);
      this.m_slicedMag = num;
      for (int index = 0; index < 2; ++index)
      {
        if (this.m_slicedEmitter[index] != null)
          PSPParticleManager.GetInstance().ClearEmitter(this.m_slicedEmitter[index]);
        this.m_slicedEmitter[index] = (PSPParticleEmitter) null;
      }
      if (Game2.game_work.inRetrySequence && this.IsOffscreen())
        return false;
      GameTask.AddSlice(this.m_pos, (float) (-(double) this.m_slicedAngle / 182.0 + 90.0), num * 0.4f, this.m_isCritical);
      uint hash1 = Fruit.fruitInfo[(int) this.m_fruitType].hash[0];
      if (this.m_isCritical)
        hash1 = StringFunctions.StringHash("crit_hit");
      if (PSPParticleManager.GetInstance().EmitterExists(hash1))
      {
        PSPParticleEmitter pspParticleEmitter = PSPParticleManager.GetInstance().AddEmitter(hash1, (Action<PSPParticleEmitter>) null);
        if (pspParticleEmitter != null)
        {
          pspParticleEmitter.sinz = -Mortar.Math.SinIdx((ushort)-this.m_slicedAngle);
          pspParticleEmitter.cosz = Mortar.Math.CosIdx((ushort)-this.m_slicedAngle);
          pspParticleEmitter.pos = this.m_pos;
        }
      }
      uint hash2 = Fruit.fruitInfo[(int) this.m_fruitType].hash[3];
      if (this.m_isCritical)
        hash2 = StringFunctions.StringHash("crit_hit_stars");
      if (PSPParticleManager.GetInstance().EmitterExists(hash2))
      {
        this.m_slicedEmitter[0] = PSPParticleManager.GetInstance().AddEmitter(hash2, (Action<PSPParticleEmitter>) null);
        this.m_slicedEmitter[1] = PSPParticleManager.GetInstance().AddEmitter(hash2, (Action<PSPParticleEmitter>) null);
        if (this.m_slicedEmitter[0] != null)
          this.m_slicedEmitter[0].pos = this.m_pos;
        if (this.m_slicedEmitter[1] != null)
          this.m_slicedEmitter[1].pos = this.m_pos2;
      }
      if (Game2.game_work.inRetrySequence)
        return false;
      AchievementManager.GetInstance().UnlockSpecificOrderAchievement(Fruit.fruitInfo[(int) this.m_fruitType].hash[0]);
      if (p_ent2 != null && !this.m_disabled && (!Game2.game_work.gameOver || (Game2.game_work.gameMode == Game2.GAME_MODE.GM_ZEN || Game2.game_work.gameMode == Game2.GAME_MODE.GM_ARCADE) && (double) Game2.game_work.gameOverTransition < 0.949999988079071 && (double) Game2.game_work.gameOverTransition > -0.10000000149011612) && !Game2.InViewer() && p_ent2 != null)
      {
        if (Fruit.fruitInfo[(int) this.m_fruitType].powers != null)
          PowerUpManager.GetInstance().ActivatePower(Fruit.fruitInfo[(int) this.m_fruitType].powers.RandomPower(), this.m_pos);
        int score = this.m_isCritical ? Fruit.CRITICAL_SCORE + Fruit.fruitInfo[(int) this.m_fruitType].score : Fruit.fruitInfo[(int) this.m_fruitType].score;
        if (Fruit.s_consecutiveType != (int) this.m_fruitType)
        {
          Fruit.s_consecutiveType = (int) this.m_fruitType;
          while (Fruit.s_consecutiveCount > 1 && Game2.game_work.gameMode == Game2.GAME_MODE.GM_CASINO)
          {
            --Fruit.s_consecutiveCount;
            score += 1 << Fruit.s_consecutiveCount;
          }
          Fruit.s_consecutiveCount = 0;
        }
        else if (Fruit.s_consecutiveCount >= 1 && Game2.game_work.gameMode == Game2.GAME_MODE.GM_CASINO)
        {
          string filename = string.Format("combo-{0}", (object) Fruit.s_consecutiveCount);
          SoundManager.GetInstance().SFXPlay(filename);
        }
        ++Fruit.s_consecutiveCount;
        if (Game2.game_work.gameMode == Game2.GAME_MODE.GM_ARCADE)
        {
          WaveManager.GetInstance().AddToSpeedLossTime(0.05f);
          uint hash3 = StringFunctions.StringHash("first_fruit");
          uint hash4 = StringFunctions.StringHash("last_fruit");
          if (Game2.game_work.saveData.GetTotal(hash3) <= 0)
            Game2.game_work.saveData.AddToTotal("first_fruit", hash3, 1 + (int) this.m_fruitType, false, false);
          Game2.game_work.saveData.AddToTotal("last_fruit", hash4, 1 + (int) this.m_fruitType - Game2.game_work.saveData.GetTotal(hash4), false, false);
        }
        Game2.AddToCurrentScore(score, this.m_forPlayer);
        uint hash5 = StringFunctions.StringHash("crit");
        AchievementManager instance = AchievementManager.GetInstance();
        if (!Game2.game_work.hasDroppedFruit)
          instance.UnlockScoreUnsulliedAchievement(Game2.game_work.currentScore);
        instance.UnlockConsecutiveAchievement(Fruit.s_consecutiveCount, Fruit.fruitInfo[(int) this.m_fruitType].hash[0]);
        Game2.game_work.saveData.AddToTotal(Fruit.fruitInfo[(int) this.m_fruitType].fruitName, Fruit.fruitInfo[(int) this.m_fruitType].hash[0], 1, false, false);
        Game2.game_work.saveData.AddToTotal(Fruit.fruitInfo[(int) this.m_fruitType].fruitNameTotal, Fruit.fruitInfo[(int) this.m_fruitType].hash[4], 1, true, false);
        int numberOfCoins = 0;
        if (Fruit.fruitInfo[(int) this.m_fruitType].coinsMax > 0)
          numberOfCoins = Fruit.fruitInfo[(int) this.m_fruitType].coinsMin >= Fruit.fruitInfo[(int) this.m_fruitType].coinsMax ? Fruit.fruitInfo[(int) this.m_fruitType].coinsMin : (int) ((long) Mortar.Math.g_random.Rand32((uint) (Fruit.fruitInfo[(int) this.m_fruitType].coinsMax - Fruit.fruitInfo[(int) this.m_fruitType].coinsMin)) + (long) Fruit.fruitInfo[(int) this.m_fruitType].coinsMin);
        if (this.m_isCritical)
        {
          numberOfCoins *= Fruit.CRITICAL_SCORE / 2;
          Game2.game_work.saveData.AddToTotal("crit", hash5, 1, false, false);
          string str = Fruit.fruitInfo[(int) this.m_fruitType].fruitName + "%scrit";
          uint hash6 = StringFunctions.StringHash(str);
          Game2.game_work.saveData.AddToTotal(str, hash6, 1, false, false);
        }
        if (numberOfCoins > 0)
          Coin.MakeCoins(numberOfCoins, 1, this.m_pos, this.m_slicedAngle, Mortar.Math.DEGREE_TO_IDX((float) Mortar.Math.MIN(360, 45 + numberOfCoins * 45)));
      }
      return false;
    }

    public void Slice()
    {
      this.m_sliceWait = 0.0f;
      ushort num1 = (ushort) ((double) Mortar.Math.MAX((float) Mortar.Math.g_random.Rand32((uint) Fruit.RANDOM_ANGLE_SLICE), (float) (Fruit.RANDOM_ANGLE_SLICE >> 1)) / 5.0);
      ushort num2 = (ushort) ((double) Mortar.Math.MAX((float) Mortar.Math.g_random.Rand32((uint) Fruit.RANDOM_ANGLE_SLICE), (float) (Fruit.RANDOM_ANGLE_SLICE >> 1)) / 5.0);
      Matrix fromQuaternion = Matrix.CreateFromQuaternion(this.m_rotation_piece[0]);
      Vector3 vector3_1 = new Vector3(0.0f, 0.0f, 1f);
      Vector3 vector3_2;
      Vector3.Transform(ref vector3_1, ref fromQuaternion, out vector3_2);
      if (Game2.game_work.gameOver)
        this.m_isCritical = false;
      bool flag1 = false;
      if ((double) Mortar.Math.ABS(vector3_2.Y) + (double) Mortar.Math.ABS(vector3_2.X) > 0.0 && (double) this.GetSmallestDelta((float) (360.0 - (double) (ushort) ((uint) Mortar.Math.Atan2Idx(vector3_2.Y, vector3_2.X) - (uint) Mortar.Math.DEGREE_TO_IDX(90f)) / 182.0), (float) (ushort) ((uint) this.m_slicedAngle - (uint) Mortar.Math.DEGREE_TO_IDX(180f)) / 182f) < 0.0)
        flag1 = true;
      float slicedMag = this.m_slicedMag;
      int num3 = Mortar.Math.g_random.Rand32(2) + 2;
      if (this.m_isCritical)
      {
        GameTask.AddSlice(this.m_pos, (float) (-(double) this.m_slicedAngle / 182.0 + 60.0), (float) ((double) slicedMag * 0.40000000596046448 * 0.699999988079071), true);
        GameTask.AddSlice(this.m_pos, (float) (-(double) this.m_slicedAngle / 182.0 - 60.0), (float) ((double) slicedMag * 0.40000000596046448 * 0.699999988079071), true);
        slicedMag *= 1.5f;
        num3 = Fruit.CRITICAL_SPLATS;
        MissControl.GetFree().MakeCritical(this.m_pos, this.m_forPlayer);
      }
      if (Fruit.fruitInfo[(int) this.m_fruitType].score == 50)
      {
        slicedMag *= 1.5f;
        num3 = Fruit.CRITICAL_SPLATS;
      }
      if (Game2.game_work.inRetrySequence && this.IsOffscreen())
        num3 = 0;
      for (int index = 0; index < num3; ++index)
      {
        ushort idx = (ushort) Mortar.Math.g_random.Rand32((uint) Mortar.Math.DEGREE_TO_IDX(360f));
        float num4 = (float) (((double) Mortar.Math.g_random.RandF(0.5f) + 1.0) * (double) slicedMag * (5.0 + (double) index * 0.20000000298023224));
        SplatEntity free = SplatEntity.GetFree();
        int fruitType = (int) this.m_fruitType;
        if (this.m_isCritical)
          fruitType += Fruit.MAX_FRUIT_TYPES;
        free.MakeSplat(this.m_pos, new Vector3(Mortar.Math.SinIdx(idx) * num4, Mortar.Math.CosIdx(idx) * num4, 0.0f), false, fruitType);
        free.m_vel.Z *= Mortar.Math.CLAMP((float) (1.0 - (double) (index - 2) / (double) num3), 0.3f, 1f);
        if (index > 2)
        {
          free.m_vel.X *= Fruit.CRITICAL_SPLAT_SPREAD;
          free.m_vel.Y *= Fruit.CRITICAL_SPLAT_SPREAD;
          SplatEntity splatEntity = free;
          splatEntity.m_cur_scale = Vector3.Multiply(splatEntity.m_cur_scale, Fruit.CRITICAL_SPLAT_SCALE);
        }
      }
      if (num3 > 0)
      {
        string filename = string.Format("Clean-Slice-{0}", (object) Mortar.Math.g_random.Rand32(3));
        SoundManager.GetInstance().SFXPlay(filename);
      }
      float hitInfluence = Fruit.fruitInfo[(int) this.m_fruitType].hitInfluence;
      ushort num5 = (ushort) ((double) Mortar.Math.MAX((float) Mortar.Math.g_random.Rand32((uint) Fruit.RANDOM_ANGLE_SLICE), (float) (Fruit.RANDOM_ANGLE_SLICE >> 1)) * (1.0 - (double) hitInfluence) * 4.0);
      ushort num6 = (ushort) ((double) Mortar.Math.MAX((float) Mortar.Math.g_random.Rand32((uint) Fruit.RANDOM_ANGLE_SLICE), (float) (Fruit.RANDOM_ANGLE_SLICE >> 1)) * (1.0 - (double) hitInfluence) * 4.0);
      ushort idx1 = (ushort) ((uint) this.m_slicedAngle - (uint) num5);
      ushort idx2 = (ushort) ((uint) this.m_slicedAngle + (uint) num6);
      if (flag1)
      {
        this.m_slicedAngle += Mortar.Math.DEGREE_TO_IDX(180f);
        idx1 = (ushort) ((uint) this.m_slicedAngle - (uint) num6);
        idx2 = (ushort) ((uint) this.m_slicedAngle + (uint) num5);
      }
      float num7;
      float num8;
      if (flag1)
      {
        num7 = Mortar.Math.SinIdx((ushort) ((uint) idx1 + (uint) Mortar.Math.DEGREE_TO_IDX(180f))) * slicedMag;
        num8 = Mortar.Math.CosIdx((ushort) ((uint) idx1 + (uint) Mortar.Math.DEGREE_TO_IDX(180f))) * slicedMag;
      }
      else
      {
        num7 = Mortar.Math.SinIdx(idx2) * slicedMag;
        num8 = Mortar.Math.CosIdx(idx2) * slicedMag;
      }
      this.m_vel2 = Vector3.Add(Vector3.Multiply(new Vector3(num7, num8, 0.0f), hitInfluence), Vector3.Multiply(this.m_vel, 1f - hitInfluence));
      float num9;
      float num10;
      if (flag1)
      {
        num9 = Mortar.Math.SinIdx((ushort) ((uint) idx2 + (uint) Mortar.Math.DEGREE_TO_IDX(180f))) * slicedMag;
        num10 = Mortar.Math.CosIdx((ushort) ((uint) idx2 + (uint) Mortar.Math.DEGREE_TO_IDX(180f))) * slicedMag;
      }
      else
      {
        num9 = Mortar.Math.SinIdx(idx1) * slicedMag;
        num10 = Mortar.Math.CosIdx(idx1) * slicedMag;
      }
      this.m_vel = Vector3.Add(Vector3.Multiply(new Vector3(num9, num10, 0.0f), hitInfluence), Vector3.Multiply(this.m_vel, 1f - hitInfluence));
      if (this.m_isCritical || Fruit.fruitInfo[(int) this.m_fruitType].score == 50)
      {
        this.m_vel2 = Vector3.Multiply(new Vector3(Mortar.Math.SinIdx((ushort) ((uint) this.m_slicedAngle + (uint) Mortar.Math.DEGREE_TO_IDX(90f))) * slicedMag, Mortar.Math.CosIdx((ushort) ((uint) this.m_slicedAngle + (uint) Mortar.Math.DEGREE_TO_IDX(90f))) * slicedMag, 0.0f), 1.75f);
        this.m_vel = Vector3.Multiply(new Vector3(Mortar.Math.SinIdx((ushort) ((uint) this.m_slicedAngle - (uint) Mortar.Math.DEGREE_TO_IDX(90f))) * slicedMag, Mortar.Math.CosIdx((ushort) ((uint) this.m_slicedAngle - (uint) Mortar.Math.DEGREE_TO_IDX(90f))) * slicedMag, 0.0f), 1.75f);
      }
      else
        Game2.MoveFruitZPositionToBack(ref this.m_z);
      this.m_isSliced = true;
      for (int index = 0; index < 2; ++index)
      {
        float num11 = Mortar.Math.ABS(this.m_rotation_speed[index].X) + Mortar.Math.ABS(this.m_rotation_speed[index].Y) + Mortar.Math.ABS(this.m_rotation_speed[index].Z);
        float num12 = !this.m_isCritical ? num11 / 2f : num11 * 2f;
        int num13 = (int) ((double) num12 * ((double) Mortar.Math.g_random.RandF(0.5f) + 0.75));
        int num14 = (int) ((double) num12 * ((double) Mortar.Math.g_random.RandF(0.5f) + 0.75));
        int num15;
        int num16;
        if (flag1)
        {
          if (Mortar.Math.g_random.Rand32(5) > 1)
            num13 = -num13;
          num15 = index == 1 ? -num13 : num13;
          num16 = index == 0 ? -num14 : num14;
        }
        else
        {
          if (Mortar.Math.g_random.Rand32(5) <= 1)
            num13 = -num13;
          num15 = index == 0 ? -num13 : num13;
          num16 = index == 1 ? -num14 : num14;
        }
        bool flag2 = Mortar.Math.g_random.Rand32(3) > 0;
        this.m_rotation_speed[index] = new Vector3(flag2 ? num12 * (Mortar.Math.g_random.RandF(0.3f) - 0.1f) : Mortar.Math.ABS((float) num15 * 1.5f), flag2 ? (float) num15 : 0.0f, (float) -num16);
        this.m_rotation_piece[index] = Quaternion.CreateFromAxisAngle(new Vector3(0.0f, 1f, 0.0f), Mortar.Math.IDX_TO_RADIANS(Mortar.Math.DEGREE_TO_IDX(-90f)));
        Quaternion fromAxisAngle = Quaternion.CreateFromAxisAngle(new Vector3(0.0f, 0.0f, 1f), -Mortar.Math.IDX_TO_RADIANS(this.m_slicedAngle));
        this.m_rotation_piece[index] = Quaternion.Multiply(fromAxisAngle, this.m_rotation_piece[index]);
      }
    }

    public void Disable() => this.m_disabled = true;

    public int GetFruitType() => (int) this.m_fruitType;

    public void SetFruitType(int type) => this.SetFruitType(type, 1f);

    public void SetFruitType(int type, float scaler)
    {
      this.m_fruitType = (byte) type;
      this.m_cur_scale = Vector3.Multiply(Vector3.Multiply(Vector3.Multiply(Vector3.One, Fruit.fruitInfo[(int) this.m_fruitType].scale), 0.01f), scaler);
      this.m_orig_scale = this.m_cur_scale;
      if ((double) Fruit.fruitInfo[(int) this.m_fruitType].scale * 0.5 + (double) Fruit.fruitInfo[(int) this.m_fruitType].collision > 0.0)
      {
        if (this.m_col_box == null)
          this.m_col_box = (Col) new ColSphere();
        this.m_col_box.centre = new Vector3(this.m_pos.X, this.m_pos.Y, 0.0f);
        ((ColSphere) this.m_col_box).Radius = (Fruit.fruitInfo[(int) this.m_fruitType].scale * 0.5f + Fruit.fruitInfo[(int) this.m_fruitType].collision) * scaler;
      }
      else
        this.m_col_box = (Col) null;
    }

    public void EnableGravity(bool state) => this.m_gravityEnabled = state;

    public bool GravityEnabled() => this.m_gravityEnabled;

    private float GetSmallestDelta(float to, float from)
    {
      float t = to - from;
      if ((double) Mortar.Math.Abs(t) > 180.0)
        t = (double) to > (double) from ? t - 360f : to + 360f - from;
      return t;
    }

    public bool CollideWithFruit() => false;

    public bool CheckHasGoneOffsceen()
    {
      bool isSliced = this.m_isSliced;
      if (isSliced && (double) Mortar.Math.Abs(this.m_gravity.X) > 0.0 && ((double) this.m_pos.Y <= -((double) Game2.SCREEN_HEIGHT / 2.0 + (double) this.m_cur_scale.Y * 50.0) || (double) this.m_pos.Y >= (double) Game2.SCREEN_HEIGHT / 2.0 + (double) this.m_cur_scale.Y * 50.0) && ((double) this.m_pos2.Y <= -((double) Game2.SCREEN_HEIGHT / 2.0 + (double) this.m_cur_scale.Y * 50.0) || (double) this.m_pos2.Y >= (double) Game2.SCREEN_HEIGHT / 2.0 + (double) this.m_cur_scale.Y * 50.0))
        return true;
      bool flag = false;
      if ((double) this.m_gravity.Y < 0.0)
      {
        if ((double) this.m_pos.Y > (double) Game2.SCREEN_HEIGHT * 0.75 && isSliced)
        {
          this.m_pos.Y = -Game2.SCREEN_HEIGHT;
          this.m_vel.Y = -1f;
        }
        if ((double) this.m_pos2.Y > (double) Game2.SCREEN_HEIGHT * 0.75 && isSliced)
        {
          this.m_pos2.Y = -Game2.SCREEN_HEIGHT;
          this.m_vel2.Y = -1f;
        }
        if ((double) this.m_pos.Y <= -((double) Game2.SCREEN_HEIGHT / 2.0 + (double) this.m_cur_scale.Y * 50.0) && (double) this.m_vel.Y < 0.0)
          flag = true;
        if (flag && (double) this.m_sliceWait <= 0.0 && (double) this.m_pos2.Y <= -((double) Game2.SCREEN_HEIGHT / 2.0 + (double) this.m_cur_scale.Y * 50.0) && (double) this.m_vel2.Y < 0.0 || isSliced && ((double) this.m_pos.X <= -((double) Game2.SCREEN_WIDTH / 2.0 + (double) this.m_cur_scale.Y * 50.0) || (double) this.m_pos.X >= (double) Game2.SCREEN_WIDTH / 2.0 + (double) this.m_cur_scale.Y * 50.0) && ((double) this.m_pos2.X <= -((double) Game2.SCREEN_WIDTH / 2.0 + (double) this.m_cur_scale.Y * 50.0) || (double) this.m_pos2.X >= (double) Game2.SCREEN_WIDTH / 2.0 + (double) this.m_cur_scale.Y * 50.0))
          return true;
      }
      if ((double) this.m_gravity.Y > 0.0)
      {
        if ((double) this.m_pos.Y < -(double) Game2.SCREEN_HEIGHT * 0.75 && isSliced)
        {
          this.m_pos.Y = Game2.SCREEN_HEIGHT;
          this.m_vel.Y = 1f;
        }
        if ((double) this.m_pos2.Y < -(double) Game2.SCREEN_HEIGHT * 0.75 && isSliced)
        {
          this.m_pos2.Y = Game2.SCREEN_HEIGHT;
          this.m_vel2.Y = 1f;
        }
        if ((double) this.m_pos.Y >= (double) Game2.SCREEN_HEIGHT / 2.0 + (double) this.m_cur_scale.Y * 50.0 && (double) this.m_vel.Y > 0.0)
          flag = true;
        if (flag && (double) this.m_sliceWait <= 0.0 && (double) this.m_pos2.Y >= (double) Game2.SCREEN_HEIGHT / 2.0 + (double) this.m_cur_scale.Y * 50.0 && (double) this.m_vel2.Y > 0.0 || isSliced && ((double) this.m_pos.X <= -((double) Game2.SCREEN_WIDTH / 2.0 + (double) this.m_cur_scale.Y * 50.0) || (double) this.m_pos.X >= (double) Game2.SCREEN_WIDTH / 2.0 + (double) this.m_cur_scale.Y * 50.0) && ((double) this.m_pos2.X <= -((double) Game2.SCREEN_WIDTH / 2.0 + (double) this.m_cur_scale.Y * 50.0) || (double) this.m_pos2.X >= (double) Game2.SCREEN_WIDTH / 2.0 + (double) this.m_cur_scale.Y * 50.0))
          return true;
      }
      if ((double) this.m_gravity.X < 0.0)
      {
        if (isSliced && (double) this.m_pos.X > (double) Game2.SCREEN_WIDTH * 0.75)
        {
          this.m_pos.X = -Game2.SCREEN_WIDTH;
          this.m_vel.X = -1f;
        }
        if (isSliced && (double) this.m_pos2.X > (double) Game2.SCREEN_WIDTH * 0.75)
        {
          this.m_pos2.X = -Game2.SCREEN_WIDTH;
          this.m_vel2.X = -1f;
        }
        if ((double) this.m_pos.X <= -((double) Game2.SCREEN_WIDTH / 2.0 + (double) this.m_cur_scale.Y * 50.0) && (double) this.m_vel.X < 0.0)
          flag = true;
        if (flag && (double) this.m_sliceWait <= 0.0 && (double) this.m_pos2.X <= -((double) Game2.SCREEN_WIDTH / 2.0 + (double) this.m_cur_scale.Y * 50.0) && (double) this.m_vel2.X < 0.0)
          return true;
      }
      if ((double) this.m_gravity.X > 0.0)
      {
        if (isSliced && (double) this.m_pos.X < -(double) Game2.SCREEN_WIDTH * 0.75)
        {
          this.m_pos.X = Game2.SCREEN_WIDTH;
          this.m_vel.X = 1f;
        }
        if (isSliced && (double) this.m_pos2.X < -(double) Game2.SCREEN_WIDTH * 0.75)
        {
          this.m_pos2.X = Game2.SCREEN_WIDTH;
          this.m_vel2.X = 1f;
        }
        if ((double) this.m_pos.X >= (double) Game2.SCREEN_WIDTH / 2.0 + (double) this.m_cur_scale.Y * 50.0 && (double) this.m_vel.X > 0.0)
          flag = true;
        if (flag && (double) this.m_sliceWait <= 0.0 && (double) this.m_pos2.X >= (double) Game2.SCREEN_WIDTH / 2.0 + (double) this.m_cur_scale.Y * 50.0 && (double) this.m_vel2.X > 0.0)
          return true;
      }
      return false;
    }

    public static int FruitType(string type) => Fruit.FruitType(type, false);

    public static int FruitType(string type, bool returnRandIfFalse)
    {
      if (type != null && type.Length > 0)
      {
        uint num = StringFunctions.StringHash(type);
        for (int index = 0; index < Fruit.MAX_FRUIT_TYPES; ++index)
        {
          if ((int) Fruit.fruitInfo[index].hash[0] == (int) num || (int) Fruit.fruitInfo[index].hash[1] == (int) num)
            return index;
        }
      }
      return !returnRandIfFalse ? -1 : (int) Mortar.Math.g_random.Rand32((uint) (Fruit.MAX_FRUIT_TYPES - 1));
    }

    public static int RandomFruit(bool canUseCoiners)
    {
      if (Fruit.totalChance <= 0)
      {
        Fruit.totalChance = 0;
        for (int index = 0; index < Fruit.MAX_FRUIT_TYPES; ++index)
        {
          Fruit.totalChance += Fruit.fruitInfo[index].chance;
          Fruit.fruitInfo[index].totalChance = Fruit.totalChance;
          if (Fruit.fruitInfo[index].coinsMax <= 0)
            Fruit.totalChanceNonCoined += Fruit.fruitInfo[index].chance;
          if (Fruit.fruitInfo[index].canBeCritical)
          {
            Fruit.totalChanceOfCritical += Fruit.fruitInfo[index].chance;
            if (Fruit.fruitInfo[index].coinsMax <= 0)
              Fruit.totalChanceOfCriticalNonCoined += Fruit.fruitInfo[index].chance;
          }
          Fruit.fruitInfo[index].totalChanceOfCritical = Fruit.totalChanceOfCritical;
        }
      }
      if (WaveManager.GetInstance().CriticalMode())
      {
        if (canUseCoiners)
        {
          int num = (int) Mortar.Math.g_random.Rand32((uint) Fruit.totalChanceOfCritical);
          for (int index = 0; index < Fruit.MAX_FRUIT_TYPES; ++index)
          {
            if (num < Fruit.fruitInfo[index].totalChanceOfCritical)
              return index;
          }
        }
        else
        {
          int num1 = 0;
          int num2 = Mortar.Math.g_random.Rand32(Fruit.totalChanceOfCriticalNonCoined);
          for (int index = 0; index < Fruit.MAX_FRUIT_TYPES; ++index)
          {
            if (Fruit.fruitInfo[index].coinsMax <= 0 && Fruit.fruitInfo[index].canBeCritical)
            {
              num1 += Fruit.fruitInfo[index].chance;
              if (num2 < num1)
                return index;
            }
          }
        }
      }
      else if (!canUseCoiners)
      {
        int num3 = 0;
        int num4 = Mortar.Math.g_random.Rand32(Fruit.totalChanceNonCoined);
        for (int index = 0; index < Fruit.MAX_FRUIT_TYPES; ++index)
        {
          if (Fruit.fruitInfo[index].coinsMax <= 0)
          {
            num3 += Fruit.fruitInfo[index].chance;
            if (num4 < num3)
              return index;
          }
        }
      }
      else
      {
        int num = Mortar.Math.g_random.Rand32(Fruit.totalChance);
        for (int index = 0; index < Fruit.MAX_FRUIT_TYPES; ++index)
        {
          if (num < Fruit.fruitInfo[index].totalChance)
            return index;
        }
      }
      return Mortar.Math.g_random.Rand32(Fruit.MAX_FRUIT_TYPES - 1);
    }

    public static uint FruitTypeHash(int type) => Fruit.fruitInfo[type].hash[0];

    public static string FruitTypeName(int type) => Fruit.fruitInfo[type].fruitName;

    public static Color FruitTypeColor(int type)
    {
      return Fruit.MAX_FRUIT_TYPES == type ? Fruit.CRITICAL_COLOUR : Fruit.fruitInfo[type].splatColor;
    }

    public static Color FruitFactColor(int type) => Fruit.fruitInfo[type].factColor;

    public static string FruitFactTexture(int type) => Fruit.fruitInfo[type].factTextureName;

    public static FRUIT_INFO FruitInfo(int type) => Fruit.fruitInfo[type];

    public static bool LoadInfo()
    {
      if (Fruit.fruitInfo != null)
        return false;
      XDocument xdocument = MortarXml.Load("xmlwp7/fruitList.xml");
      if (xdocument != null)
      {
        XElement xelement = xdocument.Element((XName) "fruitInfoFile");
        XElement element1 = xelement.Element((XName) "critical");
        if (element1 != null)
        {
          XAttribute xattribute = element1.Attribute((XName) "colour");
          if (xattribute != null && xattribute.Value.Length > 0)
          {
            string str = xattribute.Value;
            int[] numArray = new int[4]
            {
              (int) byte.MaxValue,
              (int) byte.MaxValue,
              (int) byte.MaxValue,
              (int) byte.MaxValue
            };
            int startIndex = 0;
            for (int index = 0; index < 4; ++index)
            {
              int num = str.IndexOf(',', startIndex);
              if (num == -1)
                num = str.Length;
              numArray[index] = MParser.ParseInt(str.Substring(startIndex, num - startIndex).Trim());
              if (num != str.Length)
                startIndex = num + 1;
              else
                break;
            }
            Fruit.CRITICAL_COLOUR.A = (byte) numArray[0];
            Fruit.CRITICAL_COLOUR.R = (byte) numArray[1];
            Fruit.CRITICAL_COLOUR.G = (byte) numArray[2];
            Fruit.CRITICAL_COLOUR.B = (byte) numArray[3];
          }
          element1.QueryIntAttribute("new_life_at", ref Fruit.NEW_LIFE_AT);
          element1.QueryIntAttribute("score", ref Fruit.CRITICAL_SCORE);
          element1.QueryIntAttribute("chance", ref Fruit.CRITICAL_CHANCE);
          element1.QueryIntAttribute("chance_inc", ref Fruit.CRITICAL_CHANCE_START_INC);
          element1.QueryIntAttribute("splats", ref Fruit.CRITICAL_SPLATS);
          element1.QueryFloatAttribute("scale", ref Fruit.CRITICAL_SPLAT_SCALE);
          element1.QueryFloatAttribute("spread", ref Fruit.CRITICAL_SPLAT_SPREAD);
          element1.QueryFloatAttribute("disappear_speed", ref Fruit.CRITICAL_DISAPPEAR_SPEED);
        }
        XElement element2 = xelement.Element((XName) "bomb");
        if (element2 != null)
        {
          string astr1 = element2.AttributeStr("size");
          if (astr1 != null && astr1.Length > 0)
            Game2.game_work.bombSize = (float) MParser.ParseInt(astr1);
          string astr2 = element2.AttributeStr("collision");
          if (astr2 != null && astr2.Length > 0)
            Game2.game_work.bombCollision = (float) MParser.ParseInt(astr2);
        }
        XElement element3 = xelement.Element((XName) "FruitInfo");
        Fruit.MAX_FRUIT_TYPES = 0;
        for (; element3 != null; element3 = element3.NextSiblingElement("FruitInfo"))
          ++Fruit.MAX_FRUIT_TYPES;
        Fruit.fruitInfo = ArrayInit.CreateFilledArray<FRUIT_INFO>(Fruit.MAX_FRUIT_TYPES);
        XElement element4 = xdocument.Element((XName) "fruitInfoFile").Element((XName) "FruitInfo");
        int index1 = 0;
        Fruit.fruitInfo[index1].icon = (Texture) null;
        for (; element4 != null; element4 = element4.NextSiblingElement("FruitInfo"))
        {
          FRUIT_INFO fruitInfo = Fruit.fruitInfo[index1];
          string str1 = element4.AttributeStr("name");
          if (str1 != null)
          {
            fruitInfo.fruitName = str1;
            fruitInfo.hash[0] = StringFunctions.StringHash(fruitInfo.fruitName);
            string str2 = fruitInfo.fruitName.Substring(0, 1);
            string str3 = fruitInfo.fruitName.Substring(1);
            fruitInfo.hash[1] = StringFunctions.StringHash(str2.ToUpper() + str3);
            string s1 = fruitInfo.fruitName + "_trail";
            fruitInfo.hash[2] = StringFunctions.StringHash(s1);
            string s2 = fruitInfo.fruitName + "_sliced";
            fruitInfo.hash[3] = StringFunctions.StringHash(s2);
            fruitInfo.fruitNameTotal = fruitInfo.fruitName + "_total";
            fruitInfo.hash[4] = StringFunctions.StringHash(fruitInfo.fruitNameTotal);
            fruitInfo.fruitNamePointTotal = fruitInfo.fruitName + "_point_total";
            fruitInfo.hash[5] = StringFunctions.StringHash(fruitInfo.fruitNamePointTotal);
            string str4 = "textureswp7/hud_" + str1 + ".tex";
            if (MortarFile.Exists(str4))
              fruitInfo.icon = TextureManager.GetInstance().Load(str4);
            string str5 = "textureswp7/zen_" + str1 + ".tex";
            if (MortarFile.Exists(str5))
              fruitInfo.zenIcon = TextureManager.GetInstance().Load(str5);
          }
          string str6 = element4.AttributeStr("plural");
          fruitInfo.fruitPlural = str6 == null ? fruitInfo.fruitName + "s" : str6;
          string str7 = element4.AttributeStr("singular");
          fruitInfo.fruitSingular = str7 == null ? fruitInfo.fruitName : str7;
          string str8 = element4.AttributeStr("factTexture");
          if (str8 != null)
            fruitInfo.factTextureName = str8;
          string str9 = element4.AttributeStr("modelName");
          fruitInfo.modelName = str9 == null ? fruitInfo.fruitName : str9;
          string str10 = element4.AttributeStr("colour");
          if (str10 != null)
          {
            int[] numArray = new int[4]
            {
              (int) byte.MaxValue,
              (int) byte.MaxValue,
              (int) byte.MaxValue,
              (int) byte.MaxValue
            };
            int startIndex = 0;
            for (int index2 = 0; index2 < 4; ++index2)
            {
              int num = str10.IndexOf(',', startIndex);
              if (num == -1)
                num = str10.Length;
              numArray[index2] = MParser.ParseInt(str10.Substring(startIndex, num - startIndex).Trim());
              if (num != str10.Length)
                startIndex = num + 1;
              else
                break;
            }
            fruitInfo.splatColor.R = (byte) numArray[0];
            fruitInfo.splatColor.G = (byte) numArray[1];
            fruitInfo.splatColor.B = (byte) numArray[2];
            fruitInfo.splatColor.A = (byte) numArray[3];
            if ( fruitInfo.splatColor.A == (byte) 0 )
              fruitInfo.canBeCritical = false;
          }
          string str11 = element4.AttributeStr("factColour");
          if (str11 != null)
          {
            int[] numArray = new int[3]
            {
              (int) byte.MaxValue,
              (int) byte.MaxValue,
              (int) byte.MaxValue
            };
            int startIndex = 0;
            for (int index3 = 0; index3 < 3; ++index3)
            {
              int num = str11.IndexOf(',', startIndex);
              if (num == -1)
                num = str11.Length;
              numArray[index3] = MParser.ParseInt(str11.Substring(startIndex, num - startIndex).Trim());
              if (num != str11.Length)
                startIndex = num + 1;
              else
                break;
            }
            fruitInfo.factColor.R = (byte) numArray[0];
            fruitInfo.factColor.G = (byte) numArray[1];
            fruitInfo.factColor.B = (byte) numArray[2];
            fruitInfo.factColor.A = byte.MaxValue;
          }
          fruitInfo.scale = 25f;
          fruitInfo.collision = 1f;
          fruitInfo.chance = 0;
          fruitInfo.hitInfluence = 0.75f;
          element4.QueryFloatAttribute("scale", ref fruitInfo.scale);
          element4.QueryFloatAttribute("collision", ref fruitInfo.collision);
          element4.QueryFloatAttribute("hitInfluence", ref fruitInfo.hitInfluence);
          element4.QueryIntAttribute("chance", ref fruitInfo.chance);
          element4.QueryIntAttribute("score", ref fruitInfo.score);
          element4.QueryIntAttribute("coins", ref fruitInfo.coinsMin);
          fruitInfo.coinsMax = fruitInfo.coinsMin;
          element4.QueryIntAttribute("coinsMin", ref fruitInfo.coinsMin);
          element4.QueryIntAttribute("coinsMax", ref fruitInfo.coinsMax);
          int num1 = 0;
          element4.QueryIntAttribute("hasSplatSeeds", ref num1);
          fruitInfo.hasSplatSeeds = num1 == 1;
          if (fruitInfo.score >= Fruit.CRITICAL_SCORE)
            fruitInfo.canBeCritical = false;
          string str12 = element4.AttributeStr("onside");
          string str13 = str12 == null ? element4.AttributeStr("onSide") : str12;
          fruitInfo.onSide = str13 != null && "true" == str13;
          string str14 = element4.AttributeStr("onlySprinkle");
          string str15 = str14 == null ? element4.AttributeStr("onlySprinkle") : str14;
          fruitInfo.onlySprinkle = str15 != null && "true" == str15;
          string str16 = element4.AttributeStr("noCritical");
          fruitInfo.canBeCritical = str16 != null ? "true" == str16 : fruitInfo.canBeCritical;
          fruitInfo.factCount = 0;
          fruitInfo.facts = (FruitFact[]) null;
          for (XElement element5 = element4.Element((XName) "fact"); element5 != null; element5 = element5.NextSiblingElement("fact"))
            ++fruitInfo.factCount;
          if (fruitInfo.factCount > 0)
          {
            fruitInfo.facts = ArrayInit.CreateFilledArray<FruitFact>(fruitInfo.factCount);
            int index4 = 0;
            for (XElement element6 = element4.Element((XName) "fact"); element6 != null; element6 = element6.NextSiblingElement("fact"))
            {
              fruitInfo.facts[index4].text = element6.GetText();
              ++index4;
            }
          }
          fruitInfo.numImpactSounds = 0;
          for (XElement element7 = element4.Element((XName) "impact_sound"); element7 != null; element7 = element7.NextSiblingElement("impact_sound"))
            ++fruitInfo.numImpactSounds;
          if (fruitInfo.numImpactSounds > 0)
          {
            fruitInfo.impactSounds = ArrayInit.CreateFilledArray<ImpactSound>(fruitInfo.numImpactSounds);
            int index5 = 0;
            ImpactSound impactSound1 = fruitInfo.impactSounds[index5];
            int num2 = 0;
            for (XElement element8 = element4.Element((XName) "impact_sound"); element8 != null; element8 = element8.NextSiblingElement("impact_sound"))
            {
              ImpactSound impactSound2 = fruitInfo.impactSounds[index5];
              element8.QueryIntAttribute("chance", ref impactSound2.chance);
              impactSound2.chanceTotal = (num2 += impactSound2.chance);
              string text = element8.GetText();
              impactSound2.file = text;
              ++index5;
            }
          }
          else
          {
            fruitInfo.numImpactSounds = 1;
            fruitInfo.impactSounds = ArrayInit.CreateFilledArray<ImpactSound>(1);
            fruitInfo.impactSounds[0].file = "Impact-" + fruitInfo.fruitName.Substring(0, 1).ToLower() + fruitInfo.fruitName.Substring(1);
          }
          int size = 0;
          for (XElement element9 = element4.FirstChildElement("power"); element9 != null; element9 = element9.NextSiblingElement("power"))
            ++size;
          if (size > 0)
          {
            fruitInfo.powers = new FRUIT_POWERS();
            fruitInfo.powers.numPowerUpTypes = size;
            fruitInfo.powers.powerUps = ArrayInit.CreateFilledArray<FRUIT_POWER>(size);
            int index6 = 0;
            int num3 = 0;
            for (XElement element10 = element4.FirstChildElement("power"); element10 != null; element10 = element10.NextSiblingElement("power"))
            {
              FRUIT_POWER powerUp = fruitInfo.powers.powerUps[index6];
              string s = element10.AttributeStr("name");
              if (s != null)
                powerUp.powerHash = StringFunctions.StringHash(s);
              element10.QueryIntAttribute("chance", ref powerUp.chance);
              num3 += powerUp.chance;
              powerUp.totalChance = num3;
              ++index6;
            }
          }
          ++index1;
        }
      }
      if (Fruit.fruitInfo != null)
      {
        for (int index7 = 0; index7 < 2; ++index7)
        {
          Fruit.s_fruitModels[index7] = ArrayInit.CreateInitedList<Model>(Fruit.MAX_FRUIT_TYPES);
          for (int index8 = 0; index8 < Fruit.MAX_FRUIT_TYPES; ++index8)
          {
            string loaddasdas = string.Format("models/Fruit/{0}_{1}_piece_{2}.mmd", (object) Fruit.fruitInfo[index8].modelName, (object) Fruit.fruitInfo[index8].modelName[0], (object) (index7 + 1));
            Fruit.s_fruitModels[index7][index8] = MeshManager.GetInstance().Load(loaddasdas);
          }
        }
        Fruit.s_fruitModels[2] = ArrayInit.CreateInitedList<Model>(Fruit.MAX_FRUIT_TYPES);
        for (int index = 0; index < Fruit.MAX_FRUIT_TYPES; ++index)
        {
          string str = string.Format("models/Fruit/{0}_single.mmd", (object) Fruit.fruitInfo[index].modelName);
          Fruit.s_fruitModels[2][index] = !MortarFile.Exists(str) ? (Model) null : MeshManager.GetInstance().Load(str);
        }
        Fruit.s_fruitModelsLoaded = true;
      }
      return true;
    }

    public bool IsActive() => (double) this.m_chuckWait <= 0.0;

    public void Chuck() => this.Chuck(0.0f);

    public void Chuck(float chuck)
    {
      if ((double) chuck < 0.0)
        chuck = Fruit.FRUIT_CHUCK_WAIT;
      this.m_pos2 = this.m_pos;
      this.m_chuckWait = chuck;
      uint num = StringFunctions.StringHash("freeze");
      if (Fruit.fruitInfo[(int) this.m_fruitType].powers == null || (double) Game2.game_work.saveData.timer - (double) chuck >= 8.0 || (int) Fruit.fruitInfo[(int) this.m_fruitType].powers.powerUps[0].powerHash == (int) num)
        return;
      --Fruit.s_numActivePowerUpFruits;
      this.m_destroy = true;
    }

    public bool IsOffscreen()
    {
      bool flag = false;
      if ((double) Mortar.Math.Abs(this.m_gravity.Y) > 0.0)
      {
        if ((double) this.m_pos.Y < -((double) Game2.SCREEN_HEIGHT / 2.0 + (double) this.m_cur_scale.Y * 50.0) || (double) this.m_pos.Y > (double) Game2.SCREEN_HEIGHT / 2.0 + (double) this.m_cur_scale.Y * 50.0 || (double) this.m_pos2.Y < -((double) Game2.SCREEN_HEIGHT / 2.0 + (double) this.m_cur_scale.Y * 50.0) || (double) this.m_pos2.Y > (double) Game2.SCREEN_HEIGHT / 2.0 + (double) this.m_cur_scale.Y * 50.0)
          flag = true;
      }
      else if ((double) Mortar.Math.Abs(this.m_gravity.X) > 0.0 && ((double) this.m_pos.X < -((double) Game2.SCREEN_WIDTH / 2.0 + (double) this.m_cur_scale.Y * 50.0) || (double) this.m_pos.X > (double) Game2.SCREEN_WIDTH / 2.0 + (double) this.m_cur_scale.Y * 50.0 || (double) this.m_pos2.X < -((double) Game2.SCREEN_WIDTH / 2.0 + (double) this.m_cur_scale.Y * 50.0) || (double) this.m_pos2.X > (double) Game2.SCREEN_WIDTH / 2.0 + (double) this.m_cur_scale.Y * 50.0))
        flag = true;
      return flag;
    }

    public float GetWait() => this.m_chuckWait;

    public bool Sliced() => this.m_isSliced || (double) this.m_sliceWait > -1.0;

    public static string GetFact(Action<int> getType, Action<int> getFact)
    {
      return Fruit.GetFact(getType, getFact, -1);
    }

    public static string GetFact(Action<int> getType, Action<int> getFact, int type)
    {
      return Fruit.GetFact(getType, getFact, type, -1);
    }

    public static string GetFact(Action<int> getType, Action<int> getFact, int type, int fact)
    {
      if (type < 0)
        type = Mortar.Math.g_random.Rand32(Fruit.MAX_FRUIT_TYPES);
      type = Mortar.Math.CLAMP(type, 0, Fruit.MAX_FRUIT_TYPES - 2);
      if (type == Fruit.redApple)
        type = Fruit.apple;
      if (getType != null)
        getType(type);
      if (fact < 0)
      {
        if (Fruit.fruitInfo[type].factCount <= 0)
          return Fruit.GetFact(getType, getFact);
        string str = Fruit.fruitInfo[type].fruitName + "_facts";
        fact = Game2.game_work.saveData.AddToTotal(str, StringFunctions.StringHash(str), 1, true, true) - 1;
        fact %= Fruit.fruitInfo[type].factCount;
      }
      fact = Mortar.Math.CLAMP(fact, 0, Fruit.fruitInfo[type].factCount - 1);
      if (getFact != null)
        getFact(fact);
      string text = Fruit.fruitInfo[type].facts[fact].text;
      return Mortar.Game1.instance.stringTable.GetString(text);
    }

    public int ForPlayer() => this.m_forPlayer;

    public void SetForPlayer(int forPlayer) => this.m_forPlayer = forPlayer;

    public static int GetNumActiveForPlayer(int player)
    {
      return Fruit.GetNumActiveForPlayer(player, true);
    }

    public static int GetNumActiveForPlayer(int player, bool waitForDeath)
    {
      LinkedListNode<Entity> iterator = (LinkedListNode<Entity>) null;
      Fruit fruit = (Fruit) ActorManager.GetInstance().GetEntityFirst(EntityTypes.ENTITY_BEGIN, ref iterator);
      int numActiveForPlayer = 0;
      if (waitForDeath)
      {
        for (; fruit != null; fruit = (Fruit) ActorManager.GetInstance().GetEntityNext(EntityTypes.ENTITY_BEGIN, ref iterator))
        {
          if (player == fruit.ForPlayer())
            ++numActiveForPlayer;
        }
      }
      else
      {
        for (; fruit != null; fruit = (Fruit) ActorManager.GetInstance().GetEntityNext(EntityTypes.ENTITY_BEGIN, ref iterator))
        {
          if (!fruit.IsActive())
            ++numActiveForPlayer;
        }
      }
      return numActiveForPlayer;
    }

    public static void ClearUnspawned()
    {
      LinkedListNode<Entity> iterator = (LinkedListNode<Entity>) null;
      for (Fruit fruit = (Fruit) ActorManager.GetInstance().GetEntityFirst(EntityTypes.ENTITY_BEGIN, ref iterator); fruit != null; fruit = (Fruit) ActorManager.GetInstance().GetEntityNext(EntityTypes.ENTITY_BEGIN, ref iterator))
      {
        if ((double) fruit.GetWait() > 0.0)
          fruit.KillFruit();
      }
    }

    public static void CleanupFruit()
    {
      if (Fruit.s_fruitModelsLoaded)
      {
        for (int index1 = 0; index1 < 3; ++index1)
        {
          for (int index2 = 0; index2 < Fruit.MAX_FRUIT_TYPES; ++index2)
            Fruit.s_fruitModels[index1][index2] = (Model) null;
          Fruit.s_fruitModels[index1] = (List<Model>) null;
        }
      }
      Fruit.fruitInfo = (FRUIT_INFO[]) null;
    }

    public static void RandomStartAngle(ref Quaternion q) => Fruit.RandomStartAngle(ref q, false);

    public static void RandomStartAngle(ref Quaternion q, bool faceUp)
    {
      if (faceUp)
      {
        q = Quaternion.CreateFromAxisAngle(new Vector3(-1f, 0.0f, 0.0f), (float) Mortar.Math.DEGREE_TO_IDX(290f));
      }
      else
      {
        Vector3 vector3 = new Vector3(Mortar.Math.g_random.RandF(2f) - 1f, Mortar.Math.g_random.RandF(2f) - 1f, Mortar.Math.g_random.RandF(2f) - 1f);
        ushort num = (ushort) Mortar.Math.g_random.Rand32((int) Mortar.Math.DEGREE_TO_IDX(359f));
        vector3.Normalize();
        q = Quaternion.CreateFromAxisAngle(new Vector3(vector3.X, vector3.Y, vector3.Z), (float) num);
      }
    }

    public enum FRUIT_PEICE
    {
      FRUIT_PEICE_TOP,
      FRUIT_PEICE_BOTTOM,
      MAX_FRUIT_PIECES,
    }
  }
}
