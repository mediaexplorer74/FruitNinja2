
// Type: Mortar.MeshManager
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

#nullable disable
namespace Mortar
{
  public class MeshManager
  {
    private static MeshManager instance = new MeshManager();

    public static MeshManager GetInstance() => MeshManager.instance;

    public Model Load(string loaddasdas)
    {
      loaddasdas = Path.ChangeExtension(loaddasdas, ".wp7mesh");
      string directoryName = Path.GetDirectoryName(loaddasdas);
      Model model = new Model();
      BinaryReader binaryReader1 = MortarFile.LoadBinBR(loaddasdas);
      if (binaryReader1 == null)
        return model;
      byte[] d = binaryReader1.ReadBytes((int) (binaryReader1.BaseStream.Length
          - binaryReader1.BaseStream.Position));

      binaryReader1.Dispose();

      BinaryReader binaryReader2 = new BinaryReader((Stream) new MortarFile.ByteStream(d));
      Matrix matrix1 = new Matrix();
      Vector3 vector3 = new Vector3();
      Quaternion quaternion = new Quaternion();
      Matrix identity = Matrix.Identity;
      matrix1.M11 = binaryReader2.ReadSingle();
      matrix1.M12 = binaryReader2.ReadSingle();
      matrix1.M13 = binaryReader2.ReadSingle();
      matrix1.M14 = binaryReader2.ReadSingle();
      matrix1.M21 = binaryReader2.ReadSingle();
      matrix1.M22 = binaryReader2.ReadSingle();
      matrix1.M23 = binaryReader2.ReadSingle();
      matrix1.M24 = binaryReader2.ReadSingle();
      matrix1.M31 = binaryReader2.ReadSingle();
      matrix1.M32 = binaryReader2.ReadSingle();
      matrix1.M33 = binaryReader2.ReadSingle();
      matrix1.M34 = binaryReader2.ReadSingle();
      matrix1.M41 = binaryReader2.ReadSingle();
      matrix1.M42 = binaryReader2.ReadSingle();
      matrix1.M43 = binaryReader2.ReadSingle();
      matrix1.M44 = binaryReader2.ReadSingle();
      vector3.X = binaryReader2.ReadSingle();
      vector3.Y = binaryReader2.ReadSingle();
      vector3.Z = binaryReader2.ReadSingle();
      quaternion.X = binaryReader2.ReadSingle();
      quaternion.Y = binaryReader2.ReadSingle();
      quaternion.Z = binaryReader2.ReadSingle();
      quaternion.W = binaryReader2.ReadSingle();
      identity.M11 = binaryReader2.ReadSingle();
      identity.M12 = binaryReader2.ReadSingle();
      identity.M13 = binaryReader2.ReadSingle();
      identity.M21 = binaryReader2.ReadSingle();
      identity.M22 = binaryReader2.ReadSingle();
      identity.M23 = binaryReader2.ReadSingle();
      identity.M31 = binaryReader2.ReadSingle();
      identity.M32 = binaryReader2.ReadSingle();
      identity.M33 = binaryReader2.ReadSingle();

      Matrix matrix2 = Matrix.Multiply(Matrix.Multiply(identity, 
          Matrix.Transpose(Matrix.CreateFromQuaternion(quaternion))), 
          Matrix.Transpose(Matrix.CreateTranslation(vector3)));

      model.amatrix = Matrix.Multiply(matrix1, matrix2);
      uint length = binaryReader2.ReadUInt32();
      model.vertecies = new VertexPositionColorTexture[(int)length];
      for (int index = 0; (long) index < (long) length; ++index)
      {
        float num1 = binaryReader2.ReadSingle();
        float num2 = binaryReader2.ReadSingle();
        float num3 = binaryReader2.ReadSingle();
        float num4 = binaryReader2.ReadSingle();
        float num5 = binaryReader2.ReadSingle();
        uint num6 = binaryReader2.ReadUInt32();
        Color color = new Color();
        color.PackedValue = num6;
        model.vertecies[index].Color = color;
        model.vertecies[index].Position = new Vector3(num1, num2, num3);
        model.vertecies[index].TextureCoordinate = new Vector2(num4, num5);
      }
      int count = binaryReader2.ReadInt32();
      if (count > 0)
      {
        string str = new string(binaryReader2.ReadChars(count));
        model.tex = Texture.Load(directoryName + "\\" + str);
      }
      return model;
    }

    public void Initialise()
    {
    }

    public void Initialise(int heapsize)
    {
    }
  }
}
