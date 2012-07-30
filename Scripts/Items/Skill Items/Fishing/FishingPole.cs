using System;
using System.Collections;
using Server.Targeting;
using Server.Items;
using Server.Engines.Harvest;
using System.Collections.Generic;
using Server.ContextMenus;
using Server.Network;

namespace Server.Items
{
	public class FishingPole : Item
	{
		[Constructable]
		public FishingPole() : base( 0x0DC0 )
		{
			Layer = Layer.OneHanded;
			Weight = 8.0;
		}

		public override void OnDoubleClick( Mobile from )
		{
			Point3D loc = GetWorldLocation();

			if ( !from.InLOS( loc ) || !from.InRange( loc, 2 ) )
				from.LocalOverheadMessage( MessageType.Regular, 0x3E9, 1019045 ); // I can't reach that
			else
				Fishing.System.BeginHarvesting( from, this );
		}

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			base.GetContextMenuEntries( from, list );

			BaseHarvestTool.AddContextMenuEntries( from, this, list, Fishing.System );
		}

		public FishingPole( Serial serial ) : base( serial )
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