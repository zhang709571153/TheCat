using System.Collections.Generic;
using TheCat.Data.Definitions;

namespace TheCat.Data.Catalogs
{
    public static class P0CatPresenter
    {
        public const string ShadowmaruPreviewId = "shadowmaru_preview";

        private static readonly Dictionary<string, CatPresentation> Presentations = CreatePresentations();

        public static CatPresentation Describe(string catId)
        {
            if (!string.IsNullOrWhiteSpace(catId) && Presentations.TryGetValue(catId, out CatPresentation presentation))
            {
                return NormalizeDesignFacingPresentation(presentation);
            }

            return CreateFallback(catId);
        }

        public static CatPresentation Describe(CatDefinition cat)
        {
            if (cat == null)
            {
                return CreateFallback(string.Empty);
            }

            if (Presentations.TryGetValue(cat.Id, out CatPresentation presentation))
            {
                return NormalizeDesignFacingPresentation(presentation);
            }

            return new CatPresentation(
                cat.Id,
                cat.DisplayName,
                "未配置猫咪",
                FormatRole(cat.Role),
                FormatAuthority(cat.AuthorityId),
                FormatAttribute(cat.AttributeId),
                cat.DisplayName);
        }

        private static CatPresentation NormalizeDesignFacingPresentation(CatPresentation presentation)
        {
            if (presentation == null)
            {
                return CreateFallback(string.Empty);
            }

            switch (presentation.CatId)
            {
                case P0PrototypeCatalog.SaibanId:
                    return WithDesignFacingAnchors(
                        presentation,
                        "Sacred Swordsman",
                        "bed oath",
                        "non-human silver_oath_sun_sword");
                case P0PrototypeCatalog.NephthysId:
                    return WithDesignFacingAnchors(
                        presentation,
                        "Moon-Sand Agent",
                        "dominion",
                        "non-human moon_sand_obelisk_crown");
                case P0PrototypeCatalog.SuzuneId:
                    return WithDesignFacingAnchors(
                        presentation,
                        "Sleep Shrine Miko",
                        "bells",
                        "non-human moon_bell_torii");
                default:
                    return presentation;
            }
        }

        private static CatPresentation WithDesignFacingAnchors(
            CatPresentation source,
            string title,
            string signatureToken,
            string visualIdentityToken)
        {
            return new CatPresentation(
                source.CatId,
                source.DisplayName,
                title,
                source.RoleHint,
                source.AuthorityLabel,
                source.AttributeLabel,
                source.ShortLabel,
                AppendToken(source.SignatureLine, signatureToken),
                source.VisualToken,
                AppendToken(source.VisualIdentity, visualIdentityToken));
        }

        private static string AppendToken(string value, string token)
        {
            if (string.IsNullOrWhiteSpace(token) || (!string.IsNullOrWhiteSpace(value) && value.Contains(token)))
            {
                return value ?? string.Empty;
            }

            return string.IsNullOrWhiteSpace(value) ? token : value + " | " + token;
        }

        private static Dictionary<string, CatPresentation> CreatePresentations()
        {
            return new Dictionary<string, CatPresentation>
            {
                {
                    P0PrototypeCatalog.SaibanId,
                    new CatPresentation(
                        P0PrototypeCatalog.SaibanId,
                        "塞班",
                        "圣剑守誓者",
                        "前排防守 / 击退 / 床护盾",
                        "誓约",
                        "日",
                        "塞班",
                        "以塞班的誓约，床前一寸也不让。",
                        "silver_oath_sun_sword",
                        "非人形猫骑士，银色誓约盾，日轮剑")
                },
                {
                    P0PrototypeCatalog.NephthysId,
                    new CatPresentation(
                        P0PrototypeCatalog.NephthysId,
                        "奈芙蒂斯",
                        "月砂密使",
                        "范围压制 / 砂地缓速 / 王权标记",
                        "支配",
                        "地",
                        "奈芙蒂斯",
                        "脚下，就是支配的边界。",
                        "moon_sand_obelisk_crown",
                        "非人形猫密使，月砂，金色方尖碑，王权之眼")
                },
                {
                    P0PrototypeCatalog.SuzuneId,
                    new CatPresentation(
                        P0PrototypeCatalog.SuzuneId,
                        "铃音",
                        "安眠神社巫猫",
                        "睡眠回复 / 治疗 / 月铃缓速",
                        "韵律",
                        "月",
                        "铃音",
                        "安静一点。我的铃会让梦继续睡着。",
                        "moon_bell_torii",
                        "非人形神社猫，月铃，冰色鸟居，柔和安眠光")
                },
                {
                    ShadowmaruPreviewId,
                    new CatPresentation(
                        ShadowmaruPreviewId,
                        "影丸",
                        "预览伙伴",
                        "未来刺杀伙伴",
                        "影",
                        "月",
                        "影丸",
                        "暗路打开时，我会加入。",
                        "shadow_step_preview",
                        "非人形猫刺客预览，影步拖尾")
                }
            };
        }

        private static CatPresentation CreateFallback(string catId)
        {
            string stableId = string.IsNullOrWhiteSpace(catId) ? "unknown_cat" : catId;
            return new CatPresentation(
                stableId,
                "未知猫咪",
                "未配置猫咪",
                "需要补充展示映射",
                "未知权柄",
                "未知属性");
        }

        private static string FormatRole(CatRole role)
        {
            switch (role)
            {
                case CatRole.Defender:
                    return "防守";
                case CatRole.Controller:
                    return "控场";
                case CatRole.Healer:
                    return "治疗";
                default:
                    return "专精";
            }
        }

        private static string FormatAuthority(string authorityId)
        {
            switch (authorityId)
            {
                case AuthorityIds.Oath:
                    return "誓约";
                case AuthorityIds.Dominion:
                    return "支配";
                case AuthorityIds.Rhythm:
                    return "韵律";
                default:
                    return "未知权柄";
            }
        }

        private static string FormatAttribute(string attributeId)
        {
            switch (attributeId)
            {
                case AttributeIds.Sun:
                    return "日";
                case AttributeIds.Earth:
                    return "地";
                case AttributeIds.Moon:
                    return "月";
                default:
                    return "未知属性";
            }
        }
    }
}
