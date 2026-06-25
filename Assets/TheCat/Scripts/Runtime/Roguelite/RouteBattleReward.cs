using System;

namespace TheCat.Roguelite
{
    public readonly struct RouteBattleReward
    {
        public static RouteBattleReward None => new RouteBattleReward(0, 0, 0);

        public RouteBattleReward(int dreamShards, int fishTreats, int teamExperience = 0)
        {
            if (dreamShards < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(dreamShards), dreamShards, "Dream shards must not be negative.");
            }

            if (fishTreats < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(fishTreats), fishTreats, "Fish treats must not be negative.");
            }

            if (teamExperience < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(teamExperience), teamExperience, "Team experience must not be negative.");
            }

            DreamShards = dreamShards;
            FishTreats = fishTreats;
            TeamExperience = teamExperience;
        }

        public int DreamShards { get; }

        public int FishTreats { get; }

        public int TeamExperience { get; }

        public bool HasReward => DreamShards > 0 || FishTreats > 0 || TeamExperience > 0;

        public string BuildSummary()
        {
            if (!HasReward)
            {
                return "无奖励";
            }

            string summary = string.Empty;
            if (DreamShards > 0)
            {
                summary += "+" + DreamShards + " 梦屑";
            }

            if (FishTreats > 0)
            {
                if (!string.IsNullOrWhiteSpace(summary))
                {
                    summary += " ";
                }

                summary += "+" + FishTreats + " 小鱼干";
            }

            if (TeamExperience > 0)
            {
                if (!string.IsNullOrWhiteSpace(summary))
                {
                    summary += " ";
                }

                summary += "+" + TeamExperience + " 猫咪经验";
            }

            return summary;
        }
    }
}
