using System;
using Server;
using Server.Items;

namespace Server.Mobiles 
{ 
	public class CrystalWisp : Wisp 
	{ 				
		[Constructable] 
		public CrystalWisp() : base() 
		{ 			
			Hue = 0x482;
			
			PackReg( 1 );
		}

		public override void GenerateLoot()
		{
		}

		public CrystalWisp( Serial serial ) : base( serial )
		{
		}

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