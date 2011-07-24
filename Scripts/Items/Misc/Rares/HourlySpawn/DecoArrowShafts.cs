using System;

namespace Server.Items
{
	public class DecoArrowShafts : Item
	{

		[Constructable]
		public DecoArrowShafts() : base( 0x1025 )
		{
			Movable = true;
			Stackable = false;
		}

		public DecoArrowShafts( Serial serial ) : base( serial )
		{
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