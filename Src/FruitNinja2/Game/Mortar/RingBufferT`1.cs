
// Type: Mortar.RingBufferT`1
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


#nullable disable
namespace Mortar
{
  public class RingBufferT<T>
  {
    private T[] mem;
    private int memsize;
    private int inptr;
    private int outptr;

    public void Init(int size)
    {
      this.Clear();
      this.mem = new T[size];
      this.memsize = size;
      this.inptr = 1;
      this.outptr = 0;
    }

    public bool Push(ref T v)
    {
      if (this.inptr == this.outptr)
        return false;
      this.mem[this.inptr] = v;
      if (this.inptr == this.memsize - 1)
        this.inptr = 0;
      else
        ++this.inptr;
      return true;
    }

    public bool Peek(ref T ans)
    {
      int index = this.outptr == this.memsize - 1 ? 0 : this.outptr + 1;
      if (index == this.inptr)
        return false;
      ans = this.mem[index];
      return true;
    }

    public bool Pop(ref T ans)
    {
      int index = this.outptr == this.memsize - 1 ? 0 : this.outptr + 1;
      if (index == this.inptr)
        return false;
      ans = this.mem[index];
      this.outptr = index;
      return true;
    }

    public void Clear()
    {
      if (this.mem == null)
        return;
      Delete.SAFE_DELETE_ARRAY<T[]>(ref this.mem);
    }
  }
}
