
// Type: Mortar.Font
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace Mortar
{
  public class Font
  {
    private Font.CharTemplate[] m_charTemplates;
    private Font.CharTemplate[] m_charLookUp = new Font.CharTemplate[Font.QUICKFIND_CHARS];
    private int m_charTemplateCnt;
    private Font.Page[] m_pages;
    private int m_pageCnt;
    private Font.Kerning[] m_kernings;
    private int m_kerningCnt;
    public uint m_hash_name;
    public int m_textureWidth;
    public int m_textureHeight;
    public float m_lineHeight;
    public float m_baseLine;
    public GameVertex[] quad_verts;

    public static int QUICKFIND_CHARS => 256;

    public static int QUADS_IN_BUFFER => 256;

    public static bool strnicmp(Font.CharPtr p, string tst, int l)
    {
      return p.str.astring.Substring(p.idx, l).ToLower() == tst.Substring(0, l).ToLower();
    }

    public static void cpStrcpy(Font.CharPtr p, string src)
    {
      p.str.astring = p.str.astring.Substring(0, p.idx) + src;
    }

    public static int cpStrlen(Font.CharPtr p) => p.str.astring.Length - p.idx;

    public static int SPACE_CHAR => 32;

    public static int NEW_LINE_CHAR => 13;

    public static int QUOTES_CHAR => 34;

    private int Get_Next_Value(
      Font.CharPtr readingFrom,
      Font.CharPtr valueName,
      Action<int> intValue,
      Action<Font.CharPtr> stringValue)
    {
      int num1 = 0;
      while (readingFrom.arr(num1) != '=')
      {
        ++num1;
        if ((int) readingFrom.arr(num1) == Font.NEW_LINE_CHAR || readingFrom.Offset(num1).isEos)
          return -num1;
      }
      int i1 = num1 - 1;
      while ((int) readingFrom.arr(i1) != Font.SPACE_CHAR && i1 > 0)
        --i1;
      if (valueName.isNotNull)
      {
        int startIndex = i1 + 1;
        Font.cpStrcpy(valueName, readingFrom.Str.Substring(startIndex, num1 - startIndex));
      }
      int nextValue = num1 + 1;
      if ((int) readingFrom.arr(nextValue) == Font.NEW_LINE_CHAR || readingFrom.Offset(nextValue).isEos)
        return -nextValue;
      if (intValue == null || stringValue == null)
        return nextValue;
      bool flag = false;
      if (readingFrom.arr(nextValue) == '-')
      {
        flag = true;
        ++nextValue;
        if ((int) readingFrom.arr(nextValue) == Font.NEW_LINE_CHAR || readingFrom.Offset(nextValue).isEos)
          return -nextValue;
      }
      if ((int) readingFrom.arr(nextValue) == Font.QUOTES_CHAR)
      {
        ++nextValue;
        if ((int) readingFrom.arr(nextValue) == Font.NEW_LINE_CHAR || readingFrom.Offset(nextValue).isEos)
          return -nextValue;
        int startIndex = nextValue;
        while ((int) readingFrom.arr(nextValue) != Font.QUOTES_CHAR && readingFrom.arr(nextValue) != '.')
        {
          ++nextValue;
          if ((int) readingFrom.arr(nextValue) == Font.NEW_LINE_CHAR || readingFrom.Offset(nextValue).isEos)
            return -nextValue;
        }
        if (readingFrom.arr(nextValue) == '.')
          stringValue(new Font.CharPtr(new Font.AString(readingFrom.Str.Substring(startIndex, nextValue - startIndex) + ".tex")));
        else
          stringValue(new Font.CharPtr(new Font.AString(readingFrom.Str.Substring(startIndex, nextValue - startIndex))));
      }
      else
      {
        int num2 = nextValue;
        while ((int) readingFrom.arr(nextValue) != Font.SPACE_CHAR && (int) readingFrom.arr(nextValue) != Font.NEW_LINE_CHAR && readingFrom.arr(nextValue) != char.MinValue)
          ++nextValue;
        int num3 = 0;
        int num4 = 1;
        for (int i2 = nextValue - 1; i2 >= num2; --i2)
        {
          if (readingFrom.arr(i2) < '0' || readingFrom.arr(i2) > '9')
            return nextValue;
          num3 += num4 * ((int) readingFrom.arr(i2) - 48);
          num4 *= 10;
        }
        intValue(num3 * (flag ? -1 : 1));
      }
      return (int) readingFrom.arr(nextValue) == Font.NEW_LINE_CHAR || readingFrom.Offset(nextValue).isEos ? -nextValue : nextValue;
    }

    private bool Next_Word_Is(Font.CharPtr readingFrom, string test)
    {
      int length = test.Length;
      int num = 0;
      while (num < length && readingFrom.arr(num) != char.MinValue && (int) readingFrom.arr(num) == (int) test[num] && (int) readingFrom.arr(num) != Font.SPACE_CHAR)
        ++num;
      return num >= length;
    }

    public static int INVALID_NUMBER => -43710;

    private int Parse_Char(Font.CharPtr readingFrom, ref Font.CharTemplate newChar, int sizeLeft)
    {
      int o = 0;
      newChar = new Font.CharTemplate();
      int nextValue;
      for (; o < sizeLeft; o += nextValue)
      {
        int value = Font.INVALID_NUMBER;
        Font.CharPtr stringVal = new Font.CharPtr((Font.AString) null);
        Font.CharPtr charPtr = new Font.CharPtr(new Font.AString(""));
        nextValue = this.Get_Next_Value(readingFrom.Offset(o), charPtr, (Action<int>) (v => value = v), (Action<Font.CharPtr>) (f => stringVal = f));
        if (this.Next_Word_Is(charPtr, "id"))
        {
          if (value != Font.INVALID_NUMBER)
            newChar.id = (ushort) value;
        }
        else if (this.Next_Word_Is(charPtr, "xadvance"))
        {
          if (value != Font.INVALID_NUMBER)
            newChar.xAdvance = (float) value;
        }
        else if (this.Next_Word_Is(charPtr, "xoffset"))
        {
          if (value != Font.INVALID_NUMBER)
            newChar.xOffset = (float) value;
        }
        else if (this.Next_Word_Is(charPtr, "x"))
        {
          if (value != Font.INVALID_NUMBER)
            newChar.X = (float) value;
        }
        else if (this.Next_Word_Is(charPtr, "yoffset"))
        {
          if (value != Font.INVALID_NUMBER)
            newChar.yOffset = (float) value;
        }
        else if (this.Next_Word_Is(charPtr, "y"))
        {
          if (value != Font.INVALID_NUMBER)
            newChar.Y = (float) value;
        }
        else if (this.Next_Word_Is(charPtr, "width"))
        {
          if (value != Font.INVALID_NUMBER)
            newChar.width = (float) value;
        }
        else if (this.Next_Word_Is(charPtr, "height"))
        {
          if (value != Font.INVALID_NUMBER)
            newChar.height = (float) value;
        }
        else if (this.Next_Word_Is(charPtr, "page") && value != Font.INVALID_NUMBER)
          newChar.page = (byte) value;
        if (nextValue < 0)
          return o - nextValue + 2;
      }
      return o;
    }

    private int Parse_Page(Font.CharPtr readingFrom, ref Font.Page newPage, int sizeLeft)
    {
      int o = 0;
      newPage.pageName = (string) null;
      newPage.texture = (Texture) null;
      int nextValue;
      for (; o < sizeLeft; o += nextValue)
      {
        int value = Font.INVALID_NUMBER;
        Font.CharPtr stringVal = new Font.CharPtr((Font.AString) null);
        Font.CharPtr charPtr = new Font.CharPtr(new Font.AString(""));
        nextValue = this.Get_Next_Value(readingFrom.Offset(o), charPtr, (Action<int>) (v => value = v), (Action<Font.CharPtr>) (f => stringVal = f));
        if (this.Next_Word_Is(charPtr, "file") && stringVal.isNotNull)
          newPage.pageName = stringVal.Str;
        if (nextValue < 0)
          return o - nextValue + 2;
      }
      return o;
    }

    private int Parse_Kerning(Font.CharPtr readingFrom, ref Font.Kerning newKern, int sizeLeft)
    {
      int o = 0;
      newKern.first = 0;
      newKern.second = 0;
      newKern.amount = 0.0f;
      int nextValue;
      for (; o < sizeLeft; o += nextValue)
      {
        int value = Font.INVALID_NUMBER;
        Font.CharPtr stringVal = new Font.CharPtr((Font.AString) null);
        Font.CharPtr charPtr = new Font.CharPtr(new Font.AString(""));
        nextValue = this.Get_Next_Value(readingFrom.Offset(o), charPtr, (Action<int>) (v => value = v), (Action<Font.CharPtr>) (f => stringVal = f));
        if (this.Next_Word_Is(charPtr, "first"))
        {
          if (value != Font.INVALID_NUMBER)
            newKern.first = value;
        }
        else if (this.Next_Word_Is(charPtr, "second"))
        {
          if (value != Font.INVALID_NUMBER)
            newKern.second = value;
        }
        else if (this.Next_Word_Is(charPtr, "amount") && value != Font.INVALID_NUMBER)
          newKern.amount = (float) value;
        if (nextValue < 0)
          return o - nextValue + 2;
      }
      return o;
    }

    private Font.CharTemplate GetCharTemplate(int charID) => this.GetCharTemplate(charID, 0);

    private Font.CharTemplate GetCharTemplate(int charID, int type)
    {
      if (charID < 0)
        charID = 256 + charID;
      if (charID < Font.QUICKFIND_CHARS && this.m_charLookUp[charID] != null)
        return this.m_charLookUp[charID];
      for (int index = 0; index < this.m_charTemplateCnt; ++index)
      {
        if ((int) this.m_charTemplates[index].id == charID)
          return this.m_charTemplates[index];
      }
      return (Font.CharTemplate) null;
    }

    private float GetKerning(uint first, uint second) => 0.0f;

    private Font.CharPtr FindAdvanceOfNextWord(Font.CharPtr str, float fontSpaceX, float wrapWidth)
    {
      Font.CharPtr advanceOfNextWord = str;
      float num = fontSpaceX;
      if ((double) wrapWidth <= 0.0)
        return new Font.CharPtr((Font.AString) null);
      while (str.isNotNull && str.isNotEos && str.Val != '\n' && str.Val != ' ')
      {
        if (str.Val == '<')
        {
          if (Font.strnicmp(str, "<font color=", 12))
          {
            while (str.Val != '>')
              str.Inc();
            str.Inc();
          }
          else if (Font.strnicmp(str, "</font", 6))
          {
            while (str.Val != '>')
              str.Inc();
            str.Inc();
          }
        }
        Font.CharTemplate charTemplate = this.GetCharTemplate((int) str.Val);
        str.Inc();
        if (charTemplate != null)
        {
          num += charTemplate.xAdvance + this.GetKerning((uint) str.arr(0), (uint) str.arr(1));
          if ((double) fontSpaceX < (double) wrapWidth * 0.75 && (double) num >= (double) wrapWidth)
            return advanceOfNextWord;
        }
      }
      return (double) num > (double) wrapWidth ? advanceOfNextWord : new Font.CharPtr((Font.AString) null);
    }

    private float GetLineLength(string str) => this.GetLineLength(str, 0.0f);

    private float GetLineLength(string str, float wrapWidth)
    {
      return this.GetLineLength(str, wrapWidth, (Action<float>) null);
    }

    private float GetLineLength(string str, float wrapWidth, Action<float> charSpacing)
    {
      return this.GetLineLength(new Font.CharPtr(new Font.AString(str)), wrapWidth, charSpacing);
    }

    private float GetLineLength(Font.CharPtr str, float wrapWidth, Action<float> charSpacing)
    {
      if (charSpacing != null)
        charSpacing(0.0f);
      float num = 0.0f;
      float fontSpaceX = 0.0f;
      if ((double) wrapWidth <= 0.0)
      {
        while (str.isNotNull && str.isNotEos && str.Val != '\n')
        {
          Font.CharTemplate charTemplate = this.GetCharTemplate((int) str.arr(0));
          str.Inc();
          if (charTemplate != null)
            fontSpaceX += charTemplate.xAdvance + this.GetKerning((uint) str.arr(0), (uint) str.arr(1));
        }
      }
      else
      {
        Font.CharPtr b = new Font.CharPtr((Font.AString) null);
        while (str.isNotNull && str.isNotEos)
        {
          if (str.Val == '\n' || str.Cmp(b))
          {
            if (charSpacing == null || !str.Cmp(b))
              return fontSpaceX;
            charSpacing((wrapWidth - fontSpaceX) / num);
            return wrapWidth;
          }
          Font.CharTemplate charTemplate = this.GetCharTemplate((int) str.Val);
          str.Inc();
          if (b.isNull)
          {
            b = this.FindAdvanceOfNextWord(str, fontSpaceX, wrapWidth);
            if (b.Cmp(str))
            {
              if (charSpacing == null || (double) num <= 0.0 || (double) fontSpaceX <= (double) wrapWidth * 0.75)
                return fontSpaceX;
              charSpacing((wrapWidth - fontSpaceX) / num);
              return wrapWidth;
            }
          }
          if (charTemplate != null)
          {
            ++num;
            if (charTemplate.id == (ushort) 32)
              num += 2f;
            fontSpaceX += charTemplate.xAdvance + this.GetKerning((uint) str.arr(0), (uint) str.arr(1));
          }
        }
      }
      return fontSpaceX;
    }

    public Font()
    {
      this.m_charTemplateCnt = 0;
      this.m_charTemplates = (Font.CharTemplate[]) null;
      this.m_pageCnt = 0;
      this.m_pages = (Font.Page[]) null;
      this.m_kerningCnt = 0;
      this.m_kernings = (Font.Kerning[]) null;
      for (int index = 0; index < Font.QUICKFIND_CHARS; ++index)
        this.m_charLookUp[index] = (Font.CharTemplate) null;
      this.quad_verts = new GameVertex[Font.QUADS_IN_BUFFER * 6];
      for (int index = 0; index < Font.QUADS_IN_BUFFER * 6; ++index)
      {
        this.quad_verts[index].nx = 0.0f;
        this.quad_verts[index].ny = 0.0f;
        this.quad_verts[index].nz = 1f;
        this.quad_verts[index].Z = 0.0f;
      }
    }

    public uint GetPageCount() => (uint) this.m_pageCnt;

    public Font.Page GetPage(uint pageIdx) => this.m_pages[pageIdx];

    public uint GetCharTemplateCount() => (uint) this.m_charTemplateCnt;

    public Font.CharTemplate[] GetCharTemplateArray() => this.m_charTemplates;

    public float GetLineHeight() => this.m_lineHeight;

    public float GetBaseLine() => this.m_baseLine;

    public void Load(string file)
    {
      Font.CharPtr p = new Font.CharPtr((Font.AString) null);
      p = new Font.CharPtr(new Font.AString(MortarFile.LoadText(file)));
      int num1 = Font.cpStrlen(p);
      int o = 0;
      int index1 = 0;
      int index2 = 0;
      int index3 = 0;
      int num2;
      for (; o < num1; o += num2)
      {
        int value = Font.INVALID_NUMBER;
        Font.CharPtr stringVal = new Font.CharPtr((Font.AString) null);
        Font.CharPtr charPtr = new Font.CharPtr(new Font.AString(""));
        num2 = 0;
        if (this.Next_Word_Is(p.Offset(o), "page"))
        {
          if (this.m_pages != null)
          {
            num2 = this.Parse_Page(p.Offset(o), ref this.m_pages[index2], num1 - o);
            ++index2;
          }
        }
        else if (this.Next_Word_Is(p.Offset(o), "chars"))
        {
          num2 = this.Get_Next_Value(p.Offset(o), charPtr, (Action<int>) (v => value = v), (Action<Font.CharPtr>) (f => stringVal = f));
          if (value != Font.INVALID_NUMBER && this.m_charTemplateCnt == 0)
          {
            this.m_charTemplateCnt = value;
            this.m_charTemplates = new Font.CharTemplate[value];
          }
          if (num2 < 0)
            num2 = num2 * -1 + 2;
        }
        else if (this.Next_Word_Is(p.Offset(o), "char"))
        {
          if (this.m_charTemplates != null)
          {
            num2 = this.Parse_Char(p.Offset(o), ref this.m_charTemplates[index1], num1 - o);
            this.m_charTemplates[index1].X /= (float) this.m_textureWidth;
            this.m_charTemplates[index1].Y /= (float) this.m_textureHeight;
            this.m_charTemplates[index1].width /= this.m_lineHeight;
            this.m_charTemplates[index1].height /= this.m_lineHeight;
            this.m_charTemplates[index1].xAdvance /= this.m_lineHeight;
            this.m_charTemplates[index1].xOffset /= this.m_lineHeight;
            this.m_charTemplates[index1].yOffset /= this.m_lineHeight;
            if ((int) this.m_charTemplates[index1].id < Font.QUICKFIND_CHARS)
              this.m_charLookUp[(int) this.m_charTemplates[index1].id] = this.m_charTemplates[index1];
            ++index1;
          }
        }
        else if (this.Next_Word_Is(p.Offset(o), "kernings"))
        {
          num2 = this.Get_Next_Value(p.Offset(o), charPtr, (Action<int>) (v => value = v), (Action<Font.CharPtr>) (f => stringVal = f));
          if (value != Font.INVALID_NUMBER && this.m_kerningCnt == 0)
          {
            this.m_kerningCnt = value;
            this.m_kernings = new Font.Kerning[value];
          }
          if (num2 < 0)
            num2 = num2 * -1 + 2;
        }
        else if (this.Next_Word_Is(p.Offset(o), "kerning"))
        {
          if (this.m_kernings != null)
          {
            num2 = this.Parse_Kerning(p.Offset(o), ref this.m_kernings[index3], num1 - o);
            ++index3;
          }
        }
        else
        {
          num2 = this.Get_Next_Value(p.Offset(o), charPtr, (Action<int>) (v => value = v), (Action<Font.CharPtr>) (f => stringVal = f));
          if (stringVal.isNotNull)
            stringVal = new Font.CharPtr();
          else if (value != Font.INVALID_NUMBER)
          {
            if (this.Next_Word_Is(charPtr, "pages"))
            {
              this.m_pageCnt = value;
              this.m_pages = new Font.Page[this.m_pageCnt];
            }
            else if (this.Next_Word_Is(charPtr, "scaleW"))
              this.m_textureWidth = value;
            else if (this.Next_Word_Is(charPtr, "scaleH"))
              this.m_textureHeight = value;
            else if (this.Next_Word_Is(charPtr, "lineHeight"))
              this.m_lineHeight = (float) value;
            else if (this.Next_Word_Is(charPtr, "base"))
              this.m_baseLine = (float) value;
          }
          if (num2 < 0)
            num2 = num2 * -1 + 2;
        }
      }
      for (int index4 = 0; index4 < this.m_pageCnt; ++index4)
        this.m_pages[index4].texture = Texture.Load(this.m_pages[index4].pageName);
      this.m_baseLine /= this.m_lineHeight;
    }

    public void DrawString(string stringToDraw, Vector3 pos)
    {
      this.DrawString(stringToDraw, pos, Color.White);
    }

    public void DrawString(string stringToDraw, Vector3 pos, Color colour)
    {
      this.DrawString(stringToDraw, pos, colour, 1f);
    }

    public void DrawString(string stringToDraw, Vector3 pos, Color colour, float scale)
    {
      this.DrawString(stringToDraw, pos, colour, scale, Vector2.Zero);
    }

    public void DrawString(
      string stringToDraw,
      Vector3 pos,
      Color colour,
      float scale,
      Vector2 wrap)
    {
      this.DrawString(stringToDraw, pos, colour, scale, wrap, ALIGNMENT_TYPE.ALIGN_LEFT);
    }

    public void DrawString(
      string stringToDraw,
      Vector3 pos,
      Color colour,
      float scale,
      Vector2 wrap,
      ALIGNMENT_TYPE alignType)
    {
      this.DrawString(stringToDraw, pos, colour, scale, wrap, alignType, 1f);
    }

    public void DrawString(
      string _stringToDraw,
      Vector3 pos,
      Color colour,
      float scale,
      Vector2 wrap,
      ALIGNMENT_TYPE alignType,
      float vDir)
    {
      this.DrawString(_stringToDraw, pos, colour, scale, wrap, alignType, vDir, new MortarRectangleDec?());
    }

    public void DrawString(
      string _stringToDraw,
      Vector3 pos,
      Color colour,
      float scale,
      Vector2 wrap,
      ALIGNMENT_TYPE alignType,
      float vDir,
      MortarRectangleDec? rect)
    {
      Font.CharPtr charPtr = new Font.CharPtr(new Font.AString(_stringToDraw));
      float fontSpaceX = 0.0f;
      float num1 = 0.0f;
      wrap.X /= scale;
      wrap.Y /= vDir * scale;
      float charSpacingAlignment = 0.0f;
      float num2 = 0.0f;
      switch (alignType & ALIGNMENT_TYPE.ALIGN_HCENTER)
      {
        case ALIGNMENT_TYPE.ALIGN_LEFT:
          if ((alignType & ALIGNMENT_TYPE.ALIGN_JUSTIFY) != (ALIGNMENT_TYPE) 0)
          {
            double lineLength = (double) this.GetLineLength(charPtr, wrap.X, (Action<float>) (v => charSpacingAlignment = v));
            break;
          }
          break;
        case ALIGNMENT_TYPE.ALIGN_RIGHT:
        case ALIGNMENT_TYPE.ALIGN_HCENTER:
          Action<float> action1 = (Action<float>) (v => charSpacingAlignment = v);
          num2 = wrap.X - this.GetLineLength(charPtr, wrap.X, (alignType & ALIGNMENT_TYPE.ALIGN_JUSTIFY) != (ALIGNMENT_TYPE) 0 ? action1 : (Action<float>) null);
          if ((alignType & ALIGNMENT_TYPE.ALIGN_LEFT) != (ALIGNMENT_TYPE) 0)
          {
            num2 *= 0.5f;
            break;
          }
          break;
      }
      MatrixManager.GetInstance().Reset();
      MatrixManager.GetInstance().Scale(new Vector3(scale, scale, scale));
      MatrixManager.GetInstance().TranslateGlobal(new Vector3(pos.X, pos.Y, pos.Z));
      this.m_pages[0].texture.Set();
      Color color = colour;
      Font.CharPtr b = new Font.CharPtr((Font.AString) null);
      while (charPtr.isNotNull && charPtr.isNotEos)
      {
        int num3 = 0;
        while (charPtr.isNotNull && charPtr.isNotEos && num3 < Font.QUADS_IN_BUFFER)
        {
          if (charPtr.Val == '\n' || charPtr.Cmp(b))
          {
            if (b.isNotNull && b.Val == ' ')
              charPtr.Inc();
            switch (alignType & ALIGNMENT_TYPE.ALIGN_HCENTER)
            {
              case ALIGNMENT_TYPE.ALIGN_LEFT:
                num2 = 0.0f;
                if ((alignType & ALIGNMENT_TYPE.ALIGN_JUSTIFY) != (ALIGNMENT_TYPE) 0)
                {
                  double lineLength = (double) this.GetLineLength(charPtr.Offset(charPtr.Val == '\n' ? 1 : 0), wrap.X, (Action<float>) (v => charSpacingAlignment = v));
                  break;
                }
                break;
              case ALIGNMENT_TYPE.ALIGN_RIGHT:
              case ALIGNMENT_TYPE.ALIGN_HCENTER:
                Action<float> action2 = (Action<float>) (v => charSpacingAlignment = v);
                num2 = wrap.X - this.GetLineLength(charPtr.Offset(charPtr.Val == '\n' ? 1 : 0), wrap.X, (alignType & ALIGNMENT_TYPE.ALIGN_JUSTIFY) != (ALIGNMENT_TYPE) 0 ? action2 : (Action<float>) null);
                if ((alignType & ALIGNMENT_TYPE.ALIGN_LEFT) != (ALIGNMENT_TYPE) 0)
                {
                  num2 *= 0.5f;
                  break;
                }
                break;
            }
            fontSpaceX = 0.0f;
            num1 -= vDir;
            b = new Font.CharPtr((Font.AString) null);
          }
          if (charPtr.Val == '<')
          {
            if (Font.strnicmp(charPtr, "<font color=", 12))
            {
              char ch = '\u0006';
              byte a = color.A;
              charPtr = charPtr.Offset(12);
              uint num4 = 0;
              while (charPtr.Val != '>')
              {
                if (Math.BETWEEN((int) charPtr.Val, 48, 57))
                {
                  --ch;
                  num4 = num4 << 4 | (uint) charPtr.Val - 48U;
                }
                else if (Math.BETWEEN((int) charPtr.Val, 97, 102))
                {
                  --ch;
                  num4 = num4 << 4 | (uint) ((int) charPtr.Val - 97 + 10);
                }
                else if (Math.BETWEEN((int) charPtr.Val, 65, 70))
                {
                  --ch;
                  num4 = num4 << 4 | (uint) ((int) charPtr.Val - 65 + 10);
                }
                charPtr.Inc();
              }
              if (ch >= char.MinValue)
                num4 = (uint) ((int) a << 24 | (int) num4 & 16777215);
              charPtr.Inc();
              color.PackedValue = num4;
            }
            else if (Font.strnicmp(charPtr, "</font", 6))
            {
              color = colour;
              while (charPtr.Val != '>')
                charPtr.Inc();
              charPtr.Inc();
            }
          }
          Font.CharTemplate charTemplate = this.GetCharTemplate((int) charPtr.Val);
          charPtr.Inc();
          if (b.isNull)
          {
            b = this.FindAdvanceOfNextWord(charPtr, fontSpaceX, wrap.X);
            if (b.Cmp(charPtr))
              continue;
          }
          if (charTemplate != null)
          {
            float num5 = charTemplate.width * (this.m_lineHeight / (float) this.m_textureWidth);
            float num6 = charTemplate.height * (this.m_lineHeight / (float) this.m_textureHeight);
            float num7 = fontSpaceX + charTemplate.xOffset + num2;
            float num8 = num1 - vDir * charTemplate.yOffset;
            int index1 = num3 * 6;
            this.quad_verts[index1].X = num7;
            this.quad_verts[index1].Y = num8 - vDir * charTemplate.height;
            this.quad_verts[index1].u = charTemplate.X;
            this.quad_verts[index1].v = charTemplate.Y + num6;
            if (index1 != 0)
            {
              this.quad_verts[index1 - 1].X = this.quad_verts[index1].X;
              this.quad_verts[index1 - 1].Y = this.quad_verts[index1].Y;
            }
            this.quad_verts[index1 + 1].X = num7;
            this.quad_verts[index1 + 1].Y = num8;
            this.quad_verts[index1 + 1].u = charTemplate.X;
            this.quad_verts[index1 + 1].v = charTemplate.Y;
            this.quad_verts[index1 + 2].X = num7 + charTemplate.width;
            this.quad_verts[index1 + 2].Y = num8 - vDir * charTemplate.height;
            this.quad_verts[index1 + 2].u = charTemplate.X + num5;
            this.quad_verts[index1 + 2].v = charTemplate.Y + num6;
            this.quad_verts[index1 + 3].X = num7 + charTemplate.width;
            this.quad_verts[index1 + 3].Y = num8;
            this.quad_verts[index1 + 3].u = charTemplate.X + num5;
            this.quad_verts[index1 + 3].v = charTemplate.Y;
            this.quad_verts[index1 + 4].X = this.quad_verts[index1 + 3].X;
            this.quad_verts[index1 + 4].Y = this.quad_verts[index1 + 3].Y;
            for (int index2 = 0; index2 < 6; ++index2)
            {
              this.quad_verts[index1 + index2].color = color;
              this.quad_verts[index1 + index2].Z = 0.0f;
            }
            ++num3;
            fontSpaceX += (float) ((double) charTemplate.xAdvance + (double) this.GetKerning((uint) charPtr.arr(0), (uint) charPtr.arr(1)) + (double) charSpacingAlignment * (charTemplate.id == (ushort) 32 ? 3.0 : 1.0));
          }
        }
        Vector2 vector2 = new Vector2(0.0f, 0.0f);
        if ((alignType & ALIGNMENT_TYPE.ALIGN_VCENTER) != (ALIGNMENT_TYPE) 0)
        {
          num1 -= vDir;
          MatrixManager.GetInstance().TranslateLocal(new Vector3(0.0f, (float) ((-(double) wrap.Y - (double) num1) * ((alignType & ALIGNMENT_TYPE.ALIGN_TOP) != (ALIGNMENT_TYPE) 0 ? 0.5 : 1.0)), 0.0f));
        }
        MatrixManager.GetInstance().UploadCurrentMatrices();
        Mesh.DrawTriStrip(this.quad_verts, num3 * 6);
      }
      this.m_pages[0].texture.UnSet();
    }

    public void DrawString(string stringToDraw, float x, float y)
    {
      this.DrawString(stringToDraw, x, y, 0.0f);
    }

    public void DrawString(string stringToDraw, float x, float y, float z)
    {
      this.DrawString(stringToDraw, x, y, z, Color.White);
    }

    public void DrawString(string stringToDraw, float x, float y, float z, Color colour)
    {
      this.DrawString(stringToDraw, x, y, z, colour, 1f);
    }

    public void DrawString(
      string stringToDraw,
      float x,
      float y,
      float z,
      Color colour,
      float scale)
    {
      this.DrawString(stringToDraw, x, y, z, colour, scale, 0.0f);
    }

    public void DrawString(
      string stringToDraw,
      float x,
      float y,
      float z,
      Color colour,
      float scale,
      float wrapWidth)
    {
      this.DrawString(stringToDraw, x, y, z, colour, scale, wrapWidth, 0.0f);
    }

    public void DrawString(
      string stringToDraw,
      float x,
      float y,
      float z,
      Color colour,
      float scale,
      float wrapWidth,
      float textBoxHeight)
    {
      this.DrawString(stringToDraw, x, y, z, colour, scale, wrapWidth, textBoxHeight, ALIGNMENT_TYPE.ALIGN_LEFT);
    }

    public void DrawString(
      string stringToDraw,
      float x,
      float y,
      float z,
      Color colour,
      float scale,
      float wrapWidth,
      float textBoxHeight,
      ALIGNMENT_TYPE alignType)
    {
      this.DrawString(stringToDraw, new Vector3(x, y, z), colour, scale, new Vector2(wrapWidth, textBoxHeight), alignType);
    }

    public float MeasureString(string stringToDraw) => this.GetLineLength(stringToDraw);

    public void DrawString(
      string _stringToDraw,
      Vector3 pos,
      Color colour,
      Vector2 scale,
      Vector2 wrap,
      ALIGNMENT_TYPE alignType,
      float vDir,
      MortarRectangleDec? rect)
    {
      Font.CharPtr charPtr = new Font.CharPtr(new Font.AString(_stringToDraw));
      float fontSpaceX = 0.0f;
      float num1 = 0.0f;
      wrap.X /= scale.X;
      wrap.Y /= vDir * scale.Y;
      float charSpacingAlignment = 0.0f;
      float num2 = 0.0f;
      switch (alignType & ALIGNMENT_TYPE.ALIGN_HCENTER)
      {
        case ALIGNMENT_TYPE.ALIGN_LEFT:
          if ((alignType & ALIGNMENT_TYPE.ALIGN_JUSTIFY) != (ALIGNMENT_TYPE) 0)
          {
            double lineLength = (double) this.GetLineLength(charPtr, wrap.X, (Action<float>) (v => charSpacingAlignment = v));
            break;
          }
          break;
        case ALIGNMENT_TYPE.ALIGN_RIGHT:
        case ALIGNMENT_TYPE.ALIGN_HCENTER:
          Action<float> action1 = (Action<float>) (v => charSpacingAlignment = v);
          num2 = wrap.X - this.GetLineLength(charPtr, wrap.X, (alignType & ALIGNMENT_TYPE.ALIGN_JUSTIFY) != (ALIGNMENT_TYPE) 0 ? action1 : (Action<float>) null);
          if ((alignType & ALIGNMENT_TYPE.ALIGN_LEFT) != (ALIGNMENT_TYPE) 0)
          {
            num2 *= 0.5f;
            break;
          }
          break;
      }
      MatrixManager.GetInstance().Reset();
      MatrixManager.GetInstance().Scale(new Vector3(scale.X, scale.Y, 0.0f));
      MatrixManager.GetInstance().TranslateGlobal(new Vector3(pos.X, pos.Y, pos.Z));
      this.m_pages[0].texture.Set();
      Color color = colour;
      Font.CharPtr b = new Font.CharPtr((Font.AString) null);
      while (charPtr.isNotNull && charPtr.isNotEos)
      {
        int num3 = 0;
        while (charPtr.isNotNull && charPtr.isNotEos && num3 < Font.QUADS_IN_BUFFER)
        {
          if (charPtr.Val == '\n' || charPtr.Cmp(b))
          {
            if (b.isNotNull && b.Val == ' ')
              charPtr.Inc();
            switch (alignType & ALIGNMENT_TYPE.ALIGN_HCENTER)
            {
              case ALIGNMENT_TYPE.ALIGN_LEFT:
                num2 = 0.0f;
                if ((alignType & ALIGNMENT_TYPE.ALIGN_JUSTIFY) != (ALIGNMENT_TYPE) 0)
                {
                  double lineLength = (double) this.GetLineLength(charPtr.Offset(charPtr.Val == '\n' ? 1 : 0), wrap.X, (Action<float>) (v => charSpacingAlignment = v));
                  break;
                }
                break;
              case ALIGNMENT_TYPE.ALIGN_RIGHT:
              case ALIGNMENT_TYPE.ALIGN_HCENTER:
                Action<float> action2 = (Action<float>) (v => charSpacingAlignment = v);
                num2 = wrap.X - this.GetLineLength(charPtr.Offset(charPtr.Val == '\n' ? 1 : 0), wrap.X, (alignType & ALIGNMENT_TYPE.ALIGN_JUSTIFY) != (ALIGNMENT_TYPE) 0 ? action2 : (Action<float>) null);
                if ((alignType & ALIGNMENT_TYPE.ALIGN_LEFT) != (ALIGNMENT_TYPE) 0)
                {
                  num2 *= 0.5f;
                  break;
                }
                break;
            }
            fontSpaceX = 0.0f;
            num1 -= vDir;
            b = new Font.CharPtr((Font.AString) null);
          }
          if (charPtr.Val == '<')
          {
            if (Font.strnicmp(charPtr, "<font color=", 12))
            {
              char ch = '\u0006';
              byte a = color.A;
              charPtr = charPtr.Offset(12);
              uint num4 = 0;
              while (charPtr.Val != '>')
              {
                if (Math.BETWEEN((int) charPtr.Val, 48, 57))
                {
                  --ch;
                  num4 = num4 << 4 | (uint) charPtr.Val - 48U;
                }
                else if (Math.BETWEEN((int) charPtr.Val, 97, 102))
                {
                  --ch;
                  num4 = num4 << 4 | (uint) ((int) charPtr.Val - 97 + 10);
                }
                else if (Math.BETWEEN((int) charPtr.Val, 65, 70))
                {
                  --ch;
                  num4 = num4 << 4 | (uint) ((int) charPtr.Val - 65 + 10);
                }
                charPtr.Inc();
              }
              if (ch >= char.MinValue)
                num4 = (uint) ((int) a << 24 | (int) num4 & 16777215);
              charPtr.Inc();
              color.PackedValue = num4;
            }
            else if (Font.strnicmp(charPtr, "</font", 6))
            {
              color = colour;
              while (charPtr.Val != '>')
                charPtr.Inc();
              charPtr.Inc();
            }
          }
          Font.CharTemplate charTemplate = this.GetCharTemplate((int) charPtr.Val);
          charPtr.Inc();
          if (b.isNull)
          {
            b = this.FindAdvanceOfNextWord(charPtr, fontSpaceX, wrap.X);
            if (b.Cmp(charPtr))
              continue;
          }
          if (charTemplate != null)
          {
            float num5 = charTemplate.width * (this.m_lineHeight / (float) this.m_textureWidth);
            float num6 = charTemplate.height * (this.m_lineHeight / (float) this.m_textureHeight);
            float num7 = fontSpaceX + charTemplate.xOffset + num2;
            float num8 = num1 - vDir * charTemplate.yOffset;
            int index1 = num3 * 6;
            this.quad_verts[index1].X = num7;
            this.quad_verts[index1].Y = num8 - vDir * charTemplate.height;
            this.quad_verts[index1].u = charTemplate.X;
            this.quad_verts[index1].v = charTemplate.Y + num6;
            if (index1 != 0)
            {
              this.quad_verts[index1 - 1].X = this.quad_verts[index1].X;
              this.quad_verts[index1 - 1].Y = this.quad_verts[index1].Y;
            }
            this.quad_verts[index1 + 1].X = num7;
            this.quad_verts[index1 + 1].Y = num8;
            this.quad_verts[index1 + 1].u = charTemplate.X;
            this.quad_verts[index1 + 1].v = charTemplate.Y;
            this.quad_verts[index1 + 2].X = num7 + charTemplate.width;
            this.quad_verts[index1 + 2].Y = num8 - vDir * charTemplate.height;
            this.quad_verts[index1 + 2].u = charTemplate.X + num5;
            this.quad_verts[index1 + 2].v = charTemplate.Y + num6;
            this.quad_verts[index1 + 3].X = num7 + charTemplate.width;
            this.quad_verts[index1 + 3].Y = num8;
            this.quad_verts[index1 + 3].u = charTemplate.X + num5;
            this.quad_verts[index1 + 3].v = charTemplate.Y;
            this.quad_verts[index1 + 4].X = this.quad_verts[index1 + 3].X;
            this.quad_verts[index1 + 4].Y = this.quad_verts[index1 + 3].Y;
            for (int index2 = 0; index2 < 6; ++index2)
            {
              this.quad_verts[index1 + index2].color = color;
              this.quad_verts[index1 + index2].Z = 0.0f;
            }
            ++num3;
            fontSpaceX += (float) ((double) charTemplate.xAdvance + (double) this.GetKerning((uint) charPtr.arr(0), (uint) charPtr.arr(1)) + (double) charSpacingAlignment * (charTemplate.id == (ushort) 32 ? 3.0 : 1.0));
          }
        }
        Vector2 vector2 = new Vector2(0.0f, 0.0f);
        if ((alignType & ALIGNMENT_TYPE.ALIGN_VCENTER) != (ALIGNMENT_TYPE) 0)
        {
          num1 -= vDir;
          MatrixManager.GetInstance().TranslateLocal(new Vector3(0.0f, (float) ((-(double) wrap.Y - (double) num1) * ((alignType & ALIGNMENT_TYPE.ALIGN_TOP) != (ALIGNMENT_TYPE) 0 ? 0.5 : 1.0)), 0.0f));
        }
        MatrixManager.GetInstance().UploadCurrentMatrices();
        Mesh.DrawTriStrip(this.quad_verts, num3 * 6);
      }
      this.m_pages[0].texture.UnSet();
    }

    public class CharTemplate
    {
      public ushort id;
      public float X;
      public float Y;
      public float width;
      public float height;
      public float xOffset;
      public float yOffset;
      public float xAdvance;
      public byte page;
    }

    public struct Page
    {
      public string pageName;
      public Texture texture;
    }

    public struct Kerning
    {
      public int first;
      public int second;
      public float amount;
    }

    public class AString
    {
      public string astring;

      public AString(string str) => this.astring = str;
    }

    public struct CharPtr
    {
      public Font.AString str;
      public int idx;

      public CharPtr(Font.AString s)
      {
        this.str = s != null ? s : (Font.AString) null;
        this.idx = 0;
      }

      public CharPtr(Font.AString s, int i)
      {
        if (s == null)
        {
          this.str = (Font.AString) null;
          this.idx = 0;
        }
        else
        {
          this.str = s;
          this.idx = i;
        }
      }

      public void Inc() => ++this.idx;

      public void Dec() => --this.idx;

      public char Val => this.str.astring[this.idx];

      public char arr(int i)
      {
        return i + this.idx >= this.str.astring.Length ? char.MinValue : this.str.astring[this.idx + i];
      }

      public bool isNull => this.str == null;

      public bool isNotNull => this.str != null;

      public bool isEos => this.idx == this.str.astring.Length;

      public bool isNotEos => this.idx != this.str.astring.Length;

      public bool Cmp(Font.CharPtr b)
      {
        return object.ReferenceEquals((object) this.str, (object) b.str) && b.idx == this.idx;
      }

      public Font.CharPtr Offset(int o)
      {
        return new Font.CharPtr()
        {
          str = this.str,
          idx = this.idx + o
        };
      }

      public string Str => this.str.astring.Substring(this.idx);
    }
  }
}
