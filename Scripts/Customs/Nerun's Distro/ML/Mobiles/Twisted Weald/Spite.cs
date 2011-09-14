using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class Spite : Changeling
	{
		[Constructable]
		public Spite() : base()
		{
			Name = "a Guile";
			Hue = 0x21;

			SetStr( 53, 214 );
			SetDex( 243, 367 );
			SetInt( 369, 586 );

			SetHits( 1013, 1052 );
			SetStam( 243, 367 );
			SetMana( 369, 586 );

			SetDamage( 14, 20 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 85 );
			SetResistance( ResistanceType.Fire, 46 );
			SetResistance( ResistanceType.Cold, 44 );
			SetResistance( ResistanceType.Poison, 42);
			SetResistance( ResistanceType.Energy, 47 );

			SetSkill( SkillName.Wrestling, 12.8, 16.7);
			SetSkill( SkillName.Tactics, 12.6, 131.0 );
			SetSkill( SkillName.MagicResist, 141.2, 161.6 );
			SetSkill( SkillName.Magery, 108.4, 119.2 );
			SetSkill( SkillName.EvalInt, 108.4, 120.0 );
			SetSkill( SkillName.Meditation, 109.2, 120.0 );
		}

		public Spite( Serial serial ) : base( serial )
		{
		}
		
		public override void GenerateLoot()
		{
			AddLoot( LootPack.AosUltraRich, 3 );
		}
				
//OFF		public override bool GivesMinorArtifact{ get{ return true; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			
			int version = reader.ReadInt();
		}
	}
}
