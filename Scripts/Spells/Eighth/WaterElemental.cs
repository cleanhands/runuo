using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Spells.Eighth
{
	public class WaterElementalSpell : Spell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Water Elemental", "Kal Vas Xen An Flam",
				SpellCircle.Eighth,
				269,
				9070,
				false,
				Reagent.Bloodmoss,
				Reagent.MandrakeRoot,
				Reagent.SpidersSilk
			);

		public WaterElementalSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override bool CheckCast()
		{
			if ( !base.CheckCast() )
				return false;

			if ( (Caster.Followers + 3) > Caster.FollowersMax )
			{
				Caster.SendLocalizedMessage( 1049645 ); // You have too many followers to summon that creature.
				return false;
			}

			return true;
		}

		public override void OnCast()
		{
			if ( CheckSequence() )
			{
				TimeSpan duration = TimeSpan.FromSeconds( (2 * Caster.Skills.Magery.Fixed) / 5 );

				if ( Core.AOS )
					SpellHelper.Summon( new SummonedWaterElemental(), Caster, 0x217, duration, false, false );
				else
					SpellHelper.Summon( new WaterElemental(), Caster, 0x217, duration, false, false );
			}

			FinishSequence();
		}
	}
}