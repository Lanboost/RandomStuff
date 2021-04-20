using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LansMegaPack.weapons.boomerang
{
	class BoomerangProjectile:ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Boomerang");
		}

		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;
			projectile.friendly = true;
			projectile.melee = true;
			projectile.penetrate = -1;
			projectile.hide = false;
		}

		Texture2D[] textures;

		public BoomerangProjectile()
		{
			//Dont have them in order for better effect
			textures = new Texture2D[]
			{
				ModContent.GetTexture("LansMegaPack/weapons/boomerang/BoomerangProjectile"),
				ModContent.GetTexture("LansMegaPack/weapons/boomerang/Water__05"),
				ModContent.GetTexture("LansMegaPack/weapons/boomerang/Water__02"),
				ModContent.GetTexture("LansMegaPack/weapons/boomerang/Water__04"),
				ModContent.GetTexture("LansMegaPack/weapons/boomerang/Water__03"),
			};


		}
		
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			var spriteId = ((int)(projectile.ai[1]/5))%5;

			var t = textures[spriteId];
			Vector2 drawPos = projectile.position - Main.screenPosition;

			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
			spriteBatch.Draw(t, drawPos, t.Bounds, lightColor, projectile.rotation, t.Size()/2, 1f, SpriteEffects.None, 0f);
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

			return false;
		}

		public override bool PreAI()
		{
			return true;
		}
		
		public override void AI()
		{
			if (projectile.soundDelay == 0 && projectile.type != 383)
			{
				projectile.soundDelay = 8;
				Main.PlaySound(SoundID.Item7, projectile.position);
			}
			projectile.ai[1] += 1f;

			if (projectile.ai[0] == 0f)
			{
				if (projectile.ai[1] >= 30f)
				{
					projectile.ai[0] = 1f;
					projectile.netUpdate = true;
				}
			}
			else
			{
				projectile.tileCollide = false;
				float num43 = 9f;
				float num44 = 0.4f;

				Vector2 vector2 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
				float num45 = Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) - vector2.X;
				float num46 = Main.player[projectile.owner].position.Y + (float)(Main.player[projectile.owner].height / 2) - vector2.Y;
				float num47 = (float)Math.Sqrt(num45 * num45 + num46 * num46);
				if (num47 > 3000f)
					projectile.Kill();

				num47 = num43 / num47;
				num45 *= num47;
				num46 *= num47;

				if (projectile.velocity.X < num45)
				{
					projectile.velocity.X += num44;
					if (projectile.velocity.X < 0f && num45 > 0f)
						projectile.velocity.X += num44;
				}
				else if (projectile.velocity.X > num45)
				{
					projectile.velocity.X -= num44;
					if (projectile.velocity.X > 0f && num45 < 0f)
						projectile.velocity.X -= num44;
				}

				if (projectile.velocity.Y < num46)
				{
					projectile.velocity.Y += num44;
					if (projectile.velocity.Y < 0f && num46 > 0f)
						projectile.velocity.Y += num44;
				}
				else if (projectile.velocity.Y > num46)
				{
					projectile.velocity.Y -= num44;
					if (projectile.velocity.Y > 0f && num46 < 0f)
						projectile.velocity.Y -= num44;
				}

				if (Main.myPlayer == projectile.owner)
				{
					Rectangle rectangle = new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height);
					Rectangle value2 = new Rectangle((int)Main.player[projectile.owner].position.X, (int)Main.player[projectile.owner].position.Y, Main.player[projectile.owner].width, Main.player[projectile.owner].height);
					if (rectangle.Intersects(value2))
						projectile.Kill();
				}
			}
			//projectile.rotation += 0.4f * (float)projectile.direction;
			projectile.rotation += 0.01f * (float)1;
			if (projectile.tileCollide)
			{
				try
				{
					int num187 = (int)(projectile.position.X / 16f) - 1;
					int num188 = (int)((projectile.position.X + (float)projectile.width) / 16f) + 2;
					int num189 = (int)(projectile.position.Y / 16f) - 1;
					int num190 = (int)((projectile.position.Y + (float)projectile.height) / 16f) + 2;
					if (num187 < 0)
						num187 = 0;

					if (num188 > Main.maxTilesX)
						num188 = Main.maxTilesX;

					if (num189 < 0)
						num189 = 0;

					if (num190 > Main.maxTilesY)
						num190 = Main.maxTilesY;

					Vector2 vector16 = default(Vector2);
					for (int num191 = num187; num191 < num188; num191++)
					{
						for (int num192 = num189; num192 < num190; num192++)
						{
							if (Main.tile[num191, num192] != null && Main.tile[num191, num192].nactive() && (Main.tileSolid[Main.tile[num191, num192].type] || (Main.tileSolidTop[Main.tile[num191, num192].type] && Main.tile[num191, num192].frameY == 0)))
							{
								vector16.X = num191 * 16;
								vector16.Y = num192 * 16;
								if (projectile.position.X + (float)projectile.width > vector16.X && projectile.position.X < vector16.X + 16f && projectile.position.Y + (float)projectile.height > vector16.Y && projectile.position.Y < vector16.Y + 16f)
								{
									projectile.velocity.X = 0f;
									projectile.velocity.Y = -0.2f;
								}
							}
						}
					}
				}
				catch
				{
				}
			}
		}
		
	}
}
