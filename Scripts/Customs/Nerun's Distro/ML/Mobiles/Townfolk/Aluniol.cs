using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class Aluniol : BaseVendor
	{
		private List<SBInfo> m_SBInfos = new List<SBInfo>();
		protected override List<SBInfo> SBInfos{ get { return m_SBInfos; } }
		
		public override bool CanTeach{ get{ return false; } }
		public override bool IsInvulnerable{ get{ return true; } }
		
		public override void InitSBInfo()
		{		
		}

		[Constructable]
		public Aluniol() : base( "the healer" )
		{			
			Name = "Aluniol";
		}
		
		public Aluniol( Serial serial ) : base( serial )
		{
		}
		
		public override void InitBody()
		{
			InitStats( 100, 100, 25 );
			
			Female = false;
			Race = Race.Elf;
			
			Hue = 0x8383;
			HairItemID = 0x2FBF;
			HairHue = 0x323;			
		}
		
		public override void InitOutfit()
		{
			AddItem( new ElvenBoots( 0x1BB ) );
			AddItem( new MaleElvenRobe( 0x47E ) );
			AddItem( new WildStaff() );
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