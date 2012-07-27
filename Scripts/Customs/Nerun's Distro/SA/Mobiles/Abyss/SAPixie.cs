using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a pixie corpse" )]
	public class SAPixie : BaseCreature
	{
		public override bool InitialInnocent{ get{ return true; } }

		[Constructable]
		public SAPixie() : base( AIType.AI_Mage, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = NameList.RandomName( "pixie" );
			Body = 128;
			BaseSoundID = 0x467;

			SetStr( 21, 30 );
			SetDex( 301, 400 );
			SetInt( 201, 250 );

			SetHits( 13, 18 );

			SetDamage( 9, 15 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 80, 90 );
			SetResistance( ResistanceType.Fire, 40, 50 );
			SetResistance( ResistanceType.Cold, 40, 50 );
			SetResistance( ResistanceType.Poison, 40, 50 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.EvalInt, 90.1, 100.0 );
			SetSkill( SkillName.Magery, 90.1, 100.0 );
			SetSkill( SkillName.Meditation, 90.1, 100.0 );
			SetSkill( SkillName.MagicResist, 100.5, 150.0 );
			SetSkill( SkillName.Tactics, 10.1, 20.0 );
			SetSkill( SkillName.Wrestling, 10.1, 12.5 );

			Fame = 4000;
			Karma = -4000;

			VirtualArmor = 100;
			if ( 0.02 > Utility.RandomDouble() )
				PackStatue();				
		}
		
		public override void OnDeath( Container c )
		{
			base.OnDeath( c );		
				
			#region Mondain's Legacy
//			if ( Utility.RandomDouble() < 0.3 )
//				c.DropItem( new PixieLeg() );
			#endregion
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.LowScrolls );
			AddLoot( LootPack.Gems, 2 );
		}

		public override HideType HideType{ get{ return HideType.Spined; } }
		public override int Hides{ get{ return 5; } }
		public override int Meat{ get{ return 1; } }

		public SAPixie( Serial serial ) : base( serial )
		{
		}

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.FeyAndUndead; }
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}