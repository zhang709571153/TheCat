using System;
using System.Collections.Generic;

namespace TheCat.Roguelite
{
    public static class P0DreamMapCatalog
    {
        public const string BedroomDreamMapId = "bedroom_dream";
        public const string EgyptDreamMapId = "egypt_moon_sand_dream";

        public static DreamMapDefinition GetBedroomDreamMap()
        {
            return new DreamMapDefinition(
                BedroomDreamMapId,
                "卧室梦境",
                "卧室守床",
                "中心床",
                "猫房梦境入口",
                "卧室门槛与床边",
                "守住主人睡眠，完成 10 层梦境路线。",
                true);
        }

        public static DreamMapDefinition GetEgyptDreamMap()
        {
            return new DreamMapDefinition(
                EgyptDreamMapId,
                "埃及梦境",
                "月砂遗迹",
                "月砂祭坛",
                "猫房沙月梦门",
                "月砂神殿外环",
                "作为 P0 第二梦境主题目标登记，先共享卧室战斗规则。",
                false);
        }

        public static IReadOnlyList<DreamMapDefinition> CreateP0DreamMaps()
        {
            return new[]
            {
                GetBedroomDreamMap(),
                GetEgyptDreamMap()
            };
        }

        public static DreamMapDefinition Find(string mapId)
        {
            if (mapId == BedroomDreamMapId)
            {
                return GetBedroomDreamMap();
            }

            if (mapId == EgyptDreamMapId)
            {
                return GetEgyptDreamMap();
            }

            throw new ArgumentException("Unknown P0 dream map id: " + mapId, nameof(mapId));
        }

        public static bool IsKnownMapId(string mapId)
        {
            return mapId == BedroomDreamMapId || mapId == EgyptDreamMapId;
        }
    }
}
