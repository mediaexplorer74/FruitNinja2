
// Type: Mortar.TextureManager
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using GameManager;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
//using WPR.WindowsCompability;

#nullable disable
namespace Mortar
{
  public class TextureManager
  {
    private static TextureManager instance;
    private static List<Texture> loadedTextures = new List<Texture>();

    public static TextureManager GetInstance()
    {
      if (TextureManager.instance == null)
        TextureManager.instance = new TextureManager();
      return TextureManager.instance;
    }

    public void Initialise()
    {
    }

    public void Initialise(int heapsize)
    {
    }

    public Texture Load(string texture)
    {
      Texture texture1 = Texture.Load(texture);
      if (texture1 != null)
        TextureManager.loadedTextures.Add(texture1);
      return texture1;
    }

    public Texture Load(string texture, bool localise)
    {
      string Filename;
      switch (Game.game_work.language)
      {
        case StringTableUtils.Language.LANGUAGE_ENGLISH:
        case StringTableUtils.Language.LANGUAGE_ENGLISH_UK:
          Filename = "localisedwp7/en/" + texture;
          break;
        case StringTableUtils.Language.LANGUAGE_FRENCH:
          Filename = "localisedwp7/fr/" + texture;
          break;
        case StringTableUtils.Language.LANGUAGE_SPANISH:
          Filename = "localisedwp7/es/" + texture;
          break;
        case StringTableUtils.Language.LANGUAGE_GERMAN:
          Filename = "localisedwp7/de/" + texture;
          break;
        case StringTableUtils.Language.LANGUAGE_ITALIAN:
          Filename = "localisedwp7/it/" + texture;
          break;
        default:
          Filename = "localisedwp7/en/" + texture;
          break;
      }
      Texture texture1 = Texture.Load(Filename);
      if (texture1 != null)
      {
        TextureManager.loadedTextures.Add(texture1);
        texture1.localise = localise;
        texture1.texture_filename = texture;
      }
      return texture1;
    }

    public void ReloadLocalisedTextures(int language)
    {
      foreach (Texture loadedTexture in TextureManager.loadedTextures)
      {
        if (loadedTexture.localise)
          loadedTexture.intex = (Texture2D) null;
      }
      //GC2.Collect();
      List<Texture>.Enumerator enumerator = TextureManager.loadedTextures.GetEnumerator();
      enumerator.MoveNext();
      while (enumerator.Current != null)
      {
        Texture current = enumerator.Current;
        if (current.localise)
        {
          string Filename;
          switch (language)
          {
            case 0:
            case 1:
              Filename = "localisedwp7/en/" + current.texture_filename;
              break;
            case 2:
              Filename = "localisedwp7/fr/" + current.texture_filename;
              break;
            case 3:
              Filename = "localisedwp7/es/" + current.texture_filename;
              break;
            case 4:
              Filename = "localisedwp7/de/" + current.texture_filename;
              break;
            case 5:
              Filename = "localisedwp7/it/" + current.texture_filename;
              break;
            default:
              Filename = "localisedwp7/en/" + current.texture_filename;
              break;
          }
          Texture.Reload(Filename, current);
        }
        enumerator.MoveNext();
      }
    }
  }
}
