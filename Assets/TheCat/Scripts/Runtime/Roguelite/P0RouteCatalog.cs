namespace TheCat.Roguelite
{
    public static class P0RouteCatalog
    {
        public const string RouteId = "p0_ten_layer_route";
        public const string BedroomDreamMapId = P0DreamMapCatalog.BedroomDreamMapId;
        public const string EgyptDreamMapId = P0DreamMapCatalog.EgyptDreamMapId;
        public const string LayerOneDefenseId = "layer_01_defense";
        public const string BossNodeId = "layer_10_boss_call_tyrant";
        public const string SoftRainWindowEventContentId = "event_soft_rain_window";
        public const string UnreadRedDotRainEventContentId = "event_unread_red_dot_rain";
        public const string LegacyUnreadRedDotRainEventContentId = "dream_event_unread_red_dot_rain";
        public const string MidnightKibbleShopContentId = "shop_midnight_kibble";

        public static RouteDefinition CreateTenLayerRoute()
        {
            return CreateTenLayerRoute(P0DreamMapCatalog.GetBedroomDreamMap());
        }

        public static RouteDefinition CreateTenLayerRoute(DreamMapDefinition dreamMap)
        {
            return new RouteDefinition(
                RouteId,
                dreamMap ?? P0DreamMapCatalog.GetBedroomDreamMap(),
                BuildP0LayerOptions());
        }

        public static RouteDefinition CreateEgyptPlaceholderRoute()
        {
            return CreateTenLayerRoute(P0DreamMapCatalog.GetEgyptDreamMap());
        }

        private static RouteNodeDefinition[][] BuildP0LayerOptions()
        {
            return new[]
            {
                new[]
                {
                    new RouteNodeDefinition(1, LayerOneDefenseId, RouteNodeType.Defense, "layer_01_defense")
                },
                new[]
                {
                    new RouteNodeDefinition(2, "layer_02_dream_event", RouteNodeType.DreamEvent, SoftRainWindowEventContentId),
                    new RouteNodeDefinition(2, "layer_02_shop_early", RouteNodeType.Shop, MidnightKibbleShopContentId)
                },
                new[]
                {
                    new RouteNodeDefinition(3, "layer_03_elite", RouteNodeType.Elite, "elite_cold_light_shadow"),
                    new RouteNodeDefinition(3, "layer_03_partner", RouteNodeType.Partner, "partner_shadowmaru_preview")
                },
                new[]
                {
                    new RouteNodeDefinition(4, "layer_04_partner", RouteNodeType.Partner, "partner_shadowmaru_preview"),
                    new RouteNodeDefinition(4, "layer_04_defense", RouteNodeType.Defense, "layer_06_defense")
                },
                new[]
                {
                    new RouteNodeDefinition(5, "layer_05_shop", RouteNodeType.Shop, MidnightKibbleShopContentId),
                    new RouteNodeDefinition(5, "layer_05_dream_event", RouteNodeType.DreamEvent, UnreadRedDotRainEventContentId)
                },
                new[]
                {
                    new RouteNodeDefinition(6, "layer_06_defense", RouteNodeType.Defense, "layer_06_defense")
                },
                new[]
                {
                    new RouteNodeDefinition(7, "layer_07_blessing", RouteNodeType.BlessingOffering, "authority_blessing"),
                    new RouteNodeDefinition(7, "layer_07_rest_nest", RouteNodeType.RestNest, "rest_nest")
                },
                new[]
                {
                    new RouteNodeDefinition(8, "layer_08_rest_nest", RouteNodeType.RestNest, "rest_nest"),
                    new RouteNodeDefinition(8, "layer_08_elite_teddy", RouteNodeType.Elite, "elite_falling_dream_teddy")
                },
                new[]
                {
                    new RouteNodeDefinition(9, "layer_09_elite", RouteNodeType.Elite, "elite_red_eye_alarm"),
                    new RouteNodeDefinition(9, "layer_09_elite_teddy", RouteNodeType.Elite, "elite_falling_dream_teddy"),
                    new RouteNodeDefinition(9, "layer_09_shop", RouteNodeType.Shop, MidnightKibbleShopContentId)
                },
                new[]
                {
                    new RouteNodeDefinition(10, BossNodeId, RouteNodeType.Boss, "boss_call_tyrant")
                }
            };
        }
    }
}
