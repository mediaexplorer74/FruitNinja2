
// Type: Mortar.TheGame
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using GameManager;
//using Microsoft.Devices;
//using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading;
using Game = Microsoft.Xna.Framework.Game;
using System.Reflection.Metadata;
using System.Diagnostics;

#nullable disable
namespace Mortar
{
  public class Game1 : Game
  {
    private const int MAGIC1 = 195800833;
    private const int MAGIC2 = 33479857;
    private static string CFG_DIRECTORY = "FruitNinja";
    private static string CFG_FILENAME = Game1.CFG_DIRECTORY + "/setting.dat";
    public static Game1.Settings settings = new Game1.Settings();

    public GraphicsDeviceManager graphics;
    
    public SpriteBatch spriteBatch;
    public SpriteFont font1;
    public SpriteFont font2;
    public SpriteFont font3;
    public SpriteFont font4;
    public StringTable stringTable;
    public static Texture2D picture;
    public static Game1 instance;
    public static FruitGame fgame;
    public static bool emulator = false;
    public static bool exceptionThrown = false;
    public static bool exitingGame = false;
    public static bool logInSucceeded = false;
    private static uint error_code = 2415923200;
    private bool init_failed;
    private bool load_failed;
    private bool quit_game;
    private bool getAchievements;
    private GamerServicesComponent gamerServicesInstance;
    private Texture bob;
    private Texture2D bobsCousinIt;
    public static Texture2D bobsCousinHairyMaclary;
    private Texture upsell;
    private Texture menu_purchase;
    private Texture menu_exit;
    private float timeout = 5.5f;
    private bool fadeout;
    private bool fadein;
    private FadeManager fm = new FadeManager(Game1.bobsCousinHairyMaclary, Game1.bobsCousinHairyMaclary);
    private bool showUpsell;
    private bool exit_from_upsell;
    private Vector2 pos1;
    private Vector2 pos2;
    public static int initupdates = 5;
    public LinkedList<Game1.MyTouchState> tstate = new LinkedList<Game1.MyTouchState>();
    private static float dtCo = 1f;
    private static bool sb = false;
    private static int sbc = 0;
    private bool _BackButtonWasPressed;
    public bool BackButtonWasPressed;
    private bool backButtonDownLast;
    private bool gameUpdateRequired;
    private bool curr_down;
    private bool prev_down;
    private int uid = -1;
    private int tx;
    private int ty;
    private bool inExit;
    private bool inPurchase;
    private static bool ___isActiveSim = true;
    public static bool disableNetworkCalls = false;
    private GamePadState state_prev;
    private GamePadState state_curr;
    private int skip;
    public static bool switchLanguage = false;
    public static StringTableUtils.Language switchToLanguage;
    public static float loadinScreen = 0.0f;
    private static Texture2D m_fruitTex;
    private static Texture2D m_ninjaTex;
    private static Texture2D m_headTex;
    private static Texture2D m_bodyTex;


    // LoadConfig
    public static void LoadConfig()
    {
      try
      {

        IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication();
        IsolatedStorageFileStream storageFileStream = (IsolatedStorageFileStream)null;
       
        if (!storeForApplication.DirectoryExists(Game1.CFG_DIRECTORY))
          storeForApplication.CreateDirectory(Game1.CFG_DIRECTORY);
        if (storeForApplication.FileExists(Game1.CFG_FILENAME))
        {
          storageFileStream = storeForApplication.OpenFile(Game1.CFG_FILENAME, FileMode.Open);
          int length = (int) storageFileStream.Length;
          if (length == 28 + /*Game.MAX_SYSTEM_ACHIEVEMENTS*/1 * 4)
          {
            byte[] buffer = new byte[length];
            storageFileStream.Read(buffer, 0, length);

            storageFileStream.Flush(); 
            storageFileStream.Dispose();
                        
            int int32_1 = BitConverter.ToInt32(buffer, 0);
            int int32_2 = BitConverter.ToInt32(buffer, 4);
            int int32_3 = BitConverter.ToInt32(buffer, 8);
            int int32_4 = BitConverter.ToInt32(buffer, 12);
            int int32_5 = BitConverter.ToInt32(buffer, 16);
            int int32_6 = BitConverter.ToInt32(buffer, 20);
            int int32_7 = BitConverter.ToInt32(buffer, 24);
            if (int32_1 == 195800833 && int32_2 == 33479857 && int32_3 >= 0 && int32_3 <= 5)
            {
              Game1.settings.language = int32_3;
              Game1.settings.week = int32_4;
              Game1.settings.tf = int32_5;
              Game1.settings.aw = int32_6;
              Game1.settings.bestThisWeek = int32_7;
              
              //Game.game_work.language = (StringTableUtils.Language) int32_3;
              //int startIndex = 28;
              //for (int index = 0; index < Game.MAX_SYSTEM_ACHIEVEMENTS; ++index)
              //{
              //  TheGame.settings.achievements[index] = BitConverter.ToUInt32(buffer, startIndex);
              //  startIndex += 4;
              //}
            }
            else
              Game1.SaveConfig();
          }
          else
          {
            storageFileStream.Flush();
            storageFileStream.Dispose();
            Game1.SaveConfig();
          }
        }
        else
          Game1.SaveConfig();

        //storageFileStream?.Flush();
        storageFileStream?.Dispose();
      }
      catch (Exception ex)
      {
         Debug.WriteLine("[ex] Game1 - LoadConfig ex.: " + ex.Message);
      }     
    }//LoadConfig


    // SaveConfig
    public static void SaveConfig()
    {
      
      try
      {
        IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication();
        IsolatedStorageFileStream storageFileStream = (IsolatedStorageFileStream)null;

        if (!storeForApplication.DirectoryExists(Game1.CFG_DIRECTORY))
          storeForApplication.CreateDirectory(Game1.CFG_DIRECTORY);
        storageFileStream = storeForApplication.OpenFile(Game1.CFG_FILENAME, FileMode.Create);
        if (storageFileStream == null)
          return;
        
        Game1.settings.language = 0;//(int) Game.game_work.language;
        byte[] bytes1 = BitConverter.GetBytes(Game1.settings.magic1);
        byte[] bytes2 = BitConverter.GetBytes(Game1.settings.magic2);
        byte[] bytes3 = BitConverter.GetBytes(Game1.settings.language);
        byte[] bytes4 = BitConverter.GetBytes(Game1.settings.week);
        byte[] bytes5 = BitConverter.GetBytes(Game1.settings.tf);
        byte[] bytes6 = BitConverter.GetBytes(Game1.settings.aw);
        byte[] bytes7 = BitConverter.GetBytes(Game1.settings.bestThisWeek);
        storageFileStream.Write(bytes1, 0, 4);
        storageFileStream.Write(bytes2, 0, 4);
        storageFileStream.Write(bytes3, 0, 4);
        storageFileStream.Write(bytes4, 0, 4);
        storageFileStream.Write(bytes5, 0, 4);
        storageFileStream.Write(bytes6, 0, 4);
        storageFileStream.Write(bytes7, 0, 4);
        for (int index = 0; index < /*Game.MAX_SYSTEM_ACHIEVEMENTS*/1; ++index)
        {
          byte[] bytes8 = BitConverter.GetBytes(Game1.settings.achievements[index]);
          storageFileStream.Write(bytes8, 0, 4);
        }

        storageFileStream.Flush();
        storageFileStream.Dispose();
      }
        catch (Exception ex)
        {
            Debug.WriteLine("[ex] Game1 - SaveConfig ex.: " + ex.Message);
        }
    }//SaveConfig

    private string ERROR_BUTTON_1 => Game1.instance.stringTable.GetString(703);

    private string ERROR_TITLE_1 => Game1.instance.stringTable.GetString(845);

    private static string ERROR_MESSAGE(uint code)
    {
      return string.Format("Error 0x{0:x} occurred. Fruit Ninja cannot continue and needs to exit.", (object) code);
    }

    private static string ERROR_MESSAGE_GAME()
    {
      return string.Format("Error 0x{0:x} occurred. Please check your memory usage before continuing.", (object) Game1.GetErrorCode());
    }

    public static void SetErrorCode(uint code) => Game1.error_code = code;

    public static uint GetErrorCode() => Game1.error_code;

    //constructor
    public Game1()
    {
      Game1.instance = this;
          
      graphics = new GraphicsDeviceManager(this);

      Content.RootDirectory = "Content";

      //this.IsFixedTimeStep = true;
      //this.TargetElapsedTime = TimeSpan.FromTicks(333333L);
      //this.graphics.SynchronizeWithVerticalRetrace = true;
      this.graphics.PreferredBackBufferWidth = 800;
      this.graphics.PreferredBackBufferHeight = 480;
      this.graphics.IsFullScreen = false;//true;
      this.graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft;
      //this.graphics.ApplyChanges();

      // touchscreen emulation (?)
            
      // Remove or comment out the line causing the error, as 'MouseAsTouch' is not a valid property of 'TouchPanel'.
      // Original line:
      // TouchPanel.MouseAsTouch = true;

      // Suggested fix: Remove the line or replace it with a valid alternative if needed.
      // If you need mouse input to simulate touch, you may need to implement custom logic for this functionality.
      //TouchPanel.MouseAsTouch = true;

      //Guide.IsScreenSaverEnabled = false;

      Game1.fgame = new FruitGame();

      switch (CultureInfo.CurrentCulture.Name)
      {
        case "en":
        case "en-AU":
        case "en-BZ":
        case "en-CA":
        case "en-CB":
        case "en-IE":
        case "en-JM":
        case "en-NZ":
        case "en-PH":
        case "en-ZA":
        case "en-TT":
        case "en-GB":
        case "en-ZW":
          //Game.game_work.language = StringTableUtils.Language.LANGUAGE_ENGLISH_UK;
          break;
        case "en-US":
          //Game.game_work.language = StringTableUtils.Language.LANGUAGE_ENGLISH;
          break;
        case "fr":
        case "fr-BE":
        case "fr-CA":
        case "fr-FR":
        case "fr-LU":
        case "fr-MC":
        case "fr-CH":
          //Game.game_work.language = StringTableUtils.Language.LANGUAGE_FRENCH;
          break;
        case "de":
        case "de-AT":
        case "de-DE":
        case "de-LI":
        case "de-LU":
        case "de-CH":
          //Game.game_work.language = StringTableUtils.Language.LANGUAGE_GERMAN;
          break;
        case "it":
        case "it-IT":
        case "it-CH":
          //Game.game_work.language = StringTableUtils.Language.LANGUAGE_ITALIAN;
          break;
        case "es":
        case "es-AR":
        case "es-BO":
        case "es-CL":
        case "es-CO":
        case "es-CR":
        case "es-DO":
        case "es-EC":
        case "es-SV":
        case "es-GT":
        case "es-HN":
        case "es-MX":
        case "es-NI":
        case "es-PA":
        case "es-PY":
        case "es-PE":
        case "es-PR":
        case "es-ES":
        case "es-UY":
        case "es-VE":
          //Game.game_work.language = StringTableUtils.Language.LANGUAGE_SPANISH;
          break;
        default:
          //Game.game_work.language = StringTableUtils.Language.LANGUAGE_ENGLISH;
          break;
      }
      //Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
      //Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
      try
      {
        if (!IsolatedStorageFile.GetUserStoreForApplication().FileExists(Game1.CFG_FILENAME))
          Game1.SaveConfig();
      }
      catch
      {
      }
      this.stringTable = new StringTable();
      //ResourceManager.SetUpLooper();

    }//TheGame


    // Initialize
    protected override void Initialize()
    {
      try
      {
        // initialization logic
        spriteBatch = new SpriteBatch(GraphicsDevice);

        Game1.LoadConfig();
        
        //this.stringTable.LoadHeader("stringtables/translations");
        //for (int lidx = 0; lidx < 6; ++lidx)
        //  this.stringTable.LoadLanguage(lidx);
        /*switch (Game.game_work.language)
        {
          case StringTableUtils.Language.LANGUAGE_ENGLISH_UK:
            StringManager.GetInstance().SetDefaultLanguage("english_uk");
            TheGame.instance.stringTable.UpdateDefaultLanguage();
            break;
          case StringTableUtils.Language.LANGUAGE_FRENCH:
            StringManager.GetInstance().SetDefaultLanguage("french");
            TheGame.instance.stringTable.UpdateDefaultLanguage();
            break;
          case StringTableUtils.Language.LANGUAGE_SPANISH:
            StringManager.GetInstance().SetDefaultLanguage("spanish");
            TheGame.instance.stringTable.UpdateDefaultLanguage();
            break;
          case StringTableUtils.Language.LANGUAGE_GERMAN:
            StringManager.GetInstance().SetDefaultLanguage("german");
            TheGame.instance.stringTable.UpdateDefaultLanguage();
            break;
          case StringTableUtils.Language.LANGUAGE_ITALIAN:
            StringManager.GetInstance().SetDefaultLanguage("italian");
            TheGame.instance.stringTable.UpdateDefaultLanguage();
            break;
          default:
            StringManager.GetInstance().SetDefaultLanguage("english_us");
            TheGame.instance.stringTable.UpdateDefaultLanguage();
            break;
        }*/
      }
      catch
      {
        try
        {
          MediaPlayer.IsMuted = false;
        }
        catch
        {
        }
        Game1.exitingGame = true;
        this.Exit();
      }
      try
      {
        Game1.emulator = true;//Environment.DeviceType == 1;
        if (!Game1.emulator)
        {
          //SignedInGamer.SignedIn += new EventHandler<SignedInEventArgs>(this.GamerSignedInCallback);
          //this.gamerServicesInstance = new GamerServicesComponent((Game) this);
          //this.Components.Add((IGameComponent)this.gamerServicesInstance);
        }
        base.Initialize();
      }
      catch (Exception ex)
      {
        Exception innerException = ex.InnerException;
        while (innerException != null)
          innerException = innerException.InnerException;
        string text = Game1.ERROR_MESSAGE(2147487744U);
        this.init_failed = true;
        string[] buttons = new string[1]
        {
          this.ERROR_BUTTON_1
        };
        
        //while (Guide.IsVisible)
        //  Thread.Sleep(32);
        //if (text.Length > (int) byte.MaxValue)
        //  text = text.Substring(0, 250);
        //Guide.BeginShowMessageBox(this.ERROR_TITLE_1, text, (IEnumerable<string>) buttons, 0, MessageBoxIcon.Alert, new AsyncCallback(this.InitOrLoadFailedCallback), (object) null);
      }
    }

    private void InitOrLoadFailedCallback(IAsyncResult result) => this.quit_game = true;

    private void IngameException(IAsyncResult result)
    {
        if (GameTask.GameReset(0U))
        {
            //Game.QuitToMenu();
        }
        else
            this.quit_game = true;
    }

    public void DoUpsell(bool exitGame)
    {
      //if (!Game.isWP7TrialMode() || this.showUpsell)
      //  return;
      this.exit_from_upsell = exitGame;
      this.showUpsell = true;
      this.pos1 = new Vector2((float) (800 - (this.menu_exit.intex.Width + 16)), (float) (480 - (this.menu_exit.intex.Height + 20)));
      this.pos2 = new Vector2(this.pos1.X - (float) (this.menu_purchase.intex.Width + 16), this.pos1.Y);
      GameTask.SkipToPause(false);
    }

    public bool UpsellComplete()
    {
        return !this.showUpsell;
    }

    protected override void LoadContent()
    {
      try
      {
            /*Game.SetTrialModeState();
            switch (Game.game_work.language)
            {
                case StringTableUtils.Language.LANGUAGE_ENGLISH_UK:
                this.bob = Texture.Load("localisedwp7/en/HB_logo.tex");
                break;
                case StringTableUtils.Language.LANGUAGE_FRENCH:
                this.bob = Texture.Load("localisedwp7/fr/HB_logo.tex");
                break;
                case StringTableUtils.Language.LANGUAGE_SPANISH:
                this.bob = Texture.Load("localisedwp7/es/HB_logo.tex");
                break;
                case StringTableUtils.Language.LANGUAGE_GERMAN:
                this.bob = Texture.Load("localisedwp7/de/HB_logo.tex");
                break;
                case StringTableUtils.Language.LANGUAGE_ITALIAN:
                this.bob = Texture.Load("localisedwp7/it/HB_logo.tex");
                break;
                default:
                this.bob = Texture.Load("localisedwp7/en/HB_logo.tex");
                break;
            }*/

        //Temp
        this.bob = Texture.Load("localisedwp7/en/HB_logo.tex");
        

        this.bobsCousinIt = this.Content.Load<Texture2D>("extra/MGS_WP7_Horiz_Still");
        Game1.bobsCousinHairyMaclary = this.Content.Load<Texture2D>("extra/black");
        this.upsell = TextureManager.GetInstance().Load("trial_upsell.tex", true);
        this.menu_purchase = TextureManager.GetInstance().Load("menu_purchase.tex", true);
        this.menu_exit = TextureManager.GetInstance().Load("quit_title.tex", true);
        this.fm = new FadeManager(Game1.bobsCousinHairyMaclary, Game1.bobsCousinHairyMaclary);
        Game1.instance.GraphicsDevice.Clear((ClearOptions) 3, Color.Black, 1f, 0);
        this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
        this.font1 = this.Content.Load<SpriteFont>("extra/mainFont");
        this.font2 = this.Content.Load<SpriteFont>("extra/detailFont");
        this.font3 = this.Content.Load<SpriteFont>("extra/titleFont");
        this.font4 = this.Content.Load<SpriteFont>("extra/titleFontSmall");
      }
      catch (Exception ex)
      {
        Exception innerException = ex.InnerException;
        while (innerException != null)
          innerException = innerException.InnerException;
        string text = Game1.ERROR_MESSAGE(2147491840U);
        this.load_failed = true;
        string[] buttons = new string[1]
        {
          this.ERROR_BUTTON_1
        };
        //while (Guide.IsVisible)
        //  Thread.Sleep(32);
        //if (text.Length > (int) byte.MaxValue)
        //  text = text.Substring(0, 250);
        //Guide.BeginShowMessageBox(this.ERROR_TITLE_1, text, (IEnumerable<string>) buttons, 0, MessageBoxIcon.Alert, new AsyncCallback(this.InitOrLoadFailedCallback), (object) null);
      }

        //ResourceManager.SetSpriteBatch(spriteBatch);
        //target = new RenderTarget2D(GraphicsDevice, width, height);
        //gsm = new GameStateManager(this, spriteBatch);

    }//LoadContent

    protected override void UnloadContent()
    {
    }

    public static bool TriggerShowBuyMessageBox
    {
      set
      {
        if (!value /*|| !Game.isWP7TrialMode()*/)
          return;
        Game1.sb = value;
        Game1.sbc = 40;
      }
    }

    private void NotConnected(IAsyncResult result)
    {
    }

    /// <summary>
    /// Allows the game to run logic such as updating the world,
    /// checking for collisions, gathering input, and playing audio.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Update(GameTime gameTime)
    {
      if (!this.IsActive || !Game1.instance.IsActive)
        return;
      if (this.skip > 0)
        --this.skip;
      else if (this.gameUpdateRequired)
      {
        this.gameUpdateRequired = false;
        this.HandleGameUpdateException();
      }
      else
      {
        if (Game1.sb)
        {
          --Game1.sbc;
          if (Game1.sbc <= 0)
          {
            this.DoUpsell(false);
            Game1.sb = false;
            Game1.sbc = 0;
            return;
          }
        }
        this._BackButtonWasPressed = false;
        GamePadState state1 = GamePad.GetState((PlayerIndex) 0);
        GamePadButtons buttons1 = state1.Buttons;
        bool flag = buttons1.Back == ButtonState.Pressed;
        if (!flag && this.backButtonDownLast)
          this.BackButtonWasPressed = true;
        this.backButtonDownLast = flag;
        if (this._BackButtonWasPressed)
        {
          if (Game1.___isActiveSim)
          {
            Game1.___isActiveSim = false;
            base.OnDeactivated((object) null, (EventArgs) null);
          }
          else
          {
            Game1.___isActiveSim = true;
            base.OnActivated((object) null, (EventArgs) null);
          }
        }
        if (Game1.instance != null && !Game1.instance.IsActive)
          return;
        if (this.quit_game)
        {
          try
          {
            MediaPlayer.IsMuted = false;
          }
          catch
          {
          }
          Game1.exitingGame = true;
          this.Exit();
        }
        else if (Game1.exceptionThrown)
        {
          try
          {
            base.Update(gameTime);
          }
          catch //(GameUpdateRequiredException ex)
          {
            this.gameUpdateRequired = true;
            Game1.disableNetworkCalls = true;
          }
        }
        else
        {
          TouchPanel.EnabledGestures = (GestureType) 0;
          if (this.showUpsell)
          {
            Mortar.Touch.instance.Clear();
            try
            {
              this.state_prev = this.state_curr;
              this.state_curr = GamePad.GetState((PlayerIndex) 0);
              GamePadButtons buttons2 = this.state_prev.Buttons;
              if (buttons2.Back == ButtonState.Pressed)
              {
                GamePadButtons buttons3 = this.state_curr.Buttons;
                if (buttons3.Back == null)
                {
                  if (this.exit_from_upsell)
                  {
                    try
                    {
                      MediaPlayer.IsMuted = false;
                    }
                    catch
                    {
                    }
                    Game1.exitingGame = true;
                    this.Exit();
                  }
                  else
                  {
                    this.skip = 1;
                    this.showUpsell = false;
                    //Thread.Sleep(32);
                    return;
                  }
                }
              }

              TouchCollection state2 = TouchPanel.GetState();
              this.curr_down = state2.Count > 0;
              if (this.prev_down != this.curr_down)
              {
                this.prev_down = this.curr_down;
                if (this.curr_down)
                {
                  if (this.uid == -1)
                  {
                    foreach (TouchLocation touchLocation in state2)
                    {
                      if (touchLocation.State == TouchLocationState.Pressed)
                      {
                        this.uid = touchLocation.Id;
                        this.tx = (int) touchLocation.Position.X;
                        this.ty = (int) touchLocation.Position.Y;
                        break;
                      }
                    }
                  }
                }
                else
                {
                  if (this.inExit)
                  {
                    if (this.exit_from_upsell)
                    {
                      try
                      {
                        MediaPlayer.IsMuted = false;
                      }
                      catch
                      {
                      }
                      Game1.exitingGame = true;
                      this.Exit();
                    }
                    else
                      this.showUpsell = false;
                  }
                  else if (this.inPurchase)
                  {
                    try
                    {
                      //SignedInGamer signedInGamer = default;//Gamer.SignedInGamers[(PlayerIndex) 0];
                      //if (signedInGamer.IsSignedInToLive && signedInGamer.Privileges.AllowPurchaseContent)
                      //{
                        //while (Guide.IsVisible)
                        //  Thread.Sleep(32);
                        //if (!Guide.IsVisible)
                        //{
                          //if (Game.isWP7TrialMode())
                          //  Guide.ShowMarketplace((PlayerIndex) 0);
                          //else
                          //  new MarketplaceDetailTask()
                          //  {
                          //    ContentType = ((MarketplaceContentType) 1)
                          //  }.Show();
                        //}
                      //}
                      //else
                      {
                        string[] buttons4 = new string[1]
                        {
                          this.ERROR_BUTTON_1
                        };
                        //while (Guide.IsVisible)
                        //  Thread.Sleep(32);
                        //Guide.BeginShowMessageBox(this.ERROR_TITLE_1, TheGame.NotSignedInMessage(), (IEnumerable<string>) buttons4, 0, MessageBoxIcon.Alert, new AsyncCallback(this.NotConnected), (object) null);
                      }
                    }
                    catch
                    {
                      string[] buttons5 = new string[1]
                      {
                        this.ERROR_BUTTON_1
                      };
                      //while (Guide.IsVisible)
                      //  Thread.Sleep(32);
                      //Guide.BeginShowMessageBox(this.ERROR_TITLE_1, TheGame.NotSignedInMessage(), (IEnumerable<string>) buttons5, 0, MessageBoxIcon.Alert, new AsyncCallback(this.NotConnected), (object) null);
                    }
                    this.showUpsell = false;
                  }
                  this.inExit = false;
                  this.inPurchase = false;
                  this.uid = -1;
                  this.tx = -1;
                  this.ty = -1;
                }
              }
              if (this.curr_down)
              {
                foreach (TouchLocation touchLocation in state2)
                {
                  if (this.uid == touchLocation.Id 
                                        && touchLocation.State == TouchLocationState.Released)
                  {
                    this.tx = (int) touchLocation.Position.X;
                    this.ty = (int) touchLocation.Position.Y;
                    break;
                  }
                }
                Rectangle rectangle1 = new Rectangle((int) this.pos1.X, (int) this.pos1.Y, 
                    this.menu_exit.intex.Width, this.menu_exit.intex.Height);
                Rectangle rectangle2 = new Rectangle((int) this.pos2.X, 
                    (int) this.pos2.Y, this.menu_purchase.intex.Width, this.menu_purchase.intex.Height);
                this.inExit = rectangle1.Contains(this.tx, this.ty);
                if (rectangle2.Contains(this.tx, this.ty))
                  this.inPurchase = true;
                else
                  this.inPurchase = false;
              }
              else
              {
                this.inExit = false;
                this.inPurchase = false;
              }
            }
            catch
            {
            }
          }
          else if ((double) this.timeout > 0.0)
          {
            if ((double) this.timeout > 0.5)
            {
              TouchCollection state3 = TouchPanel.GetState();
              if (state3.Count > 0)
                this.timeout = 0.5f;
            }
            this.timeout -= (float) gameTime.ElapsedGameTime.Milliseconds / 1000f;
            if ((double) this.timeout < 0.0)
              this.timeout = 0.0f;
            if (!this.fadeout && (double) this.timeout < 0.5)
            {
              this.fm.Start(FadeState.ToBlack, 250f);
              this.fadeout = true;
            }
            if (!this.fadein && (double) this.timeout < 0.25)
            {
              this.fm.Start(FadeState.ToNormal, 250f);
              this.fadein = true;
            }
            this.fm.Update(gameTime);
          }
          else
          {
            try
            {
              if (this.load_failed || this.init_failed || this.quit_game)
                return;
              if (Game1.initupdates > 0)
              {
                --Game1.initupdates;
                if (Game1.initupdates != 0)
                  return;
                Game1.fgame.Init(0U, "");
              }
              else
              {
                try
                {
                  base.Update(gameTime);
                }
                catch //(GameUpdateRequiredException ex)
                {
                  this.gameUpdateRequired = true;
                }
              }
            }
            catch (Microsoft.Xna.Framework.GameUpdateRequiredException ex)
            {
              this.gameUpdateRequired = true;
            }
            catch (Exception ex)
            {
              this.GameException();
              Exception innerException = ex.InnerException;
              while (innerException != null)
                innerException = innerException.InnerException;
              string text = Game1.ERROR_MESSAGE_GAME();
              string[] buttons6 = new string[1]
              {
                this.ERROR_BUTTON_1
              };
              //while (Guide.IsVisible)
              //  Thread.Sleep(32);
              //if (text.Length > (int) byte.MaxValue)
              //  text = text.Substring(0, 250);
              //Guide.BeginShowMessageBox(this.ERROR_TITLE_1, text, (IEnumerable<string>) buttons6, 0, MessageBoxIcon.Alert, new AsyncCallback(this.IngameException), (object) null);
            }
          }
        }
      }
    }//Update


    // HandleGameUpdateException
    private void HandleGameUpdateException()
    {
      if (this.gamerServicesInstance != null)
        this.gamerServicesInstance.Enabled = false;

      string str1 = Game1.instance.stringTable.GetString(707);
      string str2 = Game1.instance.stringTable.GetString(702);
      string title = " ";

      string[] buttons = new string[2]{ str2, str1 };

      string text = "An update is available! This update is required to connect to Xbox LIVE. Update now?";
      /*switch (Game.game_work.language)
      {
        case StringTableUtils.Language.LANGUAGE_FRENCH:
          text = "Une mise à jour est disponible ! Cette mise à jour est requise pour se connecter à Xbox LIVE. Mettre à jour dès maintenant ?";
          break;
        case StringTableUtils.Language.LANGUAGE_SPANISH:
          text = "¡Hay una actualización disponible! Esta actualización es necesaria para conectarse a Xbox LIVE. ¿Actualizar ahora?";
          break;
        case StringTableUtils.Language.LANGUAGE_GERMAN:
          text = "Ein Update ist verfügbar! Für dieses Update muss eine Verbindung zu Xbox LIVE hergestellt werden. Jetzt updaten?";
          break;
        case StringTableUtils.Language.LANGUAGE_ITALIAN:
          text = "È disponibile un aggiornamento! Questo aggiornamento è necessario per connettersi a Xbox LIVE. Aggiorna ora?";
          break;
        default:
          text = "An update is available! This update is required to connect to Xbox LIVE. Update now?";
          break;
      }*/
      //while (Guide.IsVisible)
      //  Thread.Sleep(32);
      //Guide.BeginShowMessageBox(title, text, (IEnumerable<string>) buttons, 0, MessageBoxIcon.Alert, new AsyncCallback(TheGame.GameUpdateCallback), (object) null);
    }


    // GameUpdateCallback(
    private static void GameUpdateCallback(IAsyncResult result)
    {
      try
      {
        //int? nullable = Guide.EndShowMessageBox(result);
        //if (!nullable.HasValue || !nullable.HasValue || nullable.Value != 1)
        //  return;
        /*
        if (Game.isWP7TrialMode())
        {
          //if (Guide.IsVisible)
          //  Thread.Sleep(128);
          //Guide.ShowMarketplace((PlayerIndex) 0);
        }
        else
          //new MarketplaceDetailTask()
          //{
          //  ContentType = ((MarketplaceContentType) 1)
          //}.Show();
        */
      }
      catch
      {
      }
   }//GameUpdateCallback(


   // Draw
   protected override void Draw(GameTime gameTime)
   {
      if (this.IsActive)
      {
        if (Game1.instance.IsActive)
        {
          if (Game1.switchLanguage)
          {
            DisplayManager.GetInstance().currentTexture = (Texture) null;
            Game1.switchLanguage = false;
            //Game.game_work.language = TheGame.switchToLanguage;
            switch (Game1.switchToLanguage)
            {
              case StringTableUtils.Language.LANGUAGE_ENGLISH:
                StringManager.GetInstance().SetDefaultLanguage("english_us");
                break;
              case StringTableUtils.Language.LANGUAGE_FRENCH:
                StringManager.GetInstance().SetDefaultLanguage("french");
                break;
              case StringTableUtils.Language.LANGUAGE_SPANISH:
                StringManager.GetInstance().SetDefaultLanguage("spanish");
                break;
              case StringTableUtils.Language.LANGUAGE_GERMAN:
                StringManager.GetInstance().SetDefaultLanguage("german");
                break;
              case StringTableUtils.Language.LANGUAGE_ITALIAN:
                StringManager.GetInstance().SetDefaultLanguage("italian");
                break;
            }
            Game1.instance.stringTable.UpdateDefaultLanguage();
            Game1.SaveConfig();
            //TextureManager.GetInstance().ReloadLocalisedTextures((int) Game.game_work.language);
          }
          if (this.showUpsell && this.upsell != null)
          {
            this.spriteBatch.Begin((SpriteSortMode) 1, BlendState.NonPremultiplied);
            this.spriteBatch.Draw(this.upsell.intex, new Rectangle(0, 0, 800, 480), new Rectangle?(new Rectangle(0, 0, 800, 480)), Color.White);
            if (this.inExit)
              this.spriteBatch.Draw(this.menu_exit.intex, new Vector2(this.pos1.X - (float) (this.menu_exit.intex.Width / 10), this.pos1.Y - (float) (this.menu_exit.intex.Height / 10)), new Rectangle?(), Color.White, 0.0f, Vector2.Zero, new Vector2(1.2f, 1.2f), (SpriteEffects) 0, 0.0f);
            else
              this.spriteBatch.Draw(this.menu_exit.intex, this.pos1, Color.White);
            if (this.inPurchase)
              this.spriteBatch.Draw(this.menu_purchase.intex, new Vector2(this.pos2.X - (float) (this.menu_purchase.intex.Width / 10), this.pos2.Y - (float) (this.menu_purchase.intex.Height / 10)), new Rectangle?(), Color.White, 0.0f, Vector2.Zero, new Vector2(1.2f, 1.2f), (SpriteEffects) 0, 0.0f);
            else
              this.spriteBatch.Draw(this.menu_purchase.intex, this.pos2, Color.White);
            this.spriteBatch.End();
            return;
          }
          if ((double) this.timeout > 0.0)
          {
            this.spriteBatch.Begin();

            // RnD / TEMP: this.bob != null
            if ((double) this.timeout < 0.25 && this.bob != null)
              this.spriteBatch.Draw(this.bob.intex, new Vector2(0.0f, 0.0f), new Rectangle?(), 
                  Color.White, MathHelper.ToRadians(0.0f), new Vector2(0.0f, 0.0f), new Vector2(1.66666663f, 1.5f), (SpriteEffects) 0, 0.0f);
            else
              this.spriteBatch.Draw(this.bobsCousinIt, new Vector2(0.0f, 0.0f), Color.White);
            this.fm.Draw(this.graphics.GraphicsDevice, this.spriteBatch);
            this.spriteBatch.End();
            return;
          }
          if (Game1.exceptionThrown)
          {
            base.Draw(gameTime);
            return;
          }
          
          //try
          {
            if (this.load_failed || this.init_failed || this.quit_game)
              return;
            if (Game1.initupdates == 0)
            {
              if ((double) Game1.loadinScreen < 5.0)
                Game1.loadinScreen += 0.0333333351f;
              TouchCollection state = TouchPanel.GetState();
              foreach (TouchLocation touchLocation in state)
              {
                if (touchLocation.State == TouchLocationState.Pressed
                                    || touchLocation.State == TouchLocationState.Released)
                  Mortar.Touch.instance.__UpdateInternal((uint) touchLocation.Id, true, 
                      touchLocation.Position.X, touchLocation.Position.Y, 0.0f);
                else if ( touchLocation.State == TouchLocationState.Moved || touchLocation.State == null)
                  Mortar.Touch.instance.__UpdateInternal((uint) touchLocation.Id, false,
                      touchLocation.Position.X, 
                      touchLocation.Position.Y, 0.0f);
              }
              Mortar.Touch.instance.Update();
              float dt = 0.0f;
              if (!SystemManager.GetInstance().Update(ref dt))
              {
                Game1.fgame.End();
                try
                {
                  MediaPlayer.IsMuted = false;
                }
                catch
                {
                }
                Game1.exitingGame = true;
                this.Exit();
              }
              dt = 0.0333333351f * Game1.dtCo;
              Game1.fgame.Update(dt);
              AchievementManager.Update(gameTime);
              this.BackButtonWasPressed = false;
              float timeSinceLastUpdate = 0.0333333351f * Game1.dtCo;
              DisplayManager.GetInstance().BeginFrame();
              Game1.fgame.Draw(timeSinceLastUpdate);
              //if (Game.isWP7TrialMode())
              //{
              //  this.spriteBatch.Begin();
              //  AchievementManager.UpdatePretendAchievements(this.spriteBatch);
              //  this.spriteBatch.End();
              //}
              this.spriteBatch.Begin();
              AchievementManager.UpdateUnlockDropdown(this.spriteBatch);
              this.spriteBatch.End();
              DisplayManager.GetInstance().EndFrame();
              DisplayManager.GetInstance().SwapBuffers();
            }
            if ((double) Game1.loadinScreen < 0.5)
            {
              float num = Game1.loadinScreen * 2f;
              this.spriteBatch.Begin((SpriteSortMode) 1, BlendState.NonPremultiplied);
              this.spriteBatch.Draw(this.bob.intex, new Vector2(400f, 240f), 
                  new Rectangle?(), new Color(1f, 1f, 1f, 1f - num),
                  MathHelper.ToRadians(0.0f), new Vector2(240f, 160f),
                  Vector2.Multiply(new Vector2(1.66666663f, 1.5f), 1f + num),
                  (SpriteEffects) 0, 0.0f);
              this.spriteBatch.End();
            }
            base.Draw(gameTime);
            return;
          }
          /*catch (Exception ex)
          {
            this.GameException();
            Exception innerException = ex.InnerException;
            while (innerException != null)
              innerException = innerException.InnerException;
            string text = TheGame.ERROR_MESSAGE_GAME();
            string[] buttons = new string[1]
            {
              this.ERROR_BUTTON_1
            };
            //while (Guide.IsVisible)
            //  Thread.Sleep(32);
            //if (text.Length > (int) byte.MaxValue)
            //  text = text.Substring(0, 250);
            //Guide.BeginShowMessageBox(this.ERROR_TITLE_1, text, (IEnumerable<string>) buttons, 0, MessageBoxIcon.Alert, new AsyncCallback(this.IngameException), (object) null);
            return;
          }*/
        }
      }
      try
      {
        if (Game1.m_fruitTex != null && Game1.m_ninjaTex != null && Game1.m_headTex != null)
        {
          if (Game1.m_bodyTex != null)
            goto label_6;
        }
        Game1.m_fruitTex = this.Content.Load<Texture2D>("textureswp7/fruit_text.tga");
        Game1.m_ninjaTex = this.Content.Load<Texture2D>("textureswp7/ninja_text.tga");
        Game1.m_headTex = this.Content.Load<Texture2D>("textureswp7/sensei_head_02.tga");
        Game1.m_bodyTex = this.Content.Load<Texture2D>("textureswp7/sensei_body_02.tga");
      }
      catch
      {
        Game1.m_fruitTex = (Texture2D) null;
        Game1.m_ninjaTex = (Texture2D) null;
        Game1.m_headTex = (Texture2D) null;
        Game1.m_bodyTex = (Texture2D) null;
      }
label_6:
      this.GraphicsDevice.Clear(Color.Black);
      if (Game1.m_fruitTex != null && Game1.m_ninjaTex != null && Game1.m_headTex != null && Game1.m_bodyTex != null && this.spriteBatch != null)
      {
        this.spriteBatch.Begin((SpriteSortMode) 1, BlendState.NonPremultiplied);
        this.spriteBatch.Draw(Game1.m_fruitTex, new Rectangle(280, 260, Game1.m_fruitTex.Width, Game1.m_fruitTex.Height), Color.White);
        this.spriteBatch.Draw(Game1.m_ninjaTex, new Rectangle(240 + Game1.m_fruitTex.Width + 40, 310, Game1.m_ninjaTex.Width, Game1.m_ninjaTex.Height), Color.White);
        this.spriteBatch.Draw(Game1.m_bodyTex, new Rectangle(80, 220, Game1.m_bodyTex.Width, Game1.m_bodyTex.Height), Color.White);
        this.spriteBatch.Draw(Game1.m_headTex, new Rectangle(156, 244, Game1.m_headTex.Width, Game1.m_headTex.Height), Color.White);
        this.spriteBatch.End();
      }
      base.Draw(gameTime);
    }

    private void GameException() => Game1.exceptionThrown = true;

    protected override void OnExiting(object sender, EventArgs args)
    {
      try
      {
        if (!this.load_failed && !this.init_failed)
        {
          if (!this.quit_game)
          {
            try
            {
              GameTask.SaveCurrentData(true);
            }
            catch
            {
              if ((double) this.timeout < 9.9999999747524271E-07)
                throw;
            }
          }
        }
        base.OnExiting(sender, args);
      }
      catch (Exception ex)
      {
        Exception innerException = ex.InnerException;
        while (innerException != null)
          innerException = innerException.InnerException;
        string text = Game1.ERROR_MESSAGE(2147495936U);
        string[] buttons = new string[1]
        {
          this.ERROR_BUTTON_1
        };
        //while (Guide.IsVisible)
        //  Thread.Sleep(32);
        //if (text.Length > (int) byte.MaxValue)
        //  text = text.Substring(0, 250);
        //Guide.BeginShowMessageBox(this.ERROR_TITLE_1, text, (IEnumerable<string>) buttons, 0, MessageBoxIcon.Alert, new AsyncCallback(this.InitOrLoadFailedCallback), (object) null);
      }
    }//OnExiting

    protected override void OnActivated(object sender, EventArgs args)
    {
      try
      {
        //Game.SetTrialModeState();
        //Thread.Sleep(70);
        //Game.SetTrialModeState();
        SoundManager.GetInstance().SetSFXVolume(SoundDef.DEFAULT_SFX_VOL);
       //(Game.game_work.soundEnabled ? SoundDef.DEFAULT_SFX_VOL : 0.0f);
        base.OnActivated(sender, args);
      }
      catch (Exception ex)
      {
        Exception innerException = ex.InnerException;
        while (innerException != null)
          innerException = innerException.InnerException;
        string text = Game1.ERROR_MESSAGE(2147500032U);
        string[] buttons = new string[1]
        {
          this.ERROR_BUTTON_1
        };
        //while (Guide.IsVisible)
        //  Thread.Sleep(32);
        //if (text.Length > (int) byte.MaxValue)
        //  text = text.Substring(0, 250);
        //Guide.BeginShowMessageBox(this.ERROR_TITLE_1, text, (IEnumerable<string>) buttons, 0, MessageBoxIcon.Alert, new AsyncCallback(this.InitOrLoadFailedCallback), (object) null);
      }
    }//OnActivated

        
    // OnActivate
    public void OnActivate()
    {
        //if ((double)Game.game_work.gameOverTransition != 0.0)
        //    return;
        //if (Game.game_work.hud != null)
        //    Game.game_work.hud.OnPause();
        GameTask.SkipToPause(false);
    }

    protected override void OnDeactivated(object sender, EventArgs args)
    {
      try
      {
        SoundManager.GetInstance().SetSFXVolume(0.0f);
        this.OnActivate();//Game.OnActivate();
        base.OnDeactivated(sender, args);
        Game1.SaveConfig();
      }
      catch (Exception ex)
      {
        Exception innerException = ex.InnerException;
        while (innerException != null)
          innerException = innerException.InnerException;
        string text = Game1.ERROR_MESSAGE(2147504128U);
        string[] buttons = new string[1]
        {
          this.ERROR_BUTTON_1
        };
        //while (Guide.IsVisible)
        //  Thread.Sleep(32);
        //if (text.Length > (int) byte.MaxValue)
        //  text = text.Substring(0, 250);
        //Guide.BeginShowMessageBox(this.ERROR_TITLE_1, text, (IEnumerable<string>) buttons, 0, MessageBoxIcon.Alert, new AsyncCallback(this.InitOrLoadFailedCallback), (object) null);
      }
    }//OnDeactivated


    // GamerSignedInCallback
    /*protected void GamerSignedInCallback(object sender, SignedInEventArgs args)
    {
      uint num = 2147508225;
      try
      {
        //Game.SetTrialModeState();
        num = 2147508226U;
        SignedInGamer gamer = args.Gamer;
        if (gamer != null)
        {
          if (gamer.IsSignedInToLive)
          {
            if (!this.getAchievements)
            {
              this.getAchievements = true;
              gamer.BeginGetAchievements(new AsyncCallback(AchievementManager.GetAchievementsCallback), (object) gamer);
            }
            if (gamer.Privileges.AllowProfileViewing != GamerPrivilegeSetting.Blocked && gamer.Privileges.AllowUserCreatedContent != GamerPrivilegeSetting.Blocked)
            {
              num = 2147508227U;
              try
              {
                Game1.picture = Texture2D.FromStream(this.graphics.GraphicsDevice, gamer.GetProfile().GetGamerPicture());
              }
              catch
              {
                Game1.picture = (Texture2D) null;
              }
            }
            Game1.logInSucceeded = true;
          }
          else
          {
            if (this.getAchievements)
              return;
            this.getAchievements = true;
            gamer.BeginGetAchievements(new AsyncCallback(AchievementManager.GetAchievementsCallback), (object) gamer);
          }
        }
        else
        {
          num = 2147508228U;
          throw new Exception("Failed to get gamer");
        }
      }
      catch (Exception ex)
      {
        Exception innerException = ex.InnerException;
        while (innerException != null)
          innerException = innerException.InnerException;
        string text = Game1.instance.stringTable.GetString(867) + "\n 0x" + num.ToString("X");
        string[] buttons = new string[1]
        {
          this.ERROR_BUTTON_1
        };
        //while (Guide.IsVisible)
        //  Thread.Sleep(32);
        //if (text.Length > (int) byte.MaxValue)
        //  text = text.Substring(0, 250);
        //if (num == 2147508225U)
        //  Guide.BeginShowMessageBox(this.ERROR_TITLE_1, text, (IEnumerable<string>) buttons, 0, MessageBoxIcon.Alert, new AsyncCallback(this.InitOrLoadFailedCallback), (object) null);
        //else
        //  Guide.BeginShowMessageBox(this.ERROR_TITLE_1, text, (IEnumerable<string>) buttons, 0, MessageBoxIcon.Alert, (AsyncCallback) null, (object) null);
        Game1.logInSucceeded = false;
      }
    }*/

    // NotSignedInMessage
    public static string NotSignedInMessage()
    {
      string str = "You are not signed in to Windows live.";
     /* switch (Game.game_work.language)
      {
        case StringTableUtils.Language.LANGUAGE_FRENCH:
          str = "Vous n'êtes pas connecté à Windows live.";
          break;
        case StringTableUtils.Language.LANGUAGE_SPANISH:
          str = "No has iniciado sesión en Windows Live.";
          break;
        case StringTableUtils.Language.LANGUAGE_GERMAN:
          str = "Du bist nicht auf Windows Live angemeldet.";
          break;
        case StringTableUtils.Language.LANGUAGE_ITALIAN:
          str = "Non hai effettuato l'accesso a Windows live.";
          break;
        default:
          str = "You are not signed in to Windows live.";
          break;
      }*/
      return str;
    }

    internal void Exit()
    {
       //
    }

    public class Settings
    {
      public int magic1;
      public int magic2;
      public int language;
      public int week;
      public int tf;
      public int aw;
      public int bestThisWeek;
      public uint[] achievements;

      public Settings()
      {
        this.magic1 = 195800833;
        this.magic2 = 33479857;
        this.language = 1;
        this.week = -1;
        this.tf = 0;
        this.aw = 0;
        this.bestThisWeek = 0;
        this.achievements = new uint[/*Game.MAX_SYSTEM_ACHIEVEMENTS*/1];
        for (int index = 0; index < /*Game.MAX_SYSTEM_ACHIEVEMENTS*/1; ++index)
          this.achievements[index] = 0U;
      }
    }

    public struct inputInfo
    {
      public int updateStamp;
      public uint tid;
      public bool dn;
      public float x;
      public float y;
    }

    public class MyTouchState
    {
      public Vector2 lastPoint;
      public Vector2 midPoint1;
      public Vector2 midPoint2;
      public Vector2 midPoint3;
      public Vector2 movedToPoint;
      public int id;
      public bool MovedThisFrame;
      public bool deletedThisFrame;
      public int moves;
    }
  }

    public class GamerServicesComponent
    {
        internal bool Enabled;
        private Game game;

        public GamerServicesComponent(Game game)
        {
            this.game = game;
        }
    }
}
