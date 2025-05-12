
// Type: Mortar.SoundManager
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using GameManager;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using SoundEffect = Microsoft.Xna.Framework.Audio.SoundEffect;
using System.Collections.Generic;

#nullable disable
namespace Mortar
{
  public class SoundManager
  {
    public float soundFadeOut;
    private Dictionary<string, SoundEffect> sfxs = new Dictionary<string, SoundEffect>();
    private LinkedList<SoundEffectInstance> sysManagedSfx = new LinkedList<SoundEffectInstance>();
    private static float sfx_volume = 1f;
    private static float mus_volume = 1f;
    private static SoundManager instance = new SoundManager();
    private bool customMusic;
    private bool allowMusic;
    private Song currentSong;

    public static int SOUND_MANAGER_HEAP_SIZE => 524288;

    public static byte DEFAULT_NOTE => 64;

    public static SoundManager GetInstance() => SoundManager.instance;

    public void Initialise(string project)
    {
      this.Initialise(project, SoundManager.SOUND_MANAGER_HEAP_SIZE);
    }

    public void CacheSFX()
    {
      try
      {
        string str = Game1.instance.Content.Load<string>("sfxCacheFile.txt");
        char[] separator = new char[1]{ ',' };
        foreach (string key in str.Split(separator, StringSplitOptions.RemoveEmptyEntries))
        {
          SoundEffect soundEffect = (SoundEffect) null;
          try
          {
            soundEffect = Game1.instance.Content.Load<SoundEffect>("sound/" + key + ".wav");
          }
          catch
          {
          }
          this.sfxs.Add(key, soundEffect);
        }
      }
      catch
      {
      }
    }

    public void Initialise(string project, int heap_size) => this.CacheSFX();

    public void Destroy()
    {
    }

    public void SFXPlay(int index) => this.SFXPlay(index, 0U);

    public void SFXPlay(int index, uint flags) => this.SFXPlay(index, flags, (MortarSound) null);

    public void SFXPlay(int index, uint flags, MortarSound snd)
    {
      this.SFXPlay(index, flags, snd, SoundManager.DEFAULT_NOTE);
    }

    public void SFXPlay(int index, uint flags, MortarSound snd, byte note)
    {
      this.SFXPlay(index, flags, snd, note, -1);
    }

    public void SFXPlay(int index, uint flags, MortarSound snd, byte note, int pitch)
    {
      throw new MissingMethodException();
    }

    public MortarSound SFXPlay(string filename)
    {
      this.SFXPlay(filename, 0U);
      return new MortarSound();
    }

    public void SFXPlay(string filename, uint flags)
    {
      this.SFXPlay(filename, flags, (MortarSound) null);
    }

    public void SFXPlay(string filename, uint flags, MortarSound snd)
    {
      this.SFXPlay(filename, flags, snd, SoundManager.DEFAULT_NOTE);
    }

    public void SFXPlay(string filename, uint flags, MortarSound snd, byte note)
    {
      this.SFXPlay(filename, flags, snd, note, -1);
    }

    public void SFXPlay(string filename, uint flags, MortarSound snd, byte note, int pitch)
    {
      SoundEffect soundEffect;
      if (!this.sfxs.TryGetValue(filename, out soundEffect))
      {
        soundEffect = (SoundEffect) null;
        try
        {
          soundEffect = Game1.instance.Content.Load<SoundEffect>("sound/" + filename + ".wav");
        }
        catch
        {
        }
        this.sfxs.Add(filename, soundEffect);
      }
      if (soundEffect == null)
        return;
      SoundEffectInstance instance = soundEffect.CreateInstance();
      if (snd != null)
        snd.inst = instance;
      else
        this.sysManagedSfx.AddLast(instance);
      if (!Game2.game_work.soundEnabled)
        return;
      instance.Volume = SoundManager.sfx_volume;
      instance.Play();
    }

    public void SFXStop(int index)
    {
    }

    public void SFXStop(string filename)
    {
    }

    public void SFXPause(int index)
    {
    }

    public void SFXPause(string filename)
    {
    }

    public void SFXPauseAll()
    {
    }

    public static MortarSound CreateNewSound() => new MortarSound();

    public void SongPlay(int index) => throw new MissingMethodException();

    public void AllowMusic()
    {
      this.allowMusic = true;
      MediaPlayer.Stop();
    }

    public bool CustomMusic
    {
      get => this.customMusic;
      set => this.customMusic = value;
    }

    private void playafuckingsong(string name)
    {
      this.currentSong = Game1.instance.Content.Load<Song>(name);
      SoundManager.mus_volume = 0.01f;
      MediaPlayer.Volume = SoundManager.mus_volume;
      MediaPlayer.Play(this.currentSong);
      MediaPlayer.IsRepeating = true;
    }

    public void SongPlay(string filename)
    {
      if (!this.allowMusic)
        return;
      try
      {
        this.playafuckingsong("music/" + filename + ".wma");
      }
      catch
      {
        this.currentSong = (Song) null;
        try
        {
          this.playafuckingsong("music/" + filename);
        }
        catch
        {
          this.currentSong = (Song) null;
        }
      }
    }

    public bool UserPlayingMusic() => MediaPlayer.State == MediaState.Playing;

    public void SongSwitchPlay(string filename)
    {
    }

    public void SongStop()
    {
    }

    public void SongPause()
    {
    }

    public void SongResume()
    {
    }

    public void SongSetStreaming(bool streaming)
    {
    }

    public void SongSetMemorySize(int size)
    {
    }

    public void MuteSound() => this.MuteSound(true);

    public void MuteSound(bool mute)
    {
    }

    public void Update(float dt)
    {
      LinkedListNode<SoundEffectInstance> next;
      for (LinkedListNode<SoundEffectInstance> node = this.sysManagedSfx.First; 
                node != null;
                node = next)
      {
        next = node.Next;
        if (node.Value.State == SoundState.Stopped)
          this.sysManagedSfx.Remove(node);
      }
      if (this.allowMusic)
      {
        try
        {
          if (MediaPlayer.IsMuted != !Game2.game_work.musicEnabled)
            MediaPlayer.IsMuted = !Game2.game_work.musicEnabled;
        }
        catch
        {
        }
      }
      if (this.currentSong == null || !this.allowMusic || (double)SoundManager.mus_volume >= 1.0)
        return;
      SoundManager.mus_volume += 0.0333333351f;
      if ((double) SoundManager.mus_volume > 1.0)
        SoundManager.mus_volume = 1f;
      MediaPlayer.Volume = SoundManager.mus_volume;
    }

    public void Draw(SpriteBatch batch)
    {
    }

    public void Release(MortarSound obj)
    {
    }

    public void ReleaseAll()
    {
    }

    public int GetNumTextures() => 0;

    public int GetHeapSize() => 0;

    public int GetHeapFree() => 0;

    public int GetHeapLargestBlock() => 0;

    public void DisplayUsage() => this.DisplayUsage(false);

    public void DisplayUsage(bool full)
    {
    }

    public void SetSFXVolume(float amount) => SoundManager.sfx_volume = amount;

    public float GetSFXVolume() => SoundManager.sfx_volume;

    public void SetMusicVolume(float amount)
    {
    }

    public float GetMusicVolume() => -1f;
  }
}
