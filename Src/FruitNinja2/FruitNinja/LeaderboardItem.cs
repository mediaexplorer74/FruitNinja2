
// Type: FruitNinja2.LeaderboardItem
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Mortar;

#nullable disable
namespace FruitNinja2
{
  internal class LeaderboardItem : ScrollingMenuItem
  {
    protected int m_score;
    protected int m_rank;

    public LeaderboardItem()
    {
    }

    public LeaderboardItem(string name, int rank, int score)
    {
      this.m_text = rank.ToString() + ". " + name.ToUpper();
      this.m_rank = rank;
      this.m_score = score;
      this.m_height = 25f;
      this.m_colour = new Color(116, 93, 59);
    }

    public override void Draw()
    {
      if (this.m_text == null)
        return;
      Vector3 vector3 = Vector3.Add(this.m_pos, this.m_textOffset);
      MortarRectangleDec? rect = new MortarRectangleDec?();
      MortarRectangleDec mortarRectangleDec;
      mortarRectangleDec.left = 0.0f;
      mortarRectangleDec.right = 0.0f;
      if (this.m_parentList != null)
      {
        mortarRectangleDec.top = this.m_parentList.m_pos.Y + this.m_parentList.GetHeight() / 2f;
        mortarRectangleDec.bottom = this.m_parentList.m_pos.Y - this.m_parentList.GetHeight() / 2f;
        mortarRectangleDec.left = this.m_parentList.m_pos.X - this.m_parentList.GetWidth() / 2f;
        mortarRectangleDec.right = this.m_parentList.m_pos.X + this.m_parentList.GetWidth() / 2f;
        rect = new MortarRectangleDec?(mortarRectangleDec);
      }
      Game.game_work.pGameFont.DrawString(this.m_text, Vector3.Add(new Vector3(mortarRectangleDec.left, vector3.Y, 0.0f), this.m_textOffset), this.m_colour, 18f, Vector2.Zero, ALIGNMENT_TYPE.ALIGN_VCENTER | ALIGNMENT_TYPE.ALIGN_LEFT, 1f, rect);
      Game.game_work.pGameFont.DrawString(this.m_score.ToString(), Vector3.Add(new Vector3(mortarRectangleDec.right - this.m_parentList.GetWidth() * 0.1f, vector3.Y, 0.0f), this.m_textOffset), this.m_colour, 18f, Vector2.Zero, ALIGNMENT_TYPE.ALIGN_VCENTER | ALIGNMENT_TYPE.ALIGN_RIGHT, 1f, rect);
    }
  }
}
