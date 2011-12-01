using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.ContextMenus;

namespace Server.Items
{
	public class FishBowl : BaseContainer
	{
		public override int LabelNumber{ get{ return 1074499; } } // A fish bowl

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Empty
		{
			get{ return ( Items.Count == 0 ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public BaseFish Fish
		{
			get
			{
				if ( Empty )
					return null;

				if ( Items[ 0 ] is BaseFish )
					return (BaseFish) Items[ 0 ];

				return null;
			}
		}

		public override double DefaultWeight { get { return 2.0; } }

		[Constructable]
		public FishBowl() : base( 0x241C )
		{
			Hue = 0x47E;
			MaxItems = 1;
		}

		public FishBowl( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
		}

		public override bool OnDragDrop( Mobile from, Item dropped )
		{
			if ( !IsAccessibleTo( from ) )
			{
				from.SendLocalizedMessage( 502436 ); // That is not accessible.
				return false;
			}

			if ( !( dropped is BaseFish ) )
			{
				from.SendLocalizedMessage( 1074836 ); // The container can not hold that type of object.
				return false;
			}

			if ( base.OnDragDrop( from, dropped ) )
			{
				((BaseFish) dropped).StopTimer();
				InvalidateProperties();

				return true;
			}

			return false;
		}

		public override void AddNameProperties( ObjectPropertyList list )
		{
			base.AddNameProperties( list );

			if ( !Empty )
			{
				BaseFish fish = Fish;

				if ( fish != null )
					list.Add( 1074494, "#{0}", fish.LabelNumber ); // Contains: ~1_CREATURE~
			}
		}

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			base.GetContextMenuEntries( from, list );

			if ( !Empty && IsAccessibleTo( from ) )
				list.Add( new RemoveCreature( this ) );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( version == 0 )
				Weight = DefaultWeight;
		}

		private class RemoveCreature : ContextMenuEntry
		{
			private FishBowl m_Bowl;

			public RemoveCreature( FishBowl bowl ) : base( 6242, 3 ) // Remove creature
			{
				m_Bowl = bowl;
			}

			public override void OnClick()
			{
				if ( m_Bowl == null || m_Bowl.Deleted || !m_Bowl.IsAccessibleTo( Owner.From ) )
					return;

				BaseFish fish = m_Bowl.Fish;

				if ( fish != null )
				{
					if ( fish.IsLockedDown )
					{
						Owner.From.SendLocalizedMessage( 1010449 ); // You may not use this object while it is locked down.
					}
					else if ( !Owner.From.PlaceInBackpack( fish ) )
					{
						Owner.From.SendLocalizedMessage( 1074496 ); // There is no room in your pack for the creature.
					}
					else
					{
						Owner.From.SendLocalizedMessage( 1074495 ); // The creature has been removed from the fish bowl.
						fish.StartTimer();
						m_Bowl.InvalidateProperties();
					}
				}
			}
		}
	}
}