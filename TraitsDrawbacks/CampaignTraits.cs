using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.Root;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UI.Common;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using RES = EldritchArcana.Properties.Resources;

namespace EldritchArcana
{
    internal class CampaignTraits
    {
        public static BlueprintFeatureSelection CreateCampaignTraits()
        {
            var noFeature = Helpers.PrerequisiteNoFeature(null);
            var campaignTraits = Helpers.CreateFeatureSelection("CampaignTrait", RES.CampaignTraitName_info,
                RES.CampaignTraitDescription_info,
                "f3c611a76bbc482c9c15219fa982fa17", null, FeatureGroup.None, noFeature);
            noFeature.Feature = campaignTraits;

            var choices = new List<BlueprintFeature>();

            string[] CampaignGuids = new string[200];
            //EmotionGuids = guids;
            string baseguid = "BB54279F30DA4802FFFF";
            int x = 0;
            for (long i = 442922691494; i < 442922691644; i++)
            {
                CampaignGuids[x] = baseguid + i.ToString();
                x++;
            }

            choices.Add(Helpers.CreateFeature("BastardTrait", RES.BastardTraitName_info,
                RES.BastardTraitDescription,
                "d4f7e0915bd941cbac6f655927135817",
                Image2Sprite.Create("Mods/EldritchArcana/sprites/human_bastard.png"),
                FeatureGroup.None,
                Helpers.Create<PrerequisiteFeature>(p => p.Feature = Helpers.human),
                // Other than the Prologue, there aren't many persuasion checks against members of the
                // nobility, prior to becoming a Baron. For simplicity, we simply remove the penalty after level 2.
                // (Ultimately this trait is more for RP flavor than anything.)
                Helpers.CreateAddStatBonusOnLevel(StatType.SkillPersuasion, -1, ModifierDescriptor.Penalty, 1, 2),
                Helpers.CreateAddStatBonus(StatType.SaveWill, 1, ModifierDescriptor.Trait)));

            var Outlander = Helpers.CreateFeatureSelection("OutlanderTrait", RES.OutlanderTraitName_info,
                RES.OutlanderTraitDescription_info,
                "40DABEF7A6424982BC42CD39D8440029",
                Image2Sprite.Create("Mods/EldritchArcana/sprites/outlander.png"),
                FeatureGroup.None);

            var NobleFamilyBorn = Helpers.CreateFeatureSelection("NobleFamilyBornTrait", RES.NobleFamilyBornTraitName_info,
                RES.NobleFamilyBornTraitDescription_info,
                "ecacfcbeddfe453cafc8d60fc2fb5d45",
                Image2Sprite.Create("Mods/EldritchArcana/sprites/noble_houses.png"),
                FeatureGroup.None);

            var Orlovski = Helpers.CreateFeatureSelection("NobleFamilyOrlovskyTrait", RES.NobleFamilyOrlovskyTraitName_info,
                RES.NobleFamilyOrlovskyTraitDescription_info,
                Helpers.MergeIds(Helpers.getStattypeGuid(StatType.AC), "9b03b7ff17394007a3fbec18bb42604b"),
                Image2Sprite.Create("Mods/EldritchArcana/sprites/house_orlovsky.png"),
                FeatureGroup.None,
                Helpers.CreateAddStatBonus(StatType.AdditionalCMD, 1, ModifierDescriptor.Trait));

            var OrlovskiFamilyFeats = new StatType[] {
                 StatType.SkillAthletics,
                 StatType.SkillPersuasion,
                 StatType.SkillStealth,
                 //StatType.BaseAttackBonus,
                 //StatType.SneakAttack
             }.Select(skill => Traits.CreateAddStatBonus(
                $"Orlovsky{skill}Trait",
                $"{UIUtility.GetStatText(skill)}",
                Orlovski.GetDescription(),
                Helpers.MergeIds(Helpers.GetSkillFocus(skill).AssetGuid, "2b01b7ff17394007a3fbec18bb42203b"),
                skill)).ToArray();

            //"0b183a3acaf5464eaad54276413fec08"
            // var families = new List<BlueprintFeature>() { }
            //choices.Add( Helpers.CreateAddStatBonus(
            //Orlovski.SetFeatures(OrlovskiFeatures);
            Orlovski.SetFeatures(OrlovskiFamilyFeats);

            var duelingSword = Traits.library.Get<BlueprintWeaponType>("a6f7e3dc443ff114ba68b4648fd33e9f");
            var longsword = Traits.library.Get<BlueprintWeaponType>("d56c44bc9eb10204c8b386a02c7eed21");

            var layonhandsResource = Traits.library.Get<BlueprintAbilityResource>("9dedf41d995ff4446a181f143c3db98c");
            var MutagenResource = Traits.library.Get<BlueprintAbilityResource>("3b163587f010382408142fc8a97852b6");
            var JudgmentResource = Traits.library.Get<BlueprintAbilityResource>("394088e9e54ccd64698c7bd87534027f");
            var ItemBondResource = Traits.library.Get<BlueprintAbilityResource>("fbc6de6f8be4fad47b8e3ec148de98c2");
            var kiPowerResource = Traits.library.Get<BlueprintAbilityResource>("9d9c90a9a1f52d04799294bf91c80a82");
            var ArcanePoolResourse = Traits.library.Get<BlueprintAbilityResource>("effc3e386331f864e9e06d19dc218b37");
            var ImpromptuSneakAttackResource = Traits.library.Get<BlueprintAbilityResource>("78e6008db60d8f94b9196464983ad336");
            var WildShapeResource = Traits.library.Get<BlueprintAbilityResource>("ae6af4d58b70a754d868324d1a05eda4");
            var SenseiPerformanceResource = Traits.library.Get<BlueprintAbilityResource>("ac5600c9642692145b7eb4553a703c1a");

            var snowball = Traits.library.Get<BlueprintAbility>("9f10909f0be1f5141bf1c102041f93d9");
            var fireball = Traits.library.Get<BlueprintAbility>("2d81362af43aeac4387a3d4fced489c3");
            var alchemist = Traits.library.Get<BlueprintCharacterClass>("0937bec61c0dabc468428f496580c721");
            var bard = Traits.library.Get<BlueprintCharacterClass>("772c83a25e2268e448e841dcd548235f");
            var cleric = Traits.library.Get<BlueprintCharacterClass>("67819271767a9dd4fbfd4ae700befea0");
            var druid = Traits.library.Get<BlueprintCharacterClass>("610d836f3a3a9ed42a4349b62f002e96");
            var scion = Traits.library.Get<BlueprintCharacterClass>("f5b8c63b141b2f44cbb8c2d7579c34f5");
            var magus = Traits.library.Get<BlueprintCharacterClass>("45a4607686d96a1498891b3286121780");
            var paladin = Traits.library.Get<BlueprintCharacterClass>("bfa11238e7ae3544bbeb4d0b92e897ec");
            var sorcerer = Traits.library.Get<BlueprintCharacterClass>("b3a505fb61437dc4097f43c3f8f9a4cf");
            var ranger = Traits.library.Get<BlueprintCharacterClass>("cda0615668a6df14eb36ba19ee881af6");
            var wizard = Traits.library.Get<BlueprintCharacterClass>("ba34257984f4c41408ce1dc2004e342e");
            var oracle = Traits.library.Get<BlueprintCharacterClass>("ec73f4790c1d4554871b81cde0b9399b");
            var rogue = Traits.library.Get<BlueprintCharacterClass>("299aa766dee3cbf4790da4efb8c72484");

            var BloodlineFeyWoodlandStride = Traits.library.CopyAndAdd<BlueprintFeature>(
                "11f4072ea766a5840a46e6660894527d",
                "NobleFamilyGaressTrait",
                Helpers.MergeIds(Helpers.getStattypeGuid(StatType.Reach), "9b03b7ff17394007a3fbec18bb42604b"));

            BloodlineFeyWoodlandStride.SetNameDescriptionIcon(
                RES.NobleFamilyGaressTraitName_info,
                RES.NobleFamilyGaressTraitDescription_info,
                Image2Sprite.Create("Mods/EldritchArcana/sprites/house_garess.png")
                );
            BloodlineFeyWoodlandStride.AddComponent(Helpers.CreateAddStatBonus(StatType.Speed, 5, ModifierDescriptor.Trait));


            var Lebda = Helpers.CreateFeatureSelection("NobleFamilyLebedaTrait", RES.NobleFamilyLebedaTraitName_info,
                RES.NobleFamilyLebedaTraitDescription_info,
                Helpers.MergeIds(Helpers.getStattypeGuid(StatType.Intelligence), "9b03b7ff17394007a3fbec18bb42604c"),
                Image2Sprite.Create("Mods/EldritchArcana/sprites/house_lebeda.png"),
                FeatureGroup.None,
                Helpers.CreateAddStatBonus(StatType.SkillKnowledgeArcana, 1, ModifierDescriptor.Trait));

            string[] Diffforhumans = new string[] {
                RES.NobleFamilyLebedaExtraKiDescription_info,
                RES.NobleFamilyLebedaExtraMutagenDescription_info,
                RES.NobleFamilyLebedaExtraBondedDescription_info,
                RES.NobleFamilyLebedaExtraImpromptuDescription_info,
                RES.NobleFamilyLebedaExtraJudgementDescription_info,
                RES.NobleFamilyLebedaExtraEnhanceDescription_info,
                RES.NobleFamilyLebedaExtraPerformanceDescription_info
            };
            string[] DiffforhumansT = new string[] {
                RES.NobleFamilyLebedaExtraKiName_info,
                RES.NobleFamilyLebedaExtraMutagenName_info,
                RES.NobleFamilyLebedaExtraBondedName_info,
                RES.NobleFamilyLebedaExtraImpromptuName_info,
                RES.NobleFamilyLebedaExtraJudgementName_info,
                RES.NobleFamilyLebedaExtraEnhanceName_info,
                RES.NobleFamilyLebedaExtraPerformanceName_info
            };
            var LebdaFeatures = new List<BlueprintFeature>() { };
            var Resources = new List<BlueprintAbilityResource> { kiPowerResource, MutagenResource, ItemBondResource, ImpromptuSneakAttackResource, JudgmentResource, ArcanePoolResourse, SenseiPerformanceResource };
            //CreateIncreaseResourceAmount for a few different resources
            x = 0;
            int y = 1;
            //Resources.randomelement();
            foreach (var stat in Resources)
            {
                x++;
                y = x < 3 ? 1 : x - 3;
                if (y == 0) y = 1;
                LebdaFeatures.Add(Helpers.CreateFeature($"NobleFamily{stat}Trait", String.Format(RES.NobleFamilyLebedaExtraResourceName_info, DiffforhumansT[x - 1]),
                    String.Format(RES.NobleFamilyLebedaExtraResourceDescription_info, y, Diffforhumans[x - 1]),
                    Helpers.MergeIds(stat.AssetGuid, "9b03b7ff17394007a3fbec18bb42604c"),
                    Helpers.GetIcon(stat.AssetGuid), //
                    FeatureGroup.None,
                    stat.CreateIncreaseResourceAmount(y)));
            };
            Lebda.SetFeatures(LebdaFeatures);

            var hoi = new List<BlueprintFeature>() {
                //family medyved
                Helpers.CreateFeature("NobleFamilyNoneTrait", RES.NobleFamilyNoneTraitName_info,
                RES.NobleFamilyNoneTraitDescription_info,//bow of the true world
                Helpers.MergeIds(Helpers.getStattypeGuid(StatType.AdditionalCMB), "9b03b7ff17394007a3fbec18bb42604b"),
                    Image2Sprite.Create("Mods/EldritchArcana/sprites/house_medvyed.png"),
                    FeatureGroup.None,
                    layonhandsResource.CreateIncreaseResourceAmount(4),
                    Helpers.Create<SavingThrowBonusAgainstDescriptor>(s => { s.SpellDescriptor = SpellDescriptor.Compulsion; s.Value = 1; s.ModifierDescriptor = ModifierDescriptor.Trait; })),
                
                //family lodovka + atletics and snowball
                Helpers.CreateFeature("NobleFamilyLodovkaTrait", RES.NobleFamilyLodovkaTraitName_info,
                RES.NobleFamilyLodovkaTraitDescription_info,
                Helpers.MergeIds(Helpers.getStattypeGuid(StatType.BaseAttackBonus), "9b03b7ff17394007a3fbec18bb42604b"),
                Image2Sprite.Create("Mods/EldritchArcana/sprites/house_lodovka.png"),
                FeatureGroup.None,
                Helpers.CreateAddKnownSpell(snowball,wizard,0),
                Helpers.CreateAddKnownSpell(snowball,alchemist,1),
                Helpers.CreateAddKnownSpell(snowball,bard,1),
                Helpers.CreateAddKnownSpell(snowball,cleric,1),
                Helpers.CreateAddKnownSpell(snowball,druid,1),
                Helpers.CreateAddKnownSpell(snowball,scion,1),
                Helpers.CreateAddKnownSpell(snowball,magus,1),
                Helpers.CreateAddKnownSpell(snowball,paladin,1),
                Helpers.CreateAddKnownSpell(snowball,sorcerer,1),
                Helpers.CreateAddKnownSpell(snowball,ranger,1),
                Helpers.CreateAddKnownSpell(snowball,wizard,1),
                Helpers.CreateAddKnownSpell(snowball,oracle,1),
                Helpers.CreateAddKnownSpell(snowball,rogue,1),
                //Helpers.CreateAddKnownSpell(snowball,bloodrager,1),
                //Helpers.CreateAddKnownSpell(snowball,witch,1),
                //Helpers.CreateAddKnownSpell(snowball,scald,1),
                Helpers.CreateAddStatBonus(StatType.SkillAthletics,2,ModifierDescriptor.Trait)),
                //family garess + 5 mvnt spd
                BloodlineFeyWoodlandStride,
                /*
                Helpers.CreateFeature("Noble family Garess Trait", "Noble family — Garess",
                "Familty motto: 'Strong as the Mountains'\n" +
                "House Garess is based in the western part of Brevoy, in the foothills of the Golushkin Mountains. " +
                "House Garess's crest is that of a snow-capped mountain peak in gray set against a dark blue field. There is a silvery crescent moon in the upper right corner, and there is a black hammer across the base of the peak. The Houses motto is Strong as the Mountains. "+
                "House Garess had a good relationship with the Golka dwarves until the dwarves vanished. Members of the house worked the metal that the dwarves mined. "+
                "The House has built several strongholds, Highdelve and Grayhaven, in the Golushkin Mountains. \nBenefit: Your movement speed is 5ft faster.",
                Helpers.MergeIds(Helpers.getGuids(StatType.Reach), "9b03b7ff17394007a3fbec18bb42604b"),
                Image2Sprite.Create("Mods/EldritchArcana/sprites/Icon_House_Garess.png"),
                FeatureGroup.None,
                Helpers.CreateAddStatBonus(StatType.Speed,5,ModifierDescriptor.Trait)),
                */
                //family rogarvia
                Helpers.CreateFeature("NobleFamilyRogarviaTrait", RES.NobleFamilyRogarviaTraitName_info,
                RES.NobleFamilyRogarviaTraitDescription_info,
                "B48B8234942C4FD191E99721728BF49D",
                Image2Sprite.Create("Mods/EldritchArcana/sprites/house_rogarvia.png"),
                FeatureGroup.None,
                Helpers.CreateAddStatBonusOnLevel(StatType.SkillPersuasion, -4, ModifierDescriptor.Penalty, 1, 3),
                Helpers.CreateAddKnownSpell(fireball,sorcerer,0),
                Helpers.CreateAddKnownSpell(fireball,wizard,3),
                Helpers.CreateAddKnownSpell(fireball,alchemist,3),
                Helpers.CreateAddKnownSpell(fireball,bard,3),
                Helpers.CreateAddKnownSpell(fireball,cleric,3),
                Helpers.CreateAddKnownSpell(fireball,druid,3),
                Helpers.CreateAddKnownSpell(fireball,scion,3),
                Helpers.CreateAddKnownSpell(fireball,magus,3),
                Helpers.CreateAddKnownSpell(fireball,paladin,3),
                Helpers.CreateAddKnownSpell(fireball,sorcerer,3),
                Helpers.CreateAddKnownSpell(fireball,ranger,3),
                Helpers.CreateAddKnownSpell(fireball,wizard,3),
                Helpers.CreateAddKnownSpell(fireball,oracle,3),
                Helpers.CreateAddKnownSpell(fireball,rogue,3)),

                
                /*/ family lebda
                Helpers.CreateFeature("Noble family Lebeda Trait", "Noble family — Lebeda",
                "family motto 'Success through Grace.'\n" +
                "House Lebeda is based to the southwest of Lake Reykal in Brevoy, controlling the plains and significant portions of the lake's shipping.[1] They are considered to be the Brevic noble family that epitomizes Rostland, having significant Taldan blood, an appreciation for fine things, and a love of sword fighting." +
                "\nBenefit:+1 knowledge arcana" +
                "\nBefefit:if you prepare mutagens you prepare one extra",
                Helpers.MergeIds(Helpers.getGuids(StatType.Intelligence), "9b03b7ff17394007a3fbec18bb42604c"),
                Helpers.GetIcon("79042cb55f030614ea29956177977c52"), // Great Fortitude
                FeatureGroup.None,
                Helpers.CreateAddStatBonus(StatType.SkillKnowledgeArcana,1,ModifierDescriptor.Trait),
                MutagenResource.CreateIncreaseResourceAmount(1)),
                */
                //family khartorov
                Helpers.CreateFeature("NobleFamilyKhavortorovTrait", RES.NobleFamilyKhavortorovTraitName_info,
                RES.NobleFamilyKhavortorovTraitDescription_info,
                "44DFCE0451FC4188A06E2184EF65064B",
                Image2Sprite.Create("Mods/EldritchArcana/sprites/house_khavortonov.png"),
                FeatureGroup.None,
                Helpers.Create<AddStartingEquipment>(a =>
                {

                    a.CategoryItems = new WeaponCategory[] { WeaponCategory.DuelingSword, WeaponCategory.Longsword };
                    a.RestrictedByClass = Array.Empty<BlueprintCharacterClass>();
                    a.BasicItems = Array.Empty<BlueprintItem>();

                }),
                Helpers.Create<WeaponTypeDamageBonus>(a => { a.WeaponType = duelingSword; a.DamageBonus = 1; }),
                //Helpers.Create<WeaponConditionalDamageDice>(a => {a.CheckWielder = null })
                Helpers.Create<WeaponTypeDamageBonus>(a => { a.WeaponType = longsword; a.DamageBonus = 1; })),
                // family surtova
                Helpers.CreateFeature("NobleFamilySurtovaTrait", RES.NobleFamilySurtovaTraitName_info,
                RES.NobleFamilySurtovaDescriptionName_info,
                Helpers.MergeIds(Helpers.getStattypeGuid(StatType.SneakAttack), "9b03b7ff17394007a3fbec18bb42604c"),
                Image2Sprite.Create("Mods/EldritchArcana/sprites/house_surtova.png"),
                FeatureGroup.None,
                Helpers.Create<DamageBonusAgainstFlankedTarget>(a => a.Bonus = 2))
                //
            };

            hoi.Add(Orlovski);
            hoi.Add(Lebda);
            NobleFamilyBorn.SetFeatures(hoi);
            choices.Add(NobleFamilyBorn);

            var miscdes = "Nobles think about you but they don't know:\n";
            choices.Add(Helpers.CreateFeatureSelection("NobleBornTrait", RES.NobleBornTraitSelectionName_info,
                miscdes + "You claim a tangential but legitimate connection to one of Brevoy's noble families. you've had a comfortable life, one you exploited untill you where send off to the be a monk and your luxury life ended.\nBenefits:you will start out with a bab penalty that will become a massive boon if you live the tale starts at -2 ends at +4",
                "a820521d923f4e569c3c69d091bf8865",
                Helpers.GetIcon("3adf9274a210b164cb68f472dc1e4544"), // Human Skilled
                FeatureGroup.None,
                PrerequisiteCharacterLevelExact.Create(10),
                Helpers.CreateAddStatBonusOnLevel(StatType.BaseAttackBonus, -2, ModifierDescriptor.Penalty, 1, 5),
                Helpers.CreateAddStatBonusOnLevel(StatType.BaseAttackBonus, -1, ModifierDescriptor.Penalty, 6, 10),
                //Helpers.CreateAddStatBonusOnLevel(StatType.AC, 1, ModifierDescriptor.Trait, 10,15),
                Helpers.CreateAddStatBonusOnLevel(StatType.BaseAttackBonus, 1, ModifierDescriptor.Trait, 11, 13),
                Helpers.CreateAddStatBonusOnLevel(StatType.BaseAttackBonus, 2, ModifierDescriptor.Trait, 14, 15),
                //Helpers.CreateAddStatBonusOnLevel(StatType.AC, 1, ModifierDescriptor.Trait, 15),
                Helpers.CreateAddStatBonusOnLevel(StatType.BaseAttackBonus, 3, ModifierDescriptor.Trait, 16, 17),
                Helpers.CreateAddStatBonusOnLevel(StatType.BaseAttackBonus, 4, ModifierDescriptor.Trait, 18)
                ));

            var SpellExpertise = Helpers.CreateFeatureSelection("OutlanderMissionary", RES.OutlanderMissionaryName_info,
                RES.OutlanderMissionaryDescription_info,
                "6a3dfe274f45432b85361bdbb0a3009b",
                Image2Sprite.Create("Mods/EldritchArcana/sprites/outlander.png"),
                FeatureGroup.None,
                Helpers.CreateAddStatBonus(StatType.SkillLoreReligion, 1, ModifierDescriptor.Trait));
            Traits.FillSpellSelection(SpellExpertise, 1, 9, Helpers.Create<IncreaseCasterLevelForSpellMax>());
            //choices.Add(SpellExpertise);
            /*var newthiny = CreateFeature("OutlanderLoreseeker", "Outlander:Loreseeker",
                "You have come here to see about expanding the presence of your chosen faith after receiving visions that told you your faith is needed—what that need is, though, you're not quite sure.\nBenefit: Pick one spell when you choose this trait—from this point on, whenever you cast that spell, you do so at caster level max.",
                "6a3dfe274f45432b85361bdbb0a3010c",
                Helpers.GetIcon("fe9220cdc16e5f444a84d85d5fa8e3d5");*/
            var SpellExpertise2 = Helpers.CreateFeatureSelection("OutlanderLoreseeker", RES.OutlanderLoreseekerName_info,
                RES.OutlanderLoreseekerDescription_info,
                "6a3dfe274f45432b85361bdbb0a3010c",
                Image2Sprite.Create("Mods/EldritchArcana/sprites/outlander.png"),
                FeatureGroup.None,
                Helpers.CreateAddStatBonus(StatType.SkillKnowledgeArcana, 1, ModifierDescriptor.Trait));
            //FillSpellSelection(SpellExpertise2, 1, 9, Helpers.Create<IncreaseCasterLevelForSpellMax>(), Helpers.Create<IncreaseSpellDC>());
            var spellchoises = Traits.FillTripleSpellSelection(SpellExpertise2, 1, 9, Helpers.Create<IncreaseCasterLevelForSpellMax>());
            SpellExpertise2.SetFeatures(spellchoises);
            SelectFeature_Apply_Patch.onApplyFeature.Add(SpellExpertise2, (state, unit) =>
            {
                SpellExpertise2.AddSelection(state, unit, 1);
                SpellExpertise2.AddSelection(state, unit, 1);
            });
            SelectFeature_Apply_Patch.onApplyFeature.Add(SpellExpertise, (state, unit) =>
            {
                SpellExpertise.AddSelection(state, unit, 1);
                SpellExpertise.AddSelection(state, unit, 1);
            });
            //SpellExpertise2.
            //SpellExpertise2.SetFeatures(SpellExpertise2.Features);
            //FillSpellSelection(SpellExpertise2, 3, 3, Helpers.Create<IncreaseCasterLevelForSpell>(), Helpers.Create<IncreaseSpellDC>());
            //var ding1 = Helpers.CreateAddStatBonus(StatType.SkillKnowledgeArcana, 1, ModifierDescriptor.Trait);
            //FillSpellSelection(ding1, 1, 9, Helpers.Create<IncreaseCasterLevelForSpellMax>());
            //choices.Add(SpellExpertise2);f

            //new BlueprintComponent = Helpers.Cre

            var OutlanderFeatures = new List<BlueprintFeature>()
            {
                SpellExpertise2,
                SpellExpertise,
                Helpers.CreateFeature("OutlanderExile", RES.OutlanderExileName_info,
                RES.OutlanderExileDescription_info,
                "fa2c636580ee431297de8806a046054a",
                Image2Sprite.Create("Mods/EldritchArcana/sprites/human_bastard.png"),
                FeatureGroup.None,
                Helpers.CreateAddStatBonus(StatType.Initiative, 2, ModifierDescriptor.Trait))
            };
            //FillSpellSelection(OutlanderFeatures, 1, 9, Helpers.Create<IncreaseCasterLevelForSpellMax>());
            Outlander.SetFeatures(OutlanderFeatures);
            //Outlander.AddSelection(1,SpellExpertise,1);
            choices.Add(Outlander);

            /* TODO: Noble Born. This will require some adaptation to the game. *
            var nobleBorn = Helpers.CreateFeatureSelection("NobleBornTrait", "Noble Born",
                "You claim a tangential but legitimate connection to one of Brevoy's noble families. If you aren't human, you were likely adopted by one of Brevoy's nobles or were instead a favored servant or even a childhood friend of a noble scion. Whatever the cause, you've had a comfortable life, but one far from the dignity and decadence your distant cousins know. Although you are associated with an esteemed name, your immediate family is hardly well to do, and you've found your name to be more of a burden to you than a boon in many social situations. You've recently decided to test yourself, to see if you can face the world without the aegis of a name you have little real claim or care for. An expedition into the storied Stolen Lands seems like just the test to see if you really are worth the title “noble.”",
                "a820521d923f4e569c3c69d091bf8865",
                null,
                FeatureGroup.None);
            choices.Add(nobleBorn);
            /*
            var families = new List<BlueprintFeature>();
            // TODO: Garess, Lebeda are hard to adapt to PF:K, need to invent new bonuses.
            // Idea for Garess:
            // - Feather Step SLA 1/day?
            // Lebeda:
            // - Start with extra gold? Or offer a permanent sell price bonus (perhaps 5%?)
            //
            families.Add();
            */
            var summonedBow = Traits.library.Get<BlueprintItem>("2fe00e2c0591ecd4b9abee963373c9a7");

            //ishomebrew
            var dice = Helpers.GetIcon("3fcc181a8b2094b4d9a636b639f0b79b");
            var OptimisticGambler = Helpers.CreateFeatureSelection("OptimisticGamblerTrait", RES.OptimisticGamblerTraitName_info,
                 RES.OptimisticGamblerTraitDescription_info,
                 "c88b9398af66406cac173884df308eb8",
                 Image2Sprite.Create("Mods/EldritchArcana/sprites/optimistic_gambler.png"),
                 FeatureGroup.None);
            //list with random features
            //string wwib = RES.OptimisticGamblerTraitWWIB_info;
            //string Gmbldsc = "Look at your stats and inventory or at your class ability usage stats and you will find out your bonus\n" +
            //        "Or you won't. That's the thing. Sometimes you win, and sometimes you lose. But that's what you are all about.\n" +
            //        "Benefit: To know what type of bonus you get in advance, right-click on the trait and scroll down." +
            //        $"\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n " +
            //        $"Note if you want a different bonus you need to restart the game. this bonus will stay the same." +
            //        $"\n";
            var UndeadSummonFeature = Traits.library.Get<BlueprintFeature>("f06f246950e76864fa545c13cb334ba5");
            var TristianAngelFeature = Traits.library.Get<BlueprintFeature>("96e026d5a38b24e4b87cd7dcd831cc16");
            var RingOfEnhancedSummonsFeature = Traits.library.Get<BlueprintFeature>("2bf0c2547f455894b93083e589866030");
            var randsummom = Traits.library.CopyAndAdd<BlueprintFeature>(
                UndeadSummonFeature.AssetGuid,
                "RandomEffectUndeadSummons",
                CampaignGuids[11]);

            randsummom.SetDescription(RES.OptimisticGamblerGmbldscDescription_info +
                String.Format(RES.OptimisticGamblerTraitBonus_info, randsummom.GetName()));//"undead summons"
            randsummom.SetName(RES.OptimisticGamblerTraitWWIB_info);
            randsummom.SetIcon(dice);
            randsummom.PrerequisiteNoFeature();

            var guidfeaturelist = new string[]{
                "201614af25697594a865355182fdb558",
                "d7d8d9691f5b8b84497c8789672fe1ba",
                "bf9b14d6f65fa944f91f5cc2b9d02fa0",
                "576933720c440aa4d8d42b0c54b77e80",
                "789c7539ba659174db702e18d7c2d330",
                "15bac762b599b7e42824c333717d79d9",
                "734a29b693e9ec346ba2951b27987e33",
                "3c0b706c526e0654b8af90ded235a089",
                "e66154511a6f9fc49a9de644bd8922db",
                "9c141c687eae35f4ba5399c11a4bdbc3",
                "9993edb6c470a6f4ab0bb8aac0b7522a",
                "aae0cb964bf516a4480d6745b71de4e7",
                "2ee2ba60850dd064e8b98bf5c2c946ba",
            };

            /*
            randsummom,
            RingOfEnhancedSummonsFeature,
            TristianAngelFeature,
            UndeadSummonFeature,
            */
            var OptimisticGamblerOptions = new List<BlueprintFeature>() {
                randsummom,
                //+3 healed
                Helpers.CreateFeature("RandomEffectExtraHeal", RES.OptimisticGamblerTraitWWIB_info,
                    RES.OptimisticGamblerGmbldscDescription_info + RES.OptimisticGamblerTraitExtraHealBonus_info
                    ,"c88b9398af66406cac124884df308eb8", dice, FeatureGroup.None,
                    Helpers.Create<FeyFoundlingLogic>(s => { s.flatModefier = 3; })),
                //+3 healed per die
                Helpers.CreateFeature("RandomEffectExtraHealDice", RES.OptimisticGamblerTraitWWIB_info,
                    RES.OptimisticGamblerGmbldscDescription_info + RES.OptimisticGamblerTraitExtraHealDiceBonus_info
                    ,"c88b9398af66406cac124884df308ec3", dice, FeatureGroup.None,
                    Helpers.Create<FeyFoundlingLogic>(s => { s.dieModefier = 3; })),
                //get summoned bow
                Helpers.CreateFeature("RandomEffectExtraBow", RES.OptimisticGamblerTraitWWIB_info,
                    RES.OptimisticGamblerGmbldscDescription_info + RES.OptimisticGamblerTraitExtraBowBonus_info
                    ,"e82b9398af64406cac124884df308fb9", dice, FeatureGroup.None,
                    Helpers.Create<AddStartingEquipment>(a =>
                    {
                        a.CategoryItems = Array.Empty<WeaponCategory>();
                        a.RestrictedByClass = Array.Empty<BlueprintCharacterClass>();
                        a.BasicItems = new BlueprintItem[] { summonedBow };
                    })),
                //+1 on spells damage
                Helpers.CreateFeature("RandomEffectExtraMagicDamage", RES.OptimisticGamblerTraitWWIB_info,
                    RES.OptimisticGamblerGmbldscDescription_info + RES.OptimisticGamblerTraitExtraMagicDamageBonus_info
                    ,"c88b9398af66405cac124884df338eb8", dice, FeatureGroup.None,
                    Helpers.Create<ArcaneBloodlineArcana>(),
                    Helpers.Create<ArcaneBloodlineArcana>(),
                    Helpers.Create<ArcaneBloodlineArcana>())
            };

            for (int i = 0; i < guidfeaturelist.Length; i++)
            {
                int effectnumber = 12 + i;
                var CopiedFeat = Traits.library.CopyAndAdd<BlueprintFeature>(
                guidfeaturelist[i],
                $"RandomEffectNumber{effectnumber}",
                CampaignGuids[effectnumber]);
                CopiedFeat.SetDescription(RES.OptimisticGamblerGmbldscDescription_info +
                    (CopiedFeat.GetName() == "" ? "" : String.Format(RES.OptimisticGamblerTraitBonus_info, CopiedFeat.GetName())));//feature name
                CopiedFeat.SetName(RES.OptimisticGamblerTraitWWIB_info);
                CopiedFeat.SetIcon(dice);
                CopiedFeat.PrerequisiteNoFeature();

                OptimisticGamblerOptions.Add(CopiedFeat);
            }

            var bob = new List<StatType> {
                StatType.AC,
                StatType.AdditionalAttackBonus,
                StatType.AdditionalCMB,
                StatType.BaseAttackBonus,
                StatType.Charisma,//(1.2.4)
                StatType.Reach,
                StatType.SneakAttack,
                StatType.Strength,//(1.2.4)
                StatType.Intelligence,
                StatType.Wisdom,//(1.2.4)
            };

            foreach (StatType stat in bob)
            {
                OptimisticGamblerOptions.Add(Helpers.CreateFeature($"randomeffect{stat}", RES.OptimisticGamblerTraitWWIB_info,
                    RES.OptimisticGamblerGmbldscDescription_info + String.Format(RES.OptimisticGamblerTraitStatsBonus_info, LocalizedTexts.Instance.Stats.GetText(stat))
                    , Helpers.MergeIds(Helpers.getStattypeGuid(stat), "c88b9398af66406cac173884df308eb8"), dice, FeatureGroup.None,
                    Helpers.CreateAddStatBonus(stat, 3, ModifierDescriptor.Luck)));
            }

            var weapons = new Dictionary<WeaponCategory, String> {
                { WeaponCategory.Club, RES.WeaponCategoryClubName_info },
                { WeaponCategory.Dagger, RES.WeaponCategoryDaggerName_info },
                { WeaponCategory.Dart,RES.WeaponCategoryDartName_info },
                { WeaponCategory.DuelingSword, RES.WeaponCategoryDuelingSwordName_info },
                { WeaponCategory.ElvenCurvedBlade, RES.WeaponCategoryElvenCurveBladeName_info },
                { WeaponCategory.Falcata, RES.WeaponCategoryFalcataName_info },
                { WeaponCategory.Falchion, RES.WeaponCategoryFalchionName_info },
                { WeaponCategory.Flail, RES.WeaponCategoryFlailName_info },
                { WeaponCategory.Greataxe, RES.WeaponCategoryGreataxeName_info },
                { WeaponCategory.Greatclub, RES.WeaponCategoryGreatclubName_info },
                { WeaponCategory.Greatsword, RES.WeaponCategoryGreatswordName_info },
                { WeaponCategory.Handaxe, RES.WeaponCategoryHandaxeName_info },
                { WeaponCategory.HeavyFlail, RES.WeaponCategoryHeavyFlailName_info },
                { WeaponCategory.HeavyMace, RES.WeaponCategoryHeavyMaceName_info },
                { WeaponCategory.HeavyPick, RES.WeaponCategoryHeavyPickName_info },
                { WeaponCategory.Javelin, RES.WeaponCategoryJavelinName_info},
                { WeaponCategory.LightMace, RES.WeaponCategoryLightMaceName_info },
                { WeaponCategory.Shuriken, RES.WeaponCategoryShurikenName_info },
                { WeaponCategory.Sickle, RES.WeaponCategorySickleName_info },
                { WeaponCategory.Sling, RES.WeaponCategorySlingName_info },
                { WeaponCategory.Kama, RES.WeaponCategoryKamaName_info },
                { WeaponCategory.Kukri, RES.WeaponCategoryKukriName_info },
                { WeaponCategory.Starknife, RES.WeaponCategoryStarknifeName_info },
                { WeaponCategory.ThrowingAxe, RES.WeaponCategoryThrowingAxeName_info },
                { WeaponCategory.LightPick, RES.WeaponCategoryLightPickName_info },
                { WeaponCategory.DwarvenWaraxe, RES.WeaponCategoryDwarvenWaraxeName_info },
                { WeaponCategory.Trident, RES.WeaponCategoryTridentName_info },
                { WeaponCategory.BastardSword, RES.WeaponCategoryBastardSwordName_info },
                { WeaponCategory.Battleaxe, RES.WeaponCategoryBattleaxeName_info },
                { WeaponCategory.Longsword, RES.WeaponCategoryLongswordName_info },
                { WeaponCategory.Nunchaku, RES.WeaponCategoryNunchakuName_info },
                { WeaponCategory.Rapier, RES.WeaponCategoryRapierName_info },
                { WeaponCategory.Estoc, RES.WeaponCategoryEstocName_info },
                { WeaponCategory.Sai, RES.WeaponCategorySaiName_info },
                { WeaponCategory.Scimitar, RES.WeaponCategoryScimitarName_info },
                { WeaponCategory.Scythe, RES.WeaponCategoryScytheName_info },
                { WeaponCategory.Shortsword, RES.WeaponCategoryShortswordName_info },
                { WeaponCategory.Spear, RES.WeaponCategorySpearName_info },
                { WeaponCategory.Warhammer, RES.WeaponCategoryWarhammerName_info },
                { WeaponCategory.LightHammer, RES.WeaponCategoryLightHammerName_info },
                { WeaponCategory.WeaponLightShield, RES.WeaponCategoryLightShieldName_info },
                { WeaponCategory.WeaponHeavyShield, RES.WeaponCategoryHeavyShieldName_info },
            };

            // 武器选项个数太多对其他随机项不公平，选择其中十项
            int randIndex;
            Random ran = new Random();
            var weaponsChosen = new Dictionary<WeaponCategory, String> { };
            var weaponChosenKey = new WeaponCategory();
            x = 0;

            for (int i = 0; i < 10; i++)
            {
                weaponChosenKey = weapons.Keys.ElementAt(randIndex = ran.Next(weapons.Count));
                weaponsChosen.Add(weaponChosenKey, weapons[weaponChosenKey]);
                weapons.Remove(weaponChosenKey);
            }

            foreach (var weapon in weaponsChosen)
            {
                x++;
                // Log.Write(weapon.Key.ToString());
                OptimisticGamblerOptions.Add(
                    Helpers.CreateFeature(
                        $"RandomEffectExtra{weapon.Key.ToString()}", RES.OptimisticGamblerTraitWWIB_info,
                        RES.OptimisticGamblerGmbldscDescription_info +
                            String.Format(RES.OptimisticGamblerTraitWeaponBonus_info, weapon.Value)
                        , CampaignGuids[x], dice, FeatureGroup.None,
                        Helpers.Create<WeaponCategoryAttackBonus>(b => { b.Category = weapon.Key; b.AttackBonus = 3; }),
                        //Helpers.Create<WeaponTypeDamageBonus>(c=> { c.WeaponType = weap; })
                        Helpers.Create<AddStartingEquipment>(a =>
                        {
                            a.CategoryItems = new WeaponCategory[] { weapon.Key, weapon.Key };
                            a.RestrictedByClass = Array.Empty<BlueprintCharacterClass>();
                            a.BasicItems = Array.Empty<BlueprintItem>();
                        })
                    )
                 );
            }

            // 随机也太简陋了吧……
            // int rnd = DateTime.Now.Millisecond % OptimisticGamblerOptions.Count;
            // int rnd2 = OptimisticGamblerOptions.Count - 1 - (DateTime.Now.Millisecond % OptimisticGamblerOptions.Count);
            var randList = new List<int> { ran.Next(OptimisticGamblerOptions.Count) };
            for (int i = 0; i < 6; i++)
            {
                while (!randList.Contains(randIndex = ran.Next(OptimisticGamblerOptions.Count)))
                {
                    randList.Add(randIndex);
                }
            }

            // geneates a random number that is basicly a random element from the list.
            //rnd = 0;
            //rnd2 = 1;
            // 六面骰，给六项吧……请不要吐槽图标是2d6……
            var xander = OptimisticGamblerOptions[randList[0]];
            var option2 = OptimisticGamblerOptions[randList[1]];
            var option3 = OptimisticGamblerOptions[randList[2]];
            var option4 = OptimisticGamblerOptions[randList[3]];
            var option5 = OptimisticGamblerOptions[randList[4]];
            var option6 = OptimisticGamblerOptions[randList[5]];
            OptimisticGamblerOptions = Main.settings?.CheatCustomTraits == true ? OptimisticGamblerOptions : new List<BlueprintFeature> {
                xander, option2, option3, option4, option5, option6 };
            //OptimisticGambler.SetFeatures(xander,option2);
            OptimisticGambler.SetFeatures(OptimisticGamblerOptions);
            OptimisticGambler.IgnorePrerequisites = true;
            choices.Add(OptimisticGambler);

            choices.Add(Helpers.CreateFeature("RostlanderTrait", RES.RostlanderTraitName_info,
                RES.RostlanderTraitDescription_info,
                "d99b9398af66406cac173884df308eb7",
                Image2Sprite.Create("Mods/EldritchArcana/sprites/rostlander.png"),
                FeatureGroup.None,
                Helpers.CreateAddStatBonus(StatType.SaveFortitude, 1, ModifierDescriptor.Trait)));


            //var duelingSword =Traits.library.Get<BlueprintWeaponType>("a6f7e3dc443ff114ba68b4648fd33e9f");
            //var longsword =Traits.library.Get<BlueprintWeaponType>("d56c44bc9eb10204c8b386a02c7eed21");

            choices.Add(Helpers.CreateFeature("SwordScionTrait", RES.SwordScionTraitName_info,
                RES.SwordScionTraitDescription_info,
                "e16eb56b2f964321a29076226dccb29e",
                Image2Sprite.Create("Mods/EldritchArcana/sprites/sword_scion.png"),
                FeatureGroup.None,
                Helpers.Create<AddStartingEquipment>(a =>
                {
                    a.CategoryItems = new WeaponCategory[] { WeaponCategory.DuelingSword, WeaponCategory.Longsword };
                    a.RestrictedByClass = Array.Empty<BlueprintCharacterClass>();
                    a.BasicItems = Array.Empty<BlueprintItem>();
                }),
                Helpers.Create<WeaponAttackAndCombatManeuverBonus>(a => { a.WeaponType = duelingSword; a.AttackBonus = 1; a.Descriptor = ModifierDescriptor.Trait; }),
                //Helpers.Create<WeaponAttackAndCombatManeuverBonus>(a => { a.WeaponType = dagger; a.AttackBonus = 1; a.Descriptor = ModifierDescriptor.Trait; }),
                Helpers.Create<WeaponAttackAndCombatManeuverBonus>(a => { a.WeaponType = longsword; a.AttackBonus = 1; a.Descriptor = ModifierDescriptor.Trait; })));

            choices.Add(UndoSelection.Feature.Value);
            campaignTraits.SetFeatures(choices);
            return campaignTraits;
        }
    }
}
