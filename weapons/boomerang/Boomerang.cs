using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LansMegaPack.weapons.boomerang
{
	class Boomerang:ModItem
	{
		int projectileCount = 2;

		public override void SetDefaults()
		{
			// Alter any of these values as you see fit, but you should probably keep useStyle on 1, as well as the noUseGraphic and noMelee bools
			item.shootSpeed = 10f;
			item.damage = 45;
			item.knockBack = 5f;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useAnimation = 25;
			item.useTime = 25;
			item.width = 30;
			item.height = 30;
			item.maxStack = 1;
			item.rare = ItemRarityID.Pink;

			item.consumable = false;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.autoReuse = false;
			item.thrown = true;

			item.UseSound = SoundID.Item1;
			item.value = Item.sellPrice(silver: 5);
			// Look at the javelin projectile for a lot of custom code
			// If you are in an editor like Visual Studio, you can hold CTRL and Click ExampleJavelinProjectile
			item.shoot = ModContent.ProjectileType<BoomerangProjectile>();
		}

		public override bool CanUseItem(Player player)
		{
			return countProjectileOfType(player, item.shoot) < projectileCount;
		}

		int countProjectileOfType(Player player, int type)
		{
			int c = 0;
			for (int m = 0; m < 1000; m++)
			{
				if (Main.projectile[m].active && Main.projectile[m].owner == player.whoAmI && Main.projectile[m].type == type)
				{
					c++;
				}
			}
			return c;
		}
	}
}
