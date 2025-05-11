
// Type: Mortar.ProfileManager
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Mortar
{
  internal class ProfileManager
  {
    private static LinkedList<ProfileManager.ProfileEntry> entries = new LinkedList<ProfileManager.ProfileEntry>();

    [Conditional("DEBUG")]
    public static void Begin(string name)
    {
      ProfileManager.ProfileEntry profileEntry = (ProfileManager.ProfileEntry) null;
      foreach (ProfileManager.ProfileEntry entry in ProfileManager.entries)
      {
        if (string.Compare(name, entry.name) == 0)
        {
          profileEntry = entry;
          break;
        }
      }
      if (profileEntry == null)
      {
        profileEntry = new ProfileManager.ProfileEntry(name);
        ProfileManager.entries.AddLast(profileEntry);
      }
      profileEntry.Begin();
    }

    [Conditional("DEBUG")]
    public static void End(string name)
    {
      ProfileManager.ProfileEntry profileEntry = (ProfileManager.ProfileEntry) null;
      foreach (ProfileManager.ProfileEntry entry in ProfileManager.entries)
      {
        if (string.Compare(name, entry.name) == 0)
        {
          profileEntry = entry;
          break;
        }
      }
      profileEntry?.End();
    }

    [Conditional("DEBUG")]
    public static void FrameEnd()
    {
      foreach (ProfileManager.ProfileEntry entry in ProfileManager.entries)
      {
        entry.Update();
        entry.Reset();
      }
    }

    [Conditional("DEBUG")]
    public static void ShowData()
    {
      foreach (ProfileManager.ProfileEntry entry in ProfileManager.entries)
        entry.Write();
    }

    [Conditional("DEBUG")]
    public static void Reset()
    {
      foreach (ProfileManager.ProfileEntry entry in ProfileManager.entries)
        entry.Init();
    }

    private class ProfileEntry
    {
      public string name;
      public long timeThisFrame;
      public long count;
      public long total;
      public long calls;
      public long callsMax;
      private Stopwatch sw;

      public ProfileEntry(string _name)
      {
        this.name = _name;
        this.sw = new Stopwatch();
        this.Init();
      }

      public void Begin()
      {
        if (this.sw.IsRunning)
          this.sw.Stop();
        this.sw.Reset();
        this.sw.Start();
        ++this.count;
        ++this.calls;
      }

      public void End()
      {
        if (!this.sw.IsRunning)
          return;
        this.sw.Stop();
        this.timeThisFrame += this.sw.Elapsed.Ticks;
        this.total += this.sw.Elapsed.Ticks;
      }

      public void Update()
      {
      }

      public void Write()
      {
      }

      public void Reset()
      {
        if (this.calls > this.callsMax)
          this.callsMax = this.calls;
        this.timeThisFrame = 0L;
        this.calls = 0L;
      }

      public void Init()
      {
        this.timeThisFrame = 0L;
        this.count = 1L;
        this.total = 0L;
        this.calls = 0L;
        this.callsMax = 0L;
      }
    }
  }
}
