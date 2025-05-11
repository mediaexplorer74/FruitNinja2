
// Type: GameManager.ComboChecker
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Mortar;

#nullable disable
namespace GameManager
{
  public static class ComboChecker
  {
    private static int[,] comboTypes = new int[11, 2];
    private static int numTypesInCombo = 0;
    private static string[] strings = new string[24]
    {
      "3_FRUIT",
      "4_FRUIT",
      "5_FRUIT",
      "6_FRUIT",
      "ALL_DIFFERENT",
      "7_FRUIT_PLUS",
      "ALL_APPLES",
      "ALL_ORANGES",
      "ALL_PINEAPPLES",
      "ALL_WATERMELONS",
      "ALL_KIWIS",
      "ALL_MANGOES",
      "ALL_STRAWBERRIES",
      "ALL_PEARS",
      "ALL_BANANAS",
      "ALL_LIMES",
      "ALL_LEMONS",
      "ALL_COCONUTS",
      "ALPHABETICAL",
      "FULLHOUSE",
      "2_PAIR",
      "3_OF_A_KIND",
      "4_OF_A_KIND",
      "PATTERN"
    };
    private static int[,] types = new int[13, 2]
    {
      {
        Fruit.FruitType("apple"),
        6
      },
      {
        Fruit.FruitType("apple_red"),
        6
      },
      {
        Fruit.FruitType("orange"),
        7
      },
      {
        Fruit.FruitType("pineapple"),
        8
      },
      {
        Fruit.FruitType("watermelon"),
        9
      },
      {
        Fruit.FruitType("kiwi"),
        10
      },
      {
        Fruit.FruitType("mango"),
        11
      },
      {
        Fruit.FruitType("strawberry"),
        12
      },
      {
        Fruit.FruitType("pear"),
        13
      },
      {
        Fruit.FruitType("banana"),
        14
      },
      {
        Fruit.FruitType("lime"),
        15
      },
      {
        Fruit.FruitType("lemon"),
        16
      },
      {
        Fruit.FruitType("coconut"),
        17
      }
    };
    private static string[,] textureNames = new string[24, 7]
    {
      {
        "2",
        "star_fruity.tex",
        "star_juicy.tex",
        null,
        null,
        null,
        null
      },
      {
        "2",
        "star_yummy.tex",
        "star_tasty.tex",
        null,
        null,
        null,
        null
      },
      {
        "2",
        "star_lush.tex",
        "star_delicious.tex",
        null,
        null,
        null,
        null
      },
      {
        "2",
        "star_succulent.tex",
        "star_succulent.tex",
        null,
        null,
        null,
        null
      },
      {
        "3",
        "star_fruit_salad.tex",
        "star_fruits_basket.tex",
        "star_megamix.tex",
        null,
        null,
        null
      },
      {
        "2",
        "star_amazing.tex",
        "star_exquisite.tex",
        null,
        null,
        null,
        null
      },
      {
        "1",
        "star_its_apples.tex",
        null,
        null,
        null,
        null,
        null
      },
      {
        "1",
        "star_vitamin_c.tex",
        null,
        null,
        null,
        null,
        null
      },
      {
        "1",
        "star_got_the_sweats.tex",
        null,
        null,
        null,
        null,
        null
      },
      {
        "1",
        "star_melon_mania.tex",
        null,
        null,
        null,
        null,
        null
      },
      {
        "1",
        "star_flightless_bird.tex",
        null,
        null,
        null,
        null,
        null
      },
      {
        "1",
        "star_mango_smoothie.tex",
        null,
        null,
        null,
        null,
        null
      },
      {
        "1",
        "star_full_punnet.tex",
        null,
        null,
        null,
        null,
        null
      },
      {
        "1",
        "star_pear_tree.tex",
        null,
        null,
        null,
        null,
        null
      },
      {
        "1",
        "star_banana_cake.tex",
        null,
        null,
        null,
        null,
        null
      },
      {
        "1",
        "star_scurvy_cure.tex",
        null,
        null,
        null,
        null,
        null
      },
      {
        "1",
        "star_lemon_line_up.tex",
        null,
        null,
        null,
        null,
        null
      },
      {
        "1",
        "star_lovely_bunch.tex",
        null,
        null,
        null,
        null,
        null
      },
      {
        "1",
        "star_alphabetic.tex",
        null,
        null,
        null,
        null,
        null
      },
      {
        "1",
        "star_full_house.tex",
        null,
        null,
        null,
        null,
        null
      },
      {
        "1",
        "star_two_pairs.tex",
        null,
        null,
        null,
        null,
        null
      },
      {
        "1",
        "star_three_of_a_kind.tex",
        null,
        null,
        null,
        null,
        null
      },
      {
        "1",
        "star_four_of_a_kind.tex",
        null,
        null,
        null,
        null,
        null
      },
      {
        "1",
        "star_checkers.tex",
        null,
        null,
        null,
        null,
        null
      }
    };

    public static string GetComboName(COMBO_TYPE starType) => ComboChecker.strings[(int) starType];

    public static COMBO_TYPE CheckCombo(int[] fruits, int nofruits)
    {
      return ComboChecker.CheckCombo(fruits, nofruits, out int _);
    }

    public static COMBO_TYPE CheckCombo(int[] fruits, int numFruits, out int mostOf)
    {
      mostOf = 0;
      ComboChecker.numTypesInCombo = 0;
      bool flag1 = true;
      int num1 = 0;
      int num2 = 0;
      for (int index1 = 0; index1 < numFruits; ++index1)
      {
        bool flag2 = false;
        for (int index2 = 0; index2 < ComboChecker.numTypesInCombo; ++index2)
        {
          if (ComboChecker.comboTypes[index2, 0] == fruits[index1])
          {
            ++ComboChecker.comboTypes[index2, 1];
            if (ComboChecker.comboTypes[index2, 1] > num2)
            {
              num1 = fruits[index1];
              num2 = ComboChecker.comboTypes[index2, 1];
            }
            if (index2 != ComboChecker.numTypesInCombo - 1)
              flag1 = false;
            flag2 = true;
          }
        }
        if (!flag2)
        {
          ComboChecker.comboTypes[ComboChecker.numTypesInCombo, 0] = fruits[index1];
          ComboChecker.comboTypes[ComboChecker.numTypesInCombo, 1] = 1;
          ++ComboChecker.numTypesInCombo;
          if (num2 == 0)
          {
            num1 = fruits[index1];
            num2 = 1;
          }
        }
      }
      mostOf = num1;
      switch (ComboChecker.numTypesInCombo)
      {
        case 1:
          for (int index = 0; index < 13; ++index)
          {
            if (fruits[0] == ComboChecker.types[index, 0])
              return (COMBO_TYPE) ComboChecker.types[index, 1];
          }
          break;
        case 2:
          if (!flag1)
          {
            bool flag3 = true;
            for (int index = 0; index < numFruits; ++index)
            {
              flag3 = index % 2 == 0 ? fruits[index] == ComboChecker.comboTypes[0, 0] : fruits[index] == ComboChecker.comboTypes[1, 0];
              if (!flag3)
                break;
            }
            if (flag3)
              return COMBO_TYPE.CT_PATTERN;
          }
          if (numFruits == 5 && (ComboChecker.comboTypes[0, 1] == 2 || ComboChecker.comboTypes[1, 1] == 2))
            return COMBO_TYPE.CT_FULLHOUSE;
          break;
        case 3:
          if (numFruits == 5 && (ComboChecker.comboTypes[0, 1] == 2 || ComboChecker.comboTypes[1, 1] == 2 || ComboChecker.comboTypes[1, 1] == 2))
            return COMBO_TYPE.CT_2_PAIR;
          goto default;
        default:
          if (ComboChecker.numTypesInCombo == numFruits && numFruits >= 5)
            return COMBO_TYPE.CT_ALL_DIFFERENT;
          break;
      }
      if (ComboChecker.numTypesInCombo >= 2)
      {
        COMBO_TYPE comboType = COMBO_TYPE.CT_NONE;
        for (int index = 0; index < ComboChecker.numTypesInCombo; ++index)
        {
          if (ComboChecker.comboTypes[index, 1] == 3 && comboType == COMBO_TYPE.CT_NONE)
            comboType = COMBO_TYPE.CT_3_OF_A_KIND;
          if (ComboChecker.comboTypes[index, 1] == 4 && comboType < COMBO_TYPE.CT_4_OF_A_KIND)
            comboType = COMBO_TYPE.CT_4_OF_A_KIND;
        }
        if (comboType != COMBO_TYPE.CT_NONE)
          return comboType;
      }
      switch (numFruits)
      {
        case 0:
        case 1:
        case 2:
          return COMBO_TYPE.CT_NONE;
        case 3:
          return COMBO_TYPE.CT_3_FRUIT;
        case 4:
          return COMBO_TYPE.CT_4_FRUIT;
        case 5:
          return COMBO_TYPE.CT_5_FRUIT;
        case 6:
          return COMBO_TYPE.CT_6_FRUIT;
        default:
          return COMBO_TYPE.CT_7_FRUIT_PLUS;
      }
    }

    public static Texture GetComboStarTexture(COMBO_TYPE starType)
    {
      Texture comboStarTexture = (Texture) null;
      if (starType > COMBO_TYPE.CT_NONE && starType < COMBO_TYPE.CT_MAX)
      {
        int index = Math.g_random.Rand32((int) ComboChecker.textureNames[(int) starType, 0][0] - 48) + 1;
        comboStarTexture = TextureManager.GetInstance().Load(ComboChecker.textureNames[(int) starType, index], true);
      }
      return comboStarTexture;
    }
  }
}
