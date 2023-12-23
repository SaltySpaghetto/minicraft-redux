using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vildmark.Serialization;

namespace MiniCraftRedux.Entities
{
    public class Cow : Mob
    {
        private int xa, ya;
        private int randomWalkTime = 0;

        public Cow()
       : base(10)
        {
        }
        public override void Serialize(IWriter writer)
        {
            base.Serialize(writer);

            writer.WriteValue(xa);
            writer.WriteValue(ya);
            writer.WriteValue(randomWalkTime);
        }

        public override void Deserialize(IReader reader)
        {
            base.Deserialize(reader);

            xa = reader.ReadValue<int>();
            ya = reader.ReadValue<int>();
            randomWalkTime = reader.ReadValue<int>();
        }

        public override void Update()
        {
            base.Update();

            int speed = TickTime & 1;
            if (!Move(xa * speed, ya * speed) || Random.NextInt(200) == 0)
            {
                randomWalkTime = 60;
                xa = (Random.NextInt(3) - 1) * Random.NextInt(2);
                ya = (Random.NextInt(3) - 1) * Random.NextInt(2);
            }
            if (randomWalkTime > 0)
            {
                randomWalkTime--;
            }
        }
        public override void Render(Screen screen)
        {
            int xt = 16;
            int yt = 14;

            int flip1 = WalkDist >> 3 & 1;
            int flip2 = WalkDist >> 3 & 1;

            if (Direction == Direction.Up)
            {
                xt += 2;
            }

            if (Direction == Direction.Left || Direction == Direction.Right)
            {

                flip1 = 0;
                flip2 = WalkDist >> 4 & 1;

                if (Direction == Direction.Left)
                {
                    flip1 = 1;
                }

                xt += 4 + (WalkDist >> 3 & 1) * 2;
            }

            int xo = X - 8;
            int yo = Y - 11;

            int col = Color.Get(-1, 100, 322, 432);

            if (ImmuneTime > 0)
            {
                col = Color.Get(-1, 555, 555, 555);
            }

            screen.Render(xo + 8 * flip1, yo + 0, xt + yt * 32, col, (MirrorFlags)flip1);
            screen.Render(xo + 8 - 8 * flip1, yo + 0, xt + 1 + yt * 32, col, (MirrorFlags)flip1);
            screen.Render(xo + 8 * flip1, yo + 8, xt + (yt + 1) * 32, col, (MirrorFlags)flip1);
            screen.Render(xo + 8 - 8 * flip1, yo + 8, xt + 1 + (yt + 1) * 32, col, (MirrorFlags)flip1);
        }

        public override void Die()
        {
            base.Die();

            int count = Random.NextInt(2) + 1;
            for (int i = 0; i < count; i++)
            {
                Level.Add(new ItemEntity(new ResourceItem(Resource.RawBeef), X + Random.NextInt(11) - 5, Y + Random.NextInt(11) - 5));
            }

            if (Level.GamePlayer != null)
            {
                Level.GamePlayer.score += 25;
            }

        }


    }
}
