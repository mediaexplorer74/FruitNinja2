
// Type: GameManager.EntityFactory
// Assembly: FNWP72, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6D83AD8C-624F-452F-AF0A-F1A481FF708B


using Mortar;

#nullable disable
namespace GameManager
{
  public static class EntityFactory
  {
    private static EntityFactory.EntityHash[] hashes = new EntityFactory.EntityHash[5]
    {
      new EntityFactory.EntityHash(true, "fruit", EntityTypes.ENTITY_BEGIN),
      new EntityFactory.EntityHash(true, "bomb", EntityTypes.ENTITY_BOMB),
      new EntityFactory.EntityHash(true, "slash", EntityTypes.ENTITY_SLASH),
      new EntityFactory.EntityHash(true, "blast", EntityTypes.ENTITY_BOMB_BLAST),
      new EntityFactory.EntityHash(true, "coin", EntityTypes.ENTITY_COIN)
    };

    public static Entity CreateEntity(int type)
    {
      Entity entity = (Entity) null;
      switch (type)
      {
        case 0:
          entity = (Entity) new Fruit();
          break;
        case 1:
          entity = (Entity) new Bomb();
          break;
        case 2:
          entity = (Entity) new Coin();
          break;
        case 3:
          entity = (Entity) new SlashEntity();
          break;
        case 4:
          entity = (Entity) new BombBlast();
          break;
      }
      return entity;
    }

    public static int HashTypeConvert(uint hash, ref bool update)
    {
      for (int index = 0; index < EntityFactory.hashes.Length; ++index)
      {
        if ((int) hash == (int) EntityFactory.hashes[index].hash)
        {
          update = EntityFactory.hashes[index].update;
          return (int) EntityFactory.hashes[index].type;
        }
      }
      update = false;
      return -1;
    }

    public struct EntityHash(bool u, string h, EntityTypes t)
    {
      public bool update = u;
      public uint hash = StringFunctions.StringHash(h);
      public uint type = (uint) t;
    }
  }
}
