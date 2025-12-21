using HarmonyLib;
using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Cards;
using Sorwest.LenMod.Enemies;
using Sorwest.LenMod.ExternalAPI;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Features;

public class MusicNoteManager : IRegisterable
{
    internal static IStatusEntry MusicNoteStatus { get; private set; } = null!;
    internal static List<string> ColorList = [  "4ba0a0", "4baaa2", "4fb4a1", "58be9f",
                                        "64c89b", "73d196", "86d990", "9ae189",
                                        "b1e882", "caee7c", "e4f477", "fff875"];
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        MusicNoteStatus = helper.Content.Statuses.RegisterStatus("MusicNoteStatus", new()
        {
            Definition = new()
            {
                icon = ModEntry.Instance.Sprites["MusicNote"].Sprite,
                color = new Color("ffbe1a"),
                border = new Color("4ba0a0"),
                isGood = true
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["status", "MusicNote", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["status", "MusicNote", "description"]).Localize
        });

        ModEntry.Instance.Helper.Events.RegisterAfterArtifactsHook(nameof(Artifact.OnCombatEnd), (State state) =>
        {
            int amount = state.ship.Get(MusicNoteStatus.Status);
            if (amount > 0)
                ModEntry.Instance.Helper.ModData.SetModData(state, "MusicStored", amount);
            else
                ModEntry.Instance.Helper.ModData.RemoveModData(state, "MusicStored");
        });

        ModEntry.Instance.Helper.Events.RegisterAfterArtifactsHook(nameof(Artifact.OnCombatStart), (State state) =>
        {
            state.ship.Set(MusicNoteStatus.Status, ModEntry.Instance.Helper.ModData.GetModDataOrDefault<int>(state, "MusicStored"));
        });

        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(Ship), nameof(Ship.Set)),
            prefix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(Ship_Set_Prefix))
        );

        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(State), nameof(State.PopulateRun)),
            postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(State_PopulateRun_Postfix))
        );

        ModEntry.Instance.KokoroApi.StatusRendering.RegisterHook(new MusicNoteStatusRenderingHook());
        ModEntry.Instance.KokoroApi.StatusLogic.RegisterHook(new MusicNoteLogicHook());
    }
    private static void State_PopulateRun_Postfix(State __instance)
        => ModEntry.Instance.Helper.ModData.RemoveModData(__instance, "MusicStored");
    private static void Ship_Set_Prefix(Ship __instance, Status status, ref int n)
    {
        if (status != MusicNoteStatus.Status)
            return;
        n = Math.Clamp(n, 0, 12);
    }
    private sealed class MusicNoteLogicHook : IKokoroApi.IV2.IStatusLogicApi.IHook
    {
        public bool HandleStatusTurnAutoStep(IKokoroApi.IV2.IStatusLogicApi.IHook.IHandleStatusTurnAutoStepArgs args)
        {
            if (args.Status != MusicNoteStatus.Status)
                return false;
            int note = args.Ship.Get(args.Status);
            if (note <= 0)
                return false;
            int notemod = (note - 1) % 3;
            if (args.Timing == IKokoroApi.IV2.IStatusLogicApi.StatusTurnTriggerTiming.TurnEnd)
            {
                if (note != 12
                    && (args.Ship.hull == 1 || (args.Ship.hull * 2 <= args.Ship.hullMax)))
                    args.Combat.Queue(new AStatus()
                    {
                        status = MusicNoteStatus.Status,
                        statusAmount = 1,
                        targetPlayer = args.Ship.isPlayerShip
                    });
                return false;
            }
            Card card = new LenCardMN5();
            bool isMiku = args.Ship.ai?.GetType() == typeof(MikuAI);
            if (note == 12)
            {
                if (isMiku)
                {
                    card = new MikuCardMN5();
                }

                args.Combat.Queue(new AStatus() { status = MusicNoteStatus.Status, mode = AStatusMode.Set, statusAmount = 0, targetPlayer = args.Ship.isPlayerShip });
            }
            else if (note > 9)
            {
                if (isMiku)
                {
                    card = new MikuCardMN4();
                }
                else if (note == 11)
                {
                    double chance = args.State.rngActions.Next();
                    if (chance < 50)
                    {
                        card = new LenCardMN4 { upgrade = Upgrade.A };
                    }
                    else
                    {
                        card = new LenCardMN4 { upgrade = Upgrade.B };
                    }
                }
                else
                {
                    card = new LenCardMN4 { upgrade = (Upgrade)notemod };
                }
            }
            else if (note > 6)
            {
                if (isMiku)
                {
                    card = new MikuCardMN3();
                }
                else
                {
                    card = new LenCardMN3 { upgrade = (Upgrade)notemod };
                }
            }
            else if (note > 3)
            {
                if (isMiku)
                {
                    card = new MikuCardMN2();
                }
                else
                {
                    card = new LenCardMN2 { upgrade = (Upgrade)notemod };
                }
            }
            else
            {
                if (isMiku)
                {
                    card = new MikuCardMN1();
                }
                else
                {
                    card = new LenCardMN1 { upgrade = (Upgrade)notemod };
                }
            }
            args.Combat.Queue(new AAddCard() { amount = 1, card = card, destination = CardDestination.Hand });
            args.Ship.PulseStatus(args.Status);
            return false;
        }
    }
    private sealed class MusicNoteStatusRenderingHook : IKokoroApi.IV2.IStatusRenderingApi.IHook
    {
        public IKokoroApi.IV2.IStatusRenderingApi.IStatusInfoRenderer? OverrideStatusInfoRenderer(IKokoroApi.IV2.IStatusRenderingApi.IHook.IOverrideStatusInfoRendererArgs args)
        {
            if (args.Status != MusicNoteStatus.Status)
                return null;
            Color[] colors = new Color[12];
            for (int i = 0; i < colors.Length; i++)
            {
                if (args.Amount == 12)
                    colors[i] = new Color("53dff5");
                else if (i >= args.Amount)
                    colors[i] = ModEntry.Instance.KokoroApi.StatusRendering.DefaultInactiveStatusBarColor;
                else
                    colors[i] = new Color(ColorList[i]);
            }
            return ModEntry.Instance.KokoroApi.StatusRendering.MakeBarStatusInfoRenderer().SetSegments(colors).SetRows(2).SetHorizontalSpacing(0);
        }
    }
}
