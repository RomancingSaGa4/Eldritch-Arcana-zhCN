// Copyright (c) 2019 Jennifer Messerly
// This code is licensed under MIT license (see LICENSE for details)

using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UI.Common;
using System;
using System.Collections.Generic;
using System.Text;
using RES = EldritchArcana.Properties.Resources;

namespace EldritchArcana
{
    static class HeavensMystery
    {
        static LibraryScriptableObject library => Main.library;
        static BlueprintCharacterClass oracle => OracleClass.oracle;
        static BlueprintCharacterClass[] oracleArray => OracleClass.oracleArray;

        internal static (BlueprintFeature, BlueprintFeature) Create(String mysteryDescription, BlueprintFeature classSkillFeat)
        {
            var skill1 = StatType.SkillKnowledgeArcana;
            var skill2 = StatType.SkillPerception;

            var mystery = Helpers.CreateProgression("MysteryHeavensProgression", RES.MysteryHeavensName_info, $"{mysteryDescription}\n" +
                String.Format(RES.MysteryHeavensDescription_info, UIUtility.GetStatText(skill1), UIUtility.GetStatText(skill2)),
                "dabcaefe63bc471dac44e8e23c1c330f",
                Helpers.GetIcon("91da41b9793a4624797921f221db653c"), // color spray
                UpdateLevelUpDeterminatorText.Group,
                AddClassSkillIfHasFeature.Create(skill1, classSkillFeat),
                AddClassSkillIfHasFeature.Create(skill2, classSkillFeat));
            mystery.Classes = oracleArray;

            var spells = Bloodlines.CreateSpellProgression(mystery, new String[] {
                "91da41b9793a4624797921f221db653c", // color spray
                Spells.hypnoticPatternId,
                "bf0accce250381a44b857d4af6c8e10d", // searing light (should be: daylight)
                "4b8265132f9c8174f87ce7fa6d0fe47b", // rainbow pattern
                FlySpells.overlandFlight.AssetGuid,
                "645558d63604747428d55f0dd3a4cb58", // chain lightning
                "b22fd434bdb60fb4ba1068206402c4cf", // prismatic spray
                "e96424f70ff884947b06f41a765b7658", // sunburst
                FireSpells.meteorSwarm.AssetGuid,
            });

            var revelations = new List<BlueprintFeature>()
            {
                // TODO
            };
            var description = new StringBuilder(mystery.Description).AppendLine();
            description.AppendLine(RES.MysteryHeavensDescription2_info);
            foreach (var r in revelations)
            {
                description.AppendLine($"• {r.Name}");
                r.InsertComponent(0, Helpers.PrerequisiteFeature(mystery));
            }
            mystery.SetDescription(description.ToString());

            var entries = new List<LevelEntry>();
            for (int level = 1; level <= 9; level++)
            {
                entries.Add(Helpers.LevelEntry(level * 2, spells[level - 1]));
            }
            // TODO:
            //var finalRevelation = CreateFinalRevelation();
            //entries.Add(Helpers.LevelEntry(20, finalRevelation));

            mystery.LevelEntries = entries.ToArray();
            mystery.UIGroups = Helpers.CreateUIGroups(new List<BlueprintFeatureBase>(spells) { /*TODO:finalRevelation*/ });

            var revelation = Helpers.CreateFeatureSelection("MysteryFlameRevelation", "Flame Revelation",
                mystery.Description, "40db1e0f9b3a4f5fb9fde0801b158216", null, FeatureGroup.None,
                Helpers.PrerequisiteFeature(mystery));
            revelation.Mode = SelectionMode.OnlyNew;
            revelation.SetFeatures(revelations);
            return (mystery, revelation);
        }
    }
}