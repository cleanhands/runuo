/**************************************
*Script Name: Static Exporter
*Author: Nerun
*Rewritten by: Joeku
*For use with RunUO 2.0 RC2
*Client Tested with: 6.0.9.1
*Version: 2.00
*Initial Release: 04/13/05
*Revision Date: 08/09/08
**************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;
using Server.Targeting;
using Server.Engines.Quests.Haven;
using Server.Engines.Quests.Necro;

namespace Joeku.SE
{
	public class SE_Main
	{
		public const int Version = 200; // Script version (do not change)
		public static string FilePath = @".\Export\";

		public static void Initialize()
		{
			CommandSystem.Register("StaticExport" , AccessLevel.Administrator, new CommandEventHandler(StaticExport_OnCommand));
			CommandSystem.Register("StaEx" , AccessLevel.Administrator, new CommandEventHandler(StaticExport_OnCommand));
		}

		[Usage( "StaticExport [string filename]" )]
		[Aliases( "StaEx" )]
		[Description( "Exports statics to a cfg decoration file." )]
		public static void StaticExport_OnCommand(CommandEventArgs e )
		{
			if( e.Arguments.Length > 0 )
				BeginStaEx(e.Mobile, e.ArgString );
			else
				e.Mobile.SendMessage("Format: StaticExport [string filename]" );
		}

		public static void BeginStaEx(Mobile mob, string file )
		{
	    	BoundingBoxPicker.Begin(mob, new BoundingBoxCallback(StaExBox_Callback), new object[]{ file });
		}

		private static void StaExBox_Callback(Mobile mob, Map map, Point3D start, Point3D end, object state)
		{
			object[] states = (object[])state;
			string file = (string)states[0];

			Export(mob, file, new Rectangle2D(new Point2D(start.X, start.Y), new Point2D(end.X+1, end.Y+1)));
		}

		private static void Export(Mobile mob, string file, Rectangle2D rect)
		{
			Map map = mob.Map;

			if( !Directory.Exists(FilePath) )
				Directory.CreateDirectory(FilePath);

			using(StreamWriter op = new StreamWriter(String.Format(@".\Export\{0}.cfg", file)))
			{

				mob.SendMessage("Exporting statics...");

				op.WriteLine("# Saved By Static Exporter");
				op.WriteLine("# StaticExport by Nerun");
				op.WriteLine("# Rewritten by Joeku");
				op.WriteLine();

				IPooledEnumerable eable = mob.Map.GetItemsInBounds(rect);
				int i = 0;

				try
				{
					foreach(Item item in eable)
					{
						if( item == null || item.Deleted )
							continue;
						if( item is AddonComponent )
							continue;

						string s = Construct(item);
						if( !s.Substring(0, s.IndexOf(' ')+1).Contains("+") ) // Make sure this isn't an InternalItem of a class...
						{
							op.WriteLine(s);
							op.WriteLine("{0} {1} {2}", item.X, item.Y, item.Z);
							op.WriteLine();
							i++;
						}
					}
				
					mob.SendMessage("You exported {0} statics from this facet.", i);
				}
				catch(Exception e){ mob.SendMessage(e.Message); }

				eable.Free();
			}
		}

		public static List<string[]> List = new List<string[]>();

		public static void Add(string s){ Add(s, ""); }
		public static void Add(string s1, string s2)
		{
			List.Add(new string[]{s1, s2});
		}

		public static string Construct(Item item)
		{
			string s;

			int itemID = item.ItemID;

			if( item is BaseAddon )
				for( int i = 0; i < ((BaseAddon)item).Components.Count; i++ )
					if( ((BaseAddon)item).Components[i].Offset == Point3D.Zero )
					{
						itemID = ((BaseAddon)item).Components[i].ItemID;
						break;
					}

			if( item is LocalizedStatic )
				Add("LabelNumber", ((LocalizedStatic)item).Number.ToString());
			else if( item is LocalizedSign )
				Add("LabelNumber", ((LocalizedSign)item).Number.ToString());
			else if( item is AnkhWest )
				Add("Bloodied", (item.ItemID == 0x1D98).ToString());
			else if( item is AnkhNorth )
				Add("Bloodied", (item.ItemID == 0x1E5D).ToString());
			else if( item is MarkContainer )
			{
				Add("Bone", ((MarkContainer)item).Bone.ToString());
				Add("Locked", ((MarkContainer)item).AutoLock.ToString());
				if( ((MarkContainer)item).TargetMap != null )
					Add("TargetMap", ((MarkContainer)item).TargetMap.ToString());
			}
			else if( item is WarningItem )
			{
				Add("Range", ((WarningItem)item).Range.ToString());
				if( VS(((WarningItem)item).WarningString) )
					Add("WarningString", ((WarningItem)item).WarningString);
				Add("WarningNumber", ((WarningItem)item).WarningNumber.ToString());
				if( item is HintItem )
				{
					if( VS(((HintItem)item).HintString) )
						Add("HintString", ((HintItem)item).HintString);
					Add("HintNumber", ((HintItem)item).HintNumber.ToString());
				}
				Add("Range", ((WarningItem)item).ResetDelay.ToString());
			}
			else if( item is Cannon )
				Add("CannonDirection", ((Cannon)item).CannonDirection.ToString());
			else if( item is SerpentPillar )
			{
				if( VS(((SerpentPillar)item).Word) )
					Add("Word", ((SerpentPillar)item).Word);
				Add("DestStart", ((SerpentPillar)item).Destination.Start.ToString());
				Add("DestEnd", ((SerpentPillar)item).Destination.End.ToString());
			}
			else if( item.GetType().IsSubclassOf(typeof(BaseBeverage)) ) 
				Add("Content", ((BaseBeverage)item).Content.ToString());
			else if( item.GetType().IsSubclassOf(typeof(BaseDoor)) )
				Add("Facing", GetFacing(((BaseDoor)item).Offset).ToString());

			if( item is MaabusCoffin )
				Add("SpawnLocation", ((MaabusCoffin)item).SpawnLocation.ToString());
			else if( item is BaseLight )
			{
				if( !((BaseLight)item).Burning )
					Add("Unlit", String.Empty);
				if( !((BaseLight)item).Protected )
					Add("Unprotected", String.Empty);
			}
			else if( item is Spawner )
			{
				Spawner sp = (Spawner)item;

				for(int i = 0; i < sp.SpawnNames.Count; i++)
					if( VS(sp.SpawnNames[i]) )
						Add("Spawn", sp.SpawnNames[i]);
				// if( sp.MinDelay > TimeSpan.Zero )
					Add("MinDelay", sp.MinDelay.ToString());
				// if( sp.MaxDelay > TimeSpan.Zero )
					Add("MaxDelay", sp.MaxDelay.ToString());
				// if( sp.NextSpawn > TimeSpan.Zero )
					Add("NextSpawn", sp.NextSpawn.ToString());
				// if( sp.Count > 0 )
					Add("Count", sp.Count.ToString());
				// if( sp.Team > 0 )
					Add("Team", sp.Team.ToString());
				// if( sp.HomeRange > 0 )
					Add("HomeRange", sp.HomeRange.ToString());
				// if( sp.Running )
					Add("Running", sp.Running.ToString());
				// if( sp.Group )
					Add("Group", sp.Group.ToString());
			}
			else if( item is RecallRune )
			{
				RecallRune rune = (RecallRune)item;

				if( VS(rune.Description) )
					Add("Description", rune.Description);
				Add("Marked", rune.Marked.ToString());
				if( rune.TargetMap != null )
					Add("TargetMap", rune.TargetMap.ToString());
				Add("Target", rune.Target.ToString());
			}
			else if( item is Teleporter )
			{
				Teleporter tp = (Teleporter)item;

				if( item is SkillTeleporter )
				{
					SkillTeleporter st = (SkillTeleporter)item;

					Add("Skill", st.Skill.ToString());
					// "RequiredFixedPoint" == Required * 0.1 ?
					Add("Required", st.Required.ToString());
					if( VS(st.MessageString) )
						Add("MessageString", st.MessageString);
					Add("MessageNumber", st.MessageNumber.ToString());
				}
				else if( item is KeywordTeleporter )
				{
					KeywordTeleporter kt = (KeywordTeleporter)item;

					if( VS(kt.Substring) )
						Add("Substring", kt.Substring);
					Add("Keyword", kt.Keyword.ToString());
					Add("Range", kt.Range.ToString());
				}
				Add("PointDest", tp.PointDest.ToString());
				if( tp.MapDest != null )
					Add("MapDest", tp.MapDest.ToString());
				Add("Creatures", tp.Creatures.ToString());
				Add("SourceEffect", tp.SourceEffect.ToString());
				Add("DestEffect", tp.DestEffect.ToString());
				Add("SoundID", tp.SoundID.ToString());
				Add("Delay", tp.Delay.ToString());
			}
			else if( item is FillableContainer )
				Add("ContentType", ((FillableContainer)item).ContentType.ToString());

			if( item.Light != LightType.ArchedWindowEast )
				Add("Light", item.Light.ToString());
			if( item.Hue > 0 )
				Add("Hue", item.Hue.ToString());
			if( VS(item.Name) )
				Add("Name", item.Name);
			if( item.Amount > 1 )
				Add("Amount", item.Amount.ToString());

			s = String.Format("{0} {1}", ConstructType(item), itemID);

			if( List.Count > 0 )
			{
				s += " (";
				for( int i = 0; i < List.Count; i++ )
				{
					if( List[i][1] == String.Empty )
						s += String.Format("{0}{1}", List[i][0], (i < List.Count-1 ? "; " : String.Empty));
					else
						s += String.Format("{0}={1}{2}", List[i][0], List[i][1], (i < List.Count-1 ? "; " : String.Empty));
				}
				s += ")";
			}

			List.Clear();
			return s;
		}

		public static bool VS(string s)
		{
			if( s == null || s == String.Empty )
				return false;
			return true;
		}

		public static string ConstructType(Item item)
		{
			string s = item.GetType().ToString();

			if( s.LastIndexOf('.') > -1 )
				s = s.Remove(0, s.LastIndexOf('.')+1);

			return s;
		}

		public static DoorFacing GetFacing(Point3D p)
		{
			DoorFacing facing = DoorFacing.WestCW;
			for(int i = 0; i < m_Offsets.Length; i++)
			{
				if( p == m_Offsets[i] )
				{
					facing = (DoorFacing)i;
					break;
				}
			}
			
			return facing;
		}
		private static Point3D[] m_Offsets = new Point3D[]
		{
			new Point3D(-1, 1, 0 ),
			new Point3D( 1, 1, 0 ),
			new Point3D(-1, 0, 0 ),
			new Point3D( 1,-1, 0 ),
			new Point3D( 1, 1, 0 ),
			new Point3D( 1,-1, 0 ),
			new Point3D( 0, 0, 0 ),
			new Point3D( 0,-1, 0 ),

			new Point3D( 0, 0, 0 ),
			new Point3D( 0, 0, 0 ),
			new Point3D( 0, 0, 0 ),
			new Point3D( 0, 0, 0 )
		};
	}
}