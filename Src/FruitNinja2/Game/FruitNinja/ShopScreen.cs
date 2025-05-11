
// Type: GameManager.ShopScreen
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Mortar;

#nullable disable
namespace GameManager
{
  public class ShopScreen : HUDControl3d
  {
    protected float m_time;
    protected int m_backDrawOrder;
    protected MenuButton m_quitButton;
    protected float m_buyButtonWait;
    protected MenuButton m_buyButton;
    protected DojoScreen m_dojoScreen;
    protected ScrollingMenu m_scrollingMenu;
    protected ShopListItem m_selectedItem;
    protected ShopListItem[] m_equipedListItems = new ShopListItem[2];
    protected float coins;
    protected ItemInfo m_previousItem;
    protected int m_selectedScaleInOut;
    public static float ScrollOffset;
    public int m_state;
    private static bool hackedOpen = false;
    private static int c = 0;
    private static float m_fadeTime = 0.0f;

    public static float SHOP_SCREEN_HEIGHT => 320f;

    public static float SHOP_BACK_LEFT_SIDE => 290f;

    public static float SHOP_BACK_RIGHT_SIDE => Game.SCREEN_WIDTH - 290f;

    public static float SHOP_LIST_FROM_LEFT => ShopScreen.SHOP_BACK_LEFT_SIDE / 2f;

    public static float SHOP_LIST_POS_X
    {
      get => (float) (-(double) Game.SCREEN_WIDTH / 2.0) + ShopScreen.SHOP_LIST_FROM_LEFT;
    }

    public static float QUIT_Y => (float) (-((double) Game.SCREEN_HEIGHT / 2.0) + 55.0);

    public static float WATERMELON_SELECT_SCALE => 0.75f;

    public static float DESCRIPTION_BOX_WIDTH => 160f;

    public static float POP_IN_OUT_TIME => 0.15f;

    public static float POP_SINE => (float) Math.DEGREE_TO_IDX(90f);

    public static float SHAKE_TIME => 0.25f;

    public static float SHAKE_RANGE => 5f;

    public static float BUY_NOW_X => 50f;

    public static float BUY_NOW_Y => (float) ((double) Game.SCREEN_HEIGHT / 2.0 - 56.0);

    public static float DARKNESS_HEIGHT => 120f;

    public static float SHOP_TEXT_SCALE => 0.75f;

    public static float SHOP_ITEM_HEIGHT => 80f;

    public static float ITEM_ICON_SCALE => 1f;

    public ShopScreen(DojoScreen dojoScreen)
    {
      if (!ShopListItem.s_isContentLoaded)
        ShopScreen.LoadContent();
      this.m_buyButton = (MenuButton) null;
      this.m_deleteCall = (HUDControl.HUDControlDeletedCallback) (l => l.Release());
      this.m_dojoScreen = dojoScreen;
      this.m_texture = (Texture) null;
      this.m_selectedItem = (ShopListItem) null;
      this.m_scrollingMenu = (ScrollingMenu) null;
      this.m_selfCleanUp = false;
      this.m_quitButton = (MenuButton) null;
      this.m_state = 0;
      this.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_AFTER_SPLAT;
      this.m_time = 0.0f;
      this.m_buyButtonWait = 0.0f;
      this.m_selectedScaleInOut = 0;
      this.coins = (float) Game.game_work.coins + 0.5f;
    }

    public void QuitShopCallback()
    {
      SoundManager.GetInstance().SFXPlay(SoundDef.SND_MENU_BOMB);
      this.m_state = 2;
      ((Bomb) this.m_quitButton.m_entity).EnableGravity(true);
      this.m_quitButton.m_entity.m_vel = new Vector3(Math.g_random.RandF(5f) + 5f, -Math.g_random.RandF(5f), 0.0f);
      Game.game_work.tutorialControl.ResetTutePos();
    }

    public void BuyButtonCallback()
    {
      if (this.m_selectedItem == null || this.m_selectedItem.m_info == null)
        return;
      ItemType type = this.m_selectedItem.m_info.type;
      if (this.m_selectedItem.m_info.IsLocked())
      {
        ItemManager.GetInstance().BuyItem(this.m_selectedItem.m_info.nameHash);
        this.m_selectedItem.m_text = "BOUGHT";
      }
      else if (ItemManager.GetInstance().IsEquipped(this.m_selectedItem.m_info))
      {
        ItemManager.GetInstance().SetEquippedItem(type, (ItemInfo) null);
        if (this.m_selectedItem.m_info.cost == 0)
          this.m_selectedItem.m_text = "FREE";
        else
          this.m_selectedItem.m_text = "BOUGHT";
        this.m_equipedListItems[(int) type] = (ShopListItem) null;
      }
      else
      {
        if (this.m_equipedListItems[(int) type] != null)
        {
          if (this.m_equipedListItems[(int) type].m_info.cost == 0)
            this.m_equipedListItems[(int) type].m_text = "FREE";
          else
            this.m_equipedListItems[(int) type].m_text = "BOUGHT";
        }
        this.m_equipedListItems[(int) type] = this.m_selectedItem;
        ItemManager.GetInstance().SetEquippedItem(type, this.m_selectedItem.m_info);
        this.m_selectedItem.m_text = "EQUIPPED";
      }
    }

    public void EquipCallback()
    {
      if (this.m_buyButton != null && !ShopScreen.hackedOpen)
      {
        this.m_buyButtonWait = 0.25f;
        if (this.m_selectedItem == null || this.m_selectedItem.m_info == null)
          return;
        ItemManager.GetInstance().SetEquippedItem(this.m_selectedItem.m_info.type, this.m_selectedItem.m_info);
        if (this.m_selectedItem.m_info.type == ItemType.ITEM_SLASH_MODIFIER)
        {
          SoundManager.GetInstance().SFXPlay(SoundDef.SND_EQUIP_SLASH);
        }
        else
        {
          if (this.m_selectedItem.m_info.type != ItemType.ITEM_BACKGROUND)
            return;
          SoundManager.GetInstance().SFXPlay(SoundDef.SND_EQUIP_BACKGROUND);
        }
      }
      else
      {
        if (this.m_buyButton == null || this.m_buyButton.m_entity == null)
          return;
        ((Fruit) this.m_buyButton.m_entity).m_pos2 = this.m_buyButton.m_entity.m_pos;
        this.m_buyButton.m_entity.m_vel = Vector3.Zero;
        ((Fruit) this.m_buyButton.m_entity).m_vel2 = Vector3.Zero;
        ((Fruit) this.m_buyButton.m_entity).m_gravity = Vector3.Zero;
      }
    }

    public void ConfirmCallback()
    {
      if (this.m_selectedItem != null && this.m_selectedItem.m_info != null)
        this.m_equipedListItems[(int) this.m_selectedItem.m_info.type] = this.m_selectedItem;
      this.m_state = 5;
      if (this.m_quitButton == null || this.m_quitButton.m_entity == null)
        return;
      ((Bomb) this.m_quitButton.m_entity).EnableGravity(true);
      this.m_quitButton.m_entity.m_vel = new Vector3(Math.g_random.RandF(5f) + 5f, -Math.g_random.RandF(5f), 0.0f);
      Game.game_work.tutorialControl.ResetTutePos();
    }

    public void CancelCallback()
    {
      this.m_state = 6;
      if (this.m_quitButton == null || this.m_quitButton.m_entity == null)
        return;
      ((Bomb) this.m_quitButton.m_entity).EnableGravity(true);
      this.m_quitButton.m_entity.m_vel = new Vector3(Math.g_random.RandF(5f) + 5f, -Math.g_random.RandF(5f), 0.0f);
      Game.game_work.tutorialControl.ResetTutePos();
    }

    public static void LoadContent()
    {
      ShopListItem.blankTex = TextureManager.GetInstance().Load("textureswp7/loading.tex");
      ShopListItem.dividerTex = TextureManager.GetInstance().Load("textureswp7/scratch_deviders.tex");
      ShopListItem.descriptionBox = TextureManager.GetInstance().Load("textureswp7/dialog_box_shop.tex");
      ShopListItem.buyNowTexture = TextureManager.GetInstance().Load("locked.tex", true);
      ShopListItem.equipNowTexture = TextureManager.GetInstance().Load("select_item.tex", true);
      ShopListItem.selectedTexture = TextureManager.GetInstance().Load("selected.tex", true);
      ShopListItem.selectedSmallTexture = TextureManager.GetInstance().Load("selected_sml.tex", true);
      ShopListItem.lockedStrokeTexture = TextureManager.GetInstance().Load("textureswp7/locked_stroke.tex");
      ShopListItem.newItemTexture = TextureManager.GetInstance().Load("new_item_sml.tex", true);
      ShopListItem.backGround = TextureManager.GetInstance().Load("textureswp7/BG_store.tex");
      ShopListItem.s_isContentLoaded = true;
    }

    public static void UnLoadContent()
    {
      ShopListItem.blankTex = (Texture) null;
      ShopListItem.dividerTex = (Texture) null;
      ShopListItem.descriptionBox = (Texture) null;
      ShopListItem.backGround = (Texture) null;
      ShopListItem.buyNowTexture = (Texture) null;
      ShopListItem.equipNowTexture = (Texture) null;
      ShopListItem.unequipNowTexture = (Texture) null;
      ShopListItem.coinsBuyTexture = (Texture) null;
      ShopListItem.coinsSpendTexture = (Texture) null;
      ShopListItem.swipe_to_scroll = (Texture) null;
      ShopListItem.selectedTexture = (Texture) null;
      ShopListItem.selectedSmallTexture = (Texture) null;
      ShopListItem.lockedStrokeTexture = (Texture) null;
      ShopListItem.newStrokeTexture = (Texture) null;
      ShopListItem.newItemTexture = (Texture) null;
    }

    public void DeletedMenuItem(HUDControl control)
    {
      if (this.m_buyButton == control)
      {
        int num = ShopScreen.hackedOpen ? 1 : 0;
        if (this.m_buyButton.m_entity != null)
        {
          this.m_buyButton.m_entity.m_pos.Y = -Game.SCREEN_WIDTH;
          ((Fruit) this.m_buyButton.m_entity).m_pos2.Y = -Game.SCREEN_WIDTH;
          ((Fruit) this.m_buyButton.m_entity).m_gravity = Vector3.Negate(Vector3.UnitY);
          this.m_buyButton.m_entity.m_vel.Y = -10f;
          ((Fruit) this.m_buyButton.m_entity).m_vel2.Y = -10f;
        }
        this.m_buyButtonWait += 0.05f;
        this.m_buyButton = (MenuButton) null;
      }
      if (this.m_quitButton != control)
        return;
      this.m_quitButton = (MenuButton) null;
    }

    public void SetSelected(ShopListItem item)
    {
      this.m_selectedItem = item;
      if (!this.m_scrollingMenu.IsLockedIn() || this.m_buyButton == null || this.m_selectedItem.m_info == null || this.m_state != 1)
        return;
      int type1 = Fruit.FruitType("pineapple");
      int type2 = Fruit.FruitType("black_pineapple");
      if (this.m_selectedItem.m_info.IsLocked())
      {
        this.m_buyButton.m_texture = ShopListItem.buyNowTexture;
        if (this.m_buyButton.m_entity == null)
          return;
        ((Fruit) this.m_buyButton.m_entity).SetFruitType(type2);
      }
      else
      {
        this.m_buyButton.m_texture = ShopListItem.equipNowTexture;
        if (this.m_buyButton.m_entity == null)
          return;
        ((Fruit) this.m_buyButton.m_entity).SetFruitType(type1);
      }
    }

    public ShopListItem GetSelected(ItemType type) => this.m_equipedListItems[(int) type];

    public ShopListItem GetSelected() => this.m_selectedItem;

    public override void Reset()
    {
    }

    public override void Release()
    {
      if (this.m_scrollingMenu != null)
      {
        Game.game_work.hud.RemoveControl((HUDControl) this.m_scrollingMenu);
        Delete.SAFE_DELETE<ScrollingMenu>(ref this.m_scrollingMenu);
      }
      this.m_texture = (Texture) null;
      this.m_time = 0.0f;
    }

    public override void Init()
    {
      this.Reset();
      this.m_selectedItem = (ShopListItem) null;
      for (int index = 0; index < 2; ++index)
        this.m_equipedListItems[index] = (ShopListItem) null;
      this.m_backDrawOrder = 3;
      this.m_scrollingMenu = new ScrollingMenu();
      this.m_scrollingMenu.SetWidth(290f);
      this.m_scrollingMenu.SetHeight(ShopScreen.SHOP_ITEM_HEIGHT);
      this.m_scrollingMenu.SetItemHeight(ShopScreen.SHOP_ITEM_HEIGHT);
      Game.game_work.hud.AddControl((HUDControl) this.m_scrollingMenu);
      int it = 0;
      ItemInfo info = ItemManager.GetInstance().GetFirst(ref it);
      bool flag1 = true;
      bool flag2 = true;
      ShopListItem shopListItem1 = (ShopListItem) null;
      float num1 = 0.0f;
      float num2 = 0.0f;
      for (; info != null; info = ItemManager.GetInstance().GetNext(ref it))
      {
        ShopListItem shopListItem2 = new ShopListItem();
        shopListItem2.Create(info, this);
        shopListItem2.m_backing = flag1;
        flag1 = !flag1;
        this.m_scrollingMenu.AddItem((ScrollingMenuItem) shopListItem2);
        shopListItem2.SetClickedFocusedCallback(new ScrollingMenuItem.ClickedMenuItemCallback(this.ClickedOnShopItem));
        if (ItemManager.GetInstance().IsEquipped(info))
          this.m_equipedListItems[(int) info.type] = shopListItem2;
        else if (!info.hasBeenSeen && (double) num1 == 0.0)
          num1 = num2;
        if (flag2)
        {
          this.SetSelected(shopListItem2);
          shopListItem2.isFirst = true;
        }
        flag2 = false;
        num2 += shopListItem2.GetHeight();
        shopListItem1 = shopListItem2;
      }
      if ((double) ShopScreen.ScrollOffset > 0.0)
        ShopScreen.ScrollOffset = -num1;
      shopListItem1.isLast = true;
      this.m_scrollingMenu.m_pos = new Vector3(ShopScreen.SHOP_LIST_POS_X - ShopScreen.SHOP_BACK_LEFT_SIDE * 1.5f, ShopScreen.SHOP_ITEM_HEIGHT / 2f, 0.0f);
      this.m_scrollingMenu.Init();
      this.m_scrollingMenu.m_offset.Y = ShopScreen.ScrollOffset;
      this.m_scrollingMenu.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_AFTER_SPLAT;
      this.m_scrollingMenu.Update(0.0f);
    }

    public override void Update(float dt)
    {
      float time = this.m_time;
      if (SplatEntity.NumActiveSplats() == 0)
        this.m_backDrawOrder = 2;
      if (this.m_scrollingMenu != null && this.m_selectedItem != this.m_scrollingMenu.GetItemClosestToZero() && ShopScreen.c == 0)
        this.SetSelected((ShopListItem) this.m_scrollingMenu.GetItemClosestToZero());
      ShopScreen.c = (ShopScreen.c + 1) % 10;
      this.m_drawOrder = (HUD.HUD_ORDER) this.m_backDrawOrder;
      switch (this.m_state)
      {
        case 0:
          this.m_time += (float) ((1.0 - (double) this.m_time) * 0.125);
          if ((double) this.m_time > 0.99900001287460327)
          {
            SplatEntity.RemoveAllSplats();
            this.m_buyButtonWait = 0.0f;
            this.m_time = 1f;
            this.m_state = 1;
            if (this.m_quitButton == null)
            {
              this.m_quitButton = new MenuButton(Game.game_work.backTexture, new Vector3((float) ((double) Game.SCREEN_WIDTH / 2.0 - 55.0), ShopScreen.QUIT_Y, 0.0f), new MenuButton.MenuCallback(this.QuitShopCallback), Fruit.MAX_FRUIT_TYPES);
              this.m_quitButton.Init();
              this.m_quitButton.m_triggerOnBackPress = true;
              Game.game_work.hud.AddControl((HUDControl) this.m_quitButton);
              this.m_quitButton.m_deleteCall = new HUDControl.HUDControlDeletedCallback(this.DeletedMenuItem);
              Game.game_work.tutorialControl.ResetTutePos(this.m_quitButton);
              MenuButton quitButton = this.m_quitButton;
              quitButton.m_originalScale = Vector3.Multiply(quitButton.m_originalScale, 0.825f);
              Entity entity = this.m_quitButton.m_entity;
              entity.m_cur_scale = Vector3.Multiply(entity.m_cur_scale, 0.825f);
              break;
            }
            break;
          }
          break;
        case 1:
          float num1 = -dt;
          if ((double) this.m_buyButtonWait > 0.0)
            this.m_buyButtonWait -= dt;
          else if (this.m_scrollingMenu.IsLockedIn())
          {
            if (this.m_selectedItem != null && this.m_selectedItem.m_info != null && (ItemManager.GetInstance().IsEquipped(this.m_selectedItem.m_info) || this.m_selectedItem.m_info.IsLocked()))
              Game.game_work.tutorialControl.ResetTutePos();
            if (this.m_selectedItem != null && ItemManager.GetInstance().IsEquipped(this.m_selectedItem.m_info))
              num1 = dt;
            else if (this.m_buyButton == null)
            {
              this.m_buyButton = new MenuButton(ShopListItem.buyNowTexture, new Vector3((float) ((double) Game.SCREEN_WIDTH / 2.0 - (double) ShopScreen.SHOP_BACK_RIGHT_SIDE / 2.0), ShopScreen.BUY_NOW_Y, 0.0f), new MenuButton.MenuCallback(this.EquipCallback), Fruit.FruitType("watermelon"));
              this.SetSelected(this.m_selectedItem);
              this.m_buyButton.Init();
              Game.game_work.hud.AddControl((HUDControl) this.m_buyButton);
              this.m_buyButton.m_clearOthers = false;
              ShopScreen.hackedOpen = false;
              this.m_buyButton.m_deleteCall = new HUDControl.HUDControlDeletedCallback(this.DeletedMenuItem);
              Game.game_work.tutorialControl.ResetTutePos(this.m_buyButton);
              MenuButton buyButton = this.m_buyButton;
              buyButton.m_originalScale = Vector3.Multiply(buyButton.m_originalScale, ShopScreen.WATERMELON_SELECT_SCALE);
              Entity entity = this.m_buyButton.m_entity;
              entity.m_cur_scale = Vector3.Multiply(entity.m_cur_scale, ShopScreen.WATERMELON_SELECT_SCALE);
              float num2 = (float) (((double) Math.g_random.RandF(2f) + 2.0) * (Math.g_random.Rand32(2) != 0 ? -1.0 : 1.0));
              for (int index = 0; index < 2; ++index)
              {
                Fruit.RandomStartAngle(ref ((Fruit) this.m_buyButton.m_entity).m_rotation_piece[index], true);
                ((Fruit) this.m_buyButton.m_entity).m_rotation_speed[index] = new Vector3(0.0f, 0.0f, num2);
              }
            }
          }
          else if (this.m_buyButton != null && this.m_buyButton.m_entity != null && !((Fruit) this.m_buyButton.m_entity).Sliced() && this.m_buyButton != null)
          {
            ((Fruit) this.m_buyButton.m_entity).m_isSliced = true;
            ShopScreen.hackedOpen = true;
            this.m_buyButton.m_clearOthers = false;
            ((Fruit) this.m_buyButton.m_entity).m_vel2 = Vector3.One;
          }
          this.m_selectedScaleInOut = (int) Math.CLAMP((float) this.m_selectedScaleInOut + num1 * (ShopScreen.POP_SINE / ShopScreen.POP_IN_OUT_TIME), 0.0f, ShopScreen.POP_SINE);
          break;
        case 2:
          this.m_time *= 0.75f;
          if ((double) this.m_time < 1.0 / 1000.0)
          {
            this.m_dojoScreen.Reset();
            this.m_terminate = true;
            break;
          }
          break;
        case 3:
          this.m_time *= 0.75f;
          if ((double) this.m_time < 1.0 / 1000.0)
          {
            this.m_state = 4;
            this.m_time = 0.0f;
            this.m_quitButton = new MenuButton(Game.game_work.backTexture, new Vector3((float) ((double) Game.SCREEN_WIDTH / 2.0 - 55.0), ShopScreen.QUIT_Y, 0.0f), new MenuButton.MenuCallback(this.ConfirmCallback), Fruit.MAX_FRUIT_TYPES);
            this.m_quitButton.Init();
            this.m_quitButton.m_triggerOnBackPress = true;
            this.m_quitButton.m_deleteCall = new HUDControl.HUDControlDeletedCallback(this.DeletedMenuItem);
            Game.game_work.hud.AddControl((HUDControl) this.m_quitButton);
            MenuButton quitButton = this.m_quitButton;
            quitButton.m_originalScale = Vector3.Multiply(quitButton.m_originalScale, 0.825f);
            Entity entity = this.m_quitButton.m_entity;
            entity.m_cur_scale = Vector3.Multiply(entity.m_cur_scale, 0.825f);
            break;
          }
          if (this.m_quitButton != null && this.m_quitButton.m_entity != null)
          {
            ((Bomb) this.m_quitButton.m_entity).EnableGravity(true);
            this.m_quitButton.m_entity.m_vel = new Vector3(Math.g_random.RandF(5f) + 5f, -Math.g_random.RandF(5f), 0.0f);
            Game.game_work.tutorialControl.ResetTutePos();
            this.m_quitButton = (MenuButton) null;
            break;
          }
          break;
        case 4:
          this.m_backDrawOrder = 3;
          break;
        case 5:
        case 6:
          if (this.m_quitButton != null && this.m_quitButton.m_entity != null)
          {
            ((Bomb) this.m_quitButton.m_entity).EnableGravity(true);
            this.m_quitButton.m_entity.m_vel = new Vector3(Math.g_random.RandF(5f) + 5f, -Math.g_random.RandF(5f), 0.0f);
            Game.game_work.tutorialControl.ResetTutePos();
            this.m_quitButton = (MenuButton) null;
          }
          if (ActorManager.GetInstance().GetNumEntities(1) == 0U && ActorManager.GetInstance().GetNumEntities(0) == 0U)
          {
            ItemType type = this.m_selectedItem == null || this.m_selectedItem.m_info == null ? ItemType.ITEM_TYPE_MAX : this.m_selectedItem.m_info.type;
            if (type < ItemType.ITEM_TYPE_MAX && this.m_equipedListItems[(int) this.m_selectedItem.m_info.type] != this.m_selectedItem)
              ItemManager.GetInstance().SetEquippedItem(type, this.m_equipedListItems[(int) type] != null ? this.m_equipedListItems[(int) type].m_info : (ItemInfo) null);
            this.m_state = 0;
            break;
          }
          break;
      }
      if (this.m_scrollingMenu != null)
      {
        this.m_scrollingMenu.m_pos = new Vector3(ShopScreen.SHOP_LIST_POS_X - (float) ((1.0 - (double) this.m_time) * (double) ShopScreen.SHOP_BACK_LEFT_SIDE * 1.5), ShopScreen.SHOP_ITEM_HEIGHT / 2f, 0.0f);
        ShopScreen.ScrollOffset = this.m_scrollingMenu.m_offset.Y;
      }
      if ((double) this.m_time >= (double) time)
        return;
      float num3 = (float) ((1.0 - (double) this.m_time - (1.0 - (double) time)) * (double) ShopScreen.SHOP_BACK_RIGHT_SIDE * 1.5);
      float num4 = (float) ((1.0 - (double) this.m_time - (1.0 - (double) time)) * (double) ShopScreen.SHOP_BACK_LEFT_SIDE * 1.5);
      for (int index = 0; index < SplatEntity.poolCount; ++index)
      {
        SplatEntity splatEntity = SplatEntity.pool[index];
        if (splatEntity.m_update && splatEntity.m_onWall >= 0)
        {
          if ((double) splatEntity.m_pos.X > (double) Game.SCREEN_WIDTH / 2.0 - (double) ShopScreen.SHOP_BACK_RIGHT_SIDE)
            splatEntity.m_pos.X += num3;
          else
            splatEntity.m_pos.X -= num4;
        }
      }
    }

    public override void Draw(float[] tintChannels)
    {
      float num1 = (float) ((double) Game.SCREEN_WIDTH / 2.0 - (double) ShopScreen.SHOP_BACK_RIGHT_SIDE / 2.0);
      if (this.m_drawOrder != (HUD.HUD_ORDER) this.m_backDrawOrder)
      {
        if (this.m_selectedScaleInOut <= 0)
          return;
        if ((double) this.m_time < 1.0)
          num1 += (float) ((1.0 - (double) this.m_time) * (double) ShopScreen.SHOP_BACK_RIGHT_SIDE * 1.5);
        Vector3 vector3 = Vector3.Multiply(new Vector3((float) (ShopListItem.selectedTexture.GetWidth() + 1U), (float) (ShopListItem.selectedTexture.GetHeight() + 1U), 0.0f), Game.GAME_MODE_SCALE_FIX);
        if ((double) this.m_selectedScaleInOut < (double) ShopScreen.POP_SINE)
          vector3 = Vector3.Multiply(vector3, Math.SinIdx((ushort) this.m_selectedScaleInOut) / Math.SinIdx((ushort) ShopScreen.POP_SINE));
        Matrix mtx = Matrix.Multiply(Matrix.CreateScale(vector3), Matrix.CreateTranslation(new Vector3(num1, ShopScreen.BUY_NOW_Y, 0.0f)));
        MatrixManager.GetInstance().Reset();
        MatrixManager.GetInstance().SetMatrix(mtx);
        MatrixManager.GetInstance().UploadCurrentMatrices();
        ShopListItem.selectedTexture.Set();
        Mesh.DrawQuad(Color.White);
        ShopListItem.selectedTexture.UnSet();
      }
      else
      {
        this.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_NORMAL;
        if ((double) this.m_time < 1.0)
        {
          num1 += (float) ((1.0 - (double) this.m_time) * (double) ShopScreen.SHOP_BACK_RIGHT_SIDE * 1.5);
          ShopListItem.backGround.Set();
          Matrix mtx = Matrix.Multiply(Matrix.CreateScale(new Vector3(ShopScreen.SHOP_BACK_LEFT_SIDE + 1f, Game.SCREEN_HEIGHT + 1f, 0.0f)), Matrix.CreateTranslation(new Vector3(this.m_scrollingMenu.m_pos.X, 0.0f, 0.0f)));
          MatrixManager.GetInstance().Reset();
          MatrixManager.GetInstance().SetMatrix(mtx);
          MatrixManager.GetInstance().UploadCurrentMatrices();
          Mesh.DrawQuad(Color.White, (float) ((512.0 - (double) Game.SCREEN_WIDTH) / 2.0 / 512.0), (float) (((double) ShopScreen.SHOP_BACK_LEFT_SIDE + (512.0 - (double) Game.SCREEN_WIDTH) / 2.0) / 512.0), (float) ((512.0 - (double) Game.SCREEN_HEIGHT) / 2.0 / 512.0), (float) (1.0 - (512.0 - (double) Game.SCREEN_HEIGHT) / 2.0 / 512.0));
          Math.Scale44(new Vector3(ShopScreen.SHOP_BACK_RIGHT_SIDE + 1f, Game.SCREEN_HEIGHT + 1f, 0.0f), out mtx);
          Vector3 scl = new Vector3(num1, 0.0f, 0.0f);
          Math.GlobalTranslate44(ref mtx, scl);
          MatrixManager.GetInstance().Reset();
          MatrixManager.GetInstance().SetMatrix(mtx);
          MatrixManager.GetInstance().UploadCurrentMatrices();
          Mesh.DrawQuad(Color.White, (float) (((double) ShopScreen.SHOP_BACK_LEFT_SIDE + (512.0 - (double) Game.SCREEN_WIDTH) / 2.0) / 512.0), (float) (1.0 - (512.0 - (double) Game.SCREEN_WIDTH) / 2.0 / 512.0), (float) ((512.0 - (double) Game.SCREEN_HEIGHT) / 2.0 / 512.0), (float) (1.0 - (512.0 - (double) Game.SCREEN_HEIGHT) / 2.0 / 512.0));
          ShopListItem.backGround.UnSet();
        }
        else
        {
          Matrix mtx;
          Math.Scale44(new Vector3(Game.SCREEN_WIDTH + 1f, Game.SCREEN_HEIGHT + 1f, 0.0f), out mtx);
          MatrixManager.GetInstance().Reset();
          MatrixManager.GetInstance().SetMatrix(mtx);
          MatrixManager.GetInstance().UploadCurrentMatrices();
          ShopListItem.backGround.Set();
          Mesh.DrawQuad(Color.White, (float) ((512.0 - (double) Game.SCREEN_WIDTH) / 2.0 / 512.0), (float) (1.0 - (512.0 - (double) Game.SCREEN_WIDTH) / 2.0 / 512.0), (float) ((512.0 - (double) Game.SCREEN_HEIGHT) / 2.0 / 512.0), (float) (1.0 - (512.0 - (double) Game.SCREEN_HEIGHT) / 2.0 / 512.0));
          ShopListItem.backGround.UnSet();
        }
        Matrix mtx1;
        Math.Scale44(Vector3.Multiply(new Vector3((float) (ShopListItem.descriptionBox.GetWidth() + 1U), (float) (ShopListItem.descriptionBox.GetHeight() + 1U), 0.0f), Game.GAME_MODE_SCALE_FIX), out mtx1);
        Math.GlobalTranslate44(ref mtx1, num1 - 4f, -3f, 0.0f);
        MatrixManager.GetInstance().Reset();
        MatrixManager.GetInstance().SetMatrix(mtx1);
        MatrixManager.GetInstance().UploadCurrentMatrices();
        byte num2;
        if (this.m_selectedItem.m_info.IsLocked())
        {
          ShopScreen.m_fadeTime = Math.MIN(1f, ShopScreen.m_fadeTime + Game.game_work.dt * 5f);
          num2 = (byte) ((double) byte.MaxValue - (double) ShopScreen.m_fadeTime * 120.0);
        }
        else
        {
          ShopScreen.m_fadeTime = Math.MAX(0.0f, ShopScreen.m_fadeTime - Game.game_work.dt * 5f);
          num2 = (byte) ((double) byte.MaxValue - (double) ShopScreen.m_fadeTime * 120.0);
        }
        Color col = new Color((int) num2, (int) num2, (int) num2, (int) byte.MaxValue);
        ShopListItem.descriptionBox.Set();
        Mesh.DrawQuad(col);
        ShopListItem.descriptionBox.UnSet();
      }
    }

    public float GetDescriptionTextXPos()
    {
      float num = (float) ((double) Game.SCREEN_WIDTH / 2.0 - (double) ShopScreen.SHOP_BACK_RIGHT_SIDE / 2.0);
      if ((double) this.m_time < 1.0)
        num += (float) ((1.0 - (double) this.m_time) * (double) ShopScreen.SHOP_BACK_RIGHT_SIDE * 1.5);
      return num - ShopScreen.DESCRIPTION_BOX_WIDTH / 2f;
    }

    public void ClickedOnShopItem(ScrollingMenuItem item)
    {
      if (((ShopListItem) item).m_info != null && !((ShopListItem) item).m_info.IsLocked())
      {
        if (this.m_buyButton == null)
          return;
        Game.game_work.tutorialControl.ButtonPressedAtPos(this.m_buyButton);
      }
      else
      {
        SoundManager.GetInstance().SFXPlay("equip-locked");
        ((ShopListItem) item).m_shakeTime = ShopScreen.SHAKE_TIME;
      }
    }

    public static void NewItem() => ShopScreen.ScrollOffset = 1f;

    public enum SS
    {
      SS_IN,
      SS_WAIT,
      SS_OUT,
      SS_CONFIRM_IN,
      SS_CONFIRM,
      SS_CONFIRM_OUT,
      SS_CONFIRM_CANCELLED,
    }
  }
}
