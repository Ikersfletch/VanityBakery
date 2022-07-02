using Terraria;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.IO;
using Terraria.GameContent.UI.States;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.Audio;
using ReLogic.Content;
using Terraria.ID;
using OnUICharacterList = On.Terraria.GameContent.UI.Elements.UICharacterListItem;
namespace VanityBakery
{
	public class VanityBakery : Mod
	{
        private Asset<Texture2D> CopyTex;

        private Asset<Texture2D> BakeTex;

        private Asset<Texture2D> ClearTex;

        public override void Load()
        {
            CopyTex = ModContent.Request<Texture2D>("VanityBakery/Images/Copy", AssetRequestMode.ImmediateLoad);
            BakeTex = ModContent.Request<Texture2D>("VanityBakery/Images/Bake", AssetRequestMode.ImmediateLoad);
            ClearTex = ModContent.Request<Texture2D>("VanityBakery/Images/Clear", AssetRequestMode.ImmediateLoad);


           OnUICharacterList.ctor += CreateButton;
        }
        public override void Unload()
        {
            OnUICharacterList.ctor -= CreateButton;
        }

        public static Player AppearanceCopied = null;

        public void CreateButton(OnUICharacterList.orig_ctor orig, UICharacterListItem self, PlayerFileData data, int snapPointIndex)
        {

            UIImageButton CopyAppearance = new UIImageButton(CopyTex)
            {
                VAlign = 1.0f,
                Left= StyleDimension.FromPixelsAndPercent(100, 0f)
            };
            CopyAppearance.SetVisibility(1f, 0.5f);
            CopyAppearance.OnClick += SetCurrentAppearance;
            CopyAppearance.OnMouseOver += DisplayCopyText;
            CopyAppearance.OnMouseOut += RemoveText;
            UIImageButton BakeAppearance = new UIImageButton(BakeTex)
            {
                VAlign = 1.0f,
                Left = StyleDimension.FromPixelsAndPercent(124, 0f)
            };
            BakeAppearance.SetVisibility(1f, 0.5f);
            BakeAppearance.OnClick += BakeCurrentAppearance;
            BakeAppearance.OnMouseOver += DisplayBakeText;
            BakeAppearance.OnMouseOut += RemoveText;
            UIImageButton ClearAppearance = new UIImageButton(ClearTex)
            {
                VAlign = 1.0f,
                Left = StyleDimension.FromPixelsAndPercent(148, 0f)
            };
            ClearAppearance.SetVisibility(1f, 0.5f);
            ClearAppearance.OnClick += ClearCurrentAppearance;
            ClearAppearance.OnMouseOver += DisplayClearText;
            ClearAppearance.OnMouseOut += RemoveText;

            orig(self, data, snapPointIndex);
            self.Append(CopyAppearance);
            self.Append(BakeAppearance);
            self.Append(ClearAppearance);

            UIText label = (UIText)self.GetType().GetField("_buttonLabel", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).GetValue(self);
            label.Left.Set(180,0f);
        }

        public void SetCurrentAppearance(UIMouseEvent evt, UIElement listening)
        {
            SoundEngine.PlaySound(SoundID.Item166);
            UICharacterListItem parent = listening.Parent as UICharacterListItem;
            PlayerFileData filedata = (PlayerFileData)parent.GetType().GetField("_data", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).GetValue(parent);
            AppearanceCopied = filedata.Player;
            UIText label = (UIText)parent.GetType().GetField("_buttonLabel", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).GetValue(parent);
            label.SetText("Vanity Copied!");
        }

        public void BakeCurrentAppearance(UIMouseEvent evt, UIElement listening)
        {
            SoundEngine.PlaySound(SoundID.ResearchComplete);
            UICharacterListItem parent = listening.Parent as UICharacterListItem;
            PlayerFileData filedata = (PlayerFileData)parent.GetType().GetField("_data", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).GetValue(parent);
            BakedVanityData.BakeVanities(AppearanceCopied, filedata.Player);
            UIText label = (UIText)parent.GetType().GetField("_buttonLabel", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).GetValue(parent);
            label.SetText("Vanity Baked!");
        }
        public void ClearCurrentAppearance(UIMouseEvent evt, UIElement listening)
        {
            SoundEngine.PlaySound(SoundID.Item6);
            UICharacterListItem parent = listening.Parent as UICharacterListItem;
            PlayerFileData filedata = (PlayerFileData)parent.GetType().GetField("_data", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).GetValue(parent);
            BakedVanityData.ClearVanity(filedata.Player);
            UIText label = (UIText)parent.GetType().GetField("_buttonLabel", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).GetValue(parent);
            label.SetText("Vanity Cleared!");
        }

        public void DisplayCopyText(UIMouseEvent evt, UIElement listening)
        {
            UICharacterListItem parent = listening.Parent as UICharacterListItem;
            UIText label = (UIText)parent.GetType().GetField("_buttonLabel", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).GetValue(parent);
            label.SetText("Copy this Vanity");
        }
        public void DisplayBakeText(UIMouseEvent evt, UIElement listening)
        {
            UICharacterListItem parent = listening.Parent as UICharacterListItem;
            UIText label = (UIText)parent.GetType().GetField("_buttonLabel", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).GetValue(parent);
            label.SetText(AppearanceCopied == null ? "No Vanity Copied!" : "Bake current Vanity");
        }
        public void DisplayClearText(UIMouseEvent evt, UIElement listening)
        {
            UICharacterListItem parent = listening.Parent as UICharacterListItem;
            UIText label = (UIText)parent.GetType().GetField("_buttonLabel", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).GetValue(parent);
            label.SetText("Remove Baked Vanity");
        }

        public void RemoveText(UIMouseEvent evt, UIElement listening)
        {
            UICharacterListItem parent = listening.Parent as UICharacterListItem;
            UIText label = (UIText)parent.GetType().GetField("_buttonLabel", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).GetValue(parent);
            label.SetText("");
        }
    }
}