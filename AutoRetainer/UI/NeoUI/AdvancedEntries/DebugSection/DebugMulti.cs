﻿using AutoRetainer.Internal;
using AutoRetainer.Scheduler.Tasks;
using Dalamud.Utility;
using ECommons.ExcelServices;
using ECommons.ExcelServices.TerritoryEnumeration;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using Lumina.Excel.GeneratedSheets;

namespace AutoRetainer.UI.NeoUI.AdvancedEntries.DebugSection;

internal unsafe class DebugMulti : DebugSectionBase
{
    public override void Draw()
    {
        ImGuiEx.Text($"Expected: {TaskChangeCharacter.Expected}");
        if(ImGui.Button("Force mismatch")) TaskChangeCharacter.Expected = ("AAAAAAAA", "BBBBBBB");
        if(ImGui.Button("Simulate nothing left"))
        {
            MultiMode.Relog(null, out var error, RelogReason.MultiMode);
        }
        if(ImGui.Button($"Simulate autostart"))
        {
            MultiMode.PerformAutoStart();
        }
        ImGuiEx.Text($"Moving: {AgentMap.Instance()->IsPlayerMoving}");
        ImGuiEx.Text($"Occupied: {IsOccupied()}");
        ImGuiEx.Text($"Casting: {Player.Object?.IsCasting}");
        ImGuiEx.TextCopy($"CID: {Player.CID}");
        ImGuiEx.Text($"{Svc.Data.GetExcelSheet<Addon>()?.GetRow(115)?.Text.ToDalamudString().ExtractText()}");
        ImGuiEx.Text($"Server time: {CSFramework.GetServerTime()}");
        ImGuiEx.Text($"PC time: {DateTimeOffset.Now.ToUnixTimeSeconds()}");
        if(ImGui.CollapsingHeader("HET"))
        {
            ImGuiEx.Text($"Nearest entrance: {Utils.GetNearestEntrance(out var d)}, d={d}");
            if(ImGui.Button("Enter house"))
            {
                HouseEnterTask.EnqueueTask();
            }
        }
        if(ImGui.CollapsingHeader("Estate territories"))
        {
            ImGuiEx.Text(ResidentalAreas.List.Select(x => GenericHelpers.GetTerritoryName(x)).Join("\n"));
            ImGuiEx.Text($"In residental area: {ResidentalAreas.List.Contains(Svc.ClientState.TerritoryType)}");
        }
        ImGuiEx.Text($"Is in sanctuary: {GameMain.IsInSanctuary()}");
        ImGuiEx.Text($"Is in sanctuary ExcelTerritoryHelper: {ExcelTerritoryHelper.IsSanctuary(Svc.ClientState.TerritoryType)}");
        ImGui.Checkbox($"Bypass sanctuary check", ref C.BypassSanctuaryCheck);
        if(Svc.ClientState.LocalPlayer != null && Svc.Targets.Target != null)
        {
            ImGuiEx.Text($"Distance to target: {Vector3.Distance(Svc.ClientState.LocalPlayer.Position, Svc.Targets.Target.Position)}");
            ImGuiEx.Text($"Target hitbox: {Svc.Targets.Target.HitboxRadius}");
            ImGuiEx.Text($"Distance to target's hitbox: {Vector3.Distance(Svc.ClientState.LocalPlayer.Position, Svc.Targets.Target.Position) - Svc.Targets.Target.HitboxRadius}");
        }
        if(ImGui.CollapsingHeader("CharaSelect"))
        {
            foreach(var x in Utils.GetCharacterNames())
            {
                ImGuiEx.Text($"{x.Name}@{x.World}");
            }
        }
    }
}
