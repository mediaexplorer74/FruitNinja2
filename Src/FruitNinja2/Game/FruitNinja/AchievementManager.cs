
// Type: GameManager.AchievementManager
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Mortar;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

#nullable disable
namespace GameManager
{
  public class AchievementManager
  {
    private static AchievementCollection achievements;
    private static object achievementsLockObject = new object();
    private static int awardMode = 0;
    private static int y;
    private static int delay;
    private static int unlockMode = 0;
    private static int uy;
    private static int udelay;
    private static int utype;
    private static int awardDelay = 30000;
    private static bool awarding = false;
    private static string[] achivementKeys = new string[20]
    {
      "Great Fruit Ninja",
      "Ultimate Fruit Ninja",
      "Fruit Fight",
      "Fruit Blitz",
      "Fruit Frenzy",
      "Fruit Rampage",
      "Fruit Annihilation",
      "Lucky Ninja",
      "Almost A Century",
      "Mango Magic",
      "Deja Vu",
      "Combo Mambo",
      "Moment Of Zen",
      "Year Of The Dragon",
      "The Lovely Bunch",
      "Its All Pear Shaped",
      "Wake Up",
      "Underachiever",
      "Overachiever",
      "Bomb Magnet"
    };
    private static int achievementIndex = 0;
    public static LinkedList<int> akeys = new LinkedList<int>();
    private static AchievementData[] str_en = new AchievementData[20]
    {
      new AchievementData("Got a critical hit with a Mango", "Mango Magic"),
      new AchievementData("Killed 4 of the same type of fruit in a row in Classic Mode", "Deja Vu"),
      new AchievementData("Sliced the secret fruit!", "Year Of The Dragon"),
      new AchievementData("Got the Lovely Bunch star in Zen Mode", "The Lovely Bunch"),
      new AchievementData("Killed 3 Pears in a row in Classic Mode", "Its All Pear Shaped"),
      new AchievementData("Got a score of less than 20 after all bonuses had been awarded", "Underachiever"),
      new AchievementData("Got a score of over 400 after bonuses", "Overachiever"),
      new AchievementData("Hit three bombs and score over 250 points after bonuses", "Bomb Magnet"),
      new AchievementData("Got 6 criticals in one round in Classic Mode", "Lucky Ninja"),
      new AchievementData("Killed 10000 Fruit Total", "Fruit Annihilation"),
      new AchievementData("Killed 5000 Fruit Total", "Fruit Rampage"),
      new AchievementData("Got a score of 200 in Classic Mode", "Ultimate Fruit Ninja"),
      new AchievementData("Got a score of 99", "Almost A Century"),
      new AchievementData("Killed 1000 Fruit Total", "Fruit Frenzy"),
      new AchievementData("Sliced 6 fruit in one combo", "Combo Mambo"),
      new AchievementData("Killed 500 Total Fruit", "Fruit Blitz"),
      new AchievementData("Achieved a score of 200 in Zen Mode", "Moment Of Zen"),
      new AchievementData("Killed 150 Fruit Total", "Fruit Fight"),
      new AchievementData("Got a score of 100 in Classic Mode", "Great Fruit Ninja"),
      new AchievementData("Got a score of 0", "Wake Up")
    };
    private static AchievementData[] str_fr = new AchievementData[20]
    {
      new AchievementData("Coup critique obtenu avec une mangue", "Mango Magic"),
      new AchievementData("4 fruits du même type éliminés à la suite en mode Classique.", "Deja Vu"),
      new AchievementData("Fruit secret tranché !", "Year Of The Dragon"),
      new AchievementData("Étoile Jolie Grappe obtenue en mode Zen", "The Lovely Bunch"),
      new AchievementData("3 poires éliminées à la suite en mode Classique", "Its All Pear Shaped"),
      new AchievementData("Score de moins de 20 obtenu après tous les bonus en mode Arcade.", "Underachiever"),
      new AchievementData("Score de plus de 400 obtenu après tous les bonus en mode Arcade.", "Overachiever"),
      new AchievementData("Trois bombes touchées et score de plus de 250 obtenu après tous les bonus en mode Arcade.", "Bomb Magnet"),
      new AchievementData("6 critiques obtenus en un round en mode Classique", "Lucky Ninja"),
      new AchievementData("10 000 fruits éliminés au total", "Fruit Annihilation"),
      new AchievementData("5000 fruits éliminés au total", "Fruit Rampage"),
      new AchievementData("Score de 200 obtenu en Mode Classique", "Ultimate Fruit Ninja"),
      new AchievementData("Score de 99 obtenu", "Almost A Century"),
      new AchievementData("1000 fruits éliminés au total", "Fruit Frenzy"),
      new AchievementData("6 fruits tranchés en une combo", "Combo Mambo"),
      new AchievementData("500 fruits éliminés au total", "Fruit Blitz"),
      new AchievementData("Score de 200 obtenu en mode Zen", "Moment Of Zen"),
      new AchievementData("150 fruits éliminés au total", "Fruit Fight"),
      new AchievementData("Score de 100 obtenu en Mode Classique", "Great Fruit Ninja"),
      new AchievementData("Score de 0 obtenu", "Wake Up")
    };
    private static AchievementData[] str_de = new AchievementData[20]
    {
      new AchievementData("Einen kritischen Treffer bei einer Mango erzielt", "Mango Magic"),
      new AchievementData("4 Früchte gleicher Art in einer Runde im Klassik-Modus zerstört", "Deja Vu"),
      new AchievementData("Die geheime Frucht zerteilt!", "Year Of The Dragon"),
      new AchievementData("Den \"Süßer Haufen\"-Stern im Zen-Modus erhalten", "The Lovely Bunch"),
      new AchievementData("3 Birnen in einer Runde im Klassik-Modus zerstört", "Its All Pear Shaped"),
      new AchievementData("Weniger als 20 Punkte zusammen mit allen Boni im Arcade-Modus erzielt", "Underachiever"),
      new AchievementData("Mehr als 400 Punkte zusammen mit allen Boni im Arcade-Modus erzielt", "Overachiever"),
      new AchievementData("Drei Bomben getroffen und mehr als 250 Punkte zusammen mit allen Boni im Arcade-Modus erzielt", "Bomb Magnet"),
      new AchievementData("6 kritische Treffer in einer Runde im Klassik-Modus erzielt", "Lucky Ninja"),
      new AchievementData("Insgesamt 10.000 Früchte zerstört", "Fruit Annihilation"),
      new AchievementData("Insgesamt 5000 Früchte zerstört", "Fruit Rampage"),
      new AchievementData("200 Punkte im Klassik-Modus erzielt", "Ultimate Fruit Ninja"),
      new AchievementData("99 Punkte erzielt", "Almost A Century"),
      new AchievementData("Insgesamt 1000 Früchte zerstört", "Fruit Frenzy"),
      new AchievementData("6 Früchte in einer Combo zerteilt", "Combo Mambo"),
      new AchievementData("Insgesamt 500 Früchte zerstört", "Fruit Blitz"),
      new AchievementData("200 Punkte im Zen-Modus erzielt", "Moment Of Zen"),
      new AchievementData("Insgesamt 150 Früchte zerstört", "Fruit Fight"),
      new AchievementData("100 Punkte im Klassik-Modus erzielt", "Great Fruit Ninja"),
      new AchievementData("0 Punkt erzielt", "Wake Up")
    };
    private static AchievementData[] str_it = new AchievementData[20]
    {
      new AchievementData("Hai ottenuto un colpo critico con un mango", "Mango Magic"),
      new AchievementData("Hai ucciso 4 frutti dello stesso tipo di fila, in modalità Classica.", "Deja Vu"),
      new AchievementData("Hai affettato il frutto segreto!", "Year Of The Dragon"),
      new AchievementData("Hai ottenuto la stella Un bel mucchio in modalità Zen", "The Lovely Bunch"),
      new AchievementData("Hai ucciso 3 pere di fila in modalità Classica", "Its All Pear Shaped"),
      new AchievementData("Hai ottenuto un punteggio inferiore a 20 dopo tutti i bonus in modalità Arcade.", "Underachiever"),
      new AchievementData("Hai ottenuto un punteggio inferiore a 400 dopo tutti i bonus in modalità Arcade.", "Overachiever"),
      new AchievementData("Hai colpito tre bombe e ottenuto più di 250 dopo tutti i bonus in modalità Arcade.", "Bomb Magnet"),
      new AchievementData("Hai ottenuto 6 critiche in un turno in modalità Classica", "Lucky Ninja"),
      new AchievementData("Hai ucciso 10000 frutti in totale", "Fruit Annihilation"),
      new AchievementData("Hai ucciso 5000 frutti in totale", "Fruit Rampage"),
      new AchievementData("Hai ottenuto un punteggio pari a 200 in modalità Classica", "Ultimate Fruit Ninja"),
      new AchievementData("Hai ottenuto un punteggio pari a 99", "Almost A Century"),
      new AchievementData("Hai ucciso 1000 frutti in totale", "Fruit Frenzy"),
      new AchievementData("Hai affettato 6 frutti in una combo", "Combo Mambo"),
      new AchievementData("Hai ucciso 500 frutti in totale", "Fruit Blitz"),
      new AchievementData("Hai ottenuto un punteggio pari a 200 in modalità Zen", "Moment Of Zen"),
      new AchievementData("Hai ucciso 150 frutti in totale", "Fruit Fight"),
      new AchievementData("Hai ottenuto un punteggio pari a 100 in modalità Classica", "Great Fruit Ninja"),
      new AchievementData("Hai ottenuto un punteggio pari a 0", "Wake Up")
    };
    private static AchievementData[] str_es = new AchievementData[20]
    {
      new AchievementData("Has conseguido un impacto crítico con un mango", "Mango Magic"),
      new AchievementData("Has matado 4 frutas seguidas del mismo tipo en el modo Clásico", "Deja Vu"),
      new AchievementData("¡Has cortado la fruta secreta!", "Year Of The Dragon"),
      new AchievementData("Has conseguido la estrella de Un grupo encantador en el modo Zen", "The Lovely Bunch"),
      new AchievementData("Has matado 3 peras seguidas en el modo Clásico", "Its All Pear Shaped"),
      new AchievementData("Has conseguido una puntuación inferior a 20 tras todas las bonificaciones en el modo Arcade", "Underachiever"),
      new AchievementData("Has conseguido una puntuación superior a 400 tras todas las bonificaciones en el modo Arcade", "Overachiever"),
      new AchievementData("Hit tres bombas y puntuación de más de 250 puntos después de las bonificaciones", "Bomb Magnet"),
      new AchievementData("Has conseguido 6 críticos en un asalto en el modo Clásico", "Lucky Ninja"),
      new AchievementData("Has matado 10000 frutas en total", "Fruit Annihilation"),
      new AchievementData("Has matado 5000 frutas en total", "Fruit Rampage"),
      new AchievementData("Has conseguido una puntuación de 200 en el modo Clásico", "Ultimate Fruit Ninja"),
      new AchievementData("Has conseguido una puntuación de 99", "Almost A Century"),
      new AchievementData("Has matado 1000 frutas en total", "Fruit Frenzy"),
      new AchievementData("Has cortado 6 frutas en un combo", "Combo Mambo"),
      new AchievementData("Has matado 500 frutas en total", "Fruit Blitz"),
      new AchievementData("Has conseguido una puntuación de 200 en el modo Zen", "Moment Of Zen"),
      new AchievementData("Has matado 150 frutas en total", "Fruit Fight"),
      new AchievementData("Has conseguido una puntuación de 100 en el modo Clásico", "Great Fruit Ninja"),
      new AchievementData("Has conseguido una puntuación de 0", "Wake Up")
    };
    private static AchievementData[] str1_en = new AchievementData[20]
    {
      new AchievementData("Get a critical hit with a Mango", "Mango Magic"),
      new AchievementData("Kill 4 of the same type of fruit in a row in Classic Mode", "Deja Vu"),
      new AchievementData("Slice the secret fruit!", "Year Of The Dragon"),
      new AchievementData("Get the Lovely Bunch star in Zen Mode", "The Lovely Bunch"),
      new AchievementData("Kill 3 Pears in a row in Classic Mode", "Its All Pear Shaped"),
      new AchievementData("Get a score of less than 20 after all bonuses have been awarded", "Underachiever"),
      new AchievementData("Get a score of over 400 after bonuses", "Overachiever"),
      new AchievementData("Hit three bombs and score over 250 points after bonuses", "Bomb Magnet"),
      new AchievementData("Get 6 criticals in one round in Classic Mode", "Lucky Ninja"),
      new AchievementData("Kill 10000 Fruit Total", "Fruit Annihilation"),
      new AchievementData("Kill 5000 Fruit Total", "Fruit Rampage"),
      new AchievementData("Get a score of 200 in Classic Mode", "Ultimate Fruit Ninja"),
      new AchievementData("Get a score of 99", "Almost A Century"),
      new AchievementData("Kill 1000 Fruit Total", "Fruit Frenzy"),
      new AchievementData("Slice 6 fruit in one combo", "Combo Mambo"),
      new AchievementData("Kill 500 Total Fruit", "Fruit Blitz"),
      new AchievementData("Achieve a score of 200 in Zen Mode", "Moment Of Zen"),
      new AchievementData("Kill 150 Fruit Total", "Fruit Fight"),
      new AchievementData("Get a score of 100 in Classic Mode", "Great Fruit Ninja"),
      new AchievementData("Get a score of 0", "Wake Up")
    };
    private static AchievementData[] str1_fr = new AchievementData[20]
    {
      new AchievementData("Obtenez un coup critique avec une mangue", "Mango Magic"),
      new AchievementData("Éliminez 4 fruits du même type à la suite en mode Classique.", "Deja Vu"),
      new AchievementData("Tranchez le fruit secret !", "Year Of The Dragon"),
      new AchievementData("Obtenez l'étoile Jolie Grappe en mode Zen", "The Lovely Bunch"),
      new AchievementData("Éliminez 3 poires à la suite en mode Classique", "Its All Pear Shaped"),
      new AchievementData("Obtenez un score de moins de 20 après tous les bonus en mode Arcade.", "Underachiever"),
      new AchievementData("Obtenez un score de plus de 400 après tous les bonus en mode Arcade.", "Overachiever"),
      new AchievementData("Touchez trois bombes et obtenez un score de plus de 250 après tous les bonus en mode Arcade.", "Bomb Magnet"),
      new AchievementData("Obtenez 6 critiques en un round en mode Classique", "Lucky Ninja"),
      new AchievementData("Éliminez 10 000 fruits au total", "Fruit Annihilation"),
      new AchievementData("Éliminez 5000 fruits au total", "Fruit Rampage"),
      new AchievementData("Obtenez un score de 200 en Mode Classique", "Ultimate Fruit Ninja"),
      new AchievementData("Obtenez un score de 99", "Almost A Century"),
      new AchievementData("Éliminez 1000 fruits au total", "Fruit Frenzy"),
      new AchievementData("Tranchez 6 fruits en une combo", "Combo Mambo"),
      new AchievementData("Éliminez 500 fruits au total", "Fruit Blitz"),
      new AchievementData("Obtenez un score de 200 en mode Zen", "Moment Of Zen"),
      new AchievementData("Éliminez 150 fruits au total", "Fruit Fight"),
      new AchievementData("Obtenez un score de 100 en Mode Classique", "Great Fruit Ninja"),
      new AchievementData("Obtenez un score de 0", "Wake Up")
    };
    private static AchievementData[] str1_de = new AchievementData[20]
    {
      new AchievementData("Erziele einen kritischen Treffer bei einer Mango", "Mango Magic"),
      new AchievementData("Zerstöre 4 Früchte gleicher Art in einer Runde im Klassik-Modus", "Deja Vu"),
      new AchievementData("Zerteile die geheime Frucht!", "Year Of The Dragon"),
      new AchievementData("Erhalte den \"Süßer Haufen\"-Stern im Zen-Modus", "The Lovely Bunch"),
      new AchievementData("Zerstöre 3 Birnen in einer Runde im Klassik-Modus", "Its All Pear Shaped"),
      new AchievementData("Erziele weniger als 20 Punkte zusammen mit allen Boni im Arcade-Modus", "Underachiever"),
      new AchievementData("Erziele mehr als 400 Punkte zusammen mit allen Boni im Arcade-Modus", "Overachiever"),
      new AchievementData("Triff drei Bomben und erziele mehr als 250 Punkte zusammen mit allen Boni im Arcade-Modus", "Bomb Magnet"),
      new AchievementData("Erziele 6 kritische Treffer in einer Runde im Klassik-Modus", "Lucky Ninja"),
      new AchievementData("Zerstöre insgesamt 10.000 Früchte", "Fruit Annihilation"),
      new AchievementData("Zerstöre insgesamt 5000 Früchte", "Fruit Rampage"),
      new AchievementData("Erziele 200 Punkte im Klassik-Modus", "Ultimate Fruit Ninja"),
      new AchievementData("Erziele 99 Punkte", "Almost A Century"),
      new AchievementData("Zerstöre insgesamt 1000 Früchte", "Fruit Frenzy"),
      new AchievementData("Zerteile 6 Früchte in einer Combo", "Combo Mambo"),
      new AchievementData("Zerstöre insgesamt 500 Früchte", "Fruit Blitz"),
      new AchievementData("Erziele 200 Punkte im Zen-Modus", "Moment Of Zen"),
      new AchievementData("Zerstöre insgesamt 150 Früchte", "Fruit Fight"),
      new AchievementData("Erziele 100 Punkte im Klassik-Modus", "Great Fruit Ninja"),
      new AchievementData("Erziele 0 Punkte", "Wake Up")
    };
    private static AchievementData[] str1_it = new AchievementData[20]
    {
      new AchievementData("Ottieni un colpo critico con un mango", "Mango Magic"),
      new AchievementData("Uccidi 4 frutti dello stesso tipo di fila, in modalità Classica.", "Deja Vu"),
      new AchievementData("Affetta il frutto segreto!", "Year Of The Dragon"),
      new AchievementData("Ottieni la stella Un bel mucchio in modalità Zen", "The Lovely Bunch"),
      new AchievementData("Uccidi 3 pere di fila in modalità Classica", "Its All Pear Shaped"),
      new AchievementData("Ottieni un punteggio inferiore a 20 dopo tutti i bonus in modalità Arcade.", "Underachiever"),
      new AchievementData("Ottieni un punteggio inferiore a 400 dopo tutti i bonus in modalità Arcade.", "Overachiever"),
      new AchievementData("Colpisci tre bombe e ottieni più di 250 dopo tutti i bonus in modalità Arcade.", "Bomb Magnet"),
      new AchievementData("Ottieni 6 critiche in un turno in modalità Classica", "Lucky Ninja"),
      new AchievementData("Uccidi 10000 frutti in totale", "Fruit Annihilation"),
      new AchievementData("Uccidi 5000 frutti in totale", "Fruit Rampage"),
      new AchievementData("Ottieni un punteggio pari a 200 in modalità Classica", "Ultimate Fruit Ninja"),
      new AchievementData("Ottieni un punteggio pari a 99", "Almost A Century"),
      new AchievementData("Uccidi 1000 frutti in totale", "Fruit Frenzy"),
      new AchievementData("Affetta 6 frutti in una combo", "Combo Mambo"),
      new AchievementData("Uccidi 500 frutti in totale", "Fruit Blitz"),
      new AchievementData("Ottieni un punteggio pari a 200 in modalità Zen", "Moment Of Zen"),
      new AchievementData("Uccidi 150 frutti in totale", "Fruit Fight"),
      new AchievementData("Ottieni un punteggio pari a 100 in modalità Classica", "Great Fruit Ninja"),
      new AchievementData("Ottieni un punteggio pari a 0", "Wake Up")
    };
    private static AchievementData[] str1_es = new AchievementData[20]
    {
      new AchievementData("Consigue un impacto crítico con un mango", "Mango Magic"),
      new AchievementData("Mata 4 frutas seguidas del mismo tipo en el modo Clásico", "Deja Vu"),
      new AchievementData("¡Corta la fruta secreta!", "Year Of The Dragon"),
      new AchievementData("Consigue la estrella de Un grupo encantador en el modo Zen", "The Lovely Bunch"),
      new AchievementData("Mata 3 peras seguidas en modo Clásico", "Its All Pear Shaped"),
      new AchievementData("Consigue una puntuación inferior a 20 tras todas las bonificaciones en el modo Arcade", "Underachiever"),
      new AchievementData("Consigue una puntuación superior a 400 tras todas las bonificaciones en el modo Arcade", "Overachiever"),
      new AchievementData("Hit tres bombas y puntuación de más de 250 puntos después de las bonificaciones", "Bomb Magnet"),
      new AchievementData("Consigue 6 críticos en un asalto en modo Clásico", "Lucky Ninja"),
      new AchievementData("Mata 10000 frutas en total", "Fruit Annihilation"),
      new AchievementData("Mata 5000 frutas en total", "Fruit Rampage"),
      new AchievementData("Consigue una puntuación de 200 en el modo Clásico", "Ultimate Fruit Ninja"),
      new AchievementData("Consigue una puntuación de 99", "Almost A Century"),
      new AchievementData("Mata 1000 frutas en total", "Fruit Frenzy"),
      new AchievementData("Corta 6 frutas en un combo", "Combo Mambo"),
      new AchievementData("Mata 500 frutas en total", "Fruit Blitz"),
      new AchievementData("Consigue una puntuación de 200 en el modo Zen", "Moment Of Zen"),
      new AchievementData("Mata 150 frutas en total", "Fruit Fight"),
      new AchievementData("Consigue una puntuación de 100 en el modo Clásico", "Great Fruit Ninja"),
      new AchievementData("Consigue una puntuación de 0", "Wake Up")
    };
    private static uint[] unlockTypeHashes = new uint[10]
    {
      StringFunctions.StringHash("TOTAL"),
      StringFunctions.StringHash("SCORE"),
      StringFunctions.StringHash("SCORE_UNSULLIED"),
      StringFunctions.StringHash("END_SCORE"),
      StringFunctions.StringHash("SPECIFIC"),
      StringFunctions.StringHash("CONSECUTIVE"),
      StringFunctions.StringHash("CONSECUTIVE_ANY"),
      StringFunctions.StringHash("COMBO"),
      StringFunctions.StringHash("COMBO_STAR"),
      StringFunctions.StringHash("SPECIFIC_ORDER")
    };
    private static LinkedList<AchievementManager.KeyRemove> keys = new LinkedList<AchievementManager.KeyRemove>();
    public Dictionary<uint, AchievementInfo> achievementInfo = new Dictionary<uint, AchievementInfo>();
    public Dictionary<uint, AchievementInfo>[] achievementTypeList = ArrayInit.CreateFilledArray<Dictionary<uint, AchievementInfo>>(10);
    private static AchievementManager instance;
    private static uint scoreID = 0;

    public static void Update(GameTime game)
    {
      if (Game2.isWP7TrialMode() && (Game2.isWP7TrialMode() || Mortar.Game1.logInSucceeded))
        return;
      if (AchievementManager.awardDelay > 0)
      {
        AchievementManager.awardDelay -= game.ElapsedGameTime.Milliseconds;
      }
      else
      {
        if (Mortar.Game1.settings.aw <= 0 || AchievementManager.awarding)
          return;
        if ((Mortar.Game1.settings.aw & 1 << AchievementManager.achievementIndex) == 1 << AchievementManager.achievementIndex)
        {
                    AchievementManager.awardDelay = 15000;
                    AchievementManager.AwardAchievementWP7(AchievementManager.achivementKeys[AchievementManager.achievementIndex]);
        }
        ++AchievementManager.achievementIndex;
        if (AchievementManager.achievementIndex < 20)
          return;
        AchievementManager.achievementIndex = 0;
      }
    }

    private static string GetTitle()
    {
      string title;
      switch (Game2.game_work.language)
      {
        case StringTableUtils.Language.LANGUAGE_FRENCH:
          title = "Tu as gagné un succès !";
          break;
        case StringTableUtils.Language.LANGUAGE_SPANISH:
          title = "¡Has conseguido un logro!";
          break;
        case StringTableUtils.Language.LANGUAGE_GERMAN:
          title = "Du hast einen Erfolg erzielt!";
          break;
        case StringTableUtils.Language.LANGUAGE_ITALIAN:
          title = "Hai vinto un obiettivo!";
          break;
        default:
          title = "You've earned an Achievement!";
          break;
      }
      return title;
    }

    private static string GetLine1()
    {
      string line1;
      switch (Game2.game_work.language)
      {
        case StringTableUtils.Language.LANGUAGE_FRENCH:
          line1 = "Déverrouille le jeu complet pour conserver ce succès.";
          break;
        case StringTableUtils.Language.LANGUAGE_SPANISH:
          line1 = "Desbloquea el juego completo para conservar este logro.";
          break;
        case StringTableUtils.Language.LANGUAGE_GERMAN:
          line1 = "Schalte jetzt das vollständige Spiel frei, um den Erfolg zu behalten.";
          break;
        case StringTableUtils.Language.LANGUAGE_ITALIAN:
          line1 = "Sblocca il gioco completo per conservare questo obiettivo.";
          break;
        default:
          line1 = "Unlock the Full Game now to keep this Achievement.";
          break;
      }
      return line1;
    }

    public static bool UpdatePretendAchievements(SpriteBatch batch)
    {
      switch (AchievementManager.awardMode)
      {
        case 1:
          AchievementManager.y += 4;
          if (AchievementManager.y > 0)
          {
            AchievementManager.y = 0;
            ++AchievementManager.awardMode;
            break;
          }
          break;
        case 2:
          if (++AchievementManager.delay > 90)
          {
            ++AchievementManager.awardMode;
            break;
          }
          break;
        case 3:
          AchievementManager.y -= 4;
          if (AchievementManager.y < -60)
          {
            AchievementManager.y = -60;
            AchievementManager.awardMode = 0;
                        Mortar.Game1.instance.DoUpsell(false);
            break;
          }
          break;
      }
      if (AchievementManager.awardMode > 0)
      {
        batch.Draw(Mortar.Game1.bobsCousinHairyMaclary, new Rectangle(0, AchievementManager.y, 800, 60), new Color((int) sbyte.MaxValue, (int) sbyte.MaxValue, (int) sbyte.MaxValue, (int) sbyte.MaxValue));
        batch.DrawString(Mortar.Game1.instance.font3, AchievementManager.GetTitle(), new Vector2(10f, (float) (AchievementManager.y + 10)), Color.White);
        batch.DrawString(Mortar.Game1.instance.font3, AchievementManager.GetLine1(), new Vector2(10f, (float) (AchievementManager.y + 30)), Color.White);
      }
      return AchievementManager.awardMode > 0;
    }

    public static void AwardPretendAchievement(string achievementKey)
    {
      int hashCode = achievementKey.GetHashCode();
      if (AchievementManager.akeys.Count > 0)
      {
        foreach (int akey in AchievementManager.akeys)
        {
          if (akey == hashCode)
            return;
        }
        AchievementManager.akeys.AddLast(hashCode);
      }
      else
        AchievementManager.akeys.AddLast(hashCode);
      if (AchievementManager.awardMode != 0)
        return;
      AchievementManager.awardMode = 1;
      AchievementManager.y = -60;
      AchievementManager.delay = 0;
    }

    public static void ShowUnlockDropdown(int type)
    {
      if (AchievementManager.unlockMode != 0)
        return;
      AchievementManager.unlockMode = 1;
      AchievementManager.uy = -60;
      AchievementManager.udelay = 0;
      AchievementManager.utype = type;
    }

    public static void UpdateUnlockDropdown(SpriteBatch batch)
    {
      switch (AchievementManager.unlockMode)
      {
        case 1:
          AchievementManager.uy += 4;
          if (AchievementManager.uy > 0)
          {
            AchievementManager.uy = 0;
            ++AchievementManager.unlockMode;
            break;
          }
          break;
        case 2:
          if (++AchievementManager.udelay > 90)
          {
            ++AchievementManager.unlockMode;
            break;
          }
          break;
        case 3:
          AchievementManager.uy -= 4;
          if (AchievementManager.uy < -60)
          {
            AchievementManager.uy = -60;
            AchievementManager.unlockMode = 0;
                        Mortar.Game1.instance.DoUpsell(false);
            break;
          }
          break;
      }
      if (AchievementManager.unlockMode <= 0)
        return;
      batch.Draw(Mortar.Game1.bobsCousinHairyMaclary, new Rectangle(0, AchievementManager.uy, 800, 60), new Color((int) sbyte.MaxValue, (int) sbyte.MaxValue, (int) sbyte.MaxValue, (int) sbyte.MaxValue));
      if (AchievementManager.utype == 0)
        batch.DrawString(Mortar.Game1.instance.font3, Mortar.Game1.instance.stringTable.GetString(55), new Vector2(10f, (float) (AchievementManager.uy + 20)), Color.White);
      else
        batch.DrawString(Mortar.Game1.instance.font3, Mortar.Game1.instance.stringTable.GetString(54), new Vector2(10f, (float) (AchievementManager.uy + 20)), Color.White);
    }

    public static void GetAchievementsCallback(IAsyncResult result)
    {
      //if (!(result.AsyncState is SignedInGamer asyncState))
      //  return;
      //lock (AchievementManager.achievementsLockObject)
      //{
      //  try
      //  {
      //    AchievementManager.achievements = asyncState.EndGetAchievements(result);
      //  }
      //  catch
      //  {
      //  }
      //}
    }

    public static string GetDesc(string achievementKey)
    {
      string desc = (string) null;
      switch (Game2.game_work.language)
      {
        case StringTableUtils.Language.LANGUAGE_FRENCH:
          foreach (AchievementData achievementData in AchievementManager.str_fr)
          {
            if (achievementData.key == achievementKey)
            {
              desc = achievementData.desc;
              break;
            }
          }
          break;
        case StringTableUtils.Language.LANGUAGE_SPANISH:
          foreach (AchievementData strE in AchievementManager.str_es)
          {
            if (strE.key == achievementKey)
            {
              desc = strE.desc;
              break;
            }
          }
          break;
        case StringTableUtils.Language.LANGUAGE_GERMAN:
          foreach (AchievementData achievementData in AchievementManager.str_de)
          {
            if (achievementData.key == achievementKey)
            {
              desc = achievementData.desc;
              break;
            }
          }
          break;
        case StringTableUtils.Language.LANGUAGE_ITALIAN:
          foreach (AchievementData achievementData in AchievementManager.str_it)
          {
            if (achievementData.key == achievementKey)
            {
              desc = achievementData.desc;
              break;
            }
          }
          break;
        default:
          foreach (AchievementData achievementData in AchievementManager.str_en)
          {
            if (achievementData.key == achievementKey)
            {
              desc = achievementData.desc;
              break;
            }
          }
          break;
      }
      return desc;
    }

    public static string GetHow(string achievementKey)
    {
      string how = (string) null;
      switch (Game2.game_work.language)
      {
        case StringTableUtils.Language.LANGUAGE_FRENCH:
          foreach (AchievementData achievementData in AchievementManager.str1_fr)
          {
            if (achievementData.key == achievementKey)
            {
              how = achievementData.desc;
              break;
            }
          }
          break;
        case StringTableUtils.Language.LANGUAGE_SPANISH:
          foreach (AchievementData str1E in AchievementManager.str1_es)
          {
            if (str1E.key == achievementKey)
            {
              how = str1E.desc;
              break;
            }
          }
          break;
        case StringTableUtils.Language.LANGUAGE_GERMAN:
          foreach (AchievementData achievementData in AchievementManager.str1_de)
          {
            if (achievementData.key == achievementKey)
            {
              how = achievementData.desc;
              break;
            }
          }
          break;
        case StringTableUtils.Language.LANGUAGE_ITALIAN:
          foreach (AchievementData achievementData in AchievementManager.str1_it)
          {
            if (achievementData.key == achievementKey)
            {
              how = achievementData.desc;
              break;
            }
          }
          break;
        default:
          foreach (AchievementData achievementData in AchievementManager.str1_en)
          {
            if (achievementData.key == achievementKey)
            {
              how = achievementData.desc;
              break;
            }
          }
          break;
      }
      return how;
    }

    /*public static int GetIndexForKey(string key)
    {
      try
      {
        lock (AchievementManager.achievementsLockObject)
        {
          if (AchievementManager.achievements != null)
          {
            int indexForKey = 0;
            foreach (Achievement achievement in AchievementManager.achievements)
            {
              if (achievement.Key == key)
                return indexForKey;
              ++indexForKey;
            }
          }
        }
      }
      catch
      {
      }
      return 0;
    }*/

    public static string GetDescription(int index)
    {
      string description = "...";
      /*try
      {
        lock (AchievementManager.achievementsLockObject)
        {
          if (AchievementManager.achievements != null)
          {
            int num = 0;
            foreach (Achievement achievement in AchievementManager.achievements)
            {
              if (num == index)
                return achievement.IsEarned ? AchievementManager.GetDesc(achievement.Key) : AchievementManager.GetHow(achievement.Key);
              ++num;
            }
          }
        }
      }
      catch
      {
      }*/
      return description;
    }

        public static string GetKey(int index)
        {
            lock (AchievementManager.achievementsLockObject)
            {
                if (AchievementManager.achievements != null && AchievementManager.achievements.achievemnts != null)
                {
                    int currentIndex = 0;
                    foreach (var achievement in AchievementManager.achievements.achievemnts)
                    {
                        if (currentIndex == index)
                        {
                            return achievement.Key;
                        }
                        currentIndex++;
                    }
                }
            }
            return null;
        }

    public static Texture2D GetPicture(int index)
    {
      /*lock (AchievementManager.achievementsLockObject)
      {
        if (AchievementManager.achievements != null)
        {
          Achievement achievement = AchievementManager.achievements[index];
          return Texture2D.FromStream(Mortar.Game1.instance.graphics.GraphicsDevice, achievement.GetPicture());
        }
      }*/
      return (Texture2D) null;
    }

    public static int GetGamerScore(int index)
    {
      /*lock (AchievementManager.achievementsLockObject)
      {
        if (AchievementManager.achievements != null)
          return AchievementManager.achievements[index].GamerScore;
      }*/
      return 0;
    }

    public static bool Unlocked(int index)
    {
      /*lock (AchievementManager.achievementsLockObject)
      {
        if (AchievementManager.achievements != null)
          return AchievementManager.achievements[index].IsEarned;
      }*/
      return false;
    }

    private static int FindAchievementIndex(string key)
    {
      int achievementIndex = 0;
      foreach (string achivementKey in AchievementManager.achivementKeys)
      {
        if (achivementKey == key)
          return achievementIndex;
        ++achievementIndex;
      }
      return -1;
    }

    public static void AwardAchievementWP7(string achievementKey)
    {
      if (Game2.isWP7TrialMode() || !Mortar.Game1.logInSucceeded)
      {
        if (Game2.isWP7TrialMode())
                    AchievementManager.AwardPretendAchievement(achievementKey);
        int achievementIndex = AchievementManager.FindAchievementIndex(achievementKey);
        if (achievementIndex < 0)
          return;
                Mortar.Game1.settings.aw |= 1 << achievementIndex;
                Mortar.Game1.SaveConfig();
      }
      else
      {
        //SignedInGamer signedInGamer = default;//Gamer.SignedInGamers[(PlayerIndex) 0];
        //if (signedInGamer == null)
          return;
        /*lock (AchievementManager.achievementsLockObject)
        {
          if (AchievementManager.achievements == null)
            return;
          foreach (Achievement achievement in AchievementManager.achievements)
          {
            if (achievement.Key == achievementKey)
            {
              if (achievement.IsEarned)
                break;
              signedInGamer.BeginAwardAchievement(achievementKey, new AsyncCallback(AchievementManager.AwardAchievementCallback), (object) signedInGamer);
                            AchievementManager.awarding = true;
              break;
            }
          }
        }*/
      }
    }

    public static void AwardAchievementWP7ByIndex(int index)
    {
      //SignedInGamer signedInGamer = default;//Gamer.SignedInGamers[(PlayerIndex) 0];
      //if (signedInGamer == null || !Mortar.Game1.logInSucceeded)
        return;
      /*lock (AchievementManager.achievementsLockObject)
      {
        if (AchievementManager.achievements == null)
          return;
        int num = 0;
        foreach (Achievement achievement in AchievementManager.achievements)
        {
          if (num == index)
          {
            if (achievement.IsEarned)
              break;
            signedInGamer.BeginAwardAchievement(achievement.Key, new AsyncCallback(AchievementManager.AwardAchievementCallback), (object) signedInGamer);
            AchievementManager.awarding = true;
            break;
          }
          ++num;
        }
      }*/
    }

    private static void AwardAchievementCallback(IAsyncResult result)
    {
      AchievementManager.awarding = false;
      //if (!(result.AsyncState is SignedInGamer asyncState))
      // return;
      //asyncState.EndAwardAchievement(result);
      //asyncState.BeginGetAchievements(new AsyncCallback(AchievementManager.GetAchievementsCallback), (object) asyncState);
    }

    public static int MAX_ORDER_LIST_TYPES => 10;

    public static AchievementManager GetInstance()
    {
      if (AchievementManager.instance == null)
        AchievementManager.instance = new AchievementManager();
      return AchievementManager.instance;
    }

    private string Attribute(XElement element, string name)
    {
      return element.Attribute((XName) name)?.Value;
    }

    // LoadAchievementInfo
    public void LoadAchievementInfo()
    {
      this.achievementInfo.Clear();
      NotificationControl.s_banner = TextureManager.GetInstance().Load("texturesWP7/achievment_banner.tex");
      NotificationControl.s_unlockBanner = TextureManager.GetInstance().Load("texturesWP7/hud_unlocked_dialog.tex");
      XDocument element1 = MortarXml.Load("xmlwp7/achievementList.xml");
      if (element1 == null)
        throw new Exception("Achievements xml is missing");
      /*
      label_20:
        for (XElement element2 = element1.FirstChildElement("achievementManagerFile").FirstChildElement("achievement"); element2 != null; element2 = element2.NextSiblingElement("achievement"))
        {
            string s = this.Attribute(element2, "id");
            uint num1 = StringFunctions.StringHash(s);
            if (!Game.game_work.saveData.IsAchievementUnlocked(num1))
            {
            AchievementInfo achievementInfo = new AchievementInfo();
            achievementInfo.id = s;
            achievementInfo.idHash = num1;
            string str1 = this.Attribute(element2, "name");
            achievementInfo.name = str1;
            this.achievementInfo[num1] = achievementInfo;
            element2.QueryIntAttribute("score", ref achievementInfo.score);
            element2.QueryIntAttribute("total", ref achievementInfo.total);
            int num2 = 0;
            element2.QueryIntAttribute("isGameOver", ref num2);
            achievementInfo.isGameOver = num2 == 1;
            string texture = "achievements/" + this.Attribute(element2, "texture");
            achievementInfo.texture = TextureManager.GetInstance().Load(texture);
            string str2 = this.Attribute(element2, "mode");
            achievementInfo.modeMask = 0U;
            if (str2 != null)
            {
                string[] strArray = str2.Split(',');
                for (int index = 0; index < strArray.Length; ++index)
                {
                strArray[index] = strArray[index].Trim();
                Game.GAME_MODE gameMode = Initialise.ParseGameMode(StringFunctions.StringHash(strArray[index]));
                achievementInfo.modeMask |= Initialise.GetModeBitMask(gameMode);
                }
            }
            else
                achievementInfo.modeMask = Initialise.GetModeBitMask(Game.GAME_MODE.GM_CLASSIC);
            achievementInfo.type = AchievementUnlockType.UNLOCK_TYPE_MAX;
            uint num3 = StringFunctions.StringHash(this.Attribute(element2, "type"));
            for (uint index = 0; index < 10U; ++index)
            {
                if ((int) num3 == (int) AchievementManager.unlockTypeHashes[(int) index])
                {
                achievementInfo.type = (AchievementUnlockType) index;
                switch (index)
                {
                    case 1:
                    case 2:
                    this.achievementTypeList[(int)index][AchievementManager.scoreID] = achievementInfo;
                    ++AchievementManager.scoreID;
                    goto label_20;
                    case 4:
                    case 5:
                    case 8:
                    uint key = StringFunctions.StringHash(this.Attribute(element2, "specific_type"));
                    this.achievementTypeList[(int) index][key] = achievementInfo;
                    goto label_20;
                    case 9:
                    this.achievementTypeList[(int) index][num1] = achievementInfo;
                    string text1 = this.Attribute(element2, "specific_type");
                    achievementInfo.specificOrder = new SpecificOrder(text1);
                    goto label_20;
                    default:
                    this.achievementTypeList[(int) index][(uint) achievementInfo.total] = achievementInfo;
                    string text2 = this.Attribute(element2, "specific_type");
                    if (text2 != null)
                    {
                        achievementInfo.specificOrder = new SpecificOrder(text2);
                        goto label_20;
                    }
                    else
                        goto label_20;
                }
                }
            }
            }
        }*/
    }//LoadAchievementInfo

    public void UnLoadAchievementInfo()
    {
        throw new MissingMethodException();
    }

    public bool UnlockTotalFruitAchievement(int total)
    {
      bool flag = false;
      foreach (KeyValuePair<uint, AchievementInfo> keyValuePair in this.achievementTypeList[default])
      {
        if (total >= keyValuePair.Value.total && this.QueAchievement(keyValuePair.Value, keyValuePair.Key))
          flag = true;
      }
      return flag;
    }

    public bool UnlockScoreAchievement(int score)
    {
      bool flag = false;
      foreach (KeyValuePair<uint, AchievementInfo> keyValuePair in this.achievementTypeList[default])
      {
        if (score >= keyValuePair.Value.total && (keyValuePair.Value.modeMask & Initialise.GetModeBitMask(Game2.game_work.gameMode)) > 0U && this.QueAchievement(keyValuePair.Value, keyValuePair.Key))
          flag = true;
      }
      return flag;
    }

    public bool UnlockScoreUnsulliedAchievement(int score)
    {
      bool flag = false;
      foreach (KeyValuePair<uint, AchievementInfo> keyValuePair in this.achievementTypeList[default])
      {
        if (score >= keyValuePair.Value.total && (keyValuePair.Value.modeMask & Initialise.GetModeBitMask(Game2.game_work.gameMode)) > 0U && this.QueAchievement(keyValuePair.Value, keyValuePair.Key))
          flag = true;
      }
      return flag;
    }

    public bool UnlockSpecificFruitAchievement(int total, uint hash)
    {
      AchievementInfo achievement = (AchievementInfo) null;
      uint key = 0;
      foreach (KeyValuePair<uint, AchievementInfo> keyValuePair in this.achievementTypeList[default])
      {
        if ((int) keyValuePair.Key == (int) hash)
        {
          achievement = keyValuePair.Value;
          key = keyValuePair.Key;
          break;
        }
      }
      return achievement != null && total >= achievement.total && (achievement.modeMask & Initialise.GetModeBitMask(Game2.game_work.gameMode)) > 0U && this.QueAchievement(achievement, key);
    }

    public bool UnlockEndScoreAchievement(int total, int pb)
    {
      bool flag = false;
      AchievementManager.keys.Clear();
      foreach (KeyValuePair<uint, AchievementInfo> keyValuePair in this.achievementTypeList[default])
      {
        if ((total == keyValuePair.Value.total || keyValuePair.Value.total < 0 && total == pb) && (keyValuePair.Value.modeMask & Initialise.GetModeBitMask(Game2.game_work.gameMode)) > 0U && this.QueAchievement(keyValuePair.Value, keyValuePair.Key))
          flag = true;
      }
      foreach (AchievementManager.KeyRemove key in AchievementManager.keys)
        this.achievementTypeList[default].Remove(key.key);
      return flag;
    }

    public bool UnlockConsecutiveAchievement(int total, uint hash)
    {
      AchievementInfo achievement1 = (AchievementInfo) null;
      uint key = 0;
      foreach (KeyValuePair<uint, AchievementInfo> keyValuePair 
                in this.achievementTypeList[default])
      {
        if ((int) keyValuePair.Key == (int) hash)
        {
          achievement1 = keyValuePair.Value;
          key = keyValuePair.Key;
          break;
        }
      }
      bool flag1 = false;
      if (achievement1 != null && total >= achievement1.total 
                && (achievement1.modeMask & Initialise.GetModeBitMask(Game2.game_work.gameMode)) > 0U)
        flag1 = this.QueAchievement(achievement1, key) || flag1;
      AchievementInfo achievement2 = (AchievementInfo) null;
      foreach (KeyValuePair<uint, AchievementInfo> keyValuePair 
                in this.achievementTypeList[default])
      {
        if (keyValuePair.Value.total == total)
        {
          achievement2 = keyValuePair.Value;
          key = keyValuePair.Key;
          break;
        }
      }
      if (achievement2 != null && (achievement2.modeMask & Initialise.GetModeBitMask(Game2.game_work.gameMode)) > 0U)
      {
        bool flag2 = this.QueAchievement(achievement2, key) || flag1;
      }
      return false;
    }

    public bool UnlockComboAchievement(int total)
    {
      return this.UnlockComboAchievement(total, (int[]) null);
    }

    public bool UnlockComboAchievement(int total, int[] fruitInCombo)
    {
      bool flag1 = false;
      foreach (KeyValuePair<uint, AchievementInfo> keyValuePair
                in this.achievementTypeList[default])
      {
        if (total >= keyValuePair.Value.total && (keyValuePair.Value.modeMask 
                    & Initialise.GetModeBitMask(Game2.game_work.gameMode)) > 0U)
        {
          bool flag2 = true;
          if (keyValuePair.Value.specificOrder != null)
          {
            int firstFruitTypeHash = (int) keyValuePair.Value.specificOrder.GetFirstFruitTypeHash();
            int num = 0;
            for (int index = 0; index < total; ++index)
            {
              if ((long) Fruit.FruitTypeHash(fruitInCombo[index]) == (long) firstFruitTypeHash)
                ++num;
            }
            if (num < keyValuePair.Value.total)
              flag2 = false;
          }
          if (flag2 && keyValuePair.Value.isGameOver)
          {
            flag2 = false;
            if (Game2.game_work.timeControl != null && total >= 3 && (double) Game2.game_work.timeControl.GetTime() <= 0.0)
              flag2 = true;
          }
          if (flag2 && this.QueAchievement(keyValuePair.Value, keyValuePair.Key))
            flag1 = true;
        }
      }
      return flag1;
    }

    public bool UnlockComboStarAchievement(int total, uint nameHash)
    {
      AchievementInfo achievement = (AchievementInfo) null;
      uint key = 0;
      foreach (KeyValuePair<uint, AchievementInfo> keyValuePair
                in this.achievementTypeList[default])
      {
        if ((int) keyValuePair.Key == (int) nameHash)
        {
          achievement = keyValuePair.Value;
          key = keyValuePair.Key;
          break;
        }
      }
      return achievement != null && total >= achievement.total && (achievement.modeMask & Initialise.GetModeBitMask(Game2.game_work.gameMode)) > 0U && this.QueAchievement(achievement, key);
    }

    public bool UnlockSpecificOrderAchievement(uint nameHash)
    {
      bool flag = false;
      foreach (KeyValuePair<uint, AchievementInfo> keyValuePair 
                in this.achievementTypeList[(int)new IntPtr(9)])
      {
        AchievementInfo achievementInfo = keyValuePair.Value;
        if (achievementInfo.specificOrder != null && achievementInfo.specificOrder.Check(nameHash) && this.QueAchievement(keyValuePair.Value, keyValuePair.Key))
          flag = true;
      }
      return flag;
    }

    private bool QueAchievement(AchievementInfo achievement, uint key)
    {
      if (achievement != null)
      {
        bool flag = false;
        if (Mortar.Math.BETWEEN((int) achievement.id[0], 48, 57))
        {
          if (NetworkManager.GetInstance().UserHasEnabledNetwork() && Game2.game_work.saveData.AddToQue(achievement.id, achievement.idHash))
            flag = true;
        }
        else if (Game2.game_work.saveData.AddToQue(achievement.id, achievement.idHash))
          flag = true;
        if (flag)
          AchievementManager.keys.AddLast(new AchievementManager.KeyRemove((uint) achievement.type, key));
      }
      return false;
    }

    public bool UnlockedAchievement(uint idHash, HUD hud)
    {
      AchievementInfo achievementInfo = (AchievementInfo) null;
      if (!this.achievementInfo.TryGetValue(idHash, out achievementInfo))
        return false;
      if (string.Compare(achievementInfo.name, "UNLOCKED NEW BLADE") == 0 || string.Compare(achievementInfo.name, "UNLOCKED NEW BACKGROUND") == 0)
      {
        uint hashCode = (uint) achievementInfo.id.GetHashCode();
        for (int index = 0; index < Game2.MAX_SYSTEM_ACHIEVEMENTS; ++index)
        {
          if ((int) hashCode == (int)Mortar.Game1.settings.achievements[index])
            return true;
        }
        for (int index = 0; index < Game2.MAX_SYSTEM_ACHIEVEMENTS; ++index)
        {
          if (Mortar.Game1.settings.achievements[index] == 0U)
          {
                        Mortar.Game1.settings.achievements[index] = hashCode;
            break;
          }
        }
        NotificationControl control = new NotificationControl(achievementInfo.name, achievementInfo.score, achievementInfo.texture, NotificationControl.NotificationType.ITEM_UNLOCK);
        control.Init();
        hud.AddNotification((HUDControl) control);
                Mortar.Game1.SaveConfig();
      }
      return true;
    }

    public bool AchievementExists(uint idHash)
    {
      foreach (KeyValuePair<uint, AchievementInfo> keyValuePair in this.achievementInfo)
      {
        if ((int) keyValuePair.Value.idHash == (int) idHash)
          return true;
      }
      return false;
    }

    private struct KeyRemove(uint t, uint k)
    {
      public uint type = t;
      public uint key = k;
    }
  }

    public class AchievementCollection
    {
        public Dictionary<string, int> achievemnts;
    }
}
